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

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Laserline
{
    public class LaserlineViewfinderSettingsViewModel : BaseViewModel, IViewfinderTypeViewModel
    {
        private static readonly SettingsManager settings = SettingsManager.Instance;

        public IList<LaserlineViewfinderStyleItem> AvailableStyles => new List<LaserlineViewfinderStyleItem>
        {
            new LaserlineViewfinderStyleItem(LaserlineViewfinderStyle.Legacy),
            new LaserlineViewfinderStyleItem(LaserlineViewfinderStyle.Animated)
        };

        public LaserlineViewfinderStyleItem CurrentStyle
        {
            get
            {
                return this.AvailableStyles.Where(s => s.Style == settings.LaserlineViewfinderStyle).FirstOrDefault() ?? this.AvailableStyles[0];
            }
            set
            {
                if (value == null) return;
                settings.LaserlineViewfinderStyle = value.Style;
            }
        }

        public FloatWithUnit Width
        {
            get { return settings.LaserlineViewfinderWidth; }
        }

        public IList<LaselinefinderEnabledColorItem> AvailableEnabledColors => new List<LaselinefinderEnabledColorItem> {
            new LaselinefinderEnabledColorItem(LaselinefinderEnabledColorItem.Type.Default),
            new LaselinefinderEnabledColorItem(LaselinefinderEnabledColorItem.Type.Red),
            new LaselinefinderEnabledColorItem(LaselinefinderEnabledColorItem.Type.Blue),
            new LaselinefinderEnabledColorItem(LaselinefinderEnabledColorItem.Type.White),
        };

        public LaselinefinderEnabledColorItem CurrentEnabledColor
        {
            get
            {
                return this.AvailableEnabledColors.Where(c => c.Color == settings.LaserlineViewfinderEnabledColor).FirstOrDefault() ?? this.AvailableEnabledColors[0];
            }
            set
            {
                if (value == null) return;

                settings.LaserlineViewfinderEnabledColor = value.Color;
            }
        }

        public IList<LaselinefinderDisabledColorItem> AvailableDisabledColors => new List<LaselinefinderDisabledColorItem> {
            new LaselinefinderDisabledColorItem(LaselinefinderDisabledColorItem.Type.Default),
            new LaselinefinderDisabledColorItem(LaselinefinderDisabledColorItem.Type.Blue),
            new LaselinefinderDisabledColorItem(LaselinefinderDisabledColorItem.Type.Red),
            new LaselinefinderDisabledColorItem(LaselinefinderDisabledColorItem.Type.Transparent),
        };

        public LaselinefinderDisabledColorItem CurrentDisabledColor
        {
            get
            {
                return this.AvailableDisabledColors.Where(c => c.Color == settings.LaserlineViewfinderDisabledColor).FirstOrDefault() ?? this.AvailableDisabledColors[0];
            }
            set
            {
                if (value == null) return;

                settings.LaserlineViewfinderDisabledColor = value.Color;
            }
        }

        public IViewfinder Create()
        {
            return new LaserlineViewfinder(CurrentStyle.Style)
            {
                EnabledColor = this.CurrentEnabledColor.Color,
                DisabledColor = this.CurrentDisabledColor.Color,
                Width = this.Width
            };
        }
    }

    public class LaserlineViewfinderStyleItem : IEquatable<LaserlineViewfinderStyleItem>
    {
        public LaserlineViewfinderStyleItem(LaserlineViewfinderStyle style)
        {
            Style = style;
        }

        public LaserlineViewfinderStyle Style { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as LaserlineViewfinderStyleItem);
        }

        public bool Equals(LaserlineViewfinderStyleItem other)
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
            return Enum.GetName(typeof(LaserlineViewfinderStyle), this.Style);
        }
    }


    public class LaselinefinderEnabledColorItem : IEquatable<LaselinefinderEnabledColorItem>
    {
        private readonly static Color Default = new LaserlineViewfinder().EnabledColor;

        public LaselinefinderEnabledColorItem(LaselinefinderEnabledColorItem.Type colorType)
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
                case Type.Red:
                    this.Color = Color.Red;
                    break;
                case Type.White:
                    this.Color = Color.White;
                    break;
            }
        }

        public Color Color { get; }

        public LaselinefinderEnabledColorItem.Type ColorType { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as LaselinefinderEnabledColorItem);
        }

        public bool Equals(LaselinefinderEnabledColorItem other)
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
            return Enum.GetName(typeof(LaselinefinderEnabledColorItem.Type), this.ColorType);
        }

        public enum Type
        {
            Default,
            Red,
            Blue,
            White
        }
    }

    public class LaselinefinderDisabledColorItem : IEquatable<LaselinefinderDisabledColorItem>
    {
        private readonly static Color Default = new LaserlineViewfinder().DisabledColor;

        public LaselinefinderDisabledColorItem(LaselinefinderDisabledColorItem.Type colorType)
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
                case Type.Red:
                    this.Color = Color.Red;
                    break;
                case Type.Transparent:
                    this.Color = Color.Transparent;
                    break;
            }
        }

        public Color Color { get; }

        public LaselinefinderDisabledColorItem.Type ColorType { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as LaselinefinderDisabledColorItem);
        }

        public bool Equals(LaselinefinderDisabledColorItem other)
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
            return Enum.GetName(typeof(LaselinefinderDisabledColorItem.Type), this.ColorType);
        }

        public enum Type
        {
            Default,
            Blue,
            Red,
            Transparent
        }
    }
}
