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
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Scandit.DataCapture.Text.Capture.Unified;
using Scandit.DataCapture.Text.Data.Unified;
using TextCaptureSample.Models;
using TextCaptureSample.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TextCaptureSample.ViewModels
{
    public class MainPageViewModel : BaseViewModel, ITextCaptureListener
    {
        private Models.TextType textType;
        private RecognitionArea recognitionArea;

        public Camera Camera => ScannerModel.Instance.CurrentCamera;
        public DataCaptureContext DataCaptureContext => ScannerModel.Instance.DataCaptureContext;
        public TextCapture TextCapture => ScannerModel.Instance.TextCapture;
        public RectangularViewfinder Viewfinder { get; private set; }

        public MainPageViewModel()
        {
            this.InitializeScanner(ScannerModel.Instance);
            this.SubscribeToAppMessages();
        }

        public Task OnSleep()
        {
            // Switch camera off to stop streaming frames.
            // The camera is stopped asynchronously and will take some time to completely turn off.
            // Until it is completely stopped, it is still possible to receive further results, hence
            // it's a good idea to first disable text capture as well.
            this.TextCapture.Enabled = false;
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
        }

        public async Task OnResumeAsync()
        {
            this.SynchronizeViewModel(ScannerModel.Instance);

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

        protected override void SynchronizeViewModel(ScannerModel model)
        {
            base.SynchronizeViewModel(model);

            if (this.textType != model.CurrentTextType || this.recognitionArea != model.CurrentRecognitionArea)
            {
                model.InitializeTextCapture();
                // Make Viewfinder's size the same as recognition area size.
                this.Viewfinder.SetSize(model.GetRecognitionAreaSize());

                this.textType = model.CurrentTextType;
                this.recognitionArea = model.CurrentRecognitionArea;
            }
        }

        private void SubscribeToAppMessages()
        {
            MessagingCenter.Subscribe(this, App.MessageKeys.OnResume, callback: async (App app) => await this.OnResumeAsync());
            MessagingCenter.Subscribe(this, App.MessageKeys.OnSleep, callback: async (App app) => await this.OnSleep());
        }

        private void InitializeScanner(ScannerModel model)
        {
            var rectangularViewfinder = new RectangularViewfinder(RectangularViewfinderStyle.Square, RectangularViewfinderLineStyle.Light)
            {
                Dimming = 0.2f, 
            };
            // Make Viewfinder's size the same as recognition area size.
            rectangularViewfinder.SetSize(model.GetRecognitionAreaSize());

            // Viewfinders are visual components only, and as such will not restrict the scan area.
            this.Viewfinder = rectangularViewfinder;

            // Register self as a listener to get informed whenever a new barcode got recognized.
            this.TextCapture.AddListener(this);

            this.textType = model.CurrentTextType;
            this.recognitionArea = model.CurrentRecognitionArea;
        }

        private Task ResumeFrameSource()
        {
            this.TextCapture.Enabled = true;

            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }

        #region ITextCaptureListener
        public void OnObservationStarted(TextCapture textCapture)
        { }

        public void OnObservationStopped(TextCapture textCapture)
        { }

        public void OnTextCaptured(TextCapture textCapture, TextCaptureSession session, IFrameData frameData)
        {
            if (session.NewlyCapturedTexts.Any())
            {
                // Here you may additionally process the captured text, for example to further validate it.
                CapturedText text = session.NewlyCapturedTexts.First();

                // Don't capture unnecessarily when the result is displayed.
                this.TextCapture.Enabled = false;

                // This callback may be executed on an arbitrary thread. We switch back to
                // the main thread.
                DependencyService.Get<IMessageService>().ShowAsync(text.Value, () => this.TextCapture.Enabled = true);
            }
        }
        #endregion ITextCaptureListener
    }
}
