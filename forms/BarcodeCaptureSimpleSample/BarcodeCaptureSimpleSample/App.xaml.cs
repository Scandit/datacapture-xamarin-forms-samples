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

using BarcodeCaptureSimpleSample.Services;
using BarcodeCaptureSimpleSample.Views;
using Xamarin.Forms;

namespace BarcodeCaptureSimpleSample
{
    public partial class App : Application
    {
        public class MessageKeys
        {
            public const string OnStart = nameof(OnStart);
            public const string OnSleep = nameof(OnSleep);
            public const string OnResume = nameof(OnResume);
        }

        public App()
        {
            this.InitializeComponent();
            this.MainPage = new MainPage();

            DependencyService.Register<IMessageService, MessageService>();
        }

        protected override void OnStart()
        {
            MessagingCenter.Send(this, MessageKeys.OnStart);
        }

        protected override void OnSleep()
        {
            MessagingCenter.Send(this, MessageKeys.OnSleep);
        }

        protected override void OnResume()
        {
            MessagingCenter.Send(this, MessageKeys.OnResume);
        }
    }
}
