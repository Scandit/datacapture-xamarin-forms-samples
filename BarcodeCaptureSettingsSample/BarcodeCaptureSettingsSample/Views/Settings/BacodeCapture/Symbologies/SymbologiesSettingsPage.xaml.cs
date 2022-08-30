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

using BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.Symbologies;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.BacodeCapture.Symbologies
{
    public partial class SymbologiesSettingsPage : ContentPage
    {
        private SymbologiesSettingsViewModel viewModel;

        public SymbologiesSettingsPage()
        {
            InitializeComponent();

            this.viewModel = this.BindingContext as SymbologiesSettingsViewModel;
        }

        async void symbologiesList_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            var page = new SymbologySettingsPage();
            page.BindingContext = new SymbologySettingsViewModel((e.Item as SymbologyItem).Symbology);
            await Navigation.PushAsync(page);
        }

        protected override void OnAppearing()
        {
            this.viewModel.OnResume();
        }

        async void ButtonEnableAll_Clicked(System.Object sender, System.EventArgs e)
        {
            await this.viewModel.EnableAllSymbologyAsync(true);
        }

        async void ButtonDisableAll_Clicked(System.Object sender, System.EventArgs e)
        {
            await this.viewModel.EnableAllSymbologyAsync(false);
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
