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

using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Controls;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Gestures;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Logo;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Overlay;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.PointOfInterest;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.ScanArea;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Viewfinder;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.ViewSettings
{
    public partial class ViewSettingsPage : ContentPage
    {
        public ViewSettingsPage()
        {
            InitializeComponent();
        }

        async void ViewSettingsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch ((e.Item as ViewSettingsItem).ItemType)
            {
                case ViewSettingsItem.Type.ScanArea:
                    await Navigation.PushAsync(new ScanAreaSettingsPage());
                    break;
                case ViewSettingsItem.Type.PointOfInterest:
                    await Navigation.PushAsync(new PointOfInterestSettingsPage());
                    break;
                case ViewSettingsItem.Type.Overlay:
                    await Navigation.PushAsync(new OverlaySettingsPage());
                    break;
                case ViewSettingsItem.Type.Viewfinder:
                    await Navigation.PushAsync(new ViewfinderSettingsPage());
                    break;
                case ViewSettingsItem.Type.Logo:
                    await Navigation.PushAsync(new LogoSettingsPage());
                    break;
                case ViewSettingsItem.Type.Gestures:
                    await Navigation.PushAsync(new GesturesSettingsPage());
                    break;
                case ViewSettingsItem.Type.Controls:
                    await Navigation.PushAsync(new ControlsSettingsPage());
                    break;
            }
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
