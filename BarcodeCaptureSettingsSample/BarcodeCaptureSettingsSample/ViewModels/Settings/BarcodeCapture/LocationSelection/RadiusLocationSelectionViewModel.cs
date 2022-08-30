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
using System.Collections.Generic;
using BarcodeCaptureSettingsSample.Models;
using BarcodeCaptureSettingsSample.Resources;
using BarcodeCaptureSettingsSample.ViewModels.Settings.Common;
using Scandit.DataCapture.Core.Area.Unified;
using Scandit.DataCapture.Core.Common.Geometry.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.LocationSelection
{
    public class RadiusLocationSelectionViewModel : BaseViewModel, ILocationSelectionItemViewModel, IFloatWithUnitViewModel
    {
        private static readonly SettingsManager settings = SettingsManager.Instance;

        public FloatWithUnit Size
        {
            get
            {
                return new FloatWithUnit(this.Value, this.CurrentMeasureUnit);
            }
        }

        public string Title => AppResources.Size;

        public float Value
        {
            get { return settings.LocationSelectionRadiusValue; }
            set { settings.LocationSelectionRadiusValue = value; }
        }

        public IList<MeasureUnitItem> MeasureUnitSource => new List<MeasureUnitItem>  {
                new MeasureUnitItem(MeasureUnit.Dip, CurrentMeasureUnit == MeasureUnit.Dip),
                new MeasureUnitItem(MeasureUnit.Fraction, CurrentMeasureUnit == MeasureUnit.Fraction),
                new MeasureUnitItem(MeasureUnit.Pixel, CurrentMeasureUnit == MeasureUnit.Pixel)
            };

        public MeasureUnit CurrentMeasureUnit
        {
            get
            {
                return settings.LocationSelectionRadiusMeasureUnit;
            }
            set
            {
                _ = settings.SetLocationSelectionRadiusMeasureUnitAsync(value);
                OnPropertyChanged(nameof(this.MeasureUnitSource));
            }
        }

        public ILocationSelection Build()
        {
            return RadiusLocationSelection.Create(this.Size);
        }
    }
}
