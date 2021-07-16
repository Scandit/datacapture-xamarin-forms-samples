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

namespace BarcodeCaptureSettingsSample.Views.Settings.BacodeCapture.LocationSelection
{
    public partial class RectangularLocationSelectionSettingsView : ContentView
    {
        public RectangularLocationSelectionSettingsView()
        {
            InitializeComponent();
        }

        async void WidthAndHeightContainer_WidthTapped(object sender, EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new RectangularLocationSelectionSizeSpecificationViewModels(RectangularLocationSelectionSizeSpecificationViewModels.SizeSpecificationValue.Width)
            };
            await Navigation.PushAsync(page);

        }

        async void WidthAndHeightContainer_HeightTapped(object sender, EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new RectangularLocationSelectionSizeSpecificationViewModels(RectangularLocationSelectionSizeSpecificationViewModels.SizeSpecificationValue.Height)
            };
            await Navigation.PushAsync(page);
        }

        async void WidthAndHeightAspectContainer_WidthTapped(object sender, EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new RectangularLocationSelectionSizeSpecificationViewModels(RectangularLocationSelectionSizeSpecificationViewModels.SizeSpecificationValue.Width)
            };
            await Navigation.PushAsync(page);
        }

        async void HeightAndWidthAspectContainer_HeightTapped(object sender, EventArgs e)
        {
            var page = new FloatWithUnitPage()
            {
                BindingContext = new RectangularLocationSelectionSizeSpecificationViewModels(RectangularLocationSelectionSizeSpecificationViewModels.SizeSpecificationValue.Height)
            };
            await Navigation.PushAsync(page);
        }
    }
}
