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
using System.Text;
using System.Threading.Tasks;
using IdCaptureExtendedSample.Models;
using IdCaptureExtendedSample.Services;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.ID.Capture.Unified;
using Scandit.DataCapture.ID.Data.Unified;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace IdCaptureExtendedSample.ViewModels
{
    public class ScanViewModel : BaseViewModel, IIdCaptureListener
    {
        private readonly ScannerModel model = ScannerModel.Instance;

        public DataCaptureContext DataCaptureContext => this.model.DataCaptureContext;
        public IdCapture IdCapture => this.model.IdCapture;
        public Mode Mode => this.model.Mode;

        public event EventHandler<CapturedIdEventArgs> IdCaptured;

        public ScanViewModel()
        {
            this.ConfigureIdCapture(this.model.Mode);
            this.SubscribeToAppMessages();
        }

        #region IIdCaptureListener
        public void OnIdCaptured(IdCapture idCapture, CapturedId capturedId)
        {
            idCapture.Enabled = false;

            this.IdCaptured?.Invoke(this, new CapturedIdEventArgs(capturedId));
        }

        public void OnIdRejected(IdCapture idCapture, CapturedId capturedId, RejectionReason rejection)
        {
            // Implement to handle documents recognized in a frame, but rejected.
            // A document or its part is considered rejected when (a) it's valid, but not enabled in the settings,
            // (b) it's a barcode of a correct symbology or a Machine Readable Zone (MRZ),
            // but the data is encoded in an unexpected/incorrect format.

            // Pause the IdCapture to not capture while showing the result.
            idCapture.Enabled = false;

            DependencyService.Get<IMessageService>()
                             .ShowAlertAsync("Document not supported")
                             .ContinueWith((Task t) =>
                             {
                                 // On alert dialog completion resume the IdCapture.
                                 idCapture.Enabled = true;
                             });
        }
        #endregion

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

        public void ConfigureIdCapture(Mode mode)
        {
            this.model.IdCapture?.RemoveListener(this);
            this.model.ConfigureIdCapture(mode);
            this.model.IdCapture.AddListener(this);
        }

        private void SubscribeToAppMessages()
        {
            MessagingCenter.Subscribe(this, App.MessageKeys.OnResume, callback: async (App app) => await this.OnResumeAsync());
            MessagingCenter.Subscribe(this, App.MessageKeys.OnSleep, callback: async (App app) => await this.OnSleepAsync());
        }

        private async Task ResumeFrameSourceAsync()
        {
            this.model.IdCapture.Enabled = true;

            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            await this.model.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }
    }
}
