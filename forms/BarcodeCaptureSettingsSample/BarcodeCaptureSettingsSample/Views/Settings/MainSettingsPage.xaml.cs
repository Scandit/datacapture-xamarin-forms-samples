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
using BarcodeCaptureSettingsSample.ViewModels.Settings;
using BarcodeCaptureSettingsSample.Views.Settings.BacodeCapture;
using BarcodeCaptureSettingsSample.Views.Settings.Camera;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings
{
    public partial class MainSettingsPage : ContentPage
    {
        public MainSettingsPage()
        {
            InitializeComponent();
        }

        async void MainSettingsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch ((e.Item as MainSettingsItem).ItemType)
            {
                case MainSettingsItem.Type.BarcodeCapture:
                    await Navigation.PushAsync(new BarcodeCaptureSettingsPage());
                    break;
                case MainSettingsItem.Type.View:
                    await Navigation.PushAsync(new ViewSettingsPage());
                    break;
                case MainSettingsItem.Type.Camera:
                    await Navigation.PushAsync(new CameraSettingsPage());
                    break;
                case MainSettingsItem.Type.Result:
                    await Navigation.PushAsync(new Result.ResultSettingsPage());
                    break;
                default:
                    throw new InvalidOperationException($"Missing {nameof(MainSettingsItem.Type)} case");
            }
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
