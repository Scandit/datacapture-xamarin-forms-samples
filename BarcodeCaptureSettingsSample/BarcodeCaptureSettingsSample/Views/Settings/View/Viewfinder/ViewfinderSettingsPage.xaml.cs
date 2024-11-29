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
using System.Threading.Tasks;
using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Viewfinder.Aimer;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Viewfinder.Rectangular;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Viewfinder
{
    public partial class ViewfinderSettingsPage : ContentPage
    {
        private readonly ViewfinderSettingsViewModel viewModel;

        private View viewFinderTypeSettingsPage;

        public ViewfinderSettingsPage()
        {
            InitializeComponent();

            viewModel = BindingContext as ViewfinderSettingsViewModel;
        }

        async void ViewFindersList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            await Task.Run(() =>
            {
                viewModel.ChangeViewfinder(e.Item as ViewfinderSettingsItem);
            });

            updateView();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            updateView();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            this.viewModel.ApplyViewfinderChanges();
        }

        private void updateView()
        {
            if (this.viewFinderTypeSettingsPage != null)
                this.viewFinderContainer.Children.Remove(viewFinderTypeSettingsPage);
            this.viewFinderTypeSettingsPage = GetChildView(viewModel.CurrentViewfinderSettingsItemType);

            if (this.viewFinderTypeSettingsPage != null)
            {
                this.viewFinderTypeSettingsPage.BindingContext = viewModel.CurrentViewfinderViewModel;
                this.viewFinderContainer.Children.Add(viewFinderTypeSettingsPage);
            }
        }

        private View GetChildView(ViewfinderSettingsItem.Type selectedItem)
        {
            switch (selectedItem)
            {
                case ViewfinderSettingsItem.Type.Rectangular:
                    var rectangularView = new RectangularViewfinderSettingsView();
                    rectangularView.Dissapering += (object sender, EventArgs args) => this.viewModel.ApplyViewfinderChanges();
                    return rectangularView;

                case ViewfinderSettingsItem.Type.Aimer:
                    return new AimerViewfinderSettingsView();

                default:
                    return null;
            }
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
