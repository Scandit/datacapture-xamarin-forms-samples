/*
 * This file is part of the Scandit Data Capture SDK
 *
 * Copyright (C) 2021- Scandit AG. All rights reserved.
 */

using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Services
{
    public class MessageService : IMessageService
    {
        public Task ShowAsync(string message, Action handler = null)
        {
            return Device.InvokeOnMainThreadAsync(() =>
            {
                var alertStatus = App.Current.MainPage.DisplayAlert("Scandit", message, "OK");
                alertStatus.ContinueWith((Task) => handler?.Invoke());
            });
        }

        public Task ShowAsync(string title, string message, Action handler = null)
        {
            return Device.InvokeOnMainThreadAsync(() =>
            {
                var alertStatus = App.Current.MainPage.DisplayAlert(title, message, "OK");
                alertStatus.ContinueWith((Task) => handler?.Invoke());
            });
        }
    }
}
