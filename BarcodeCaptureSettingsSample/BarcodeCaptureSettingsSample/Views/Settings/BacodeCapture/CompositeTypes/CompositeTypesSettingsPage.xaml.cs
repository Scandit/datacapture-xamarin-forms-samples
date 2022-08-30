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

using BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.CompositeTypes;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.BacodeCapture.CompositeTypes
{
    public partial class CompositeTypesSettingsPage : ContentPage
    {
        private CompositeTypesSettingsViewModel viewModel;
        public CompositeTypesSettingsPage()
        {
            InitializeComponent();

            this.viewModel = BindingContext as CompositeTypesSettingsViewModel;
        }

        async void CompositeTypesList_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            await this.viewModel.ToggleCompositeType(e.Item as CompositeTypeItem);
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
