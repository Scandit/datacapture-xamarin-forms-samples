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

using System.Text;
using System.Threading.Tasks;
using USDLVerificationSample.Models;
using USDLVerificationSample.Services;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.ID.Capture.Unified;
using Scandit.DataCapture.ID.Data.Unified;
using Scandit.DataCapture.ID.Verification.AamvaVizBarcode.Unified;
using Xamarin.Essentials;
using Xamarin.Forms;
using System;
using Scandit.DataCapture.Core.Data.Unified;

namespace USDLVerificationSample.ViewModels
{
    public class ScanViewModel : BaseViewModel, IIdCaptureListener
    {
        private readonly DataCaptureManager model = DataCaptureManager.Instance;

        public DataCaptureContext DataCaptureContext => this.model.DataCaptureContext;
        public IdCapture IdCapture => this.model.IdCapture;

        public event EventHandler<CapturedIdEventArgs> IdCaptured;
        public event EventHandler AlignBack;

        public ScanViewModel()
        {
            this.SubscribeToScannerMessages();
            this.SubscribeToAppMessages();
        }

        public Task OnSleepAsync()
        {
            this.model.IdCapture.Enabled = false;
            return this.model.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
        }

        public async Task OnResumeAsync()
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (permissionStatus == PermissionStatus.Granted)
                {
                    await this.ResumeFrameSourceAsync();
                }
            }
            else
            {
                await this.ResumeFrameSourceAsync();
            }
        }

        private void SubscribeToScannerMessages()
        {
            this.IdCapture.AddListener(this);
        }

        private void SubscribeToAppMessages()
        {
            MessagingCenter.Subscribe(this, App.MessageKeys.OnResume, callback: async (App app) => await this.OnResumeAsync());
            MessagingCenter.Subscribe(this, App.MessageKeys.OnSleep, callback: async (App app) => await this.OnSleepAsync());
        }

        #region IIdCaptureListener
        public void OnIdCaptured(IdCapture capture, IdCaptureSession session, IFrameData frameData)
        {
            CapturedId capturedId = session.NewlyCapturedId;

            // Pause the idCapture to not capture while showing the result.
            capture.Enabled = false;

            if (capturedId.DocumentType == DocumentType.DrivingLicense && 
                capturedId.IssuingCountryIso.ToUpper() == "USA" && 
                capturedId.Viz != null &&
                capturedId.Viz.BackSideCaptureSupported)
            {
                // Until the back side is scanned, IdCapture will keep reporting the front side.
                // If we are looking for the back side we just return.
                if (capturedId.Viz.CapturedSides == SupportedSides.FrontOnly)
                {
                    // Scan the back side of the docuemt.
                    this.AlignBack?.Invoke(this, EventArgs.Empty);
                    capture.Enabled = true;
                }
                else
                {
                    // Front and back were scanned; perform a verification of the captured ID.
                    var result = AamvaVizBarcodeComparisonVerifier.Create().Verify(capturedId);
                    this.IdCaptured?.Invoke(this, new CapturedIdEventArgs(capturedId, result));
                }
            }
            else
            {
                this.OnIdRejected(capture, session, frameData);
            }
        }

        public void OnErrorEncountered(IdCapture capture, IdCaptureError error, IdCaptureSession session, IFrameData frameData)
        {
            // Don't capture unnecessarily when the error is displayed.
            capture.Enabled = false;

            // Implement to handle an error encountered during the capture process.
            // The error message can be retrieved from the IdCaptureError class type.
            DependencyService.Get<IMessageService>()
                             .ShowAlertAsync(GetErrorMessage(error))
                             .ContinueWith((Task t) =>
                             {
                                 // On alert dialog completion resume the IdCapture.
                                 capture.Enabled = true;
                             });
        }

        public void OnIdRejected(IdCapture capture, IdCaptureSession session, IFrameData frameData)
        {
            // Implement to handle documents recognized in a frame, but rejected.
            // A document or its part is considered rejected when (a) it's valid, but not enabled in the settings,
            // (b) it's a barcode of a correct symbology or a Machine Readable Zone (MRZ),
            // but the data is encoded in an unexpected/incorrect format.

            // Pause the IdCapture to not capture while showing the result.
            capture.Enabled = false;

            DependencyService.Get<IMessageService>()
                             .ShowAlertAsync("Document is not a US driver’s license.")
                             .ContinueWith((Task t) =>
                             {
                                 // On alert dialog completion resume the IdCapture.
                                 capture.Reset();
                                 capture.Enabled = true;
                             });
        }

        public void OnObservationStarted(IdCapture idCapture)
        {
            // In this sample we are not interested in this callback.
        }

        public void OnObservationStopped(IdCapture idCapture)
        {
            // In this sample we are not interested in this callback.
        }

        public void OnIdLocalized(IdCapture capture, IdCaptureSession session, IFrameData frameData)
        {
            // Implement to handle a personal identification document or its part localized within
            // a frame. A document or its part is considered localized when it's detected in a frame,
            // but its data is not yet extracted.

            // In this sample we are not interested in this callback.
        }
        #endregion

        private Task ResumeFrameSourceAsync()
        {
            this.model.IdCapture.Enabled = true;

            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.model.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }

        private static string GetErrorMessage(IdCaptureError error)
        {
            return new StringBuilder(error.Type.ToString()).Append($": {error.Message}").ToString();
        }
    }
}
