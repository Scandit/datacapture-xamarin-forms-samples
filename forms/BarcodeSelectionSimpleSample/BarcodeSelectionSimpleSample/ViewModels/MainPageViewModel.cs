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
using BarcodeSelectionSimpleSample.Models;
using BarcodeSelectionSimpleSample.Services;

using Scandit.DataCapture.Barcode.Selection.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace BarcodeSelectionSimpleSample.ViewModels
{
    public class MainPageViewModel : BaseViewModel, IBarcodeSelectionListener
    {
        public Camera Camera => ScannerModel.Instance.CurrentCamera;

        public DataCaptureContext DataCaptureContext => ScannerModel.Instance.DataCaptureContext;

        public BarcodeSelection BarcodeSelection => ScannerModel.Instance.BarcodeSelection;

        public BarcodeSelectionSettings BarcodeSelectionSettings => ScannerModel.Instance.BarcodeSelectionSettings;

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

        public bool SwitchToAimToSelectMode()
        {
            if (this.BarcodeSelectionSettings.SelectionType is BarcodeSelectionTapSelection)
            {
                this.BarcodeSelectionSettings.SelectionType = BarcodeSelectionAimerSelection.Create();
                this.BarcodeSelection.ApplySettingsAsync(this.BarcodeSelectionSettings)
                                     .ContinueWith((task) =>
                                         // Switch the camera to On state in case it froze through
                                         // double tap while on TapToSelect selection type. 
                                         this.Camera.SwitchToDesiredStateAsync(FrameSourceState.On));

                return true;
            }

            return false;
        }

        public bool SwitchToTapToSelectMode()
        {
            if (this.BarcodeSelectionSettings.SelectionType is BarcodeSelectionAimerSelection)
            {
                this.BarcodeSelectionSettings.SelectionType = BarcodeSelectionTapSelection.Create();
                this.BarcodeSelection.ApplySettingsAsync(this.BarcodeSelectionSettings);

                return true;
            }

            return false;
        }

        private void SubscribeToAppMessages()
        {
            MessagingCenter.Subscribe(this, App.MessageKeys.OnResume, callback: async (App app) => await this.OnResumeAsync());
            MessagingCenter.Subscribe(this, App.MessageKeys.OnSleep, callback: async (App app) => await this.OnSleep());
        }

        private void InitializeScanner()
        {
            // Register self as a listener to get informed whenever a new barcode got recognized.
            this.BarcodeSelection.AddListener(this);
        }

        private Task ResumeFrameSource()
        {
            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return this.Camera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }

        #region IBarcodeSelectionListener
        public void OnObservationStarted(BarcodeSelection barcodeCapture)
        { }

        public void OnObservationStopped(BarcodeSelection barcodeCapture)
        { }

        public void OnSelectionUpdated(BarcodeSelection barcodeSelection, BarcodeSelectionSession session, IFrameData frameData)
        {
            if (!session.NewlySelectedBarcodes.Any())
            {
                return;
            }

            Barcode barcode = session.NewlySelectedBarcodes.First();

            // Get barcode selection count.
            int selectionCount = session.GetCount(barcode);

            // Get the human readable name of the symbology and assemble the result to be shown.
            SymbologyDescription description = new SymbologyDescription(barcode.Symbology);
            string result = barcode.Data + " (" + description.ReadableName + ")" + "\nTimes: " + selectionCount;

            DependencyService.Get<IMessageService>().ShowAsync(result);
        }

        public void OnSessionUpdated(BarcodeSelection barcodeSelection, BarcodeSelectionSession session, IFrameData frameData)
        { }
        #endregion
    }
}
