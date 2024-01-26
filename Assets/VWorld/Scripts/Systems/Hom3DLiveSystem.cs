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
        private Home3DFaceDataCalculator faceDataCalculater;
        
        [Inject]
        public Hom3DLiveSystem(Home3DFaceDataCalculator faceDataCalculator, ModelData modelData, ModelController modelController, FaceLandmarkerRunner runner) 
        : base(modelData, modelController, runner)
        {
            this.faceDataCalculater = faceDataCalculator;
            runner.Result.Subscribe(result => 
            {
                if (result.faceLandmarks != null && result.faceLandmarks.Count > 0)
                {
                    faceDataCalculater.OnLandmarkDetectionOutput(result.faceLandmarks[0].landmarks);
                }
            });
        }

        protected override async UniTask InitSubSystem()
        {
            // if (modelController is Home3DModelController controller)
            // {
            //     faceDataCalculater.OnFaceDataOutput += controller.OnFaceDataOutput;
            // }

            await base.InitSubSystem();
        }
    }
}