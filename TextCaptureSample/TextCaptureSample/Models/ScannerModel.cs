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
using System.Threading;
using Scandit.DataCapture.Core.Area.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Common.Geometry.Unified;
using Scandit.DataCapture.Core.Source.Unified;
using Scandit.DataCapture.Text.Capture.Unified;

namespace TextCaptureSample.Models
{
    public class ScannerModel
    {
        public const string SCANDIT_LICENSE_KEY = "Aa2k0xbKMtvDJWNgLU02Cr8aLxUjNtOuqXCjHUxVAUf/d66Y5Tm74sJ+8L0rGQUZ20e52VlMY9I7YW4W13kWbvp36R8jbqQy6yZUGS50G5n4fRItJD6525RcbTYZQjoIGHQqle9jj08ra19ZUy9RliVlOn3hHz4WrGO8vORyATmFXJpULzk0I5RpiT84ckXhG2Ri8jtIzoISX3zsoiLtXVRGjjrkbuGZzGbKA180JKEpdfSQwVyupLti5yNYHAeKihS6IOklCTz8CM1BfRC4zBdIDjbVEJPFgAsLvMU0rTyJhHkB5Ds4wfHbKNFhW0T2XkYLKkvZ7X/HnEVD5oz9Kl4T4rtRkepJfsXUWHUgVugjLO5vqwhMcHNV5XpK2Pk/SLrzGF1PDRu8f4ZhBLrWKknWq+5TSK8GWi4wmGpVvbxqHhLljzOzplYs8I5TtphZ3otJNLs10lhk1YN9cmdaxpdUuF4k0WDU1Qfco75p5G+MBlsAVVFrs0xMF9fSMJkQ+4UU+G+py5781HPkpw4kaGwmJhGrzA/Lbhf4tL+XfynseLw42oygpfVabYEYRHSQx+1j5RpFSR6V9t4jlKsJu2xgYz0A96I82gIHItRRxZkT2oEsZCgYlgCiQsFcsFdo9N9bzDL9mVR5Nj0RPIVvKc01AVtKvXLx86g2rNPv45eBaJFrdsWmv97V8+Pv6M9d+Wr1qcTeT1BY8fvWUEDmU1HF6eCJ1A6cDAM+Nq4sAP9D2lH7D6rHwK+x07F56bMZibLeDoGKanE8PhhamhxBVemE/ByCoMoItBtSbpeBubHVsSHlGF3/AAKi6flY6j0htptgPOM8eOwGXx6YvVxu3KOMF+2RBIQai8LP0YEuhVJ0ST7WX5seeVSu5RMKUx/euHoQB6qID+ydzkXGzYZLTPPskmJSWqrboJQPIjZ/ruCtJepZ/+Lr7g5nCyb01w==";

        // For a comprehensive list of available JSON fields refer to the documentation.
        private static readonly string SettingsJson = "{{\"regex\" : \"{0}\"}}";
        private static readonly Lazy<ScannerModel> instance = new Lazy<ScannerModel>(() => new ScannerModel(), LazyThreadSafetyMode.PublicationOnly);

        public static ScannerModel Instance => instance.Value;

        public static readonly SizeWithUnit GS1RecognitionAreaSize = new SizeWithUnit(
            new FloatWithUnit(0.9f, MeasureUnit.Fraction),
            new FloatWithUnit(0.1f, MeasureUnit.Fraction)
        );

        public static readonly SizeWithUnit LOTRecognitionAreaSize = new SizeWithUnit(
                new FloatWithUnit(0.6f, MeasureUnit.Fraction),
                new FloatWithUnit(0.1f, MeasureUnit.Fraction)
        );

        public TextType CurrentTextType { get; set; } = TextType.GS1_AI;
        public RecognitionArea CurrentRecognitionArea { get; set; } = RecognitionArea.Center;

        #region DataCaptureContext
        public DataCaptureContext DataCaptureContext { get; private set; }
        #endregion

        #region CamerSettings
        public Camera CurrentCamera { get; private set; } = Camera.GetCamera(CameraPosition.WorldFacing);
        public CameraSettings CameraSettings { get; } = TextCapture.RecommendedCameraSettings;
        #endregion

        #region TextCapture
        public TextCapture TextCapture { get; private set; }
        public TextCaptureSettings TextCaptureSettings { get; private set; }
        #endregion

        public SizeWithUnit GetRecognitionAreaSize()
        {
            if (this.CurrentTextType == TextType.GS1_AI)
            {
                return GS1RecognitionAreaSize;
            }

            return LOTRecognitionAreaSize;
        }

        public void InitializeTextCapture()
        {
            this.TextCaptureSettings = TextCaptureSettings.FromJson(this.ReadTextCaptureSettingsJson());

            // We will limit the recognition to the specific area. It's a rectangle taking the 90% for GS1_AI text type
            // and 60% for LOT text type width of a frame, and 10% of it's height. We will move the center of this rectangle
            // depending on whether `Top`, `Center`, and `Bottom` RecognitionArea is selected,
            // by controlling TextCapture's `pointOfInterest` property.
            var locationSelection = RectangularLocationSelection.Create(this.GetRecognitionAreaSize());
            this.TextCaptureSettings.LocationSelection = locationSelection;

            if (this.TextCapture == null)
            {
                // Create a mode responsible for recognizing the text. This mode is automatically added
                // to the given DataCaptureContext.
                this.TextCapture = TextCapture.Create(this.DataCaptureContext, this.TextCaptureSettings);
            }
            else
            {
                this.TextCapture.ApplySettingsAsync(this.TextCaptureSettings);
            }

            // We set the center of the location selection.
            this.TextCapture.PointOfInterest = this.GetPointOfInterest();
        }

        private ScannerModel()
        {
            // Adjust camera settings - set Full HD resolution.
            this.CameraSettings.PreferredResolution = VideoResolution.FullHd;
            this.CurrentCamera?.ApplySettingsAsync(this.CameraSettings);

            // Create data capture context using your license key and set the camera as the frame source.
            this.DataCaptureContext = DataCaptureContext.ForLicenseKey(SCANDIT_LICENSE_KEY);
            this.DataCaptureContext.SetFrameSourceAsync(this.CurrentCamera);

            this.InitializeTextCapture();
        }

        private string ReadTextCaptureSettingsJson()
        {
            // While some of the TextCaptureSettings can be modified directly, some of them, like
            // `regex`, would normally be configured by a JSON you receive from us, tailored to your
            // specific use-case.
            return string.Format(SettingsJson, this.CurrentTextType.Regex);
        }

        private PointWithUnit GetPointOfInterest()
        {
            float centerY = this.CurrentRecognitionArea.CenterY;

            return new PointWithUnit(
                    new FloatWithUnit(0.5f, MeasureUnit.Fraction),
                    new FloatWithUnit(centerY, MeasureUnit.Fraction)
            );
        }
    }
}
