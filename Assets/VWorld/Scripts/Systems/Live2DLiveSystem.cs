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


namespace VWorld
{
    public class Live2DLiveSystem : LiveSystem
    {
        private Live2DFaceDataCalculator faceDataCalculator;
        
        [Inject]
        public Live2DLiveSystem(Live2DFaceDataCalculator faceDataCalculator, ModelData modelData, ModelController modelController, FaceLandmarkerRunner runner) 
        : base(modelData, modelController, runner)
        {
            this.faceDataCalculator = faceDataCalculator;
            runner.Result.Subscribe(result => 
            {
                if (result.faceLandmarks != null && result.faceLandmarks.Count > 0)
                {
                    faceDataCalculator.OnLandmarkDetectionOutput(result.faceLandmarks[0].landmarks);
                }
            });
        }

        protected override async UniTask InitSubSystem()
        {

            // graph.OnFaceLandmarksWithIrisOutput += faceDataCalculator.OnLandmarksOutput;

            // if (modelController is Live2DModelController controller)
            // {
            //     faceDataCalculator.OnFaceDataOutput += controller.OnFaceDataOutput;
            // }

            await base.InitSubSystem();
        }
    }
}
