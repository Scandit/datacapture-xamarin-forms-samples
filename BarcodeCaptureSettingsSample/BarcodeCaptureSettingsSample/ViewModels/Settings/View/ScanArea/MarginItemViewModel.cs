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

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.ScanArea
{
    public class MarginItemViewModel : BaseViewModel, IFloatWithUnitViewModel
    {
        private readonly MarginItem marginItem;

        private static SettingsManager settings = SettingsManager.Instance;

        public MarginItemViewModel(MarginItem marginItem)
        {
            this.marginItem = marginItem;
        }

        public string Title => Enum.GetName(typeof(MarginItem.Type), marginItem.MarginItemType);

        public float Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        private void SetValue(float newValue)
        {
            switch (marginItem.MarginItemType)
            {
                case MarginItem.Type.Top:
                    settings.SetScanAreaTopMargin(new FloatWithUnit(newValue, this.CurrentMeasureUnit));
                    break;
                case MarginItem.Type.Right:
                    settings.SetScanAreaRightMargin(new FloatWithUnit(newValue, this.CurrentMeasureUnit));
                    break;
                case MarginItem.Type.Bottom:
                    settings.SetScanAreaBottomMargin(new FloatWithUnit(newValue, this.CurrentMeasureUnit));
                    break;
                case MarginItem.Type.Left:
                    settings.SetScanAreaLeftMargin(new FloatWithUnit(newValue, this.CurrentMeasureUnit));
                    break;
            }
        }

        private float GetValue()
        {
            switch (marginItem.MarginItemType)
            {
                case MarginItem.Type.Top:
                    return settings.ScanAreaMargins.Top.Value;
                case MarginItem.Type.Right:
                    return settings.ScanAreaMargins.Right.Value;
                case MarginItem.Type.Bottom:
                    return settings.ScanAreaMargins.Bottom.Value;
                case MarginItem.Type.Left:
                    return settings.ScanAreaMargins.Left.Value;
                default:
                    return 0f;
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
            switch (marginItem.MarginItemType)
            {
                case MarginItem.Type.Top:
                    settings.SetScanAreaTopMargin(new FloatWithUnit(settings.ScanAreaMargins.Top.Value, newValue));
                    break;
                case MarginItem.Type.Right:
                    settings.SetScanAreaRightMargin(new FloatWithUnit(settings.ScanAreaMargins.Right.Value, newValue));
                    break;
                case MarginItem.Type.Bottom:
                    settings.SetScanAreaBottomMargin(new FloatWithUnit(settings.ScanAreaMargins.Bottom.Value, newValue));
                    break;
                case MarginItem.Type.Left:
                    settings.SetScanAreaLeftMargin(new FloatWithUnit(settings.ScanAreaMargins.Left.Value, newValue));
                    break;
            }
        }

        private MeasureUnit GetMeasureUnit()
        {
            switch (marginItem.MarginItemType)
            {
                case MarginItem.Type.Top:
                    return settings.ScanAreaMargins.Top.Unit;
                case MarginItem.Type.Right:
                    return settings.ScanAreaMargins.Right.Unit;
                case MarginItem.Type.Bottom:
                    return settings.ScanAreaMargins.Bottom.Unit;
                case MarginItem.Type.Left:
                    return settings.ScanAreaMargins.Left.Unit;
                default:
                    throw new IndexOutOfRangeException($"No maring item found for {Enum.GetName(typeof(MarginItem.Type), marginItem.MarginItemType)}");
            }
        }

        public IList<MeasureUnitItem> MeasureUnitSource
        {
            get
            {
                return new List<MeasureUnitItem>  {
                    new MeasureUnitItem(MeasureUnit.Dip, this.CurrentMeasureUnit == MeasureUnit.Dip),
                    new MeasureUnitItem(MeasureUnit.Fraction, this.CurrentMeasureUnit == MeasureUnit.Fraction),
                    new MeasureUnitItem(MeasureUnit.Pixel, this.CurrentMeasureUnit == MeasureUnit.Pixel)
                };
            }
        }

    }
}
