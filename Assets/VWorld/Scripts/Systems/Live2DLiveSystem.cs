/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;
using Cysharp.Threading.Tasks;
using VContainer;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.FaceLandmarker;
using UniRx;
using VWorld.Data;


namespace VWorld
{
    public class Live2DLiveSystem : LiveSystem
    {
        [Inject]
        public Live2DLiveSystem(ICalculator<FaceData> faceDataCalculator, ModelData modelData, Live2DModelController modelController, FaceLandmarkerRunner runner) 
        : base(modelData, modelController, runner)
        {
            runner.Result.Subscribe(result => 
            {
                if (result.faceLandmarks != null && result.faceLandmarks.Count > 0)
                {
                    faceDataCalculator.OnLandmarkDetectionOutput(result.faceLandmarks[0].landmarks);
                }
            }).AddTo(disposables);

            faceDataCalculator.LastestData.Subscribe(data => 
            {
                modelController.OnFaceDataOutput(data);
            }).AddTo(disposables);
        }
    }
}
