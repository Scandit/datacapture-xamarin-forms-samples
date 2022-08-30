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
using System.Threading.Tasks;
using BarcodeCaptureSettingsSample.Models;
using Scandit.DataCapture.Barcode.Data.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.Symbologies
{
    public class SymbologySettingsViewModel : BaseViewModel
    {
        private readonly static SettingsManager settings = SettingsManager.Instance;

        private readonly Symbology symbology;
        private readonly SymbologyDescription symbologyDescription;

        public SymbologySettingsViewModel(Symbology symbology)
        {
            this.symbology = symbology;
            this.symbologyDescription = new SymbologyDescription(symbology);
        }

        public string Title => Enum.GetName(typeof(Symbology), this.symbology);

        public bool IsRangeSettingsAvailable
        {
            get
            {
                Range range = this.symbologyDescription.ActiveSymbolCountRange;
                return range.Minimum != range.Maximum;
            }
        }

        public bool ExtensionsAvailable => this.symbologyDescription.SupportedExtensions.Any();

        public bool ColorInvertedSettingsAvailable => this.symbologyDescription.ColorInvertible;

        public bool IsEnabled
        {
            get
            {
                return settings.IsSymbologyEnabled(this.symbology);
            }
            set
            {
                _ = settings.EnableSymbologyAsync(this.symbology, value, true);
            }
        }

        public bool IsColorInverted
        {
            get
            {
                return settings.IsColorInverted(this.symbology);
            }
            set
            {
                _ = settings.SetColorInvertedAsync(this.symbology, value);
            }
        }

        public IList<SymbologyExtensionItem> SymbologyExtensions =>
             this.symbologyDescription.SupportedExtensions
            .Select(e => new SymbologyExtensionItem(e, settings.IsExtensionEnabled(this.symbology, e)))
            .ToList();

        public IList<int> AvailableMinimumRanges => GetAvailableMinRangeItems();

        private IList<int> GetAvailableMinRangeItems()
        {
            var result = new List<int>();
            if (!IsRangeSettingsAvailable)
                return result;

            Range range = this.symbologyDescription.ActiveSymbolCountRange;

            // We allow selection from the minimum symbol count allowed by the symbology until the
            // currently selected maximum symbol count.
            int minAllowedSymbolCount = range.Minimum;
            int maxAllowedSymbolCount = settings.GetMaxSymbolCount(this.symbology);

            int step = range.Step;

            for (int i = minAllowedSymbolCount; i <= maxAllowedSymbolCount; i += step)
            {
                result.Add(i);
            }
            return result;
        }

        public int CurrentMinimumRange
        {
            get
            {
                return settings.GetMinSymbolCount(this.symbology);
            }
            set
            {
                _ = settings.SetMinSymbolCountAsync(this.symbology, (short)value);
            }
        }

        public IList<int> AvailableMaximumRanges => GetAvailableMaxRangeItems();

        private IList<int> GetAvailableMaxRangeItems()
        {
            var result = new List<int>();
            if (!IsRangeSettingsAvailable)
                return result;

            Range range = this.symbologyDescription.ActiveSymbolCountRange;

            // We allow selection from the currently selected minimum symbol count until the maximum
            // allowed by the symbology.
            int minAllowedSymbolCount = settings.GetMinSymbolCount(this.symbology);
            int maxAllowedSymbolCount = range.Maximum;
            int step = range.Step;

            for (int i = minAllowedSymbolCount; i <= maxAllowedSymbolCount; i += step)
            {
                result.Add(i);
            }

            return result;
        }

        public int CurrentMaximumRange
        {
            get
            {
                return settings.GetMaxSymbolCount(this.symbology);
            }
            set
            {
                _ = settings.SetMaxSymbolCountAsync(this.symbology, (short)value);
            }
        }

        public async Task ToggleExtensionAsync(string extension)
        {
            await settings.SetExtensionEnabledAsync(
                this.symbology,
                extension,
                !settings.IsExtensionEnabled(this.symbology, extension));

            OnPropertyChanged(nameof(this.SymbologyExtensions));
        }
    }

    public class SymbologyExtensionItem
    {
        public SymbologyExtensionItem(string symbologyExtensionName, bool isSelected)
        {
            this.SymbologyExtensionName = symbologyExtensionName;

            this.IsSelected = isSelected;
        }

        public string SymbologyExtensionName { get; }

        public bool IsSelected { get; }
    }
}
