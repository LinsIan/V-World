/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using Mediapipe.Tasks.Vision.Core;
using UniRx;

namespace VWorld
{
    public class Hom3DLiveSystem : LiveSystem
    {
        [Inject]
        public Hom3DLiveSystem(Home3DFaceDataCalculator faceDataCalculator, ModelData modelData, Home3DModelController modelController, FaceLandmarkerRunner runner) 
        : base(modelData, modelController, runner)
        {
            runner.Result.Subscribe(result => 
            {
                if (result.faceLandmarks != null && result.faceLandmarks.Count > 0)
                {
                    faceDataCalculator.OnLandmarkDetectionOutput(result.faceLandmarks[0].landmarks);
                }
            });

            faceDataCalculator.LastestData.Subscribe(data => 
            {
                modelController.OnFaceDataOutput(data);
            });
        }
    }
}