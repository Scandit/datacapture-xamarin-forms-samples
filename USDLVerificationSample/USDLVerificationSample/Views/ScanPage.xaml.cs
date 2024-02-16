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
using USDLVerificationSample.ViewModels;
using Scandit.DataCapture.ID.UI.Unified;

namespace USDLVerificationSample.Views
{
    public partial class ScanPage : ContentPage
    {
        private readonly IdCaptureOverlay overlay;

        public ScanPage()
        {
            this.InitializeComponent();
            this.overlay = IdCaptureOverlay.Create(this.viewModel.IdCapture);
            this.overlay.IdLayoutStyle = IdLayoutStyle.Square;
        }

        public event EventHandler<CapturedIdEventArgs> IdCaptured
        {
            add { this.viewModel.IdCaptured += value; }
            remove { this.viewModel.IdCaptured -= value; }
        }

        public void VerificationChecksRunning()
        {
            this.VerificationCheckLabel.IsVisible = true;
        }

        public void VerificationChecksCompleted()
        {
            this.VerificationCheckLabel.IsVisible = false;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _ = this.viewModel.OnResumeAsync();

            if (this.overlay != null)
            {
                this.dataCaptureView.AddOverlay(this.overlay);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.dataCaptureView.RemoveOverlay(this.overlay);
            this.viewModel.OnSleepAsync();
        }
    }
}
