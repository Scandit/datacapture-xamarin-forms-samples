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

using System.ComponentModel;
using BarcodeCaptureSettingsSample.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ListView), typeof(NoBounceListViewRenderer))]
namespace BarcodeCaptureSettingsSample.iOS.Renderers
{
    public class NoBounceListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (Element != null)
            {
                Control.AlwaysBounceVertical = Element.IsPullToRefreshEnabled;
                Control.Bounces = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(Element.IsPullToRefreshEnabled))
            {
                Control.AlwaysBounceVertical = Element.IsPullToRefreshEnabled;
                Control.Bounces = false;
            }
        }

    }
}
