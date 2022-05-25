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

using System.Linq;
using System.Threading.Tasks;
using BarcodeCaptureViewsSample.Models;
using BarcodeCaptureViewsSample.Resources;
using BarcodeCaptureViewsSample.Services;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BarcodeCaptureViewsSample.ViewModels
{
    public class FullscreenPageViewModel : BaseViewModel, IBarcodeCaptureListener
    {
        private readonly DataCaptureManager dataCaptureManger = DataCaptureManager.Instance;

        public DataCaptureContext DataCaptureContext => this.dataCaptureManger.DataCaptureContext;
        public BarcodeCapture BarcodeCapture => this.dataCaptureManger.BarcodeCapture;

        public FullscreenPageViewModel()
        {
            this.InitializeScanner();
            this.SubscribeToAppMessages();
        }

        public Task OnSleep()
        {
            return this.dataCaptureManger.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
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
            this.dataCaptureManger.InitializeCamera();
            this.dataCaptureManger.InitializeBarcodeCapture();

            // Register self as a listener to get informed whenever a new barcode got recognized.
            this.dataCaptureManger.BarcodeCapture.AddListener(this);
        }

        private Task ResumeFrameSource()
        {
            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.dataCaptureManger.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.On);
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

            Barcode barcode = session.NewlyRecognizedBarcodes[0];

            // Stop recognizing barcodes for as long as we are displaying the result. There won't be any new results until
            // the capture mode is enabled again. Note that disabling the capture mode does not stop the camera, the camera
            // continues to stream frames until it is turned off.
            barcodeCapture.Enabled = false;

            // If you are not disabling barcode capture here and want to continue scanning, consider
            // setting the codeDuplicateFilter when creating the barcode capture settings to around 500
            // or even -1 if you do not want codes to be scanned more than once.

            // Get the human readable name of the symbology and assemble the result to be shown.
            SymbologyDescription description = new SymbologyDescription(barcode.Symbology);
            string result = string.Format(AppResources.ScanResultFormat, description.ReadableName, barcode.Data, barcode.SymbolCount);

            DependencyService.Get<IMessageService>().ShowAsync(result, () => barcodeCapture.Enabled = true);
        }

        public void OnSessionUpdated(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        { }
        #endregion
    }
}
