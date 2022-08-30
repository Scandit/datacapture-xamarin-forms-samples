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
using Scandit.DataCapture.Core.Capture.Unified;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings
{
    public class MainSettingsViewModel : BaseViewModel
    {
        public string SDKVersion => $"Barcode Capture Settings Sample {Environment.NewLine} SDK {DataCaptureVersion.Version}";

        public IList<MainSettingsItem> Source =>
            Enum.GetValues(typeof(MainSettingsItem.Type)).OfType<MainSettingsItem.Type>().Select(settingItemSelector).ToList();

        private static MainSettingsItem settingItemSelector(MainSettingsItem.Type item)
        {
            return new MainSettingsItem(item);
        }
    }
}
