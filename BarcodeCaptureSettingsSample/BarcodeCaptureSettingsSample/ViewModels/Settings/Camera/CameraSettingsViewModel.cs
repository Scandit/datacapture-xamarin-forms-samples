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
using System.Linq;
using System.Collections.Generic;
using BarcodeCaptureSettingsSample.Models;
using Scandit.DataCapture.Core.Source.Unified;
using Xamarin.Forms;

namespace BarcodeCaptureSettingsSample.ViewModels.Settings.Camera
{
    public class CameraSettingsViewModel : BaseViewModel
    {
        private readonly static SettingsManager settings = SettingsManager.Instance;

        public IList<CameraPositionItem> AvailableCameraPositions => new List<CameraPositionItem>
        {
            new CameraPositionItem(CameraPositionItem.PositionType.WORLD_FACING, settings.CameraPosition == CameraPosition.WorldFacing),
            new CameraPositionItem(CameraPositionItem.PositionType.USER_FACING, settings.CameraPosition == CameraPosition.UserFacing)
        };

        public CameraPositionItem CurrentCameraPosition
        {
            get
            {
                return this.AvailableCameraPositions.FirstOrDefault(p => p.Equals(settings.CameraPosition)) ?? this.AvailableCameraPositions[0];
            }
            set
            {
                _ = settings.SetCameraPositionAsync(value.ToCameraPosition())
                    .ContinueWith(t => OnPropertyChanged(nameof(this.AvailableCameraPositions)));

            }
        }

        public IList<DesiredTorchStateItem> AvailableDesiredTorchStates =>
            Enum.GetValues(typeof(TorchState))
                .OfType<TorchState>()
                .Select(t => new DesiredTorchStateItem(t)).ToList();

        public DesiredTorchStateItem CurrentDesiredTorchState
        {
            get
            {
                return this.AvailableDesiredTorchStates.FirstOrDefault(t => t.TorchState == settings.TorchState) ?? this.AvailableDesiredTorchStates[0];
            }
            set
            {
                settings.SetTorchState(value.TorchState);
            }
        }

        public IList<PreferredResolutionItem> AvailablePreferredResolutions =>
            Enum.GetValues(typeof(VideoResolution))
                .OfType<VideoResolution>()
                .Where(videoResolution => IsValidResolution(videoResolution))
                .Select(v => new PreferredResolutionItem(v))
                .ToList();

        private static bool IsValidResolution(VideoResolution videoResolution)
        {
            // UHD4K is not supported in the Android Camera API 1.
            return !(Device.Android == Device.RuntimePlatform && videoResolution == VideoResolution.Uhd4k);
        }

        public PreferredResolutionItem CurrentPreferredResolution
        {
            get
            {
                return this.AvailablePreferredResolutions.FirstOrDefault(v => v.VideoResolution == settings.VideoResolution) ?? this.AvailablePreferredResolutions[0];
            }
            set
            {
                _ = settings.SetVideoResolutionAsync(value.VideoResolution);
            }

        }

        public float ZoomFactorValue
        {
            get
            {
                return settings.ZoomFactor;
            }
            set
            {
                _ = settings.SetZoomFactorAsync(value);
                OnPropertyChanged(nameof(ZoomFactorValue));
            }
        }

        public float ZoomGestureZoomFactor
        {
            get
            {
                return settings.ZoomGestureZoomFactor;
            }
            set
            {
                _ = settings.SetZoomGestureZoomFactorAsync(value);
                OnPropertyChanged(nameof(ZoomGestureZoomFactor));
            }
        }

        public IList<FocustGestureStrategyItem> AvailableFocusGestureStrategies =>
            Enum.GetValues(typeof(FocusGestureStrategy))
                .OfType<FocusGestureStrategy>()
                .Select(t => new FocustGestureStrategyItem(t)).ToList();

        public FocustGestureStrategyItem CurrentFocusGestureStrategy
        {
            get
            {
                return this.AvailableFocusGestureStrategies.FirstOrDefault(t => t.FocusGestureStrategy == settings.FocusGestureStrategy) ?? this.AvailableFocusGestureStrategies[0];
            }
            set
            {
                _ = settings.SetFocusGestureStrategyAsync(value.FocusGestureStrategy);
            }
        }
    }

    public class FocustGestureStrategyItem : IEquatable<FocustGestureStrategyItem>
    {
        public FocustGestureStrategyItem(FocusGestureStrategy focusGestureStrategy)
        {
            this.FocusGestureStrategy = focusGestureStrategy;
        }

        public FocusGestureStrategy FocusGestureStrategy { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as FocustGestureStrategyItem);
        }

        public bool Equals(FocustGestureStrategyItem other)
        {
            return other != null &&
                 this.FocusGestureStrategy == other.FocusGestureStrategy;
        }

        public override int GetHashCode()
        {
            return this.FocusGestureStrategy.GetHashCode();
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(FocusGestureStrategy), this.FocusGestureStrategy);
        }
    }

    public class PreferredResolutionItem : IEquatable<PreferredResolutionItem>
    {
        public PreferredResolutionItem(VideoResolution videoResolution)
        {
            this.VideoResolution = videoResolution;
        }

        public VideoResolution VideoResolution { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as PreferredResolutionItem);
        }

        public bool Equals(PreferredResolutionItem other)
        {
            return other != null &&
                this.VideoResolution == other.VideoResolution;
        }

        public override int GetHashCode()
        {
            return this.VideoResolution.GetHashCode();
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(VideoResolution), this.VideoResolution).ToUpper();
        }
    }

    public class DesiredTorchStateItem : IEquatable<DesiredTorchStateItem>
    {
        public DesiredTorchStateItem(TorchState torchState)
        {
            TorchState = torchState;
        }

        public TorchState TorchState { get; }

        public override bool Equals(object obj)
        {
            return Equals(obj as DesiredTorchStateItem);
        }

        public bool Equals(DesiredTorchStateItem other)
        {
            return other != null &&
                this.TorchState == other.TorchState;
        }

        public override int GetHashCode()
        {
            return this.TorchState.GetHashCode();
        }

        public override string ToString()
        {
            return Enum.GetName(typeof(TorchState), this.TorchState).ToUpper();
        }
    }

    public class CameraPositionItem
    {
        public CameraPositionItem(CameraPositionItem.PositionType position, bool isCurrentPosition)
        {
            this.Position = position;
            this.DisplayText = Enum.GetName(typeof(CameraPositionItem.PositionType), this.Position);
            this.IsCurrentPosition = isCurrentPosition;
        }

        public string DisplayText { get; }

        public PositionType Position { get; }

        public bool IsCurrentPosition { get; }

        public bool Equals(CameraPosition cameraPosition)
        {
            switch (cameraPosition)
            {
                case CameraPosition.WorldFacing:
                    return this.Position == PositionType.WORLD_FACING;
                case CameraPosition.UserFacing:
                    return this.Position == PositionType.USER_FACING;
            }
            return false;
        }

        public CameraPosition ToCameraPosition()
        {
            switch (this.Position)
            {
                case PositionType.WORLD_FACING:
                    return CameraPosition.WorldFacing;
                case PositionType.USER_FACING:
                    return CameraPosition.UserFacing;
            }
            return CameraPosition.Unspecified;
        }

        public enum PositionType
        {
            WORLD_FACING,
            USER_FACING
        }
    }
}
