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
using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Rectangular;
using BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Common;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Viewfinder.Rectangular
{
    public partial class RectangularViewfinderSettingsView : ContentView
    {
        public RectangularViewfinderSettingsView()
        {
            InitializeComponent();
        }

        public event EventHandler Dissapering;

        async void WidthAndHeightContainer_WidthTapped(System.Object sender, System.EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new SizeSpecificationWidthAndHeightViewModel(SizeSpecificationWidthAndHeightViewModel.SizeSpecificationValue.Width)
            };

            page.Disappearing += PageDisappearing;
            await Navigation.PushAsync(page);

        }

        async void WidthAndHeightContainer_HeightTapped(System.Object sender, System.EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new SizeSpecificationWidthAndHeightViewModel(SizeSpecificationWidthAndHeightViewModel.SizeSpecificationValue.Height)
            };

            page.Disappearing += PageDisappearing;
            await Navigation.PushAsync(page);
        }

        async void WidthAndHeightAspectContainer_WidthTapped(System.Object sender, System.EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new SizeSpecificationWidthAndHeightViewModel(SizeSpecificationWidthAndHeightViewModel.SizeSpecificationValue.Width)
            };

            page.Disappearing += PageDisappearing;
            await Navigation.PushAsync(page);
        }

        async void HeightAndWidthAspectContainer_HeightTapped(System.Object sender, System.EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new SizeSpecificationWidthAndHeightViewModel(SizeSpecificationWidthAndHeightViewModel.SizeSpecificationValue.Height)
            };

            page.Disappearing += PageDisappearing;
            await Navigation.PushAsync(page);
        }

        async void ShorterDimensionLongerAspectContainer_ShorterDimensionTapped(System.Object sender, System.EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new SizeSpecificationShorterDimensionViewModel()
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
