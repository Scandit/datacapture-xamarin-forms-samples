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
using Scandit.DataCapture.Core.Common.Geometry.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Rectangular
{
    public class RectangularLocationSelectionSizeSpecificationViewModels : BaseViewModel, IFloatWithUnitViewModel
    {
        private static SettingsManager settings = SettingsManager.Instance;

        private readonly SizeSpecificationValue sizeSpecificationValue;

        public RectangularLocationSelectionSizeSpecificationViewModels(SizeSpecificationValue sizeSpecificationValue)
        {
            this.sizeSpecificationValue = sizeSpecificationValue;
        }

        public string Title => Enum.GetName(typeof(SizeSpecificationValue), sizeSpecificationValue);

        public float Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        private void SetValue(float newValue)
        {
            switch (sizeSpecificationValue)
            {
                case SizeSpecificationValue.Width:
                    settings.LocationSelectionRectangularWidth = new FloatWithUnit(newValue, CurrentMeasureUnit);
                    break;
                case SizeSpecificationValue.Height:
                    settings.LocationSelectionRectangularHeight = new FloatWithUnit(newValue, CurrentMeasureUnit);
                    break;
            }
        }

        private float GetValue()
        {
            switch (sizeSpecificationValue)
            {
                case SizeSpecificationValue.Width:
                    return settings.LocationSelectionRectangularWidth.Value;
                case SizeSpecificationValue.Height:
                    return settings.LocationSelectionRectangularHeight.Value;
                default:
                    return 0;
            }
        }

        public MeasureUnit CurrentMeasureUnit
        {
            get { return GetMeasureUnit(); }
            set
            {
                SetMeasureunit(value);
                OnPropertyChanged(nameof(MeasureUnitSource));
            }
        }

        private void SetMeasureunit(MeasureUnit newValue)
        {
            switch (sizeSpecificationValue)
            {
                case SizeSpecificationValue.Width:
                    settings.LocationSelectionRectangularWidth = new FloatWithUnit(Value, newValue);
                    break;
                case SizeSpecificationValue.Height:
                    settings.LocationSelectionRectangularHeight = new FloatWithUnit(Value, newValue);
                    break;
            }
        }

        private MeasureUnit GetMeasureUnit()
        {
            switch (sizeSpecificationValue)
            {
                case SizeSpecificationValue.Width:
                    return settings.LocationSelectionRectangularWidth.Unit;
                case SizeSpecificationValue.Height:
                    return settings.LocationSelectionRectangularHeight.Unit;
                default:
                    throw new IndexOutOfRangeException($"No measure unit found for {Enum.GetName(typeof(SizeSpecificationWidthAndHeightViewModel.SizeSpecificationValue), sizeSpecificationValue)}");
            }
        }

        public IList<MeasureUnitItem> MeasureUnitSource => new List<MeasureUnitItem>  {
                new MeasureUnitItem(MeasureUnit.Dip, CurrentMeasureUnit == MeasureUnit.Dip),
                new MeasureUnitItem(MeasureUnit.Fraction, CurrentMeasureUnit == MeasureUnit.Fraction),
                new MeasureUnitItem(MeasureUnit.Pixel, CurrentMeasureUnit == MeasureUnit.Pixel)
            };

        public enum SizeSpecificationValue
        {
            Width,
            Height
        }
    }
}