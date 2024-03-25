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
using VWorld.Common;
using UniRx;
using Mediapipe.Tasks.Components.Containers;

namespace VWorld
{
    public class FaceDataCalculater : ICalculator<FaceData>
    {
        public ReactiveProperty<FaceData> LastestData { get; } = new ReactiveProperty<FaceData>();
        
        protected FaceLandmarkKeyPoints keyPoints;
        protected IReadOnlyList<NormalizedLandmark> landmarks;
        protected List<int> FilteredkeyPointsIndex = new List<int>();
        protected List<ScalarKalmanFilter> filters = new List<ScalarKalmanFilter>();
        protected Dictionary<int, Vector3> filteredKeyPoints = new Dictionary<int, Vector3>();

        protected readonly float landmarkScale = 100;
        protected readonly float BodyRate = 3;
        protected readonly float EyeOpenConstanst = 0.7f;
        protected readonly float WinkEyeDistance = 0.3f;
        protected readonly float MouthOpenConstanst = 0.4f;

        protected int MidFaceDirectionPointIndex;
        protected int LeftFaceDirectionPointIndex;
        protected int RightFaceDirectionPointIndex;
        protected int LeftIrisPointIndex;

        protected float EyeLOpen =>
        (landmarks[keyPoints.LeftEyePoints[Direction.Down]].y - landmarks[keyPoints.LeftEyePoints[Direction.Up]].y) * landmarkScale - EyeOpenConstanst;
        
        protected float EyeROpen =>
        (landmarks[keyPoints.RightEyePoints[Direction.Down]].y - landmarks[keyPoints.RightEyePoints[Direction.Up]].y) * landmarkScale - EyeOpenConstanst;

        protected float MouthOpenY =>
        (landmarks[keyPoints.InnerLipsPoints[Direction.Down]].y - landmarks[keyPoints.InnerLipsPoints[Direction.Up]].y) * landmarkScale - MouthOpenConstanst;

        public FaceDataCalculater(FaceLandmarkKeyPoints keyPoints)
        {
            this.keyPoints = keyPoints;
            SetFilteredKeyPointsIndex();

            for (int i = 0; i < FilteredkeyPointsIndex.Count; i++)
            {
                filters.Add(new ScalarKalmanFilter());
                filteredKeyPoints.Add(FilteredkeyPointsIndex[i], Vector3.zero);
            }
        }

        public void OnLandmarkDetectionOutput(IReadOnlyList<NormalizedLandmark> landmarks)
        {
            this.landmarks = landmarks;
            LastestData.Value = Calculate();
        }

        protected virtual void SetFilteredKeyPointsIndex() 
        {
            MidFaceDirectionPointIndex = keyPoints.FaceDirectionPoints[Direction.Mid];
            LeftFaceDirectionPointIndex = keyPoints.FaceDirectionPoints[Direction.Left];
            RightFaceDirectionPointIndex = keyPoints.FaceDirectionPoints[Direction.Right];
            LeftIrisPointIndex = keyPoints.LeftIrisPoint;

            FilteredkeyPointsIndex.Add(MidFaceDirectionPointIndex);
            FilteredkeyPointsIndex.Add(LeftFaceDirectionPointIndex);
            FilteredkeyPointsIndex.Add(RightFaceDirectionPointIndex);
            FilteredkeyPointsIndex.Add(LeftIrisPointIndex);

            foreach (var point in keyPoints.LeftEyePoints)
            {
                FilteredkeyPointsIndex.Add(point.Value);
            }
        }

        protected virtual FaceData Calculate()
        {
            return new FaceData();
        }

        protected Vector3 GetCenterPoint(IDictionary<Direction, int> pointsIndex, bool useFilteredPoint = true)
        {
            var sum = Vector3.zero;
            
            if (useFilteredPoint)
            {
                foreach (var index in pointsIndex)
                {
                    sum += filteredKeyPoints[index.Value];
                }
            }
            else
            {
                foreach (var index in pointsIndex)
                {
                    sum += landmarks[index.Value].ToVector3();
                }
            }

            return sum / pointsIndex.Count;
        }

        protected Vector3 GetFaceEulerAngles(Vector3 midPoint, Vector3 rightPoint, Vector3 leftPoint)
        {
            //angle X&Y
            var faceDirection = midPoint - (rightPoint + leftPoint) / 2;
            var angle = Quaternion.FromToRotation(Vector3.back, faceDirection.normalized).eulerAngles;

            //angle Z
            var skewVector = leftPoint - rightPoint;
            skewVector.z = 0;
            angle.z = Quaternion.FromToRotation(Vector3.left, skewVector).eulerAngles.z;
            return angle;
        }

        protected void FiltKeyPoints()
        {
            for (int i = 0; i < FilteredkeyPointsIndex.Count; i++)
            {
                int index = FilteredkeyPointsIndex[i];
                Vector3 point = landmarks[index].ToVector3();
                Vector3 filt = filters[i].Filt(point);

                filteredKeyPoints[index] = filt;
            }
        }
    }
}