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
using Scandit.DataCapture.Core.Area.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.BarcodeCapture.LocationSelection
{
    public class LocationSelectionSettingsViewModel : BaseViewModel
    {
        private static readonly SettingsManager settings = SettingsManager.Instance;

        public LocationSelectionSettingsViewModel()
        {
            if (settings.LocationSelection == null)
            {
                this.LocationSelectionItemViewModel = new NoneLocationSelectionViewModel();
                this.CurrentLocationSelectionItemType = LocationSelectionItem.Type.None;
            }
            else if (settings.LocationSelection is RadiusLocationSelection)
            {
                this.LocationSelectionItemViewModel = new RadiusLocationSelectionViewModel();
                this.CurrentLocationSelectionItemType = LocationSelectionItem.Type.Radius;
            }
            else
            {
                this.LocationSelectionItemViewModel = new RectangularLocationSelectionViewModel();
                this.CurrentLocationSelectionItemType = LocationSelectionItem.Type.Rectangular;
            }
        }

        public IList<LocationSelectionItem> AvailableLocationSelections => new List<LocationSelectionItem>
        {
            new LocationSelectionItem(LocationSelectionItem.Type.None,  this.CurrentLocationSelectionItemType == LocationSelectionItem.Type.None),
            new LocationSelectionItem(LocationSelectionItem.Type.Radius, this.CurrentLocationSelectionItemType == LocationSelectionItem.Type.Radius),
            new LocationSelectionItem(LocationSelectionItem.Type.Rectangular,  this.CurrentLocationSelectionItemType == LocationSelectionItem.Type.Rectangular)
        };

        public LocationSelectionItem.Type CurrentLocationSelectionItemType { get; private set; }

        public ILocationSelectionItemViewModel LocationSelectionItemViewModel { get; private set; }

        public async Task SetLocationTypeAsync(LocationSelectionItem locationType)
        {
            switch (locationType.LocationSelectionType)
            {
                case LocationSelectionItem.Type.None:
                    this.LocationSelectionItemViewModel = new NoneLocationSelectionViewModel();
                    break;
                case LocationSelectionItem.Type.Radius:
                    this.LocationSelectionItemViewModel = new RadiusLocationSelectionViewModel();
                    break;
                case LocationSelectionItem.Type.Rectangular:
                    this.LocationSelectionItemViewModel = new RectangularLocationSelectionViewModel();
                    break;
            }
            this.CurrentLocationSelectionItemType = locationType.LocationSelectionType;
            await settings.SetLocationSelectionAsync(this.LocationSelectionItemViewModel.Build());
            OnPropertyChanged(nameof(this.AvailableLocationSelections));
        }
    }

    public class LocationSelectionItem
    {
        public LocationSelectionItem(LocationSelectionItem.Type type, bool isSelected)
        {
            this.LocationSelectionType = type;
            this.IsSelected = isSelected;
            this.DisplayText = Enum.GetName(typeof(LocationSelectionItem.Type), this.LocationSelectionType);
        }

        public Type LocationSelectionType { get; }

        public bool IsSelected { get; }

        public string DisplayText { get; }

        public enum Type
        {
            None,
            Radius,
            Rectangular
        }
    }
}

