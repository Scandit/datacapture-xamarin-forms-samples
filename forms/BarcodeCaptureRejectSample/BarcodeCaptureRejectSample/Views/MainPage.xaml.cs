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

using BarcodeCaptureRejectSample.ViewModels;
using ScanditBrush = Scandit.DataCapture.Core.UI.Style.Unified;
using Xamarin.Forms;
using Scandit.DataCapture.Barcode.UI.Unified;
using System;

namespace BarcodeCaptureRejectSample.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            this.viewModel = this.BindingContext as MainPageViewModel;

            this.viewModel.AcceptedCode += (object sender, EventArgs e) =>
            {
                // If the code is accepted, we want to make sure to use
                // the default brush to highlight the code.
                this.Overlay.Brush = BarcodeCaptureOverlay.DefaultBrush;
            };

            this.viewModel.RejectedCode += (object sender, EventArgs e) =>
            {
                // If the code is rejected we temporarily change the brush 
                // used to highlight recognized barcodes, to a transparent brush.
                this.Overlay.Brush = ScanditBrush.Brush.TransparentBrush;
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
    }
}
