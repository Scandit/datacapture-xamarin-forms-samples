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
using System.Linq;
using System.Collections.Generic;
using BarcodeCaptureSettingsSample.Models;
using Scandit.DataCapture.Core.Common.Geometry.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Logo
{
    public class LogoSettingsViewModel : BaseViewModel
    {
        private readonly static SettingsManager settings = SettingsManager.Instance;

        public FloatWithUnit AnchorXOffset
        {
            get
            {
                return settings.AnchorXOffset;
            }
            set
            {
                settings.AnchorXOffset = value;
            }
        }

        public FloatWithUnit AnchorYOffset
        {
            get
            {
                return settings.AnchorYOffset;
            }
            set
            {
                settings.AnchorYOffset = value;
            }
        }

        public IList<LogoAnchorItem> AvailableAnchors =>
            Enum.GetValues(typeof(Anchor)).OfType<Anchor>().Select(anchor => new LogoAnchorItem(anchor)).ToList();

        public LogoAnchorItem CurrentAnchor
        {
            get
            {
                return this.AvailableAnchors.FirstOrDefault(s => s.Anchor == settings.LogoAnchor) ?? this.AvailableAnchors.First();
            }
            set
            {
                if (value == null) return;
                settings.LogoAnchor = value.Anchor;
            }
        }

        public void OnResume()
        {
            OnPropertyChanged(nameof(AnchorXOffset));
            OnPropertyChanged(nameof(AnchorYOffset));
        }
    }

    public class LogoAnchorItem : IEquatable<LogoAnchorItem>
    {
        public LogoAnchorItem(Anchor anchor)
        {
            this.Anchor = anchor;
        }

        public Anchor Anchor { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as LogoAnchorItem);
        }

        public bool Equals(LogoAnchorItem other)
        {
            return other != null
                 && other.Anchor == this.Anchor;
        }

        public override int GetHashCode()
        {
            return this.Anchor.GetHashCode();
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(Anchor), this.Anchor);
        }
    }
}
