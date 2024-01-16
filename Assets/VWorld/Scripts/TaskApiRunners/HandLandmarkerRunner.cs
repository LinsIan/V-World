// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Cysharp.Threading.Tasks;
using Mediapipe;
using Mediapipe.Tasks.Vision.Core;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using VContainer;
using UniRx;

namespace VWorld
{
    public class HandLandmarkerRunner : TaskApiRunner<HandLandmarker>
    {
        public readonly HandLandmarkDetectionConfig config = new();
        public IReadOnlyReactiveProperty<HandLandmarkerResult> Result => result;
        private ReactiveProperty<HandLandmarkerResult> result = new();

        [Inject]
        public HandLandmarkerRunner(Bootstrap bootstrap, Mediapipe.Unity.Screen screen) : base(bootstrap, screen)
        {
        }

        protected override async UniTask InitTaskApi()
        {
            Debug.Log($"Delegate = {config.Delegate}");
            Debug.Log($"Running Mode = {config.RunningMode}");
            Debug.Log($"NumHands = {config.NumHands}");
            Debug.Log($"MinHandDetectionConfidence = {config.MinHandDetectionConfidence}");
            Debug.Log($"MinHandPresenceConfidence = {config.MinHandPresenceConfidence}");
            Debug.Log($"MinTrackingConfidence = {config.MinTrackingConfidence}");

            await AssetLoader.PrepareAssetAsync(config.ModelPath);

            var options = config.GetHandLandmarkerOptions(config.RunningMode == RunningMode.LIVE_STREAM ? OnHandLandmarkDetectionOutput : null);
            taskApi = HandLandmarker.CreateFromOptions(options);
        }

        protected override void Detect(Image image, ImageProcessingOptions options)
        {
            taskApi.Detect(image, options);
        }

        protected override void DetectAsync(Image image, int timestamp, ImageProcessingOptions options)
        {
            taskApi.DetectAsync(image, timestamp, options);
        }

        protected override void DetectForVideo(Image image, int timestamp, ImageProcessingOptions options)
        {
            taskApi.DetectForVideo(image, timestamp, options);
        }

        private void OnHandLandmarkDetectionOutput(HandLandmarkerResult result, Image image, int timestamp)
        {
            this.result.Value = result;
        }
    }
}