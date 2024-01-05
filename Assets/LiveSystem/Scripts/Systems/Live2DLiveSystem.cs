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
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.IrisTracking;
using System.Linq;
using Cysharp.Threading.Tasks;
using VContainer;

namespace LiveSystem
{
    public class Live2DLiveSystem : LiveSystem
    {
        [Inject] private IrisTrackingGraph graph;
        [Inject] private Live2DFaceDataCalculator faceDataCalculator;

        protected override async UniTask InitSubSystem()
        {
            calculaters.Add(faceDataCalculator);

            await UniTask.WaitForEndOfFrame(graph);

            graph.OnFaceLandmarksWithIrisOutput += faceDataCalculator.OnLandmarksOutput;

            if (modelController is Live2DModelController controller)
            {
                faceDataCalculator.OnFaceDataOutput += controller.OnFaceDataOutput;
            }

            await base.InitSubSystem();
        }
    }
}
