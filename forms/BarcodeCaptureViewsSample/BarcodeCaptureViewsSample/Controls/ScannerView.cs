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

using Scandit.DataCapture.Barcode.Capture.Unified;
using Scandit.DataCapture.Core.Capture.Unified;
using Scandit.DataCapture.Core.UI.Viewfinder.Unified;
using Xamarin.Forms;

namespace BarcodeCaptureViewsSample.Controls
{
    public class ScannerView : ContentView
    {
        public static readonly BindableProperty DataCaptureContextProperty = BindableProperty.Create(
            nameof(DataCaptureContext),
            typeof(DataCaptureContext),
            typeof(ScannerView),
            default(DataCaptureContext));

        public static readonly BindableProperty BarcodeCaptureProperty = BindableProperty.Create(
            nameof(BarcodeCapture),
            typeof(BarcodeCapture),
            typeof(ScannerView),
            default(BarcodeCapture));

        public static readonly BindableProperty ViewfinderProperty = BindableProperty.Create(
            nameof(Viewfinder),
            typeof(IViewfinder),
            typeof(ScannerView),
            default(IViewfinder));

        public DataCaptureContext DataCaptureContext
        {
            get => (DataCaptureContext)this.GetValue(DataCaptureContextProperty);
            set => this.SetValue(DataCaptureContextProperty, value);
        }

        public BarcodeCapture BarcodeCapture
        {
            get => (BarcodeCapture)this.GetValue(BarcodeCaptureProperty);
            set => this.SetValue(BarcodeCaptureProperty, value);
        }

        public IViewfinder Viewfinder
        {
            get => (IViewfinder)this.GetValue(ViewfinderProperty);
            set => this.SetValue(ViewfinderProperty, value);
        }
    }
}
