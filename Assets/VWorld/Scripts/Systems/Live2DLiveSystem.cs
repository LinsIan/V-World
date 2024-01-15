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
using Mediapipe.Tasks.Vision.Core;

namespace VWorld
{
    public class Live2DLiveSystem : LiveSystem
    {
        private Live2DFaceDataCalculator faceDataCalculator;
        
        [Inject]
        public Live2DLiveSystem(ModelData modelData, ModelController modelController, FaceLandmarkerRunner runner, Live2DFaceDataCalculator faceDataCalculator) 
        : base(modelData, modelController, runner)
        {
            this.faceDataCalculator = faceDataCalculator;
            calculaters.Add(faceDataCalculator);
        }

        protected override async UniTask InitSubSystem()
        {
            calculaters.Add(faceDataCalculator);

            graph.OnFaceLandmarksWithIrisOutput += faceDataCalculator.OnLandmarksOutput;

            if (modelController is Live2DModelController controller)
            {
                faceDataCalculator.OnFaceDataOutput += controller.OnFaceDataOutput;
            }

            await base.InitSubSystem();
        }
    }
}
