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
using Xamarin.Forms;

using Scandit.DataCapture.Barcode.Count.UI.Unified;
using System.Linq;
using MatrixScanCountSimpleSample.Views;

namespace MatrixScanCountSimpleSample
{
    public partial class App : Application, IScanResultsPageListener
    {
        private BarcodeCountPage barcodeCountPage;

        // Enter your Scandit License key here.
        // Your Scandit License key is available via your Scandit SDK web account.
        public const string SCANDIT_LICENSE_KEY = "-- ENTER YOUR SCANDIT LICENSE KEY HERE --";

        public class MessageKeys
        {
            public const string OnStart = nameof(OnStart);
            public const string OnSleep = nameof(OnSleep);
            public const string OnResume = nameof(OnResume);
        }

        public App()
        {
            this.InitializeComponent();

            // Set BarcodeCountPage as the main page.
            this.barcodeCountPage = new BarcodeCountPage();
            this.MainPage = new NavigationPage(this.barcodeCountPage);
        }

        public void ShowScanResults(bool isOrderCompleted)
        {
            if (this.MainPage is NavigationPage navigationPage)
            {
                // Get a list of ScannedItem to display
                var scannedItems = BarcodeManager.Instance.GetScanResults().Values.ToList();

                navigationPage.PushAsync(new ScanResultsPage(this, scannedItems, isOrderCompleted));
            }
        }

        public void RestartScanning()
        {
            this.barcodeCountPage.RestartScanning();
        }

        protected override void OnStart()
        {
            MessagingCenter.Send(this, MessageKeys.OnStart);
        }

        protected override void OnSleep()
        {
            MessagingCenter.Send(this, MessageKeys.OnSleep);
        }

        protected override void OnResume()
        {
            MessagingCenter.Send(this, MessageKeys.OnResume);
        }
    }
}
