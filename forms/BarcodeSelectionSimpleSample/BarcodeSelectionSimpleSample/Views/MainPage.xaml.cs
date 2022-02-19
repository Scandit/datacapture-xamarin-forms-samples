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
using Xamarin.Forms;

namespace BarcodeSelectionSimpleSample.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.TapToSelectButton.FontAttributes = FontAttributes.Bold;
        }

        public void ShowScanResults(string result)
        {
            this.ScanResults.IsVisible = true;
            this.Label.Text = result ?? string.Empty;
        }

        public void HideScanResults()
        {
            this.ScanResults.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = this.viewModel.OnResumeAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.viewModel.OnSleep();
        }

        private void AimToSelectButtonClicked(object sender, EventArgs e)
        {
            if (this.viewModel.SwitchToAimToSelectMode())
            {
                this.AimToSelectButton.FontAttributes = FontAttributes.Bold;
                this.TapToSelectButton.FontAttributes = FontAttributes.None;
            }
        }

        private void TapToSelectButtonClicked(object sender, EventArgs e)
        {
            if (this.viewModel.SwitchToTapToSelectMode())
            {
                this.TapToSelectButton.FontAttributes = FontAttributes.Bold;
                this.AimToSelectButton.FontAttributes = FontAttributes.None;
            }
        }
    }
}
