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
using Scandit.DataCapture.ID.Data.Unified;
using Scandit.DataCapture.ID.Verification.AamvaVizBarcode.Unified;
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
        public bool ChecksPassed { get; private set; }
        public bool ExpirationVisible => !string.IsNullOrEmpty(this.ExpirationText);
        public bool IsExpired { get; private set; }
        public string ExpirationText { get; private set; }
        public string VerificationText { get; private set; }

        public ResultViewModel(CapturedId capturedId, AamvaVizBarcodeComparisonResult verificationResult)
        {
            IResultPresenter resultPresenter = this.factory.Create(capturedId);

            this.Items = resultPresenter.Rows.ToList();
            this.FaceImage = capturedId.GetImageBitmapForType(IdImageType.Face)?.Source;
            this.IdFrontImage = capturedId.GetImageBitmapForType(IdImageType.IdFront)?.Source;
            this.IdBackImage = capturedId.GetImageBitmapForType(IdImageType.IdBack)?.Source;
            this.ImagesVisible = this.FaceImage != null || this.IdFrontImage != null || this.IdBackImage != null;

            this.ChecksPassed = verificationResult.ChecksPassed;
            this.VerificationText = this.ChecksPassed ?
                "Information on front and back match." :
                "Information on front and back do not match.";

            if (this.ChecksPassed && capturedId.DateOfExpiry != null)
            {
                this.IsExpired = capturedId.DateOfExpiry.Date < DateTime.Now.Date;

                if (!this.IsExpired)
                {
                    this.ExpirationText = "Document is not expired.";
                }
                else
                {
                    this.ExpirationText = "Document is expired.";
                }
            }
            else
            {
                this.ExpirationText = string.Empty;
            }
        }
    }
}
