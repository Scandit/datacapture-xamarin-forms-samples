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
using System.ComponentModel;
using System.Diagnostics;
using Android.Graphics;
using Android.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MatrixScanBubblesSample.Effects;
using MatrixScanBubblesSample.Droid.Effects;

[assembly: ResolutionGroupName("MatrixScanBubblesSample")]
[assembly: ExportEffect(typeof(RoundCornersEffectAndroid), nameof(RoundCornersEffect))]

namespace MatrixScanBubblesSample.Droid.Effects
{
    public class RoundCornersEffectAndroid : PlatformEffect
    {
        private Android.Views.View View => this.Control ?? this.Container;

        protected override void OnAttached()
        {
            try
            {
                this.PrepareContainer();
                this.SetCornerRadius();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        protected override void OnDetached()
        {
            try
            {
                this.View.OutlineProvider = ViewOutlineProvider.Background;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == RoundCornersEffect.CornerRadiusProperty.PropertyName)
            {
                this.SetCornerRadius();
            }
        }

        private void PrepareContainer()
        {
            this.View.ClipToOutline = true;
        }

        private void SetCornerRadius()
        {
            var cornerRadius = RoundCornersEffect.GetCornerRadius(Element) * GetDensity();
            this.View.OutlineProvider = new RoundedOutlineProvider(cornerRadius);
        }

        private static double GetDensity() => DeviceDisplay.MainDisplayInfo.Density;

        private class RoundedOutlineProvider : ViewOutlineProvider
        {
            private readonly double radius;

            public RoundedOutlineProvider(double radius)
            {
                this.radius = radius;
            }

            public override void GetOutline(Android.Views.View view, Outline outline)
            {
                outline?.SetRoundRect(0, 0, view.Width, view.Height, (float)this.radius);
            }
        }
    }
}
