// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.


using UnityEngine;
using Cysharp.Threading.Tasks;
using Mediapipe;
using Mediapipe.Tasks.Vision.Core;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;
using VContainer;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using UniRx;

namespace VWorld
{
    public class PoseLandmarkerRunner : TaskApiRunner<PoseLandmarker>
    {
        private readonly PoseLandmarkDetectionConfig config = new();
        public IReadOnlyReactiveProperty<PoseLandmarkerResult> Result => result;

        private ReactiveProperty<PoseLandmarkerResult> result = new();

        [Inject]
        public PoseLandmarkerRunner(Bootstrap bootstrap, Mediapipe.Unity.Screen screen) : base(bootstrap, screen)
        {
        }

        protected override async UniTask InitTaskApi()
        {
            Debug.Log($"Delegate = {config.Delegate}");
            Debug.Log($"Model = {config.ModelName}");
            Debug.Log($"Running Mode = {config.RunningMode}");
            Debug.Log($"NumPoses = {config.NumPoses}");
            Debug.Log($"MinPoseDetectionConfidence = {config.MinPoseDetectionConfidence}");
            Debug.Log($"MinPosePresenceConfidence = {config.MinPosePresenceConfidence}");
            Debug.Log($"MinTrackingConfidence = {config.MinTrackingConfidence}");
            Debug.Log($"OutputSegmentationMasks = {config.OutputSegmentationMasks}");

            await AssetLoader.PrepareAssetAsync(config.ModelPath);

            var options = config.GetPoseLandmarkerOptions(config.RunningMode == RunningMode.LIVE_STREAM ? OnPoseLandmarkDetectionOutput : null);
            taskApi = PoseLandmarker.CreateFromOptions(options);
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

        private void OnPoseLandmarkDetectionOutput(PoseLandmarkerResult result, Image image, int timestamp)
        {
            this.result.Value = result;
        }
    }
}