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
using Mediapipe.Unity;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity.Sample.FaceLandmarkDetection;
using VContainer;
using UniRx;

namespace VWorld
{
    using RunningMode = Mediapipe.Tasks.Vision.Core.RunningMode;

    public class FaceLandmarkerRunner : TaskApiRunner<FaceLandmarker>
    {
        public readonly FaceLandmarkDetectionConfig config = new();
        public IReadOnlyReactiveProperty<FaceLandmarkerResult> Result => result;

        private ReactiveProperty<FaceLandmarkerResult> result = new();
        private FaceLandmarkerResultAnnotationController annotationController;

        [Inject]
        public FaceLandmarkerRunner(FaceLandmarkerResultAnnotationController annotationController, Bootstrap bootstrap, Mediapipe.Unity.Screen screen) : base(bootstrap, screen)
        {
            this.annotationController = annotationController;
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

        protected override void SetupAnnotationController(ImageSource imageSource ,bool expectedToBeMirrored = false)
        {
            annotationController.isMirrored = expectedToBeMirrored;
            annotationController.imageSize = new Vector2Int(imageSource.textureWidth, imageSource.textureHeight);
        }

        protected override void Detect(Image image, ImageProcessingOptions options)
        {
            var result = taskApi.Detect(image, options);
            annotationController.DrawNow(result);
        }

        protected override void DetectForVideo(Image image, int timestamp, ImageProcessingOptions options)
        {
            var result = taskApi.DetectForVideo(image, timestamp, options);
            annotationController.DrawNow(result);
        }

        protected override void DetectAsync(Image image, int timestamp, ImageProcessingOptions options)
        {
            taskApi.DetectAsync(image, timestamp, options);
        }

        private void OnFaceLandmarkDetectionOutput(FaceLandmarkerResult result, Image image, int timestamp)
        {
            this.result.Value = result;
            annotationController.DrawLater(result);
        }

    }
}