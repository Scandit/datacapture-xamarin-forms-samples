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

using BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.BacodeCapture
{
    public partial class BarcodeCaptureSettingsPage : ContentPage
    {
        public BarcodeCaptureSettingsPage()
        {
            InitializeComponent();
        }

        async void barcodeCaptureSettingsList_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            switch ((e.Item as BarcodeCaptureSettingsItem).ItemType)
            {
                case BarcodeCaptureSettingsItem.Type.Symbologies:
                    await Navigation.PushAsync(new Symbologies.SymbologiesSettingsPage());
                    break;
                case BarcodeCaptureSettingsItem.Type.CompositeTypes:
                    await Navigation.PushAsync(new CompositeTypes.CompositeTypesSettingsPage());
                    break;
                case BarcodeCaptureSettingsItem.Type.LocationSelection:
                    await Navigation.PushAsync(new LocationSelection.LocationSelectionSettingsPage());
                    break;
                case BarcodeCaptureSettingsItem.Type.Feedback:
                    await Navigation.PushAsync(new Feedback.FeedbackSettingsPage());
                    break;
                case BarcodeCaptureSettingsItem.Type.CodeDuplicateFilter:
                    await Navigation.PushAsync(new DuplicateFilter.CodeDuplicateFilterSettingsPage());
                    break;
                default:
                    break;
            }
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
