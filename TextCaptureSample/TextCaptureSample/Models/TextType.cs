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
    /// Various types of text we recognize in this sample along with regexes
    /// that allow TextCapture to filter them.
    /// </summary>
    public class TextType : IEquatable<TextType>
    {
        /*
         * In fact GS1 AI comes in such variety that makes it impossible to cover all the cases with
         * reasonable regex. If recognizing such codes is your use-case, then you probably don't need
         * to recognize just any GS1 AI, but a specific kind.
         */
        public static TextType GS1_AI = new TextType("((?:\\\\([0-9]+\\\\)[A-Za-z0-9]+)+)", "GS1 AI");
        public static TextType LOT = new TextType("([A-Z0-9]{6,8})", "LOT");

        public string Regex { get; }
        public string Name { get; }

        private TextType(string regex, string name)
        {
            this.Regex = regex;
            this.Name = name;
        }

        public bool Equals(TextType other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(this.Regex, other.Regex);
        }

        public override bool Equals(object obj)
        {
            if (obj is TextType type)
            {
                return Equals(type);
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return this.Regex.GetHashCode();
        }

        public static bool operator ==(TextType first, TextType second)
        {
            if (first is null)
            {
                return second is null;
            }

            return first.Equals(second);
        }

        public static bool operator !=(TextType first, TextType second)
        {
            return !(first == second);
        }
    }
}
