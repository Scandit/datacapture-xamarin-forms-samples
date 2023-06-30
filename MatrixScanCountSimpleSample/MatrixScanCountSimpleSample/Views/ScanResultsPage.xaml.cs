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
using System.Collections.ObjectModel;
using System.Linq;
using MatrixScanCountSimpleSample.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixScanCountSimpleSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScanResultsPage : ContentPage
    {
        private readonly bool isOrderCompleted;
        private readonly IScanResultsPageListener listener;

        public ObservableCollection<ViewCellData> Items { get; set; }

        public ScanResultsPage(IScanResultsPageListener listener, IList<ScanItem> scanResults, bool isOrderCompleted)
        {
            this.InitializeComponent();

            this.Items = this.GetViewCells(scanResults);
			this.MyListView.ItemsSource = Items;
            this.listener = listener;

            this.scanButton.Text = isOrderCompleted ? "Scan next order" : "Resume scanning";
            this.itemsCountLabel.Text = $"Items {GetScanResultsCount(scanResults)}";
            this.isOrderCompleted = isOrderCompleted;
        }

        protected override void OnDisappearing()
        {
            if (this.isOrderCompleted)
            {
                this.listener?.RestartScanning();
            }

            base.OnDisappearing();
        }

        private ObservableCollection<ViewCellData> GetViewCells(IList<ScanItem> scanItems)
        {
            ObservableCollection<ViewCellData> items = new ObservableCollection<ViewCellData>();

            // Get non-unique items first.
            int index = 0;
            foreach (var nonUniqueItem in scanItems.Where(i => i.Quantity > 1))
            {
                items.Add(new ViewCellData(nonUniqueItem, ++index));
            }

            // Get unique items.
            index = 0;
            foreach(var uniqueItem in scanItems.Where(i => i.Quantity == 1))
            {
                items.Add(new ViewCellData(uniqueItem, ++index));
            };

            return items;
        }

        private static int GetScanResultsCount(IList<ScanItem> scanResults)
        {
            return scanResults.Sum(i => i.Quantity);
        }

        private void ScanButtonClicked(System.Object sender, System.EventArgs e)
        {
            (Application.Current.MainPage as NavigationPage)?.PopAsync(animated: true);
        }

        private void ClearButtonClicked(System.Object sender, System.EventArgs e)
        {
            this.listener.RestartScanning();
            (Application.Current.MainPage as NavigationPage)?.PopAsync(animated: true);
        }
    }
}
