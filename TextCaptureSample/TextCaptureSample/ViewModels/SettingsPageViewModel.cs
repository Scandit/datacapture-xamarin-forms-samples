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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TextCaptureSample.Models;

using Xamarin.Forms;

namespace TextCaptureSample.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public List<CheckedItem<RecognitionArea>> ScanLocations { get; } = new List<CheckedItem<RecognitionArea>>
        {
            new CheckedItem<RecognitionArea>(RecognitionArea.Top, ScannerModel.Instance.CurrentRecognitionArea == RecognitionArea.Top),
            new CheckedItem<RecognitionArea>(RecognitionArea.Center, ScannerModel.Instance.CurrentRecognitionArea == RecognitionArea.Center),
            new CheckedItem<RecognitionArea>(RecognitionArea.Bottom, ScannerModel.Instance.CurrentRecognitionArea == RecognitionArea.Bottom)
        };
        
        public List<CheckedItem<Models.TextType>> TextTypes { get; } = new List<CheckedItem<Models.TextType>>
        {
            new CheckedItem<Models.TextType>(Models.TextType.GS1_AI, ScannerModel.Instance.CurrentTextType == Models.TextType.GS1_AI),
            new CheckedItem<Models.TextType>(Models.TextType.LOT, ScannerModel.Instance.CurrentTextType == Models.TextType.LOT)
        };

        public ICommand TextTypeTapped { get; }
        public ICommand ScanLocationsTapped { get; }

        public SettingsPageViewModel()
        {
            this.TextTypeTapped = new Command<Models.TextType>((Models.TextType textType) =>
            {
                if (textType != null)
                {
                    this.TextTypes.ForEach(type => type.Checked = false);
                    this.TextTypes.Where(type => type.Item == textType).First().Checked = true;
                    ScannerModel.Instance.CurrentTextType = textType;
                }
            });
            this.ScanLocationsTapped = new Command<RecognitionArea>((RecognitionArea recognitionArea) =>
            {
                if (recognitionArea != null)
                {
                    this.ScanLocations.ForEach(location => location.Checked = false);
                    this.ScanLocations.Where(location => location.Item == recognitionArea).First().Checked = true;
                    ScannerModel.Instance.CurrentRecognitionArea = recognitionArea;
                }
            });
        }
    }
}
