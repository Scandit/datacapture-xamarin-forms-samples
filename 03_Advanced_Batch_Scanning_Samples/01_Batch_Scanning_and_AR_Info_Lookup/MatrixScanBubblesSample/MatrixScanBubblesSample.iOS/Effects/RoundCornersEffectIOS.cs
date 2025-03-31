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
using CoreAnimation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using MatrixScanBubblesSample.Effects;
using MatrixScanBubblesSample.iOS.Effects;

[assembly: ResolutionGroupName("MatrixScanBubblesSample")]
[assembly: ExportEffect(typeof(RoundCornersEffectIOS), nameof(RoundCornersEffect))]

namespace MatrixScanBubblesSample.iOS.Effects
{
    public class RoundCornersEffectIOS : PlatformEffect
    {
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
                this.Container.Layer.CornerRadius = new nfloat(0);
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
            this.Container.ClipsToBounds = true;
            this.Container.Layer.AllowsEdgeAntialiasing = true;
            this.Container.Layer.EdgeAntialiasingMask = CAEdgeAntialiasingMask.All;
        }

        private void SetCornerRadius()
        {
            var cornerRadius = RoundCornersEffect.GetCornerRadius(Element);
            this.Container.Layer.CornerRadius = new nfloat(cornerRadius);
        }
    }
}
