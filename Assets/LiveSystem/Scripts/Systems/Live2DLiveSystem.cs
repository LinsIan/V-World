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
using Mediapipe.Unity.IrisTracking;
using System.Linq;

namespace LiveSystem
{
    public class Live2DLiveSystem : LiveSystem
    {
        [InjectField] private IrisTrackingGraph graph;
        [InjectField] private Live2DFaceDataCalculator faceDataCalculator;

        protected override IEnumerator InitSubSystem()
        {
            calculaters.Add(faceDataCalculator);
            graph.OnFaceLandmarksWithIrisOutput += faceDataCalculator.OnLandmarksOutput;

            if (modelController is Live2DModelController controller)
            {
                faceDataCalculator.OnFaceDataOutput += controller.OnFaceDataOutput;
            }

            yield return base.InitSubSystem();
        }
    }
}
