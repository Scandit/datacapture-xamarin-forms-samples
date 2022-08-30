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
using System.Threading.Tasks;
using BarcodeCaptureSettingsSample.Models;
using Scandit.DataCapture.Barcode.Data.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.CompositeTypes
{
    public class CompositeTypesSettingsViewModel : BaseViewModel
    {
        private static readonly SettingsManager settings = SettingsManager.Instance;

        public IList<CompositeTypeItem> Source => new List<CompositeTypeItem>
        {
            new CompositeTypeItem(CompositeType.A, settings.IsCompositeTypeEnabled(CompositeType.A)),
            new CompositeTypeItem(CompositeType.B, settings.IsCompositeTypeEnabled(CompositeType.B)),
            new CompositeTypeItem(CompositeType.C, settings.IsCompositeTypeEnabled(CompositeType.C))
        };

        public async Task ToggleCompositeType(CompositeTypeItem entry)
        {
            CompositeType compositeTypes = settings.GetEnabledCompositeTypes();
            if (compositeTypes.HasFlag(entry.CompositeType))
            {
                compositeTypes &= ~entry.CompositeType;
            }
            else
            {
                compositeTypes |= entry.CompositeType;
            }

            await settings.SetEnabledCompositeTypes(compositeTypes);
            OnPropertyChanged(nameof(this.Source));
        }
    }
    public class CompositeTypeItem
    {
        public CompositeTypeItem(CompositeType compositeType, bool isSelected)
        {
            CompositeType = compositeType;
            IsSelected = isSelected;
        }

        public string DisplayName => Enum.GetName(typeof(CompositeType), this.CompositeType);
        public CompositeType CompositeType { get; }
        public bool IsSelected { get; }
    }
}
