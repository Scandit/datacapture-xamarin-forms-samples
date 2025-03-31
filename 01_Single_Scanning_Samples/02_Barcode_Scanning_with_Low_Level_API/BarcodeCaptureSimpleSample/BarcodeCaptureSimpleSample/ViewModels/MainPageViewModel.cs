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
using System.Linq;
using System.Threading.Tasks;
using BarcodeCaptureSimpleSample.Models;
using BarcodeCaptureSimpleSample.Services;

using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Common.Feedback.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;

using Xamarin.Essentials;
using Xamarin.Forms;

using ScanditStyle = Scandit.DataCapture.Core.UI.Style.Unified;

namespace BarcodeCaptureSimpleSample.ViewModels
{
    public class MainPageViewModel : BaseViewModel, IBarcodeCaptureListener
    {
        private static IMessageService MessageService;
        private IViewfinder viewfinder;
        
        public Feedback Feedback => ScannerModel.Instance.Feedback;

        public Camera Camera { get; private set; } = ScannerModel.Instance.CurrentCamera;

        public DataCaptureContext DataCaptureContext { get; private set; } = ScannerModel.Instance.DataCaptureContext;

        public BarcodeCapture BarcodeCapture { get; private set; } = ScannerModel.Instance.BarcodeCapture;
        
        public event EventHandler RejectedCode;
        public event EventHandler AcceptedCode;

        public IViewfinder Viewfinder
        {
            get { return this.viewfinder; }
            set
            {
                this.viewfinder = value;
                this.OnPropertyChanged(nameof(Viewfinder));
            }
        }

        public MainPageViewModel()
        {
            this.InitializeScanner();
            this.SubscribeToAppMessages();
        }

        public Task OnSleep()
        {
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
            this.BarcodeCapture.AddListener(this);

            // Rectangular viewfinder with an embedded Scandit logo.
            // The rectangular viewfinder is displayed when the recognition is active and hidden when it is not.
            this.Viewfinder = new RectangularViewfinder(RectangularViewfinderStyle.Square, RectangularViewfinderLineStyle.Light);
        }

        private Task ResumeFrameSource()
        {
            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }

        #region IBarcodeCaptureListener
        public void OnObservationStarted(BarcodeCapture barcodeCapture)
        { }

        public void OnObservationStopped(BarcodeCapture barcodeCapture)
        { }

        public void OnBarcodeScanned(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        {
            if (session.NewlyRecognizedBarcode == null)
            {
                return;
            }

            Barcode barcode = session.NewlyRecognizedBarcode;
            
            // Use the following code to reject barcodes.
            // By uncommenting the following lines, barcodes not starting with 09: are ignored.
            // if (barcode.Data?.StartsWith("09:") == false)
            // {
            //     this.RejectedCode?.Invoke(this, EventArgs.Empty);
            //     return;
            // }
            // this.AcceptedCode?.Invoke(this, EventArgs.Empty);
            
            // We also want to emit a feedback (vibration and, if enabled, sound).
            // By default, every time a barcode is scanned, a sound (if not in silent mode) and a vibration are played.
            // To emit a feedback only when necessary, it is necessary to set a success feedback without sound and vibration
            // when setting up Barcode Capture (in this case in the constructor of the `ScannerModel` class).
            // this.Feedback.Emit();

            // Stop recognizing barcodes for as long as we are displaying the result. There won't be any new results until
            // the capture mode is enabled again. Note that disabling the capture mode does not stop the camera, the camera
            // continues to stream frames until it is turned off.
            barcodeCapture.Enabled = false;

            // If you don't want the codes to be scanned more than once, consider setting the codeDuplicateFilter when
            // creating the barcode capture settings to -1.
            // You can set any other value (e.g. 500) to set a fixed timeout and override the smart behaviour enabled
            // by default.

            // Get the human readable name of the symbology and assemble the result to be shown.
            SymbologyDescription description = new SymbologyDescription(barcode.Symbology);
            string result = "Scanned: " + barcode.Data + " (" + description.ReadableName + ")";

            if (MessageService == null)
            {
                MessageService = DependencyService.Get<IMessageService>();
            }

            MessageService.ShowAsync(result, () => barcodeCapture.Enabled = true);
        }

        public void OnSessionUpdated(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        { }
        #endregion
    }
}
