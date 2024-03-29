﻿/*
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
using System.Threading.Tasks;
using System.Timers;
using TextCaptureSample.Views;
using Xamarin.Forms;

namespace TextCaptureSample.Services
{
    public class MessageService : IMessageService
    {
        public Task ShowAsync(string message, Action handler = null)
        {
            return Device.InvokeOnMainThreadAsync(() =>
            {
                var alertStatus = App.Current.MainPage.DisplayAlert("Recognized Text", message, "Resume");
                alertStatus.ContinueWith((Task) => handler?.Invoke());
            });
        }
    }
}
