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

using BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.LocationSelection;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.BacodeCapture.LocationSelection
{
    public partial class LocationSelectionSettingsPage : ContentPage
    {
        private LocationSelectionSettingsViewModel viewModel;

        public LocationSelectionSettingsPage()
        {
            InitializeComponent();

            this.viewModel = BindingContext as LocationSelectionSettingsViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateChildView();
        }

        async void locationSelectionList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await this.viewModel.SetLocationTypeAsync(e.Item as LocationSelectionItem);
            UpdateChildView();
        }

        private void UpdateChildView()
        {
            this.locationSelectionContainer.Children.Clear();

            switch (this.viewModel.CurrentLocationSelectionItemType)
            {
                case LocationSelectionItem.Type.Radius:
                    var radiusView = new RadiusLocationSelectionSettingsView
                    {
                        BindingContext = this.viewModel.LocationSelectionItemViewModel
                    };
                    this.locationSelectionContainer.Children.Add(radiusView);
                    break;
                case LocationSelectionItem.Type.Rectangular:
                    var rectangularView = new RectangularLocationSelectionSettingsView()
                    {
                        BindingContext = this.viewModel.LocationSelectionItemViewModel
                    };
                    this.locationSelectionContainer.Children.Add(rectangularView);
                    break;
            }
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
