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

namespace TextCaptureSample.ViewModels
{
    public class CheckedItem<T> : BaseViewModel
    {
        private bool @checked = false;

        public T Item { get; }

        public bool Checked
        {
            get => this.@checked;
            set => this.SetProperty(ref this.@checked, value, nameof(Checked));
        }

        public CheckedItem(T item, bool @checked = false)
        {
            this.Item = item;
            this.Checked = @checked;
        }
    }
}
