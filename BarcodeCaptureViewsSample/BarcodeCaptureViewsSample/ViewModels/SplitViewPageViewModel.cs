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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using BarcodeCaptureViewsSample.Models;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Area.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Common.Geometry.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BarcodeCaptureViewsSample.ViewModels
{
    public class SplitViewPageViewModel : BaseViewModel, IBarcodeCaptureListener
    {
        private readonly DataCaptureManager dataCaptureManger = DataCaptureManager.Instance;
        private readonly Timer scannerTimeoutTimer;
        private bool isTapToContinueVisible = false;

        public DataCaptureContext DataCaptureContext => this.dataCaptureManger.DataCaptureContext;
        public BarcodeCapture BarcodeCapture => this.dataCaptureManger.BarcodeCapture;
        public IViewfinder Viewfinder { get; } = new LaserlineViewfinder(LaserlineViewfinderStyle.Animated);
        public ICollection<ScanResult> Results { get; } = new ObservableCollection<ScanResult>();

        public bool IsTapToContinueVisible
        {
            get { return this.isTapToContinueVisible; }
            set { this.SetProperty(ref this.isTapToContinueVisible, value); }
        }

        public SplitViewPageViewModel()
        {
            this.InitializeScanner();
            this.SubscribeToAppMessages();

            this.scannerTimeoutTimer = new Timer()
            {
                AutoReset = false,
                Interval = TimeSpan.FromSeconds(10).TotalMilliseconds,
                Enabled = false
            };
            this.scannerTimeoutTimer.Elapsed += OnScannerTimeoutTimerElapsed;
        }

        public void ClearResults()
        {
            this.Results.Clear();
            this.OnPropertyChanged(nameof(this.Results));
        }

        public Task OnSleep()
        {
            this.IsTapToContinueVisible = true;
            this.BarcodeCapture.Enabled = false;
            return this.dataCaptureManger.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
        }

        public async Task OnResumeAsync()
        {
            this.IsTapToContinueVisible = false;
            this.BarcodeCapture.Enabled = true;

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
            this.dataCaptureManger.InitializeCamera();
            this.dataCaptureManger.InitializeBarcodeCapture();

            // Setting the code duplicate filter to one means that the scanner won't report the same code as recognized
            // for one second once it's recognized.
            this.dataCaptureManger.BarcodeCaptureSettings.CodeDuplicateFilter = TimeSpan.FromSeconds(1);

            // By setting the radius to zero, the barcode's frame has to contain the point of interest.
            // The point of interest is at the center of the data capture view by default, as in this case.
            this.dataCaptureManger.BarcodeCaptureSettings.LocationSelection = RadiusLocationSelection.Create(new FloatWithUnit(0f, MeasureUnit.Fraction));
            this.dataCaptureManger.BarcodeCapture.ApplySettingsAsync(this.dataCaptureManger.BarcodeCaptureSettings);

            // Register self as a listener to get informed whenever a new barcode got recognized.
            this.dataCaptureManger.BarcodeCapture.AddListener(this);
        }

        private Task ResumeFrameSource()
        {
            this.scannerTimeoutTimer.Stop();

            // We want to start the countdown to show the tap-to-continue overlay only if the license is valid.
            if (this.dataCaptureManger.IsLicenseValid)
            {
                this.scannerTimeoutTimer.Start();
            }

            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.dataCaptureManger.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }

        private void OnScannerTimeoutTimerElapsed(object sender, ElapsedEventArgs args)
        {
            this.IsTapToContinueVisible = true;
            this.BarcodeCapture.Enabled = false;
        }

        #region IBarcodeCaptureListener
        public void OnObservationStarted(BarcodeCapture barcodeCapture)
        { }

        public void OnObservationStopped(BarcodeCapture barcodeCapture)
        { }

        public void OnBarcodeScanned(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        {
            if (!session.NewlyRecognizedBarcodes.Any())
            {
                return;
            }

            // Reset the scanner timeout timer.
            this.scannerTimeoutTimer.Stop();
            this.scannerTimeoutTimer.Start();

            Barcode barcode = session.NewlyRecognizedBarcodes[0];

            // Get the human readable name of the symbology and assemble the result to be shown.
            SymbologyDescription description = new SymbologyDescription(barcode.Symbology);

            // Store results and notify view to update.
            this.Results.Add(new ScanResult(description.ReadableName, barcode.Data));
            this.OnPropertyChanged(nameof(this.Results));
        }

        public void OnSessionUpdated(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        { }
        #endregion        
    }
}
