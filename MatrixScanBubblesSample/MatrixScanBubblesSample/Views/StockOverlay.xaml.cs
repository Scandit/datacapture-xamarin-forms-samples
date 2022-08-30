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

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MatrixScanBubblesSample.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StockOverlay : ContentView
    {
        public StockOverlay(string barcode, int shelfCount, int backRoom)
        {
            this.InitializeComponent();
            this.StockCountImage.BackgroundColor = Color.FromRgba(0.35, 0.84, 0.78, 1);
            
            this.Barcode.Text = barcode;
            this.Description.Text = string.Format("Shelf: {0} Back room: {1}", shelfCount, backRoom);

            this.HeightRequest = 50;
            this.WidthRequest = 250;
        }

        private void TapGestureRecognizerTapped(object sender, System.EventArgs e)
        {
            this.Title.IsVisible = !this.Title.IsVisible;
            this.Description.IsVisible = !this.Description.IsVisible;
            this.Barcode.IsVisible = !this.Barcode.IsVisible;
        }
    }
}
