/*
 * This file is part of the Scandit Data Capture SDK
 *
 * Copyright (C) 2023- Scandit AG. All rights reserved.
 */

#nullable enable

using System.Threading.Tasks;
using Scandit.DataCapture.ID.Data.Unified;
using Scandit.DataCapture.ID.Verification.AamvaBarcode.Unified;
using Scandit.DataCapture.ID.Verification.AamvaVizBarcode.Unified;
using USDLVerificationSample.Models;
using Xamarin.Forms;

namespace USDLVerificationSample.Services
{
    public class DriverLicenseVerificationService : IDriverLicenseVerificationService
    {
        private readonly AamvaVizBarcodeComparisonVerifier comparisonVerifier;
        private readonly AamvaBarcodeVerifier barcodeVerifier;

        public DriverLicenseVerificationService()
        {
            this.comparisonVerifier = AamvaVizBarcodeComparisonVerifier.Create();
            this.barcodeVerifier = AamvaBarcodeVerifier.Create(DataCaptureManager.Instance.DataCaptureContext);
        }

        public async Task<DriverLicenseVerificationResult> VerifyAsync(CapturedId capturedId)
        {
            AamvaVizBarcodeComparisonResult comparisonResult = this.comparisonVerifier.Verify(capturedId);
            bool isFrontBackComparisonSuccessful = comparisonResult.ChecksPassed;

            bool? barcodeVerificationResult = null;
            bool? barcodeVerificationError = null;

            // If front and back match and ID is not expired, run barcode verification.
            // Please note barcode verification always returns false for expired documents.
            if (isFrontBackComparisonSuccessful && capturedId.Expired.HasValue && !capturedId.Expired.Value)
            {
                try
                {
                    AamvaBarcodeVerificationResult barcodeResult =
                        await this.barcodeVerifier.VerifyCapturedIdAsync(capturedId);
                    barcodeVerificationResult = barcodeResult.AllChecksPassed;
                }
                catch
                {
                    barcodeVerificationError = true;
                }
            }

            return new DriverLicenseVerificationResult(
                frontAndBackMatchResult: isFrontBackComparisonSuccessful,
                expired: capturedId.Expired,
                barcodeVerificationResult: barcodeVerificationResult,
                barcodeVerificationError: barcodeVerificationError);
        }
    }
}
