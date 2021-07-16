
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

using BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.Feedback;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.BacodeCapture.Feedback
{
    public partial class FeedbackSettingsPage : ContentPage
    {
        private FeedbackSettingsViewModel viewModel;

        public FeedbackSettingsPage()
        {
            InitializeComponent();

            this.viewModel = BindingContext as FeedbackSettingsViewModel;
        }

        void SoundSwitch_Tapped(object sender, System.EventArgs e)
        {
            this.viewModel.IsSoundEnabled = !this.viewModel.IsSoundEnabled;
        }

        void VibrationSwitch_Tapped(object sender, System.EventArgs e)
        {
            this.viewModel.IsVibrationEnabled = !this.viewModel.IsVibrationEnabled;
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
