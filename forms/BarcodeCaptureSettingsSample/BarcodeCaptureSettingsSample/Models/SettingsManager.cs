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
using System.Linq;
using System.Threading.Tasks;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Barcode.UI.Unified;
using Scandit.DataCapture.Core.Area.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Common.Feedback.Unified;
using Scandit.DataCapture.Core.Common.Geometry.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Core.UI.Style.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;

namespace BarcodeCaptureSettingsSample.Models
{
    public class SettingsManager
    {
        private const string SCANDIT_LICENSE_KEY = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

        private static readonly Lazy<SettingsManager> instance = new Lazy<SettingsManager>(
            valueFactory: () => new SettingsManager(),
            isThreadSafe: true);

        public static SettingsManager Instance { get { return instance.Value; } }

        public DataCaptureContext DataCaptureContext { get; private set; }
        public BarcodeCaptureSettings BarcodeCaptureSettings { get; private set; }
        public BarcodeCapture BarcodeCapture { get; private set; }

        #region Viewfinder Settings

        public IViewfinder CurrentViewfinder { get; set; }

        public FloatWithUnit RectangularViewfinderWidth { get; set; } = new FloatWithUnit(0.80f, MeasureUnit.Fraction);
        public FloatWithUnit RectangularViewfinderHeight { get; set; } = new FloatWithUnit(0.32f, MeasureUnit.Fraction);
        public FloatWithUnit RectangularViewfinderShorterDimension { get; set; }
        public float RectangularViewfinderWidthAspect { get; set; }
        public float RectangularViewfinderHeightAspect { get; set; }
        public Xamarin.Forms.Color RectangularViewfinderColor { get; set; }
        public Xamarin.Forms.Color RectangularViewfinderDisabledColor { get; set; }
        public SizeSpecification RectangularViewfinderSizeSpecification { get; set; } = SizeSpecification.WidthAndHeight;
        public float RectangularViewfinderLongerDimensionAspect { get; set; }
        public RectangularViewfinderStyle RectangularViewfinderStyle = RectangularViewfinderStyle.Legacy;
        public RectangularViewfinderLineStyle RectangularViewfinderLineStyle = RectangularViewfinderLineStyle.Light;
        public bool RectangularViewfinderAnimationEnabled { get; set; } = false;
        public bool RectangularViewfinderAnimationLoopingEnabled { get; set; } = false;
        public float RectangularViewfinderDimming { get; set; }

        public LaserlineViewfinderStyle LaserlineViewfinderStyle = LaserlineViewfinderStyle.Legacy;
        public FloatWithUnit LaserlineViewfinderWidth { get; set; } = new FloatWithUnit(0.75f, MeasureUnit.Fraction);
        public Xamarin.Forms.Color LaserlineViewfinderEnabledColor { get; set; }
        public Xamarin.Forms.Color LaserlineViewfinderDisabledColor { get; set; }

        public Xamarin.Forms.Color AimerViewfinderFrameColor { get; set; }
        public Xamarin.Forms.Color AimerViewfinderDotColor { get; set; }

        #endregion

        #region ScanArea Settings
        public MarginsWithUnit ScanAreaMargins { get; set; } = new MarginsWithUnit(
            new FloatWithUnit(0, MeasureUnit.Dip),
            new FloatWithUnit(0, MeasureUnit.Dip),
            new FloatWithUnit(0, MeasureUnit.Dip),
            new FloatWithUnit(0, MeasureUnit.Dip)
        );

        public bool ShouldShowScanAreaGuides { get; set; }


        public void SetScanAreaTopMargin(FloatWithUnit topMargin)
        {
            this.ScanAreaMargins = new MarginsWithUnit(
                this.ScanAreaMargins.Left,
                topMargin,
                this.ScanAreaMargins.Right,
                this.ScanAreaMargins.Bottom);
        }

        public void SetScanAreaRightMargin(FloatWithUnit rightMargin)
        {
            this.ScanAreaMargins = new MarginsWithUnit(
                this.ScanAreaMargins.Left,
                this.ScanAreaMargins.Top,
                rightMargin,
                this.ScanAreaMargins.Bottom);
        }

        public void SetScanAreaBottomMargin(FloatWithUnit bottomMargin)
        {
            this.ScanAreaMargins = new MarginsWithUnit(
                this.ScanAreaMargins.Left,
                this.ScanAreaMargins.Top,
                this.ScanAreaMargins.Right,
                bottomMargin);
        }

        public void SetScanAreaLeftMargin(FloatWithUnit leftMargin)
        {
            this.ScanAreaMargins = new MarginsWithUnit(
                leftMargin,
                this.ScanAreaMargins.Top,
                this.ScanAreaMargins.Right,
                this.ScanAreaMargins.Bottom);
        }
        #endregion

        #region Point of Interest Settings
        public PointWithUnit PointOfInterest { get; set; } = new PointWithUnit(
            new FloatWithUnit(0.5f, MeasureUnit.Fraction),
            new FloatWithUnit(0.5f, MeasureUnit.Fraction)
        );
        #endregion

        #region Overlay Settings

        public Brush DefaultBrush => BarcodeCaptureOverlay.DefaultBrush;

        public Brush CurrentBrush { get; set; }

        #endregion

        #region Logo Settings

        public Anchor LogoAnchor { get; set; } = Anchor.BottomRight;
        public FloatWithUnit AnchorXOffset { get; set; } = new FloatWithUnit(0f, MeasureUnit.Fraction);
        public FloatWithUnit AnchorYOffset { get; set; } = new FloatWithUnit(0f, MeasureUnit.Fraction);

        #endregion

        #region Gestures Settings

        public bool IsTapToFocusEnabled { get; set; } = true;

        public bool IsSwipeToZoomEnabled { get; set; } = true;

        #endregion

        #region Controls

        public bool IsTorchButtonEnabled { get; set; } = false;

        #endregion

        #region Camera Settings

        public Camera CurrentCamera { get; private set; } = Camera.GetDefaultCamera();

        public TorchState TorchState { get; private set; } = TorchState.Off;
        public CameraPosition CameraPosition { get; private set; } = CameraPosition.WorldFacing;
        public CameraSettings CameraSettings { get; } = BarcodeCapture.RecommendedCameraSettings;

        public async Task SetCameraPositionAsync(CameraPosition cameraPosition)
        {
            if (this.CameraPosition != cameraPosition)
            {
                Camera camera = Camera.GetCamera(cameraPosition);
                if (camera != null)
                {
                    await camera.ApplySettingsAsync(this.CameraSettings);
                    await this.DataCaptureContext.SetFrameSourceAsync(camera);
                    this.CurrentCamera = camera;
                    this.CameraPosition = cameraPosition;
                }
            }
        }

        public void SetTorchState(TorchState torchState)
        {
            if (this.CurrentCamera != null)
            {
                this.CurrentCamera.DesiredTorchState = torchState;
                this.TorchState = torchState;
            }
        }

        public VideoResolution VideoResolution => this.CameraSettings.PreferredResolution;

        public async Task SetVideoResolutionAsync(VideoResolution videoResolution)
        {
            this.CameraSettings.PreferredResolution = videoResolution;
            await this.ApplyCameraSettingsAsync();
        }

        public float ZoomFactor => this.CameraSettings.ZoomFactor;

        public async Task SetZoomFactorAsync(float value)
        {
            this.CameraSettings.ZoomFactor = value;
            await this.ApplyCameraSettingsAsync();
        }

        public float ZoomGestureZoomFactor => this.CameraSettings.ZoomGestureZoomFactor;

        public async Task SetZoomGestureZoomFactorAsync(float value)
        {
            this.CameraSettings.ZoomGestureZoomFactor = value;
            await this.ApplyCameraSettingsAsync();
        }

        public FocusGestureStrategy FocusGestureStrategy => this.CameraSettings.FocusGestureStrategy;

        public async Task SetFocusGestureStrategyAsync(FocusGestureStrategy strategy)
        {
            this.CameraSettings.FocusGestureStrategy = strategy;
            await this.ApplyCameraSettingsAsync();
        }

        public async Task ApplyCameraSettingsAsync()
        {
            await this.CurrentCamera?.ApplySettingsAsync(this.CameraSettings);
        }
        #endregion

        #region Symbology Settings
        public SymbologySettings GetSymbologySettings(Symbology symbology)
        {
            return this.BarcodeCaptureSettings.GetSymbologySettings(symbology);
        }

        public bool IsSymbologyEnabled(string symbologyIdentifier)
        {
            Symbology symbology = SymbologyDescription.ForIdentifier(symbologyIdentifier).Symbology;
            return this.IsSymbologyEnabled(symbology);
        }

        public bool IsSymbologyEnabled(Symbology symbology)
        {
            return this.GetSymbologySettings(symbology).Enabled;
        }

        public async Task EnableAllSymbologies(bool enabled)
        {
            foreach (SymbologyDescription description in SymbologyDescription.All())
            {
                this.BarcodeCaptureSettings.EnableSymbology(description.Symbology, enabled);
            }

            await this.ApplyBarcodeCaptureSettingsAsync();
        }

        public async Task EnableSymbologyAsync(Symbology symbology, bool enabled, bool updateBarcodeCaptureSettings = false)
        {
            this.BarcodeCaptureSettings.EnableSymbology(symbology, enabled);
            if (updateBarcodeCaptureSettings)
            {
                await this.ApplyBarcodeCaptureSettingsAsync();
            }
        }

        public IEnumerable<Symbology> EnabledSymbologies => this.BarcodeCaptureSettings.EnabledSymbologies;

        public bool IsColorInverted(Symbology symbology)
        {
            return this.GetSymbologySettings(symbology).ColorInvertedEnabled;
        }

        public async Task SetColorInvertedAsync(Symbology symbology, bool colorInvertible)
        {
            this.GetSymbologySettings(symbology).ColorInvertedEnabled = colorInvertible;
            await this.ApplyBarcodeCaptureSettingsAsync();
        }

        public bool IsExtensionEnabled(Symbology symbology, string extension)
        {
            return this.GetSymbologySettings(symbology).IsExtensionEnabled(extension);
        }

        public async Task SetExtensionEnabledAsync(Symbology symbology, string extension, bool enabled)
        {
            this.GetSymbologySettings(symbology).SetExtensionEnabled(extension, enabled);
            await this.ApplyBarcodeCaptureSettingsAsync();
        }

        public short GetMinSymbolCount(Symbology symbology)
        {
            var activeSymbolsCount = this.GetSymbologySettings(symbology).ActiveSymbolCounts;

            return activeSymbolsCount.Count > 0 ? activeSymbolsCount.Min() : (short)0;
        }

        public async Task SetMinSymbolCountAsync(Symbology symbology, short minSymbolCount)
        {
            SymbologySettings symbologySettings = this.GetSymbologySettings(symbology);
            short maxSymbolCount = symbologySettings.ActiveSymbolCounts.Max();
            await this.SetSymbolCountAsync(symbologySettings, minSymbolCount, maxSymbolCount);
        }

        public short GetMaxSymbolCount(Symbology symbology)
        {
            var activeSymbolsCount = this.GetSymbologySettings(symbology).ActiveSymbolCounts;

            return activeSymbolsCount.Count > 0 ? activeSymbolsCount.Max() : (short)0;
        }

        public async Task SetMaxSymbolCountAsync(Symbology symbology, short maxSymbolCount)
        {
            SymbologySettings symbologySettings = this.GetSymbologySettings(symbology);
            short minSymbologyCount = symbologySettings.ActiveSymbolCounts.Min();
            await this.SetSymbolCountAsync(symbologySettings, minSymbologyCount, maxSymbolCount);
        }

        private async Task SetSymbolCountAsync(SymbologySettings symbologySettings, short minSymbolCount, short maxSymbolCount)
        {
            HashSet<short> symbolCount = new HashSet<short>();

            if (minSymbolCount >= maxSymbolCount)
            {
                symbolCount.Add(maxSymbolCount);
            }
            else
            {
                for (short i = minSymbolCount; i <= maxSymbolCount; i++)
                {
                    symbolCount.Add(i);
                }
            }

            symbologySettings.ActiveSymbolCounts = symbolCount;
            await this.ApplyBarcodeCaptureSettingsAsync();
        }

        #endregion

        #region Location Settings

        public ILocationSelection LocationSelection
        {
            get
            {
                return this.BarcodeCaptureSettings.LocationSelection;
            }
            set
            {
                this.BarcodeCaptureSettings.LocationSelection = value;
            }
        }
        public float LocationSelectionRadiusValue { get; set; }
        public MeasureUnit LocationSelectionRadiusMeasureUnit { get; set; } = MeasureUnit.Dip;
        public SizeSpecification LocationSelectionRectangularSizeSpecification { get; set; } = SizeSpecification.WidthAndHeight;
        public FloatWithUnit LocationSelectionRectangularWidth { get; set; } = new FloatWithUnit(0f, MeasureUnit.Dip);
        public FloatWithUnit LocationSelectionRectangularHeight { get; set; } = new FloatWithUnit(0f, MeasureUnit.Dip);
        public float LocationSelectionRectangularWidthAspect { get; set; }
        public float LocationSelectionRectangularHeightAspect { get; set; }
        public FloatWithUnit LocationSelectionRectangularShorterDimension { get; set; }
        public float LocationSelectionRectangularLongerDimensionAspect { get; set; }

        public Task SetLocationSelectionAsync(ILocationSelection locationSelection)
        {
            this.LocationSelection = locationSelection;
            return this.ApplyBarcodeCaptureSettingsAsync();
        }

        public Task SetLocationSelectionRadiusValueAsync(float value)
        {
            if (this.LocationSelection is RadiusLocationSelection)
            {
                MeasureUnit currentUnit = ((RadiusLocationSelection)this.LocationSelection).Radius.Unit;
                FloatWithUnit newRadius = new FloatWithUnit(value, currentUnit);
                this.LocationSelectionRadiusValue = value;
                return this.SetLocationSelectionAsync(RadiusLocationSelection.Create(newRadius));
            }
            return Task.CompletedTask;
        }

        public Task SetLocationSelectionRadiusMeasureUnitAsync(MeasureUnit value)
        {
            if (this.LocationSelection is RadiusLocationSelection)
            {
                float currentValue = ((RadiusLocationSelection)this.LocationSelection).Radius.Value;
                FloatWithUnit newRadius = new FloatWithUnit(currentValue, value);
                this.LocationSelectionRadiusMeasureUnit = value;
                return this.SetLocationSelectionAsync(RadiusLocationSelection.Create(newRadius));
            }
            return Task.CompletedTask;
        }

        #endregion

        #region Feedback Settings

        public bool SoundEnabled
        {
            get
            {
                return this.BarcodeCapture.Feedback.Success.Sound != null;
            }
            set
            {
                var currentVibration = this.BarcodeCapture.Feedback.Success.Vibration;
                this.BarcodeCapture.Feedback.Success = new Feedback(currentVibration, value == true ? Sound.DefaultSound : null);
            }
        }

        public bool VibrationEnabled
        {
            get
            {
                return this.BarcodeCapture.Feedback.Success.Vibration != null;
            }
            set
            {
                var currentSound = this.BarcodeCapture.Feedback.Success.Sound;
                this.BarcodeCapture.Feedback.Success = new Feedback(value == true ? Vibration.DefaultVibration : null, currentSound);
            }
        }

        #endregion

        #region Composite Type settings

        public Task SetEnabledCompositeTypes(CompositeType types)
        {
            this.BarcodeCaptureSettings.EnabledCompositeTypes = types;
            this.BarcodeCaptureSettings.EnableSymbologies(types);
            return this.ApplyBarcodeCaptureSettingsAsync();
        }

        public bool IsCompositeTypeEnabled(CompositeType compositeType)
        {
            var enabledCompositeType = GetEnabledCompositeTypes();
            return enabledCompositeType.HasFlag(compositeType);
        }


        public CompositeType GetEnabledCompositeTypes()
        {
            return this.BarcodeCaptureSettings.EnabledCompositeTypes;
        }

        #endregion

        #region Code Duplicate Filter

        public TimeSpan CodeDuplicateFilter
        {
            get
            {
                return this.BarcodeCaptureSettings.CodeDuplicateFilter;
            }
            set
            {
                this.BarcodeCaptureSettings.CodeDuplicateFilter = value;
                _ = this.ApplyBarcodeCaptureSettingsAsync();
            }
        }

        #endregion

        #region ScanResult Settings

        public bool ContinuousScanningEnabled { get; set; } = false;

        #endregion

        public Task ApplyBarcodeCaptureSettingsAsync()
        {
            return this.BarcodeCapture.ApplySettingsAsync(this.BarcodeCaptureSettings);
        }

        private SettingsManager()
        {
            this.CurrentCamera?.ApplySettingsAsync(this.CameraSettings);

            // The barcode capturing process is configured through barcode capture settings
            // which are then applied to the barcode capture instance that manages barcode recognition.
            this.BarcodeCaptureSettings = BarcodeCaptureSettings.Create();

            // Create data capture context using your license key and set the camera as the frame source.
            this.DataCaptureContext = DataCaptureContext.ForLicenseKey(SCANDIT_LICENSE_KEY);
            this.DataCaptureContext.SetFrameSourceAsync(this.CurrentCamera);

            // Create new barcode capture mode with the settings from above.
            this.BarcodeCapture = BarcodeCapture.Create(this.DataCaptureContext, this.BarcodeCaptureSettings);
            this.BarcodeCapture.Enabled = true;
        }
    }
}
