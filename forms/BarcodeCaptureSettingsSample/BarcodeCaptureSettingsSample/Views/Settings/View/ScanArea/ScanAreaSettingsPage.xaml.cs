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

using System.ComponentModel;
using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.ScanArea;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Common;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.ScanArea
{
    public partial class ScanAreaSettingsPage : ContentPage
    {
        private readonly ScanAreaSettingsViewModel viewModel;

        public ScanAreaSettingsPage()
        {
            InitializeComponent();

            this.viewModel = BindingContext as ScanAreaSettingsViewModel;

        }

        void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            this.viewModel.ShouldShowScanAreaGuides = !this.viewModel.ShouldShowScanAreaGuides;
        }

        async void ViewSettingsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var marginItemPage = new FloatWithUnitPage
            {
                BindingContext = new MarginItemViewModel(e.Item as MarginItem)
            };

            await Navigation.PushAsync(marginItemPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewSettingsList.ItemsSource = this.viewModel.Source;
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
