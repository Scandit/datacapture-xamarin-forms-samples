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
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using ListBuildingSample.Models;

using Scandit.DataCapture.Barcode.Data.Unified;
using Scandit.DataCapture.Barcode.Spark.Capture.Unified;
using Scandit.DataCapture.Barcode.Spark.UI.Unified;
using Scandit.DataCapture.Core.Capture.Unified;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace ListBuildingSample.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public DataCaptureContext DataCaptureContext => ScannerModel.Instance.DataCaptureContext;
        public SparkScan SparkScan => ScannerModel.Instance.SparkScan;
        public SparkScanViewSettings ViewSettings { get; private set; } = new SparkScanViewSettings();

        public event EventHandler Sleep;
        public event EventHandler Resume;

        public ObservableCollection<ListItem> ScanResults { get; set; } = new ObservableCollection<ListItem>();

        public MainPageViewModel()
        {
            this.Initialize();
            this.SubscribeToAppMessages();
        }

        public string ItemCount
        {
            get 
            {
                var itemCount = this.ScanResults.Count;
                var text = itemCount == 1 ? "Item" : "Items";
                var label = $"{itemCount} {text}";
                return label;
            }
        }

        public void OnSleep()
        {
            this.Sleep?.Invoke(this, EventArgs.Empty);
        }

        public void OnResume()
        {
            this.Resume?.Invoke(this, EventArgs.Empty);
        }

        public void ClearScannedItems()
        {
            this.ScanResults.Clear();
            this.OnPropertyChanged(nameof(ItemCount));
        }

        public async Task CheckCameraPermissionAsync()
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (permissionStatus != PermissionStatus.Granted)
            {
                permissionStatus = await Permissions.RequestAsync<Permissions.Camera>();
                if (permissionStatus == PermissionStatus.Granted)
                {
                    this.OnResume();
                }
            }
            else
            {
                this.OnResume();
            }
        }

        public bool IsBarcodeValid(Barcode barcode)
        {
            return barcode.Data != "123456789";
        }

        private void Initialize()
        {
            this.SparkScan.BarcodeScanned += BarcodeScanned;
        }

        private void BarcodeScanned(object sender, SparkScanEventArgs args)
        {
            if (args.Session.NewlyRecognizedBarcodes.Count == 0)
            {
                return;
            }

            var barcode = args.Session.NewlyRecognizedBarcodes.First();

            Device.InvokeOnMainThreadAsync(() =>
            {
                if (IsBarcodeValid(barcode))
                {
                    this.ScanResults.Add(
                        new ListItem(
                            index: this.ScanResults.Count + 1,
                            symbology: new SymbologyDescription(barcode.Symbology).ReadableName,
                            data: barcode.Data));
                    this.OnPropertyChanged(nameof(ItemCount));
                }
            });
        }

        private void SubscribeToAppMessages()
        {
            MessagingCenter.Subscribe(this, App.MessageKeys.OnResume, callback: async (App app) => await this.CheckCameraPermissionAsync());
            MessagingCenter.Subscribe(this, App.MessageKeys.OnStart, callback: async (App app) => await this.CheckCameraPermissionAsync());
            MessagingCenter.Subscribe(this, App.MessageKeys.OnSleep, callback: (App app) => this.OnSleep());
        }
    }
}
