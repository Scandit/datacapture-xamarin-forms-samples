/*
 * This file is part of the Scandit Data Capture SDK
 *
 * Copyright (C) 2023- Scandit AG. All rights reserved.
 */

#nullable enable

using System.Threading.Tasks;
using Scandit.DataCapture.ID.Data.Unified;
using USDLVerificationSample.Models;

namespace USDLVerificationSample.Services
{
    public interface IDriverLicenseVerificationService
    {
        /// <summary>
        /// Verify a captured driver's license by running up to three checks:
        ///
        /// 1. First, a simple on-device verification where the data from the front of the card is
        /// compared with the data encoded in the barcode from the back of the card.
        ///
        /// 2. Check if the driver's license is not expired.
        ///
        /// 3. If front & back match and the driver's license has not expired, run a barcode
        /// verification.
        /// </summary>
        public Task<DriverLicenseVerificationResult> VerifyAsync(CapturedId capturedId);
    }
}
