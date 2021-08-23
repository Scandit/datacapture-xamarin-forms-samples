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
using BarcodeCaptureSettingsSample.ViewModels;
using BarcodeCaptureSettingsSample.Views.Settings;
using Scandit.DataCapture.Core.UI.Gestures.Unified;
using Scandit.DataCapture.Core.UI.Unified;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel viewModel;

        private readonly TorchSwitchControl torchSwitchControl = new TorchSwitchControl();


        public MainPage()
        {
            InitializeComponent();

            this.viewModel = this.BindingContext as MainPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = this.viewModel.OnResumeAsync();
            if (this.viewModel.IsTorchButtonEnabled)
            {
                this.dataCaptureView.AddControl(torchSwitchControl);
            }
            else
            {
                this.dataCaptureView.RemoveControl(torchSwitchControl);
            }

            this.dataCaptureView.ZoomGesture = this.viewModel.IsSwipeToZoomEnabled ? new SwipeToZoom() : null;
            this.dataCaptureView.FocusGesture = this.viewModel.IsTapToFocusEnabled ? new TapToFocus() : null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.viewModel.OnSleep();
        }

        async void Settings_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainSettingsPage());
        }
    }
}
