/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using Mediapipe;
using VWorld.Data;
using VWorld.Common;
using VContainer;
using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;

namespace VWorld
{
    public class Live2DFaceDataCalculator : FaceDataCalculater
    {
        [Inject]
        public Live2DFaceDataCalculator(FaceLandmarkKeyPoints keyPoints) : base(keyPoints)
        {
        }

        protected override FaceData Calculate(IReadOnlyList<NormalizedLandmarks> data)
        {
            return base.Calculate(data);
        }

        protected override FaceData Calculate(NormalizedLandmarkList data)
        {
            var landmarks = data.Landmark;
            var eyeLOpen = (landmarks[keyPoints.LeftEyePoints[Direction.Down]].Y - landmarks[keyPoints.LeftEyePoints[Direction.Up]].Y) * landmarkScale - EyeOpenConstanst;
            var eyeROpen = (landmarks[keyPoints.RightEyePoints[Direction.Down]].Y - landmarks[keyPoints.RightEyePoints[Direction.Up]].Y) * landmarkScale - EyeOpenConstanst;
            var mouthOpenY = (landmarks[keyPoints.InnerLipsPoints[Direction.Down]].Y - landmarks[keyPoints.InnerLipsPoints[Direction.Up]].Y) * landmarkScale - MouthOpenConstanst;
            if (eyeLOpen - eyeROpen <= WinkEyeDistance && eyeROpen - eyeLOpen <= WinkEyeDistance)
            {
                eyeROpen = eyeLOpen;
            }

            FiltData(data);

            var nose = landmarks[keyPoints.NosePoint];
            var eulerAngle = GetFaceEulerAngles(landmarks[keyPoints.FaceDirectionPoints[Direction.Mid]], landmarks[keyPoints.FaceDirectionPoints[Direction.Left]], landmarks[keyPoints.FaceDirectionPoints[Direction.Right]]);

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

            //rightEye = GetCenterPoint(keyPoints.RightEyePoints, data);
            var leftEye = GetCenterPoint(keyPoints.LeftEyePoints, data);
            var eyeBallX = (landmarks[keyPoints.LeftIrisPoint].X - leftEye.x) * -landmarkScale;
            var eyeBallY = (landmarks[keyPoints.LeftIrisPoint].Y - leftEye.y) * -landmarkScale;
            var bodyAngleX = eulerAngle.y / BodyRate;
            var bodyAngleY = eulerAngle.x / BodyRate;
            var bodyAngleZ = eulerAngle.z / BodyRate;

            return new FaceData(eulerAngle.y, eulerAngle.x, eulerAngle.z, eyeLOpen, eyeROpen, eyeBallX, eyeBallY, mouthOpenY, bodyAngleX, bodyAngleY, bodyAngleZ);
        }
    }
}