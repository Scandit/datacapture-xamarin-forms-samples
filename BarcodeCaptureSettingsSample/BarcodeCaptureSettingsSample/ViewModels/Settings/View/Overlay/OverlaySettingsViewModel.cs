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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BarcodeCaptureSettingsSample.Models;
using BarcodeCaptureSettingsSample.Resources;
using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Barcode.UI.Unified;
using Scandit.DataCapture.Core.UI.Style.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Overlay
{
    public class OverlaySettingsViewModel : BaseViewModel
    {
        private static SettingsManager settings = SettingsManager.Instance;

        private static Brush DefaultBrush => BarcodeCaptureOverlay.DefaultBrushForStyle(settings.OverlayStyle);

        private IList<OverlaySettingsBrushItem> availableBrushes =
            new Lazy<IList<OverlaySettingsBrushItem>>(GetBrushes).Value;

        private static IList<OverlaySettingsBrushItem> GetBrushes()
        {
            return new List<OverlaySettingsBrushItem>
            {
                new OverlaySettingsBrushItem(DefaultBrush, AppResources.Brush_Defaults),
                // Transparent Red and Red colors used here
                new OverlaySettingsBrushItem(
                    new Brush(fillColor: Xamarin.Forms.Color.FromHex("#33FF0000"),
                        strokeColor: Xamarin.Forms.Color.FromHex("#FFFF0000"),
                        strokeWidth: DefaultBrush.StrokeWidth), AppResources.Brush_Red),
                // Transparent Green and Green colors used here
                new OverlaySettingsBrushItem(
                    new Brush(fillColor: Xamarin.Forms.Color.FromHex("#3300FF00"),
                        strokeColor: Xamarin.Forms.Color.FromHex("#FF00FF00"),
                        strokeWidth: DefaultBrush.StrokeWidth), AppResources.Brush_Green)
            };            
        }
        public IList<OverlaySettingsBrushItem> AvailableBrushes => availableBrushes;

        public IList<OverlaySetingsStyleItem> AvailableStyles => new[]
        {
            new OverlaySetingsStyleItem(BarcodeCaptureOverlayStyle.Legacy, "Legacy"),
            new OverlaySetingsStyleItem(BarcodeCaptureOverlayStyle.Frame, "Frame")
        };

        public OverlaySettingsBrushItem CurrentBrush
        {
            get
            {
                return this.GetSettingsBrush(settings.CurrentBrush) ?? this.AvailableBrushes[0];
            }
            set
            {
                Brush brush = this.GetSettingsBrush(value?.Brush)?.Brush ?? this.AvailableBrushes[0].Brush;
                settings.CurrentBrush = brush;
            }
        }

        public OverlaySetingsStyleItem CurrentStyle
        {
            get => this.AvailableStyles.First(style => settings.OverlayStyle == style.Style);
            set
            {
                settings.OverlayStyle = value.Style;
                OnPropertyChanged(nameof(this.AvailableStyles));
                OnPropertyChanged(nameof(this.AvailableBrushes));
                OnPropertyChanged(nameof(this.CurrentBrush));
            }
        }

        private OverlaySettingsBrushItem GetSettingsBrush(Brush brush)
        {
            return brush == null ? this.AvailableBrushes.First() : this.AvailableBrushes.FirstOrDefault(item => eq(item.Brush, brush));
        }

        private static bool eq(Brush first, Brush other)
        {
            if (first == null || other == null)
            {
                return false;
            }

            return first.FillColor.Equals(other.FillColor)  &&
                   first.StrokeColor.Equals(other.StrokeColor) &&
                   first.StrokeWidth == other.StrokeWidth;
        }
    }

    public class OverlaySettingsBrushItem
    {
        public OverlaySettingsBrushItem(Brush brush, string displayText)
        {
            this.Brush = brush;
            this.DisplayText = displayText;
        }

        public Brush Brush { get; }

        public string DisplayText { get; }

        public override string ToString()
        {
            return DisplayText;
        }
    }

    public class OverlaySetingsStyleItem
    {
        public OverlaySetingsStyleItem(BarcodeCaptureOverlayStyle style, string displayText)
        {
            this.Style = style;
            this.DisplayText = displayText;
        }
        
        public BarcodeCaptureOverlayStyle Style { get; }
            
        public string DisplayText { get; }

        public bool IsSelected => SettingsManager.Instance.OverlayStyle == this.Style;
    }
}
