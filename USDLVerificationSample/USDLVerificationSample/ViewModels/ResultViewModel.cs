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
using System.Diagnostics;
using System.Linq;
using Scandit.DataCapture.ID.Data.Unified;
using Scandit.DataCapture.ID.Verification.AamvaBarcode.Unified;
using Scandit.DataCapture.ID.Verification.AamvaVizBarcode.Unified;
using USDLVerificationSample.Models;
using USDLVerificationSample.Results;
using USDLVerificationSample.Results.Presenters;
using Xamarin.Forms;

#nullable enable

namespace USDLVerificationSample.ViewModels
{
    public class ResultViewModel : BaseViewModel
    {
        private readonly ResultPresenterFactory factory = new ResultPresenterFactory();

        public List<ResultEntry> Items { get; private set; }

        public ImageSource? FaceImage { get; private set; }
        public ImageSource? IdFrontImage { get; private set; }
        public ImageSource? IdBackImage { get; private set; }

        public bool ImagesVisible { get; private set; }
        public bool FrontAndBackMatch { get; private set; }
        public bool ExpirationVisible => !string.IsNullOrEmpty(this.ExpirationText);
        public bool IsExpired { get; private set; }
        public bool BarcodeVerificationPass { get; private set; }
        public bool BarcodeVerificationVisible => !string.IsNullOrEmpty(this.BarcodeVerificationText);
        public string ExpirationText { get; private set; }
        public string FrontAndBackMatchText { get; private set; }
        public string BarcodeVerificationText { get; private set; }

        public ResultViewModel(CapturedId capturedId, DriverLicenseVerificationResult verificationResult)
        {
            IResultPresenter resultPresenter = this.factory.Create(capturedId);

            this.Items = resultPresenter.Rows.ToList();
            this.FaceImage = capturedId.GetImageBitmapForType(IdImageType.Face)?.Source;
            this.IdFrontImage = capturedId.GetImageBitmapForType(IdImageType.IdFront)?.Source;
            this.IdBackImage = capturedId.GetImageBitmapForType(IdImageType.IdBack)?.Source;
            this.ImagesVisible = this.FaceImage != null || this.IdFrontImage != null || this.IdBackImage != null;
            this.FrontAndBackMatch = verificationResult.FrontAndBackMatchResult;
            this.FrontAndBackMatchText = this.FrontAndBackMatch ?
                "Information on front and back matches." :
                "Information on front and back does not match.";

            if (this.FrontAndBackMatch && verificationResult.Expired.HasValue)
            {
                this.IsExpired = verificationResult.Expired.Value;
                this.ExpirationText = this.IsExpired ?
                    "Document is expired." :
                    "Document is not expired.";
            }
            else
            {
                this.ExpirationText = string.Empty;
            }

            if (verificationResult.BarcodeVerificationResult.HasValue)
            {
                this.BarcodeVerificationPass = verificationResult.BarcodeVerificationResult.Value;
                this.BarcodeVerificationText = this.BarcodeVerificationPass ?
                    "Verification checks passed." :
                    "Verification checks failed.";
            }
            else
            {
                this.BarcodeVerificationText = string.Empty;
            }
        }
    }
}
