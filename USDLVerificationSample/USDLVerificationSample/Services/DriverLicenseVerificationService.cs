/*
 * This file is part of the Scandit Data Capture SDK
 *
 * Copyright (C) 2023- Scandit AG. All rights reserved.
 */

#nullable enable

using System.Threading.Tasks;
using Scandit.DataCapture.ID.Data.Unified;
using Scandit.DataCapture.ID.Verification.AamvaBarcode.Unified;
using USDLVerificationSample.Models;
using Xamarin.Forms;

namespace USDLVerificationSample.Services
{
    public class DriverLicenseVerificationService : IDriverLicenseVerificationService
    {
        private readonly AamvaBarcodeVerifier barcodeVerifier;

        public DriverLicenseVerificationService()
        {
            this.barcodeVerifier = AamvaBarcodeVerifier.Create(DataCaptureManager.Instance.DataCaptureContext);
        }

        public async Task<DriverLicenseVerificationResult> VerifyAsync(CapturedId capturedId)
        {
            bool? barcodeVerificationResult = null;
            bool? barcodeVerificationError = null;

            // If front and back match and ID is not expired, run barcode verification.
            // Please note barcode verification always returns false for expired documents.
            if (capturedId.Expired.HasValue && !capturedId.Expired.Value)
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
                expired: capturedId.Expired,
                barcodeVerificationResult: barcodeVerificationResult,
                barcodeVerificationError: barcodeVerificationError);
        }
    }
}
