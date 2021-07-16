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

using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Logo;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Common;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Logo
{
    public partial class LogoSettingsPage : ContentPage
    {
        private LogoSettingsViewModel viewModel;

        public LogoSettingsPage()
        {
            InitializeComponent();

            this.viewModel = BindingContext as LogoSettingsViewModel;
        }

        async void OnAnchorXOffsetTapped(System.Object sender, System.EventArgs e)
        {
            var page = new FloatWithUnitPage
            {
                BindingContext = new AnchorOffsetViewModel(AnchorOffsetViewModel.Type.X)
            };
            await Navigation.PushAsync(page);

        }

        async void OnAnchorYOffsetTapped(System.Object sender, System.EventArgs e)
        {
            var page = new FloatWithUnitPage
            {
                BindingContext = new AnchorOffsetViewModel(AnchorOffsetViewModel.Type.Y)
            };
            await Navigation.PushAsync(page);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.viewModel.OnResume();
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
