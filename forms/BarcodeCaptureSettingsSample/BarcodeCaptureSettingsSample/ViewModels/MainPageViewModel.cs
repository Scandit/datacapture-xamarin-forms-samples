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
using BarcodeCaptureSettingsSample.Models;
using BarcodeCaptureSettingsSample.Resources;
using BarcodeCaptureSettingsSample.Services;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Common.Geometry.Unified;
using Scandit.DataCapture.Core.Data.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.UI.Style.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Xamarin.Essentials;

namespace BarcodeCaptureSettingsSample.ViewModels
{
    public class MainPageViewModel : BaseViewModel, IBarcodeCaptureListener
    {
        private static readonly SettingsManager settings = SettingsManager.Instance;

        public DataCaptureContext DataCaptureContext => SettingsManager.Instance.DataCaptureContext;

        public BarcodeCapture BarcodeCapture => SettingsManager.Instance.BarcodeCapture;

        public MainPageViewModel()
        {
            this.BarcodeCapture.AddListener(this);
        }

        private IMessageService messageService;

        private IMessageService MessageService
        {
            get
            {
                if (messageService == null)
                {
                    messageService = Xamarin.Forms.DependencyService.Get<IMessageService>();
                }
                return messageService;
            }
        }

        public IViewfinder Viewfinder
        {
            get { return settings.CurrentViewfinder; }
            set { settings.CurrentViewfinder = value; }
        }

        public bool ShouldShowScanAreaGuides
        {
            get { return settings.ShouldShowScanAreaGuides; }
            set { settings.ShouldShowScanAreaGuides = value; }
        }

        public MarginsWithUnit ScanAreaMargins
        {
            get
            {
                return settings.ScanAreaMargins;
            }
        }

        public PointWithUnit PointOfInterest
        {
            get
            {
                return settings.PointOfInterest;
            }
        }

        public Anchor LogoAnchor
        {
            get
            {
                return settings.LogoAnchor;
            }
        }

        public PointWithUnit LogoOffset
        {
            get
            {
                return new PointWithUnit(settings.AnchorXOffset, settings.AnchorYOffset);
            }
        }

        public Brush CurrentBrush
        {
            get { return settings.CurrentBrush; }
            set { settings.CurrentBrush = value; }
        }

        public bool IsSwipeToZoomEnabled
        {
            get { return settings.IsSwipeToZoomEnabled; }
        }

        public bool IsTapToFocusEnabled
        {
            get { return settings.IsTapToFocusEnabled; }
        }

        public Task OnSleep()
        {
            return settings.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.Off);
        }

        public bool IsTorchButtonEnabled
        {
            get
            {
                return settings.IsTorchButtonEnabled;
            }
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
            OnPropertyChanged(nameof(this.ShouldShowScanAreaGuides));
            OnPropertyChanged(nameof(this.BarcodeCapture));
            OnPropertyChanged(nameof(this.Viewfinder));
            OnPropertyChanged(nameof(this.CurrentBrush));
            OnPropertyChanged(nameof(this.ScanAreaMargins));
            OnPropertyChanged(nameof(this.PointOfInterest));
            OnPropertyChanged(nameof(this.LogoAnchor));
            OnPropertyChanged(nameof(this.LogoOffset));
        }

        private Task ResumeFrameSource()
        {
            // Switch camera on to start streaming frames.
            // The camera is started asynchronously and will take some time to completely turn on.
            return settings.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.On);
        }

        public void StartFrameSource() => settings.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.On);

        public void StopFrameSource() => settings.CurrentCamera?.SwitchToDesiredStateAsync(FrameSourceState.Off);

        public void ResumeScanning() => settings.BarcodeCapture.Enabled = true;

        public void PauseScanning() => settings.BarcodeCapture.Enabled = false;

        public bool IsContinousScanLabelVisible { get; set; }

        public string ScanResult { get; set; }

        #region IBarcodeCaptureListener
        public void OnBarcodeScanned(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        {
            if (session.NewlyRecognizedBarcodes.Any())
            {
                if(!settings.ContinuousScanningEnabled)
                    this.PauseScanning();

                Barcode barcode = session.NewlyRecognizedBarcodes[0];
                _ = ShowScanResultAsync(barcode);
            }
        }

        public void OnObservationStarted(BarcodeCapture barcodeCapture)
        { }

        public void OnObservationStopped(BarcodeCapture barcodeCapture)
        { }

        public void OnSessionUpdated(BarcodeCapture barcodeCapture, BarcodeCaptureSession session, IFrameData frameData)
        { }
        #endregion


        private async Task ShowScanResultAsync(Barcode barcode)
        {
            string compositeType = string.Empty;
            string data = barcode.Data;

            if (!string.IsNullOrEmpty(barcode.AddOnData))
            {
                data += " " + barcode.AddOnData;
            }

            if (!string.IsNullOrEmpty(barcode.CompositeData))
            {
                data += " " + barcode.CompositeData;
                compositeType = this.GetStringFromCompositeFlag(barcode.CompositeFlag);
            }

            SymbologyDescription description = new SymbologyDescription(barcode.Symbology);
            string symbology = description.ReadableName;
            int symbolCount = barcode.SymbolCount;

            if (string.IsNullOrEmpty(compositeType))
            {
                this.ScanResult =
                    string.Format(AppResources.ResultParametrised_Symbology, symbology, data) +
                    Environment.NewLine +
                    string.Format(AppResources.ResultParametrised_SymbolCount, symbolCount);
            }
            else
            {
                this.ScanResult =
                    string.Format(AppResources.CcResultParametrised_Type, compositeType) +
                    Environment.NewLine +
                    string.Format(AppResources.CcResultParametrised_Symbology, symbology, data) +
                    Environment.NewLine +
                    string.Format(AppResources.CcResultParametrised_SymbolCount, symbolCount);
            }

            if (settings.ContinuousScanningEnabled)
            {
                this.IsContinousScanLabelVisible = true;
                OnPropertyChanged(nameof(this.ScanResult));
                OnPropertyChanged(nameof(this.IsContinousScanLabelVisible));
                await Task.Delay(500);
                this.IsContinousScanLabelVisible = false;
                OnPropertyChanged(nameof(this.IsContinousScanLabelVisible));
            }
            else
            {
                await this.MessageService.ShowAsync(AppResources.ScanResult, this.ScanResult, () => this.ResumeScanning());
            }
        }

        private string GetStringFromCompositeFlag(CompositeFlag compositeFlag)
        {
            switch (compositeFlag)
            {
                case CompositeFlag.Gs1TypeA:
                    return "A";
                case CompositeFlag.Gs1TypeB:
                    return "B";
                case CompositeFlag.Gs1TypeC:
                    return "C";
                default:
                    return string.Empty;
            };
        }
    }
}
