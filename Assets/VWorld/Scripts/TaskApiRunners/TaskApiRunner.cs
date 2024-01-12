// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using UnityEngine;
using VContainer;
using Stopwatch = System.Diagnostics.Stopwatch;
using VContainer.Unity;
using Cysharp.Threading.Tasks;
using System.Threading;
using Mediapipe;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Experimental;
using Mediapipe.Tasks.Vision.Core;

namespace VWorld
{
    using Screen = Mediapipe.Unity.Screen;

    public abstract class TaskApiRunner<TTask> : IStartable where TTask : Mediapipe.Tasks.Vision.Core.BaseVisionTaskApi
    {
        protected Bootstrap bootstrap;
        protected Screen screen;
        protected bool isPaused;
        protected TTask taskApi;
        protected CancellationTokenSource cancellationTokenSource;
        private readonly Stopwatch _stopwatch = new();
        private TextureFramePool textureFramePool;

        public TaskApiRunner(Bootstrap bootstrap, Screen screen)
        {
            this.bootstrap = bootstrap;
            this.screen = screen;
        }

        public virtual async void Start()
        {
            await UniTask.WaitUntil(() => bootstrap.isFinished);
            Play();
        }

        public virtual void Play()
        {
            if (cancellationTokenSource != null)
            {
                Stop();
            }
            
            isPaused = false;
            _stopwatch.Restart();
            cancellationTokenSource = new CancellationTokenSource();
            Run().Forget();
        }

        public virtual void Pause()
        {
            isPaused = true;
            ImageSourceProvider.ImageSource.Pause();
        }

        public virtual void Resume()
        {
            isPaused = false;
            ImageSourceProvider.ImageSource.Resume().ToUniTask().Forget();
        }

        public virtual void Stop()
        {
            isPaused = true;
            _stopwatch.Stop();
            ImageSourceProvider.ImageSource.Stop();
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = null;
            taskApi?.Close();
            taskApi = null;
        }

        protected virtual async UniTaskVoid Run()
        {
            await InitTaskApi();

            var imageSource = ImageSourceProvider.ImageSource;

            await imageSource.Play();

            if (!imageSource.isPrepared)
            {
                Debug.LogError("Failed to start ImageSource, exiting...");
                throw new OperationCanceledException();
            }

            // Use RGBA32 as the input format.
            // TODO: When using GpuBuffer, MediaPipe assumes that the input format is BGRA, so maybe the following code needs to be fixed.
            textureFramePool = new TextureFramePool(imageSource.textureWidth, imageSource.textureHeight, TextureFormat.RGBA32, 10);

            // NOTE: The screen will be resized later, keeping the aspect ratio.
            screen.Initialize(imageSource);

            var transformationOptions = imageSource.GetTransformationOptions();
            var flipHorizontally = transformationOptions.flipHorizontally;
            var flipVertically = transformationOptions.flipVertically;
            var imageProcessingOptions = new ImageProcessingOptions(rotationDegrees: (int)transformationOptions.rotationAngle);

            while (true)
            {
                if (isPaused)
                {
                    await UniTask.WaitWhile(() => isPaused);
                }

                if (!textureFramePool.TryGetTextureFrame(out var textureFrame))
                {
                    await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
                    continue;
                }

                // Copy current image to TextureFrame
                var req = textureFrame.ReadTextureAsync(imageSource.GetCurrentTexture(), flipHorizontally, flipVertically);
                await UniTask.WaitUntil(() => req.done);

                if (req.hasError)
                {
                    Debug.LogError($"Failed to read texture from the image source, exiting...");
                    break;
                }

                var image = textureFrame.BuildCPUImage();
                switch (taskApi.runningMode)
                {
                    case RunningMode.IMAGE:
                        Detect(image, imageProcessingOptions);
                        break;
                    case RunningMode.VIDEO:
                        DetectForVideo(image, (int)GetCurrentTimestampMillisec(), imageProcessingOptions);
                        break;
                    case RunningMode.LIVE_STREAM:
                        DetectAsync(image, (int)GetCurrentTimestampMillisec(), imageProcessingOptions);
                        break;
                }

                textureFrame.Release();
            }
        }

        protected abstract UniTask InitTaskApi();
        protected abstract void Detect(Image image, ImageProcessingOptions options);
        protected abstract void DetectForVideo(Image image, int timestamp, ImageProcessingOptions options);
        protected abstract void DetectAsync(Image image, int timestamp, ImageProcessingOptions options);

        protected long GetCurrentTimestampMillisec() => _stopwatch.IsRunning ? _stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond : -1;
    }
}
