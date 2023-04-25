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
using Scandit.DataCapture.Barcode.Spark.UI.Unified;
using Xamarin.Forms;

namespace ListBuildingSample.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.SubscribeToViewModelEvents();
        }

        private void SubscribeToViewModelEvents()
        {
            this.viewModel.Sleep += (object sender, EventArgs args) =>
            {
                this.SparkScanView.PauseScanning();
            };

            this.viewModel.ErrorFeedback += (object sender, EventArgs args) =>
            {
                var feedback = new SparkScanViewErrorFeedback(message: "This code should not have been scanned",
                                                              resumeCapturingDelay: TimeSpan.FromSeconds(60));
                this.SparkScanView.EmitFeedback(feedback);
            };

            this.viewModel.SuccessFeedback += (object sender, EventArgs args) =>
            {
                this.SparkScanView.EmitFeedback(new SparkScanViewSuccessFeedback());
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.SparkScanView.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            this.SparkScanView.OnDisappearing();
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            this.viewModel.ClearScannedItems();
        }
    }
}
