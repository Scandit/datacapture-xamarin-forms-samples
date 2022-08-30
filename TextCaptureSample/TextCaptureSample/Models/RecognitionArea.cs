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

namespace TextCaptureSample.Models
{
    /// <summary>
    /// Controls which part of the frame will be taken into account when performing text recognition.
    /// The rest of the frame will be ignored. This option is useful when you know exactly where
    /// the text that you want to recognize will appear - it allows you to remove noise and improve
    /// accuracy.
    ///
    /// While this sample uses only general `Top`, `Center` and `Bottom`, the location selection might
    /// be for example a rectangle or a circle that you can size and position to tailor it to your
    /// needs.
    /// </summary>
    public class RecognitionArea : IEquatable<RecognitionArea>
    {
        public enum Location
        {
            Center, 
            Top,
            Bottom
        }

        public float CenterY { get; }
        public Location Position { get; }

        public static RecognitionArea Top = new RecognitionArea(Location.Top, 0.25f);
        public static RecognitionArea Center = new RecognitionArea(Location.Center, 0.5f);
        public static RecognitionArea Bottom = new RecognitionArea(Location.Bottom, 0.75f);

        private RecognitionArea(Location scanPosition, float centerY)
        {
            this.Position = scanPosition;
            this.CenterY = centerY;
        }

        public bool Equals(RecognitionArea other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.CenterY, other.CenterY) && Equals(this.Position, other.Position);
        }

        public override bool Equals(object obj)
        {
            if (obj is RecognitionArea type)
            {
                return Equals(type);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.CenterY.GetHashCode() ^ this.Position.GetHashCode();
        }

        public static bool operator ==(RecognitionArea first, RecognitionArea second)
        {
            if (first is null)
            {
                return second is null;
            }

            return first.Equals(second);
        }

        public static bool operator !=(RecognitionArea first, RecognitionArea second)
        {
            return !(first == second);
        }
    }
}
