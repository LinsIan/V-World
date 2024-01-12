// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using UnityEngine;
using Mediapipe.Tasks.Vision.FaceLandmarker;
using Cysharp.Threading.Tasks;
using Mediapipe;
using Mediapipe.Tasks.Vision.Core;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using VContainer;

namespace VWorld
{
    public class FaceLandmarkerRunner : TaskApiRunner<FaceLandmarker>
    {
        private readonly FaceLandmarkDetectionConfig config = new();

        [Inject]
        public FaceLandmarkerRunner(Bootstrap bootstrap, Mediapipe.Unity.Screen screen) : base(bootstrap, screen)
        {
        }

        protected override async UniTask InitTaskApi()
        {
            Debug.Log($"Delegate = {config.Delegate}");
            Debug.Log($"Running Mode = {config.RunningMode}");
            Debug.Log($"NumFaces = {config.NumFaces}");
            Debug.Log($"MinFaceDetectionConfidence = {config.MinFaceDetectionConfidence}");
            Debug.Log($"MinFacePresenceConfidence = {config.MinFacePresenceConfidence}");
            Debug.Log($"MinTrackingConfidence = {config.MinTrackingConfidence}");
            Debug.Log($"OutputFaceBlendshapes = {config.OutputFaceBlendshapes}");
            Debug.Log($"OutputFacialTransformationMatrixes = {config.OutputFacialTransformationMatrixes}");

            await AssetLoader.PrepareAssetAsync(config.ModelPath);

            var options = config.GetFaceLandmarkerOptions(config.RunningMode == RunningMode.LIVE_STREAM ? OnFaceLandmarkDetectionOutput : null);
            taskApi = FaceLandmarker.CreateFromOptions(options);
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

        private void OnFaceLandmarkDetectionOutput(FaceLandmarkerResult result, Image image, int timestamp)
        {
        }

    }
}