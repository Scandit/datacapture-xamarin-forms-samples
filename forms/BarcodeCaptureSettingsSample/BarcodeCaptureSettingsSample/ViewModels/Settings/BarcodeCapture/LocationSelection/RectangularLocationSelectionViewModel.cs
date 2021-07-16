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
using Scandit.DataCapture.Core.Area.Unified;
using Scandit.DataCapture.Core.Common.Geometry.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.LocationSelection
{
    public class RectangularLocationSelectionViewModel : BaseViewModel, ILocationSelectionItemViewModel
    {
        private static readonly SettingsManager settings = SettingsManager.Instance;

        public IList<SizeSpecification> AvailableSizeSpecifications => new List<SizeSpecification> {
            SizeSpecification.WidthAndHeight,
            SizeSpecification.WidthAndHeightAspect,
            SizeSpecification.HeightAndWidthAspect
        };

        public SizeSpecification CurrentSizeSpecification
        {
            get { return settings.RectangularViewfinderSizeSpecification; }
            set
            {
                settings.RectangularViewfinderSizeSpecification = value;
                OnPropertyChanged(nameof(this.IsSizeSpecificationWidthAndHeight));
                OnPropertyChanged(nameof(this.IsSizeSpecificationWidthAndHeightAspect));
                OnPropertyChanged(nameof(this.IsSizeSpecificationHeightAndWidthAspect));
            }
        }

        public FloatWithUnit Width
        {
            get
            {
                return settings.LocationSelectionRectangularWidth;
            }
        }

        public FloatWithUnit Height
        {
            get
            {
                return settings.LocationSelectionRectangularHeight;
            }
        }

        public float HeightAspect
        {
            get
            {
                return settings.LocationSelectionRectangularHeightAspect;
            }
            set
            {
                settings.LocationSelectionRectangularHeightAspect = value;
            }
        }

        public float WidthAspect
        {
            get
            {
                return settings.LocationSelectionRectangularWidthAspect;
            }
            set
            {
                settings.LocationSelectionRectangularWidthAspect = value;
            }
        }

        public FloatWithUnit ShorterDimension
        {
            get
            {
                return settings.LocationSelectionRectangularShorterDimension;
            }
        }

        public float LongerDimensionAspect
        {
            get
            {
                return settings.LocationSelectionRectangularLongerDimensionAspect;
            }
            set
            {
                settings.LocationSelectionRectangularLongerDimensionAspect = value;
            }
        }

        public bool IsSizeSpecificationWidthAndHeight
        {
            get
            {
                return this.CurrentSizeSpecification.Equals(SizeSpecification.WidthAndHeight);
            }
        }

        public bool IsSizeSpecificationWidthAndHeightAspect
        {
            get
            {
                return this.CurrentSizeSpecification.Equals(SizeSpecification.WidthAndHeightAspect);
            }
        }

        public bool IsSizeSpecificationHeightAndWidthAspect
        {
            get
            {
                return this.CurrentSizeSpecification.Equals(SizeSpecification.HeightAndWidthAspect);
            }
        }

        public ILocationSelection Build()
        {
            if (this.IsSizeSpecificationWidthAndHeight)
            {
                return RectangularLocationSelection.Create(new SizeWithUnit(Width, Height));
            }
            else if (this.IsSizeSpecificationHeightAndWidthAspect)
            {
                return RectangularLocationSelection.CreateWithHeightAndAspectRatio(Height, WidthAspect);
            }
            else if (this.IsSizeSpecificationWidthAndHeightAspect)
            {
                return RectangularLocationSelection.CreateWithWidthAndAspectRatio(Width, HeightAspect);
            }
            return null;
        }
    }
}
