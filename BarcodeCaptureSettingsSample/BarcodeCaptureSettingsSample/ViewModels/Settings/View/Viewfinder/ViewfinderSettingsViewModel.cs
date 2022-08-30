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
using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Aimer;
using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Laserline;
using BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder.Rectangular;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.Viewfinder
{
    public class ViewfinderSettingsViewModel : BaseViewModel
    {
        private readonly static SettingsManager settings = SettingsManager.Instance;

        public ViewfinderSettingsViewModel()
        {
            if (settings.CurrentViewfinder is AimerViewfinder)
            {
                this.CurrentViewfinderSettingsItemType = ViewfinderSettingsItem.Type.Aimer;
                this.CurrentViewfinderViewModel = new AimerViewfinderSettingsViewModel();
            }
            else if (settings.CurrentViewfinder is RectangularViewfinder)
            {
                this.CurrentViewfinderSettingsItemType = ViewfinderSettingsItem.Type.Rectangular;
                this.CurrentViewfinderViewModel = new RectangularViewfinderSettingsViewModel();
            }
            else if (settings.CurrentViewfinder is LaserlineViewfinder)
            {
                this.CurrentViewfinderSettingsItemType = ViewfinderSettingsItem.Type.Laserline;
                this.CurrentViewfinderViewModel = new LaserlineViewfinderSettingsViewModel();
            }
            else
            {
                this.CurrentViewfinderSettingsItemType = ViewfinderSettingsItem.Type.None;
            }
        }

        internal void ApplyViewfinderChanges()
        {
            settings.CurrentViewfinder = this.CurrentViewfinderViewModel?.Create();
        }

        public IList<ViewfinderSettingsItem> AvailableViewfinders => new List<ViewfinderSettingsItem>
        {
            new ViewfinderSettingsItem(ViewfinderSettingsItem.Type.None, settings.CurrentViewfinder == null),
            new ViewfinderSettingsItem(ViewfinderSettingsItem.Type.Rectangular, settings.CurrentViewfinder is RectangularViewfinder),
            new ViewfinderSettingsItem(ViewfinderSettingsItem.Type.Laserline, settings.CurrentViewfinder is LaserlineViewfinder),
            new ViewfinderSettingsItem(ViewfinderSettingsItem.Type.Aimer, settings.CurrentViewfinder is AimerViewfinder)
        };

        public IViewfinderTypeViewModel CurrentViewfinderViewModel { get; private set; }

        public void ChangeViewfinder(ViewfinderSettingsItem newValue)
        {
            switch (newValue.ItemType)
            {
                case ViewfinderSettingsItem.Type.None:
                    settings.CurrentViewfinder = null;
                    this.CurrentViewfinderViewModel = null;
                    break;
                case ViewfinderSettingsItem.Type.Rectangular:
                    settings.CurrentViewfinder = new RectangularViewfinder();
                    this.CurrentViewfinderViewModel = new RectangularViewfinderSettingsViewModel();
                    break;
                case ViewfinderSettingsItem.Type.Laserline:
                    settings.CurrentViewfinder = new LaserlineViewfinder();
                    this.CurrentViewfinderViewModel = new LaserlineViewfinderSettingsViewModel();
                    break;
                case ViewfinderSettingsItem.Type.Aimer:
                    settings.CurrentViewfinder = new AimerViewfinder();
                    this.CurrentViewfinderViewModel = new AimerViewfinderSettingsViewModel();
                    break;
            }
            this.CurrentViewfinderSettingsItemType = newValue.ItemType;
            OnPropertyChanged(nameof(AvailableViewfinders));
        }

        public ViewfinderSettingsItem.Type CurrentViewfinderSettingsItemType { get; private set; }
    }

    public class ViewfinderSettingsItem
    {
        public ViewfinderSettingsItem(ViewfinderSettingsItem.Type itemType, bool isSelected)
        {
            this.DisplayText = Enum.GetName(typeof(ViewfinderSettingsItem.Type), itemType);
            this.ItemType = itemType;
            this.IsSelected = isSelected;
        }

        public string DisplayText { get; }
        public Type ItemType { get; }
        public bool IsSelected { get; }

        public enum Type
        {
            None,
            Rectangular,
            Laserline,
            Aimer
        }
    }
}
