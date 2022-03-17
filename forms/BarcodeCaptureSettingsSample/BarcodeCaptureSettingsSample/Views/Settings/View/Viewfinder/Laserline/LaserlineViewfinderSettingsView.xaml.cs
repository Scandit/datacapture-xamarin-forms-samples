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
using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Laserline;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Common;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Viewfinder.Laserline
{
    public partial class LaserlineViewfinderSettingsView : ContentView
    {
        public LaserlineViewfinderSettingsView()
        {
            InitializeComponent();
        }

        public event EventHandler Dissapering;

        async void OnWidthTapped(System.Object sender, System.EventArgs e)
        {
            var page = new FloatWithUnitPage
            {
                BindingContext = new LaserlineViewfinderWidthViewModel()
            };

            page.Disappearing += PageDisappearing;
            await Navigation.PushAsync(page);
        }

        private void PageDisappearing(object sender, EventArgs args)
        {
            this.Dissapering?.Invoke(sender, args);
        }
    }
}
