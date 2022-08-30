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
using Scandit.DataCapture.Core.Common.Geometry.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.ScanArea
{
    public class ScanAreaSettingsViewModel : BaseViewModel
    {
        private static SettingsManager settings = SettingsManager.Instance;

        public ScanAreaSettingsViewModel()
        {
            var itemsCount = Enum.GetValues(typeof(MarginItem.Type)).Length;

            this.Height = (itemsCount + 1) * 42;
        }

        public IList<MarginItem> Source => GetItems();

        private IList<MarginItem> GetItems()
        {
            return new List<MarginItem>
            {
                new MarginItem(MarginItem.Type.Top, settings.ScanAreaMargins.Top),
                new MarginItem(MarginItem.Type.Right, settings.ScanAreaMargins.Right),
                new MarginItem(MarginItem.Type.Bottom, settings.ScanAreaMargins.Bottom),
                new MarginItem(MarginItem.Type.Left   , settings.ScanAreaMargins.Left)
            };
        }

        public int Height { get; private set; }

        public bool ShouldShowScanAreaGuides
        {
            get { return settings.ShouldShowScanAreaGuides; }
            set
            {
                settings.ShouldShowScanAreaGuides = value;
                OnPropertyChanged(nameof(this.ShouldShowScanAreaGuides));
            }
        }
    }

    public class MarginItem
    {
        public MarginItem(MarginItem.Type type, FloatWithUnit value)
        {
            DisplayNameResource = "Margin_" + Enum.GetName(typeof(MarginItem.Type), type);
            MarginItemType = type;
            Value = value;
        }

        public string DisplayNameResource { get; }

        public Type MarginItemType { get; }

        public FloatWithUnit Value { get; }

        public enum Type
        {
            Top,
            Right,
            Bottom,
            Left
        }
    }
}
