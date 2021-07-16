/*
 * This file is part of the Scandit Data Capture SDK
 *
 * Copyright (C) 2021- Scandit AG. All rights reserved.
 */

using System;
using System.Threading.Tasks;

namespace BarcodeCaptureSettingsSample.Services
{
    public interface IMessageService
    {
        Task ShowAsync(string message, Action handler = null);

        Task ShowAsync(string title, string message, Action handler = null);
    }
}
