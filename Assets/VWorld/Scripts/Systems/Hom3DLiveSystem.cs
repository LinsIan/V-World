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

namespace VWorld
{
    public class Hom3DLiveSystem : LiveSystem
    {
        [Inject] private HolisticTrackingGraph graph;
        [Inject] private Home3DFaceDataCalculator faceDataCalculater;

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