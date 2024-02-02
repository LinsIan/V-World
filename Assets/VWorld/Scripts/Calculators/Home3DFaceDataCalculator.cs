/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VWorld.Data;

namespace VWorld
{
    public class Home3DFaceDataCalculator : FaceDataCalculater
    {
        protected int NosePointIndex;

        public Home3DFaceDataCalculator(FaceLandmarkKeyPoints keyPoints) : base(keyPoints)
        {
        }

        protected override void SetFilteredKeyPointsIndex() 
        {
            NosePointIndex = keyPoints.NosePoint;
            FilteredkeyPointsIndex.Add(NosePointIndex);
            base.SetFilteredKeyPointsIndex();
        }

        protected override FaceData Calculate()
        {
            bool isNotWink = (EyeLOpen - EyeROpen <= WinkEyeDistance) && (EyeROpen - EyeLOpen <= WinkEyeDistance);
            FiltKeyPoints();

            var eulerAngle = GetFaceEulerAngles(filteredKeyPoints[MidFaceDirectionPointIndex], filteredKeyPoints[LeftFaceDirectionPointIndex], filteredKeyPoints[RightFaceDirectionPointIndex]);

			if (eulerAngle.x > 180)
			{
				eulerAngle.x -= 360;
			}

            eulerAngle.y *= -1;
			if (eulerAngle.y < -180)
            {
                eulerAngle.y += 360;
            }

            eulerAngle.z *= -1;
            if (eulerAngle.z < -180)
            {
                eulerAngle.z += 360;
            }

            /*
                body:
                    X軸左到右1 ~ 0
                    Z軸遠到近 0 ~ -1
                    Y軸 1/5的facez
                    基準點 (0.5, 0, -0.5)
             */

            var leftEye = GetCenterPoint(keyPoints.LeftEyePoints);
            var leftIrys = filteredKeyPoints[LeftIrisPointIndex];
            var eyeBallX = (leftIrys.x - leftEye.x) * -landmarkScale;
            var eyeBallY = (leftIrys.y - leftEye.y) * -landmarkScale;

            var nose = filteredKeyPoints[NosePointIndex];
            var noseDirectoin = new Vector3(nose.x - 0.5f, nose.y, nose.z + 0.5f);
            var bodyAngle = Quaternion.FromToRotation(Vector3.up, noseDirectoin).eulerAngles;

            //var bodyAngleX = bodyAngle.x;
            //var bodyAngleY = eulerAngle.y / BodyRate;
            //var bodyAngleZ = bodyAngle.z;

            return new FaceData
            (
                eulerAngle.x,
                eulerAngle.y,
                eulerAngle.z,
                EyeLOpen,
                isNotWink ? EyeLOpen : EyeROpen,
                eyeBallX,
                eyeBallY,
                MouthOpenY,
                bodyAngle.x,
                bodyAngle.y,
                bodyAngle.z
            );
        }
    }
}