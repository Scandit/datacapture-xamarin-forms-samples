/*
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixScanSimpleSample.Models;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Barcode.Tracking.Capture.Unified;
using Scandit.DataCapture.Barcode.Tracking.Data.Unified;
using Scandit.DataCapture.Barcode.Tracking.UI.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace MatrixScanSimpleSample.ViewModels
{
    public class MainPageViewModel : BaseViewModel, IBarcodeTrackingListener, IBarcodeTrackingBasicOverlayListener
    {
        private readonly Scandit.DataCapture.Core.UI.Style.Unified.Brush highlightedBrush;
        private readonly HashSet<ScanResult> scanResults = new HashSet<ScanResult>();

        public Camera Camera { get; private set; } = ScannerModel.Instance.CurrentCamera;
        public DataCaptureContext DataCaptureContext { get; private set; } = ScannerModel.Instance.DataCaptureContext;
        public BarcodeTracking BarcodeTracking { get; private set; } = ScannerModel.Instance.BarcodeTracking;

        public IEnumerable<ScanResult> ScanResults { get => this.scanResults; }

        public MainPageViewModel()
        {
            this.InitializeScanner();
            this.SubscribeToAppMessages();

            this.highlightedBrush = new Scandit.DataCapture.Core.UI.Style.Unified.Brush(Color.Transparent, Color.White, 2.0f);
        }

        public Task OnSleep()
        {
            // Switch camera off to stop streaming frames.
            // The camera is stopped asynchronously and will take some time to completely turn off.
            // Until it is completely stopped, it is still possible to receive further results, hence
            // it's a good idea to first disable barcode tracking as well.

            this.BarcodeTracking.Enabled = false;
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
        }

        public async Task OnResumeAsync()
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (permissionStatus == PermissionStatus.Granted)
                {
                    await this.ResumeFrameSource();
                }
            }
            else
            {
                await this.ResumeFrameSource();
            }
        }

        private void SubscribeToAppMessages()
        {
            MessagingCenter.Subscribe(this, App.MessageKeys.OnResume, callback: async (App app) => await this.OnResumeAsync());
            MessagingCenter.Subscribe(this, App.MessageKeys.OnSleep, callback: async (App app) => await this.OnSleep());
        }

        private void InitializeScanner()
        {
            // Register self as a listener to get informed whenever a new barcode got recognized.
            this.BarcodeTracking.AddListener(this);
        }

        private Task ResumeFrameSource()
        {
            this.scanResults.Clear();
            this.BarcodeTracking.Enabled = true;

            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }

        #region IBarcodeTrackingListener
        public void OnObservationStarted(BarcodeTracking barcodeTracking)
        { }

        public void OnObservationStopped(BarcodeTracking barcodeTracking)
        { }

        public void OnSessionUpdated(BarcodeTracking barcodeTracking, BarcodeTrackingSession session, IFrameData frameData)
        {
            // This method is called whenever objects are updated and it's the right place to react to the tracking results.
            lock (this.scanResults)
            {
                foreach (var trackedBarcode in session.AddedTrackedBarcodes)
                {
                    SymbologyDescription description = new SymbologyDescription(trackedBarcode.Barcode.Symbology);
                    this.scanResults.Add(new ScanResult(trackedBarcode.Identifier)
                    {
                        Data = trackedBarcode.Barcode.Data,
                        Symbology = description.ReadableName
                    });
                }

                foreach (int removedTrackedBarcodeId in session.RemovedTrackedBarcodes)
                {
                    this.scanResults.RemoveWhere(t => t.Id == removedTrackedBarcodeId);
                }
            }
        }
        #endregion

        #region IBarcodeTrackingBasicOverlayListener
        public Scandit.DataCapture.Core.UI.Style.Unified.Brush BrushForTrackedBarcode(BarcodeTrackingBasicOverlay overlay, TrackedBarcode trackedBarcode)
        {
            return this.highlightedBrush;
        }

        public void OnTrackedBarcodeTapped(BarcodeTrackingBasicOverlay overlay, TrackedBarcode trackedBarcode)
        { }
        #endregion
    }
}
