/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity.Holistic;

namespace LiveSystem
{
    public class Hom3DLiveSystem : LiveSystem
    {
        protected override IEnumerator InitSubSystem()
        {
            var newModelController = new Home3DModelController(modelData, LiveMode.FaceOnly);
            var graph = solution?.GetComponent<HolisticTrackingGraph>();
            var faceDataCalculater = new Home3DFaceDataCalculator(keyPoints);
            //lefthand、righthand
            //pose (world?)

            graph.OnFaceLandmarksOutput += faceDataCalculater.OnLandmarksOutput;
            // graph.OnLeftIrisLandmarksOutput += (faceDataCalculater.OnLeftIrisLandmarksOutput);
            // graph.OnRightHandLandmarksOutput += faceDataCalculater.OnRightIrisLandmarksOutput;
            faceDataCalculater.OnFaceDataOutput += newModelController.OnFaceDataOutput;
            calculaters.Add(faceDataCalculater);
            modelController = newModelController;
            
            yield return base.InitSubSystem();
        }
    }
}