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
using System.Linq;
using IdCaptureExtendedSample.Models;
using IdCaptureExtendedSample.ViewModels;
using Scandit.DataCapture.ID.UI.Unified;
using Xamarin.Forms;

namespace IdCaptureExtendedSample.Views
{
    public partial class ScanPage : ContentPage
    {
        private IdCaptureOverlay idCaptureOverlay;

        public event EventHandler<CapturedIdEventArgs> IdCaptured
        {
            add { this.viewModel.IdCaptured += value; }
            remove { this.viewModel.IdCaptured -= value; }
        }

        public ScanPage()
        {
            this.InitializeComponent();
            this.ConfigureIdCaptureOverlay();
            this.UpdateButtonsState(this.viewModel.Mode);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = this.viewModel.OnResumeAsync();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.viewModel.OnSleepAsync();
        }

        private void ButtonClicked(object sender, EventArgs args)
        {
            if (sender is Button button)
            {
                if (Enum.TryParse(button.Text, out Mode mode))
                {
                    this.viewModel.ConfigureIdCapture(mode);
                    this.ConfigureIdCaptureOverlay();
                    this.UpdateButtonsState(mode);
                }
            }
        }

        private void ConfigureIdCaptureOverlay()
        {
            if (this.idCaptureOverlay != null)
            {
                this.dataCaptureView?.RemoveOverlay(this.idCaptureOverlay);
            }

            this.idCaptureOverlay = IdCaptureOverlay.Create(this.viewModel.IdCapture, this.dataCaptureView);
            this.idCaptureOverlay.IdLayoutStyle = IdLayoutStyle.Rounded;
        }

        private void UpdateButtonsState(Mode currentMode)
        {
            this.buttonsLayout.Children.Where(i => i is Button).Cast<Button>().All(button =>
            {
                if (Enum.TryParse(button.Text, out Mode mode))
                {
                    if (mode == currentMode)
                    {
                        button.FontAttributes = FontAttributes.Bold;
                        button.BackgroundColor = Color.White;
                        button.TextColor = Color.Black;
                    }
                    else
                    {
                        button.FontAttributes = FontAttributes.None;
                        button.BackgroundColor = Color.Black;
                        button.TextColor = Color.White;
                    }
                }

                return true;
            });
        }
    }
}
