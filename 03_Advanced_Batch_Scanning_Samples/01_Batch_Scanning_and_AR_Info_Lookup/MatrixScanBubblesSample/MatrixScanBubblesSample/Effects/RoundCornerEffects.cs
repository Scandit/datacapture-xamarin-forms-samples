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

using System.Linq;
using Xamarin.Forms;

namespace MatrixScanBubblesSample.Effects
{
    public class RoundCornersEffect : RoutingEffect
    {
        public RoundCornersEffect() : base("MatrixScanBubblesSample.RoundCornersEffect")
        { }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.CreateAttached(
                "CornerRadius",
                typeof(int),
                typeof(RoundCornersEffect),
                0,
                propertyChanged: OnCornerRadiusChanged);

        public static int GetCornerRadius(BindableObject view) =>
            (int)view.GetValue(CornerRadiusProperty);

        public static void SetCornerRadius(BindableObject view, int value) =>
            view.SetValue(CornerRadiusProperty, value);

        private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view))
            {
                return;
            }

            var cornerRadius = (int)newValue;
            var effect = view.Effects.OfType<RoundCornersEffect>().FirstOrDefault();

            if (cornerRadius > 0 && effect == null)
            {
                view.Effects.Add(new RoundCornersEffect());
            }

            if (cornerRadius == 0 && effect != null)
            {
                view.Effects.Remove(effect);
            }
        }
    }
}
