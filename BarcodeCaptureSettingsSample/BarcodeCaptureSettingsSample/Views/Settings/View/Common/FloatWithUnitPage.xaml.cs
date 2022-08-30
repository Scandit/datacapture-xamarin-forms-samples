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

using System.Linq;
using BarcodeCaptureSettingsSample.ViewModels.Settings.Common;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.ViewSettings.Common
{
    public partial class FloatWithUnitPage : ContentPage
    {
        public FloatWithUnitPage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null && !(BindingContext is IFloatWithUnitViewModel))
                throw new System.ArgumentException($"Invalid {nameof(BindingContext)} set. It must be of type {nameof(IFloatWithUnitViewModel)}");
        }

        private void MeasureUnitList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            (BindingContext as IFloatWithUnitViewModel).CurrentMeasureUnit = (e.Item as MeasureUnitItem).MeasureUnit;
        }

        private async void Title_Tapped(object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private void EntryTextChanged(object sender, TextChangedEventArgs e)
        {
            // Simple text input validation to avoid entries with multiply dots entered.
            if (sender is Entry value && e.NewTextValue.Count(c => c == '.') > 1)
            {
                value.Text = e.OldTextValue;
            }
        }
    }
}
