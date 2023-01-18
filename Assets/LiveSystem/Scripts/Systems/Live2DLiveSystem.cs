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
        protected override IEnumerator InitSubSystem()
        {
            var newModelController = new Live2DModelController(modelData);
            var graph = solution?.GetComponent<IrisTrackingGraph>();
            var faceDataCalculater = new Live2DFaceDataCalculator(keyPoints);

            graph.OnFaceLandmarksWithIrisOutput += faceDataCalculater.OnLandmarksOutput;
            faceDataCalculater.OnFaceDataOutput += newModelController.OnFaceDataOutput;
            calculaters.Add(faceDataCalculater);
            modelController = newModelController;

            yield return base.InitSubSystem();
        }
    }
}
