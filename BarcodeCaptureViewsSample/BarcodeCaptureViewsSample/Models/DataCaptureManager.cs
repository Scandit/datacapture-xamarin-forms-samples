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
using System.Threading;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.Source.Unified;

namespace BarcodeCaptureViewsSample.Models
{
    public class DataCaptureManager
    {
        // There is a Scandit sample license key set below here.
        // This license key is enabled for sample evaluation only.
        // If you want to build your own application, get your license key by signing up for a trial at https://ssl.scandit.com/dashboard/sign-up?p=test
        public const string SCANDIT_LICENSE_KEY = "AfUkdmKlRiP5FdlOFQnOhu4V3j5LFKttPGTWXFd7CkuRaTAstDqq78RrBm2ZG9LRu1T8CNgP6oLScGrUoEwfmP1TUXonIGCl2g9Fo5NYtmK/aEV8FX/YcdRKfWS5bJrTcWGDHdcsJxT6Me5C3RMdWZkdqeR5GEjDzT6dO4ZPWOBbNLjpkgZ0/MjtYQPKqSV+bSZC7+ekFaXovSKWfXV89BXtta/6sZHFJOMKxyvzh6zw5yA+NDR67OXoWKCrrNq4AOuBlt1ZelIHCqjQgTy/SZG110eJr5e4pth38Bx0fXE8FGX92BoxwJr1EG+P5CEJF8EFMy2zf87aJQYuzHmg0nM7czcNqLUd9F23uxntZYjKlwgWmmSzev/ozaumEvbW9RVW1bUQmV8pQ1SWILBuzQPeAw8iWOWgnTH18tH7cT+fUJumvM2rn7LWx9JYLAKBKRuwe2sDh3l5eqobZKdarIRsKVgXa4pw+gkYKuplzTo+Bzh70rbmtgq3IJ8hSpdoZITzfUQSwXkrgdQa5Cmrpxz9gXManBRt01h3eFXG7znZU9w0+uzzV/b5e6MQcPncODrCQOq0kfEBYgRoLAwVCOKnxyWQkqRbUpsTN2wy2MTg10flYhR/zf1eXdiUjgPUhWj8LtmgxJELYky7uMu46abfCkAw73e+12iJmlf9/tmTFk34La9ZQiF/BYps5h327ZW8qobay+Esx1i9dsaFKYt/nCN8jZdUYD/df+/vApyK4PMbph9EPRe5u0alg8BqpEExnkQsy1W7r85yngO/rxSXsY6rTMoTXb/87ul8uQnsrD41ZLtFdzo0OlbNTeNOI1mJz/E6/SOLbRRK";

        private static readonly Lazy<DataCaptureManager> instance = new Lazy<DataCaptureManager>(() => new DataCaptureManager(), LazyThreadSafetyMode.PublicationOnly);

        public static DataCaptureManager Instance => instance.Value;

        public bool IsLicenseValid { get; private set; } = false;

        private DataCaptureManager()
        {
            // Create DataCaptureContext using your license key.
            this.DataCaptureContext = DataCaptureContext.ForLicenseKey(SCANDIT_LICENSE_KEY);

            // Handle DataCaptureContext status changes.
            this.DataCaptureContext.StatusChanged += OnStatusChanged;
        }

        public void InitializeCamera()
        {
            // Set the device's default camera as DataCaptureContext's FrameSource. DataCaptureContext
            // passes the frames from it's FrameSource to the added modes to perform capture.
            this.DataCaptureContext.SetFrameSourceAsync(this.CurrentCamera);
            this.CurrentCamera?.ApplySettingsAsync(this.CameraSettings);
        }

        public void InitializeBarcodeCapture()
        {
            this.DataCaptureContext.RemoveAllModes();

            // The barcode capturing process is configured through barcode capture settings
            // which are then applied to the barcode capture instance that manages barcode recognition.
            this.BarcodeCaptureSettings = BarcodeCaptureSettings.Create();

            // The settings instance initially has all types of barcodes (symbologies) disabled.
            // For the purpose of this sample we enable a very generous set of symbologies.
            // In your own app ensure that you only enable the symbologies that your app requires as
            // every additional enabled symbology has an impact on processing times.
            HashSet<Symbology> symbologies = new HashSet<Symbology>
            {
                Symbology.Ean13Upca,
                Symbology.Ean8,
                Symbology.Upce,
                Symbology.Qr,
                Symbology.DataMatrix,
                Symbology.Code39,
                Symbology.Code128,
                Symbology.InterleavedTwoOfFive
            };

            this.BarcodeCaptureSettings.EnableSymbologies(symbologies);
            this.BarcodeCapture = BarcodeCapture.Create(this.DataCaptureContext, this.BarcodeCaptureSettings);
        }

        #region DataCaptureContext
        public DataCaptureContext DataCaptureContext { get; private set; }
        #endregion

        #region CamerSettings
        public Camera CurrentCamera { get; private set; } = Camera.GetDefaultCamera();
        public CameraSettings CameraSettings { get; } = BarcodeCapture.RecommendedCameraSettings;
        #endregion

        #region BarcodeCapture
        public BarcodeCapture BarcodeCapture { get; private set; }
        public BarcodeCaptureSettings BarcodeCaptureSettings { get; private set; }
        #endregion

        public void OnStatusChanged(object sender, StatusChangedEventArgs args)
        {
            this.IsLicenseValid = args.Status.Valid;
        }
    }
}
