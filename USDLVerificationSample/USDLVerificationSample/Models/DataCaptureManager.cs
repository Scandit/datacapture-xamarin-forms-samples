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
using Scandit.DataCapture.ID.Capture.Unified;
using Scandit.DataCapture.ID.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Source.Unified;

namespace USDLVerificationSample.Models
{
    public class DataCaptureManager
    {
        // Enter your Scandit License key here.
        public const string SCANDIT_LICENSE_KEY = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

        private static readonly Lazy<DataCaptureManager> instance = new Lazy<DataCaptureManager>(() => new DataCaptureManager(), LazyThreadSafetyMode.PublicationOnly);

        public static DataCaptureManager Instance => instance.Value;

        private DataCaptureManager()
        {
            this.CurrentCamera?.ApplySettingsAsync(this.CameraSettings);

            // Create data capture context using your license key and set the camera as the frame source.
            this.DataCaptureContext = DataCaptureContext.ForLicenseKey(SCANDIT_LICENSE_KEY);
            this.DataCaptureContext.SetFrameSourceAsync(this.CurrentCamera);

            this.ConfigureIdCapture();
        }

        #region DataCaptureContext
        public DataCaptureContext DataCaptureContext { get; private set; }
        #endregion

        #region CamerSettings
        public Camera CurrentCamera { get; set; } = Camera.GetCamera(CameraPosition.WorldFacing);
        public CameraSettings CameraSettings { get; set; } = IdCapture.RecommendedCameraSettings;
        #endregion

        #region IdCapture
        public IdCapture IdCapture { get; private set; }
        #endregion

        private void ConfigureIdCapture()
        {
            this.DataCaptureContext.RemoveAllModes();

            // Create a mode responsible for recognizing documents. This mode is automatically added
            // to the passed DataCaptureContext.
            IdCaptureSettings settings = new IdCaptureSettings();
            settings.SupportedDocuments = IdDocumentType.DlViz;
            settings.SetShouldPassImageTypeToResult(IdImageType.Face, true);
            settings.SetShouldPassImageTypeToResult(IdImageType.IdBack, true);
            settings.SetShouldPassImageTypeToResult(IdImageType.IdFront, true);
            settings.SupportedSides = SupportedSides.FrontAndBack;

            this.IdCapture = IdCapture.Create(this.DataCaptureContext, settings);
        }
    }
}
