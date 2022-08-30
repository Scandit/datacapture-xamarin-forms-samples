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
using BarcodeCaptureSettingsSample.ViewModels.Settings.Camera;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.Views.Settings.Camera
{
    public partial class CameraSettingsPage : ContentPage
    {
        private CameraSettingsViewModel cameraSettingsViewModel;
        public CameraSettingsPage()
        {
            InitializeComponent();
            cameraSettingsViewModel = BindingContext as CameraSettingsViewModel;
        }

        void cameraPositionList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            cameraSettingsViewModel.CurrentCameraPosition = e.Item as CameraPositionItem;
        }

        async void Title_Tapped(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}
