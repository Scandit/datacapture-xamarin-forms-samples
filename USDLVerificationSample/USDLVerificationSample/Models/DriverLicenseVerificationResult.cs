/*
 * This file is part of the Scandit Data Capture SDK
 *
 * Copyright (C) 2023- Scandit AG. All rights reserved.
 */

#nullable enable

namespace USDLVerificationSample.Models
{
    public class DriverLicenseVerificationResult
    {
        public bool? Expired { get; }

        public bool? BarcodeVerificationResult { get; }

        public bool? BarcodeVerificationError { get; }

        public DriverLicenseVerificationResult(
            bool? expired,
            bool? barcodeVerificationResult,
            bool? barcodeVerificationError)
        {
            this.Expired = expired;
            this.BarcodeVerificationResult = barcodeVerificationResult;
            this.BarcodeVerificationError = barcodeVerificationError;
        }
    }
}
