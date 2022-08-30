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

using System.Collections.Generic;
using BarcodeCaptureSettingsSample.Models;
using BarcodeCaptureSettingsSample.Resources;
using BarcodeCaptureSettingsSample.ViewModels.Settings.Common;
using Scandit.DataCapture.Core.Common.Geometry.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Laserline
{
    public class LaserlineViewfinderWidthViewModel : BaseViewModel, IFloatWithUnitViewModel
    {
        private static SettingsManager settings = SettingsManager.Instance;

        public string Title => AppResources.Width;

        public float Value
        {
            get { return settings.LaserlineViewfinderWidth.Value; }
            set { settings.LaserlineViewfinderWidth = new FloatWithUnit(value, this.CurrentMeasureUnit); }
        }

        public MeasureUnit CurrentMeasureUnit
        {
            get { return settings.LaserlineViewfinderWidth.Unit; }
            set
            {
                settings.LaserlineViewfinderWidth = new FloatWithUnit(this.Value, value); ;
                OnPropertyChanged(nameof(this.MeasureUnitSource));
            }
        }

        public IList<MeasureUnitItem> MeasureUnitSource => new List<MeasureUnitItem>  {
                new MeasureUnitItem(MeasureUnit.Dip, this.CurrentMeasureUnit == MeasureUnit.Dip),
                new MeasureUnitItem(MeasureUnit.Fraction,this.CurrentMeasureUnit == MeasureUnit.Fraction),
                new MeasureUnitItem(MeasureUnit.Pixel,this. CurrentMeasureUnit == MeasureUnit.Pixel)
            };
    }
}
