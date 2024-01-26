/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using VWorld.Data;
using VWorld.Common;
using VContainer;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mediapipe.Tasks.Components.Containers;
using System;


namespace VWorld
{
    public class Live2DFaceDataCalculator : FaceDataCalculater
    {
        [Inject]
        public Live2DFaceDataCalculator(FaceLandmarkKeyPoints keyPoints) : base(keyPoints) {}

        protected override FaceData Calculate()
        {
            bool isNotWink = (EyeLOpen - EyeROpen <= WinkEyeDistance) && (EyeROpen - EyeLOpen <= WinkEyeDistance);
            FiltKeyPoints();
            Vector3 eulerAngle = GetFaceEulerAngles(filteredKeyPoints[MidFaceDirectionPointIndex], filteredKeyPoints[LeftFaceDirectionPointIndex], filteredKeyPoints[RightFaceDirectionPointIndex]);

            if (eulerAngle.y > 180)
            {
                eulerAngle.y -= 360;
            }

            eulerAngle.x *= -1;

            if (eulerAngle.x < -180)
            {
                eulerAngle.x += 360;
            }

            eulerAngle.z *= -1;
            
            if (eulerAngle.z < -180)
            {
                eulerAngle.z += 360;
            }

            var leftEye = GetCenterPoint(keyPoints.LeftEyePoints);
            var leftIrys = filteredKeyPoints[LeftIrisPointIndex];
            var eyeBallX = (leftIrys.x - leftEye.x) * -landmarkScale;
            var eyeBallY = (leftIrys.y - leftEye.y) * -landmarkScale;
            var bodyAngleX = eulerAngle.y / BodyRate;
            var bodyAngleY = eulerAngle.x / BodyRate;
            var bodyAngleZ = eulerAngle.z / BodyRate;

            return new FaceData
            (
                eulerAngle.y,
                eulerAngle.x,
                eulerAngle.z,
                EyeLOpen,
                isNotWink ? EyeLOpen : EyeROpen,
                eyeBallX,
                eyeBallY,
                MouthOpenY,
                bodyAngleX,
                bodyAngleY,
                bodyAngleZ
            );
        }
    }
}