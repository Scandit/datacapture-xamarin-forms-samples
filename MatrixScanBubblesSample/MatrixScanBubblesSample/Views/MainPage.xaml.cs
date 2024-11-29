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
using MatrixScanBubblesSample.ViewModels;
using Scandit.DataCapture.Barcode.Batch.Data.Unified;
using Scandit.DataCapture.Core.Common.Geometry.Unified;
using Xamarin.Forms;

namespace MatrixScanBubblesSample.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly float barcodeToScreenTresholdRatio = 0.1f;
        private readonly MainPageViewModel viewModel;
        private bool freezeEnabled = true;

        public MainPage()
        {
            this.InitializeComponent();

            this.viewModel = this.BindingContext as MainPageViewModel;
            this.viewModel.ShouldHideOverlay = (TrackedBarcode trackedBarcode) =>
            {
                double GetWidth(Quadrilateral barcodeLocation)
                {
                    return Math.Max(barcodeLocation.BottomRight.X - barcodeLocation.BottomLeft.X, barcodeLocation.TopRight.X - barcodeLocation.TopLeft.X);
                }

                // If the barcode is wider than the desired percent of the data capture view's width,
                // show it to the user.
                var width = GetWidth(this.DataCaptureView.MapFrameQuadrilateralToView(trackedBarcode.Location));
                return (width / this.DataCaptureView.Width) <= this.barcodeToScreenTresholdRatio;
            };
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

        private void FreezeButtonClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button)
            {
                if (this.freezeEnabled)
                {
                    button.Source = ImageSource.FromFile("freeze_disabled.png");
                }
                else
                {
                    button.Source = ImageSource.FromFile("freeze_enabled.png");
                }

                this.freezeEnabled = !this.freezeEnabled;
            }
        }
    }
}
