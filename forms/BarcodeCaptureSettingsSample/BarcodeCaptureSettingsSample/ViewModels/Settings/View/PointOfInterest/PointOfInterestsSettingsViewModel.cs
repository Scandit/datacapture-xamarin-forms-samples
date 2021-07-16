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

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.ViewSettings.PointOfInterest
{
    public class PointOfInterestSettingsViewModel : BaseViewModel
    {
        private static SettingsManager settings = SettingsManager.Instance;

        public IList<PointOfInterestItem> Source => new List<PointOfInterestItem>
        {
            new PointOfInterestItem(PointOfInterestItem.Type.X, settings.PointOfInterest.X),
            new PointOfInterestItem(PointOfInterestItem.Type.Y, settings.PointOfInterest.Y),
        };
    }

    public class PointOfInterestItem
    {
        public PointOfInterestItem(PointOfInterestItem.Type type, FloatWithUnit value)
        {
            this.DisplayText = Enum.GetName(typeof(PointOfInterestItem.Type), type);

            this.ItemType = type;

            this.Value = value;
        }

        public String DisplayText { get; }

        public Type ItemType { get; }

        public FloatWithUnit Value { get; }

        public enum Type
        {
            X,
            Y
        }
    }
}
