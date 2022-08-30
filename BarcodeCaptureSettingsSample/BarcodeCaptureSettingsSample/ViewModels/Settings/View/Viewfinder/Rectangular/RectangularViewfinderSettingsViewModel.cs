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
using System.Linq;
using BarcodeCaptureSettingsSample.Models;
using Scandit.DataCapture.Core.Common.Geometry.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Rectangular
{
    public class RectangularViewfinderSettingsViewModel : BaseViewModel, IViewfinderTypeViewModel
    {
        private static SettingsManager settings = SettingsManager.Instance;

        public IList<RectangularViewfinderStyleItem> AvailableStyles => new List<RectangularViewfinderStyleItem>
        {
            new RectangularViewfinderStyleItem(RectangularViewfinderStyle.Legacy),
            new RectangularViewfinderStyleItem(RectangularViewfinderStyle.Rounded),
            new RectangularViewfinderStyleItem(RectangularViewfinderStyle.Square)
        };

        public RectangularViewfinderStyleItem CurrentStyle
        {
            get
            {
                return this.AvailableStyles.Where(s => s.Style == settings.RectangularViewfinderStyle).FirstOrDefault() ?? this.AvailableStyles[0];
            }
            set
            {
                if (value == null) return;
                settings.RectangularViewfinderStyle = value.Style;
            }
        }

        public IList<RectangularViewfinderLineStyleItem> AvailableLineStyles => new List<RectangularViewfinderLineStyleItem> {
            new RectangularViewfinderLineStyleItem(RectangularViewfinderLineStyle.Light),
            new RectangularViewfinderLineStyleItem(RectangularViewfinderLineStyle.Bold)
        };


        public RectangularViewfinderLineStyleItem CurrentLineStyle
        {
            get
            {
                return this.AvailableLineStyles.Where(s => s.Style == settings.RectangularViewfinderLineStyle).FirstOrDefault() ?? this.AvailableLineStyles[0];
            }
            set
            {
                if (value == null) return;
                settings.RectangularViewfinderLineStyle = value.Style;
            }
        }

        public float Dimming
        {
            get { return settings.RectangularViewfinderDimming; }
            set
            {
                if (value > 1.0f)
                    value = 1.0f;

                settings.RectangularViewfinderDimming = value;
            }
        }

        public IList<RectangularViewfinderColorItem> AvailableColors => new List<RectangularViewfinderColorItem> {
            new RectangularViewfinderColorItem(RectangularViewfinderColorItem.Type.Default),
            new RectangularViewfinderColorItem(RectangularViewfinderColorItem.Type.Blue),
             new RectangularViewfinderColorItem(RectangularViewfinderColorItem.Type.Black)
        };

        public RectangularViewfinderColorItem CurrentColor
        {
            get
            {
                return this.AvailableColors.Where(c => c.Color == settings.RectangularViewfinderColor).FirstOrDefault() ?? this.AvailableColors[0];
            }
            set
            {
                if (value == null) return;

                settings.RectangularViewfinderColor = value.Color;
            }
        }

        public IList<RectangularViewfinderDisabledColorItem> AvailableDisabledColors => new List<RectangularViewfinderDisabledColorItem> {
            new RectangularViewfinderDisabledColorItem(RectangularViewfinderDisabledColorItem.Type.Default),
            new RectangularViewfinderDisabledColorItem(RectangularViewfinderDisabledColorItem.Type.Black),
             new RectangularViewfinderDisabledColorItem(RectangularViewfinderDisabledColorItem.Type.White)
        };

        public RectangularViewfinderDisabledColorItem CurrentDisabledColor
        {
            get
            {
                return this.AvailableDisabledColors.Where(c => c.DisabledColor == settings.RectangularViewfinderDisabledColor).FirstOrDefault() ?? this.AvailableDisabledColors[0];
            }
            set
            {
                if (value == null) return;

                settings.RectangularViewfinderDisabledColor = value.DisabledColor;
            }
        }

        public bool Animation
        {
            get
            {
                return settings.RectangularViewfinderAnimationEnabled;
            }
            set
            {
                settings.RectangularViewfinderAnimationEnabled = value;
                OnPropertyChanged(nameof(this.Animation));
            }
        }

        public bool Looping
        {
            get
            {
                return settings.RectangularViewfinderAnimationLoopingEnabled;
            }
            set
            {
                settings.RectangularViewfinderAnimationLoopingEnabled = value;
            }
        }

        public IList<SizeSpecification> AvailableSizeSpecifications => new List<SizeSpecification> {
            SizeSpecification.WidthAndHeight,
            SizeSpecification.WidthAndHeightAspect,
            SizeSpecification.HeightAndWidthAspect,
            SizeSpecification.ShorterDimensionAndAspect
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
                OnPropertyChanged(nameof(this.IsSizeSpecificationShorterDimensionAndAspect));
            }
        }

        public FloatWithUnit Width
        {
            get
            {
                return settings.RectangularViewfinderWidth;
            }
        }

        public FloatWithUnit Height
        {
            get
            {
                return settings.RectangularViewfinderHeight;
            }
        }

        public float HeightAspect
        {
            get
            {
                return settings.RectangularViewfinderHeightAspect;
            }
            set
            {
                settings.RectangularViewfinderHeightAspect = value;
            }
        }

        public float WidthAspect
        {
            get
            {
                return settings.RectangularViewfinderWidthAspect;
            }
            set
            {
                settings.RectangularViewfinderWidthAspect = value;
            }
        }

        public FloatWithUnit ShorterDimension
        {
            get
            {
                return settings.RectangularViewfinderShorterDimension;
            }
        }

        public float LongerDimensionAspect
        {
            get
            {
                return settings.RectangularViewfinderLongerDimensionAspect;
            }
            set
            {
                settings.RectangularViewfinderLongerDimensionAspect = value;
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

        public bool IsSizeSpecificationShorterDimensionAndAspect
        {
            get
            {
                return this.CurrentSizeSpecification.Equals(SizeSpecification.ShorterDimensionAndAspect);
            }
        }


        public IViewfinder Create()
        {
            RectangularViewfinder viewfinder = new RectangularViewfinder(this.CurrentStyle.Style, this.CurrentLineStyle.Style)
            {
                Color = this.CurrentColor.Color,
                DisabledColor = this.CurrentDisabledColor.DisabledColor,
                Dimming = this.Dimming,
                Animation = this.Animation ? new RectangularViewfinderAnimation(this.Looping) : null,
            };

            if (IsSizeSpecificationWidthAndHeight)
            {
                viewfinder.SetSize(new SizeWithUnit(this.Width, this.Height));
            }
            else if (IsSizeSpecificationWidthAndHeightAspect)
            {
                viewfinder.SetWidthAndAspectRatio(this.Width, this.HeightAspect);
            }
            else if (IsSizeSpecificationHeightAndWidthAspect)
            {
                viewfinder.SetHeightAndAspectRatio(this.Height, this.WidthAspect);
            }
            else if (IsSizeSpecificationShorterDimensionAndAspect)
            {
                viewfinder.SetShorterDimensionAndAspectRatio(this.ShorterDimension.Value, this.LongerDimensionAspect);
            }

            return viewfinder;
        }
    }

    public class RectangularViewfinderStyleItem : IEquatable<RectangularViewfinderStyleItem>
    {
        public RectangularViewfinderStyleItem(RectangularViewfinderStyle style)
        {
            Style = style;
        }

        public RectangularViewfinderStyle Style { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as RectangularViewfinderStyleItem);
        }

        public bool Equals(RectangularViewfinderStyleItem other)
        {
            return other != null &&
               this.Style == other.Style;
        }

        public override int GetHashCode()
        {
            return this.Style.GetHashCode();

        }

        public override string ToString()
        {
            return Enum.GetName(typeof(RectangularViewfinderStyle), this.Style);
        }
    }

    public class RectangularViewfinderLineStyleItem : IEquatable<RectangularViewfinderLineStyleItem>
    {
        public RectangularViewfinderLineStyleItem(RectangularViewfinderLineStyle style)
        {
            Style = style;
        }

        public RectangularViewfinderLineStyle Style { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as RectangularViewfinderLineStyleItem);
        }

        public bool Equals(RectangularViewfinderLineStyleItem other)
        {
            return other != null &&
                this.Style == other.Style;
        }

        public override int GetHashCode()
        {
            return this.Style.GetHashCode();
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(RectangularViewfinderLineStyle), this.Style);
        }
    }

    public class RectangularViewfinderColorItem : IEquatable<RectangularViewfinderColorItem>
    {
        private readonly static Color Default = new RectangularViewfinder().Color;

        public RectangularViewfinderColorItem(RectangularViewfinderColorItem.Type colorType)
        {
            this.ColorType = colorType;

            switch (ColorType)
            {
                case Type.Default:
                    this.Color = Default;
                    break;
                case Type.Blue:
                    this.Color = Color.Blue;
                    break;
                case Type.Black:
                    this.Color = Color.Black;
                    break;
            }
        }

        public Color Color { get; }

        public RectangularViewfinderColorItem.Type ColorType { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as RectangularViewfinderColorItem);
        }

        public bool Equals(RectangularViewfinderColorItem other)
        {
            return other != null
                && this.ColorType == other.ColorType;
        }

        public override int GetHashCode()
        {
            return this.ColorType.GetHashCode();
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(RectangularViewfinderColorItem.Type), this.ColorType);
        }

        public enum Type
        {
            Default,
            Blue,
            Black
        }
    }

    public class RectangularViewfinderDisabledColorItem : IEquatable<RectangularViewfinderDisabledColorItem>
    {
        private readonly static Color Default = new RectangularViewfinder().DisabledColor;

        public RectangularViewfinderDisabledColorItem(RectangularViewfinderDisabledColorItem.Type colorType)
        {
            this.ColorType = colorType;

            switch (ColorType)
            {
                case Type.Default:
                    this.DisabledColor = Default;
                    break;
                case Type.Black:
                    this.DisabledColor = Color.Black;
                    break;
                case Type.White:
                    this.DisabledColor = Color.White;
                    break;
            }
        }

        public Color DisabledColor { get; }

        public RectangularViewfinderDisabledColorItem.Type ColorType { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as RectangularViewfinderDisabledColorItem);
        }

        public bool Equals(RectangularViewfinderDisabledColorItem other)
        {
            return other != null
                 && this.ColorType == other.ColorType;
        }

        public override int GetHashCode()
        {
            return this.ColorType.GetHashCode();
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(RectangularViewfinderDisabledColorItem.Type), this.ColorType);
        }

        public enum Type
        {
            Default,
            Black,
            White
        }
    }
}
