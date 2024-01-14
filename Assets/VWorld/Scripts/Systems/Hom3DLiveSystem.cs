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
using Mediapipe.Unity.Sample.Holistic;
using VContainer;
using Mediapipe.Tasks.Vision.Core;

namespace VWorld
{
    public class Hom3DLiveSystem : LiveSystem
    {
        [Inject] private HolisticTrackingGraph graph;
        [Inject] private Home3DFaceDataCalculator faceDataCalculater;
        
        [Inject]
        public Hom3DLiveSystem(ModelData modelData, ModelController modelController, ITaskApiRunner runner) : base(modelData, modelController, runner)
        {
        }

        protected override async UniTask InitSubSystem()
        {
            calculaters.Add(faceDataCalculater);
            graph.OnFaceLandmarksOutput += faceDataCalculater.OnLandmarksOutput;
            // graph.OnLeftIrisLandmarksOutput += (faceDataCalculater.OnLeftIrisLandmarksOutput);
            // graph.OnRightHandLandmarksOutput += faceDataCalculater.OnRightIrisLandmarksOutput;
            
            if (modelController is Home3DModelController controller)
            {
                faceDataCalculater.OnFaceDataOutput += controller.OnFaceDataOutput;
            }

            await base.InitSubSystem();
        }
    }
}