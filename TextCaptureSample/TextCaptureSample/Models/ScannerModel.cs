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
        public const string SCANDIT_LICENSE_KEY = "AfUkdmKlRiP5FdlOFQnOhu4V3j5LFKttPGTWXFd7CkuRaTAstDqq78RrBm2ZG9LRu1T8CNgP6oLScGrUoEwfmP1TUXonIGCl2g9Fo5NYtmK/aEV8FX/YcdRKfWS5bJrTcWGDHdcsJxT6Me5C3RMdWZkdqeR5GEjDzT6dO4ZPWOBbNLjpkgZ0/MjtYQPKqSV+bSZC7+ekFaXovSKWfXV89BXtta/6sZHFJOMKxyvzh6zw5yA+NDR67OXoWKCrrNq4AOuBlt1ZelIHCqjQgTy/SZG110eJr5e4pth38Bx0fXE8FGX92BoxwJr1EG+P5CEJF8EFMy2zf87aJQYuzHmg0nM7czcNqLUd9F23uxntZYjKlwgWmmSzev/ozaumEvbW9RVW1bUQmV8pQ1SWILBuzQPeAw8iWOWgnTH18tH7cT+fUJumvM2rn7LWx9JYLAKBKRuwe2sDh3l5eqobZKdarIRsKVgXa4pw+gkYKuplzTo+Bzh70rbmtgq3IJ8hSpdoZITzfUQSwXkrgdQa5Cmrpxz9gXManBRt01h3eFXG7znZU9w0+uzzV/b5e6MQcPncODrCQOq0kfEBYgRoLAwVCOKnxyWQkqRbUpsTN2wy2MTg10flYhR/zf1eXdiUjgPUhWj8LtmgxJELYky7uMu46abfCkAw73e+12iJmlf9/tmTFk34La9ZQiF/BYps5h327ZW8qobay+Esx1i9dsaFKYt/nCN8jZdUYD/df+/vApyK4PMbph9EPRe5u0alg8BqpEExnkQsy1W7r85yngO/rxSXsY6rTMoTXb/87ul8uQnsrD41ZLtFdzo0OlbNTeNOI1mJz/E6/SOLbRRK";

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
