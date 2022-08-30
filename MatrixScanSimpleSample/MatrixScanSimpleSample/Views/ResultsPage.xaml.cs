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

namespace MatrixScanSimpleSample.Views
{
    public partial class ResultsPage : ContentPage
    {
        public ResultsPage()
        {
            this.InitializeComponent();
        }

        private void ButtonClicked(object sender, System.EventArgs e)
        {
            ((NavigationPage)Application.Current.MainPage).PopAsync();
        }
    }
}
