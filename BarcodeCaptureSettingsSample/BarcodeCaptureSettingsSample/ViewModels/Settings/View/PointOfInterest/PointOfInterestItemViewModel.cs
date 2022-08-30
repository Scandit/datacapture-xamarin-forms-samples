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
using BarcodeCaptureSettingsSample.ViewModels.Settings.Common;
using Scandit.DataCapture.Core.Common.Geometry.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.PointOfInterest
{
    public class PointOfInterestItemViewModel : BaseViewModel, IFloatWithUnitViewModel
    {
        private static SettingsManager settings = SettingsManager.Instance;
        private readonly PointOfInterestItem pointOfInterestItem;

        public PointOfInterestItemViewModel(PointOfInterestItem item)
        {
            pointOfInterestItem = item;
        }

        public string Title => Enum.GetName(typeof(PointOfInterestItem.Type), pointOfInterestItem.ItemType);

        public float Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        private void SetValue(float newValue)
        {
            switch (pointOfInterestItem.ItemType)
            {
                case PointOfInterestItem.Type.X:
                    settings.PointOfInterest = new PointWithUnit(new FloatWithUnit(newValue, settings.PointOfInterest.X.Unit), settings.PointOfInterest.Y);
                    break;
                case PointOfInterestItem.Type.Y:
                    settings.PointOfInterest = new PointWithUnit(settings.PointOfInterest.X, new FloatWithUnit(newValue, settings.PointOfInterest.Y.Unit));
                    break;
            }
        }

        private float GetValue()
        {
            switch (pointOfInterestItem.ItemType)
            {
                case PointOfInterestItem.Type.X:
                    return settings.PointOfInterest.X.Value;
                case PointOfInterestItem.Type.Y:
                    return settings.PointOfInterest.Y.Value;
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
            switch (pointOfInterestItem.ItemType)
            {
                case PointOfInterestItem.Type.X:
                    settings.PointOfInterest = new PointWithUnit(new FloatWithUnit(settings.PointOfInterest.X.Value, newValue), settings.PointOfInterest.Y);
                    break;
                case PointOfInterestItem.Type.Y:
                    settings.PointOfInterest = new PointWithUnit(settings.PointOfInterest.X, new FloatWithUnit(settings.PointOfInterest.Y.Value, newValue));
                    break;
            }
        }

        private MeasureUnit GetMeasureUnit()
        {
            switch (pointOfInterestItem.ItemType)
            {
                case PointOfInterestItem.Type.X:
                    return settings.PointOfInterest.X.Unit;
                case PointOfInterestItem.Type.Y:
                    return settings.PointOfInterest.Y.Unit;
                default:
                    throw new IndexOutOfRangeException($"No measure unit found for {Enum.GetName(typeof(PointOfInterestItem.Type), pointOfInterestItem.ItemType)}");
            }
        }

        public IList<MeasureUnitItem> MeasureUnitSource => new List<MeasureUnitItem>  {
                new MeasureUnitItem(MeasureUnit.Dip, CurrentMeasureUnit == MeasureUnit.Dip),
                new MeasureUnitItem(MeasureUnit.Fraction, CurrentMeasureUnit == MeasureUnit.Fraction),
                new MeasureUnitItem(MeasureUnit.Pixel, CurrentMeasureUnit == MeasureUnit.Pixel)
            };
    }
}
