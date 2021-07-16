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
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Aimer
{
    public class AimerViewfinderSettingsViewModel : BaseViewModel, IViewfinderTypeViewModel
    {
        private static readonly SettingsManager settings = SettingsManager.Instance;

        public IList<AimerViewfinderFrameColorItem> AvailableFrameColors => new List<AimerViewfinderFrameColorItem> {
            new AimerViewfinderFrameColorItem(AimerViewfinderFrameColorItem.Type.Default),
            new AimerViewfinderFrameColorItem(AimerViewfinderFrameColorItem.Type.Blue),
             new AimerViewfinderFrameColorItem(AimerViewfinderFrameColorItem.Type.Red)
        };

        public AimerViewfinderFrameColorItem CurrentFrameColor
        {
            get
            {
                return this.AvailableFrameColors.Where(c => c.FrameColor == settings.AimerViewfinderFrameColor).FirstOrDefault() ?? this.AvailableFrameColors[0];
            }
            set
            {
                if (value == null) return;

                settings.AimerViewfinderFrameColor = value.FrameColor;
            }
        }

        public IList<AimerViewfinderDotColorItem> AvailableDotColors => new List<AimerViewfinderDotColorItem> {
            new AimerViewfinderDotColorItem(AimerViewfinderDotColorItem.Type.Default),
            new AimerViewfinderDotColorItem(AimerViewfinderDotColorItem.Type.Blue),
             new AimerViewfinderDotColorItem(AimerViewfinderDotColorItem.Type.Red)
        };

        public AimerViewfinderDotColorItem CurrentDotColor
        {
            get
            {
                return this.AvailableDotColors.Where(c => c.DotColor == settings.AimerViewfinderDotColor).FirstOrDefault() ?? this.AvailableDotColors[0];
            }
            set
            {
                if (value == null) return;

                settings.AimerViewfinderDotColor = value.DotColor;
            }
        }

        public IViewfinder Create()
        {
            return new AimerViewfinder
            {
                DotColor = this.CurrentDotColor.DotColor,
                FrameColor = this.CurrentFrameColor.FrameColor
            };
        }
    }

    public class AimerViewfinderFrameColorItem : IEquatable<AimerViewfinderFrameColorItem>
    {
        private readonly static Color Default = new AimerViewfinder().FrameColor;

        public AimerViewfinderFrameColorItem(AimerViewfinderFrameColorItem.Type colorType)
        {
            this.ColorType = colorType;

            switch (ColorType)
            {
                case Type.Default:
                    this.FrameColor = Default;
                    break;
                case Type.Blue:
                    this.FrameColor = Color.Blue;
                    break;
                case Type.Red:
                    this.FrameColor = Color.Red;
                    break;
            }
        }

        public Color FrameColor { get; }

        public AimerViewfinderFrameColorItem.Type ColorType { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as AimerViewfinderFrameColorItem);
        }

        public bool Equals(AimerViewfinderFrameColorItem other)
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
            return Enum.GetName(typeof(AimerViewfinderFrameColorItem.Type), this.ColorType);
        }

        public enum Type
        {
            Default,
            Blue,
            Red
        }
    }


    public class AimerViewfinderDotColorItem : IEquatable<AimerViewfinderDotColorItem>
    {
        private readonly static Color Default = new AimerViewfinder().FrameColor;

        public AimerViewfinderDotColorItem(AimerViewfinderDotColorItem.Type colorType)
        {
            this.ColorType = colorType;

            switch (ColorType)
            {
                case Type.Default:
                    this.DotColor = Default;
                    break;
                case Type.Blue:
                    this.DotColor = Color.Blue;
                    break;
                case Type.Red:
                    this.DotColor = Color.Red;
                    break;
            }
        }

        public Color DotColor { get; }

        public AimerViewfinderDotColorItem.Type ColorType { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as AimerViewfinderDotColorItem);
        }

        public bool Equals(AimerViewfinderDotColorItem other)
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
            return Enum.GetName(typeof(AimerViewfinderDotColorItem.Type), this.ColorType);
        }

        public enum Type
        {
            Default,
            Blue,
            Red
        }
    }
}
