﻿/*
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
        public const string SCANDIT_LICENSE_KEY = "AYjTKgwFKLhZGtmHmyNAawklGVUpLfmaJ2JN39hPFcbHRdb8Sh3UX45m7PRkJtORsQzsAeBZw7aAZ/VBZlp5ykVZZOOYUI8ZAxAsZ3tOrh5HXX2CzFyh2yNzGtUXQuR5eFHqhXNx8+mfbsvN2zErPt0+TW4TESKXSx4764U8HnIF/01crbTR4/qxeWvIgdmGJkoV2YZc4wfZjpQI2Uvd3/J2jFcv/WrVHgWZ/VAC2lHTzC3JdwtTNJKxxDpsqKp1sDlARxGjw4hlebrAUbft3aWMjbtpVn2T4D+tBN3GVuwlD9Uo7MN3Sto17fSVSD1JLymYPHP7zxsnByy9mCBhKqTf3YKCh8DughdNJpIIWaaoY6t6OTof+TxY25XAboYM1Ii3FdaK1MjK2x9bVujInqaIYzPRYRwQj6lPyVaYSiRRJTsR6l3RLXyorSeqM6Mjyspyb9Gl3ht1grXe8TzMwVUFLYwBlV1zYcKfCVxHIaPo8irO1X7+sImu0166pNeK962FxzUx+rJMsvEIhy8mzF//yRI8WBLZvuBS5AH8EJHBb5p6DcdLgNVf3AwQWw6S5ENIw1Nu+eS2p+nm7msRRWP5jbqo8TfwgoellmtHaljlvmQ47kXfZvo9feDd7qZtGvWuX22yZkb+3k0OEfNKZaBKLrfzKU6X5TlmMvyhU7mF6mMdkBwex+NuKhRl1fYVjzD1hk75j70/QgXyjMv9nJpSEIXEt//AVHZTG4lGvAT0l3hPOie/zS0ixEH11+LJvbzsZQXYngggsJ40oCbajRxnvrMEcJQ5Lcxnp/Ov8qTmApOqK+XmLAV/s+MdeeIatFNTk6o9xGar+cB8";

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
