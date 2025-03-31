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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GS1ParserSample.Models;
using GS1ParserSample.Services;

using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Scandit.DataCapture.Parser.Unified;
using Xamarin.Essentials;
using Xamarin.Forms;

using ScanditStyle = Scandit.DataCapture.Core.UI.Style.Unified;

namespace GS1ParserSample.ViewModels
{
    public class MainPageViewModel : BaseViewModel, IBarcodeCaptureListener
    {
        private static IMessageService MessageService;
        private IViewfinder viewfinder;
        private ScanditStyle.Brush highlightingBrush;

        public Camera Camera => ScannerModel.Instance.CurrentCamera;

        public DataCaptureContext DataCaptureContext => ScannerModel.Instance.DataCaptureContext;

        public BarcodeCapture BarcodeCapture => ScannerModel.Instance.BarcodeCapture;

        public Parser Parser => ScannerModel.Instance.Parser;

        public IViewfinder Viewfinder
        {
            get { return this.viewfinder; }
            set
            {
                this.viewfinder = value;
                this.OnPropertyChanged(nameof(this.Viewfinder));
            }
        }

        public ScanditStyle.Brush HighlightingBrush
        {
            get { return this.highlightingBrush; }
            set
            {
                this.highlightingBrush = value;
                this.OnPropertyChanged(nameof(this.HighlightingBrush));
            }
        }

        public MainPageViewModel()
        {
            this.InitializeScanner();
            this.SubscribeToAppMessages();
        }

        public Task OnSleep()
        {
            // Switch camera off to stop streaming frames.
            // The camera is stopped asynchronously and will take some time to completely turn off.
            // Until it is completely stopped, it is still possible to receive further results, hence
            // it's a good idea to first disable barcode capture as well.
            this.BarcodeCapture.Enabled = false;
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

            // Adjust the overlay's barcode highlighting to match the new viewfinder styles and improve the visibility of feedback.
            // With 6.10 we will introduce this visual treatment as a new style for the overlay.
            this.HighlightingBrush = new ScanditStyle.Brush(fillColor: Color.Transparent,
                                                            strokeColor: Color.White,
                                                            strokeWidth: 3);
        }

        private Task ResumeFrameSource()
        {
            this.BarcodeCapture.Enabled = true;

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

            // Stop recognizing barcodes for as long as we are displaying the result. There won't be any new results until
            // the capture mode is enabled again. Note that disabling the capture mode does not stop the camera, the camera
            // continues to stream frames until it is turned off.

            // If you don't want the codes to be scanned more than once, consider setting the codeDuplicateFilter when
            // creating the barcode capture settings to -1.
            // You can set any other value (e.g. 500) to set a fixed timeout and override the smart behaviour enabled
            // by default.

            barcodeCapture.Enabled = false;

            try
            {
                ParsedData parsedData = this.Parser.ParseString(barcode.Data);
                StringBuilder sb = new StringBuilder();

                foreach (ParsedField field in parsedData.Fields)
                {
                    sb.Append(field.Name)
                      .Append(": ")
                      .Append(JsonSerializer.Serialize(field.Parsed))
                      .Append(Environment.NewLine);
                }

                if (MessageService == null)
                {
                    MessageService = DependencyService.Get<IMessageService>();
                }

                MessageService.ShowAsync(sb.ToString(), "Parser result", () => barcodeCapture.Enabled = true);
            }
            catch (ArgumentException e)
            {
                // Print the parser failure and continue.
                Debug.WriteLine(e.Message);
                barcodeCapture.Enabled = true;
            }
        }

        public void OnSessionUpdated(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        { }
        #endregion
    }
}
