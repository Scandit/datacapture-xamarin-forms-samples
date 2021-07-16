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

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Logo
{
    public class AnchorOffsetViewModel : BaseViewModel, IFloatWithUnitViewModel
    {
        private readonly Type type;

        private static SettingsManager settings = SettingsManager.Instance;

        public AnchorOffsetViewModel(Type type)
        {
            this.type = type;
        }

        public string Title => Enum.GetName(typeof(Type), type);

        public float Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        private void SetValue(float newValue)
        {
            switch (type)
            {
                case Type.X:
                    settings.AnchorXOffset = new FloatWithUnit(newValue, CurrentMeasureUnit);
                    break;
                case Type.Y:
                    settings.AnchorYOffset = new FloatWithUnit(newValue, CurrentMeasureUnit);
                    break;
            }
        }

        private float GetValue()
        {
            switch (type)
            {
                case Type.X:
                    return settings.AnchorXOffset.Value;
                case Type.Y:
                    return settings.AnchorYOffset.Value;
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
                OnPropertyChanged(nameof(this.MeasureUnitSource));
            }
        }

        private void SetMeasureunit(MeasureUnit newValue)
        {
            switch (type)
            {
                case Type.X:
                    settings.AnchorXOffset = new FloatWithUnit(Value, newValue);
                    break;
                case Type.Y:
                    settings.AnchorYOffset = new FloatWithUnit(Value, newValue);
                    break;
            }
        }

        private MeasureUnit GetMeasureUnit()
        {
            switch (type)
            {
                case Type.X:
                    return settings.AnchorXOffset.Unit;
                case Type.Y:
                    return settings.AnchorYOffset.Unit;
                default:
                    throw new IndexOutOfRangeException($"No measure unit found for {Enum.GetName(typeof(Type), type)}");
            }
        }

        public IList<MeasureUnitItem> MeasureUnitSource => new List<MeasureUnitItem>  {
                new MeasureUnitItem(MeasureUnit.Dip, this.CurrentMeasureUnit == MeasureUnit.Dip),
                new MeasureUnitItem(MeasureUnit.Fraction, this.CurrentMeasureUnit == MeasureUnit.Fraction),
                new MeasureUnitItem(MeasureUnit.Pixel, this.CurrentMeasureUnit == MeasureUnit.Pixel)
            };

        public enum Type
        {
            X,
            Y
        }
    }
}
