/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VWorld.Data;
using VWorld.Common;
using Cysharp.Threading.Tasks;
using VContainer;

namespace VWorld
{
    public class Home3DModelController : ModelController
    {
        protected Interpolator<FaceData> faceDataInterpolator;
        protected Home3DModel model;
        protected FaceData calibrationFaceData = default;

        [Inject]
        public Home3DModelController(ModelData modelData) : base(modelData)
        {
        }

        public override async UniTask Init()
        {
            await base.Init();   
            faceDataInterpolator = new Interpolator<FaceData>(FaceData.Lerp);
            model = modelObj.GetComponent<Home3DModel>();
            isPause = false;
        }

        public override void UpdateModel()
        {
            if (isPause || !faceDataInterpolator.HasInputData) return;

            var currentFaceData = faceDataInterpolator.GetCurrentData();
            var faceAngle = new Vector3(currentFaceData.AngleX + calibrationFaceData.AngleX, currentFaceData.AngleY + calibrationFaceData.AngleY, currentFaceData.AngleZ + calibrationFaceData.AngleZ);
            var bodyAngle = new Vector3(currentFaceData.BodyAngleX + calibrationFaceData.BodyAngleX, currentFaceData.BodyAngleY + calibrationFaceData.BodyAngleY, currentFaceData.BodyAngleZ + calibrationFaceData.BodyAngleZ);
            var eyeAngle = new Vector3(currentFaceData.EyeBallX + calibrationFaceData.EyeBallX, currentFaceData.EyeBallY + calibrationFaceData.EyeBallY, 0);
            var eyeLOpen = currentFaceData.EyeLOpen + calibrationFaceData.EyeLOpen;
            var eyeROpen = currentFaceData.EyeROpen + calibrationFaceData.EyeROpen;

			//bodyAngle.x = 0;
			bodyAngle.y = 0;
			bodyAngle.z = 0;
			Debug.Log(bodyAngle);

			//model.SetBoneRotation(ParamId.ParamNeck, faceAngle);
			model.SetBoneRotation(ParamId.ParamSpine, bodyAngle);
			//model.SetBoneRotation(ParamId.ParamLeftEye, eyeAngle);
			//model.SetBoneRotation(ParamId.ParamRightEye, eyeAngle);
			//model.SetBlendShapeValue(BlendShapePreset.Blink_L, eyeLOpen);
			//model.SetBlendShapeValue(BlendShapePreset.Blink_R, eyeROpen);
			//嘴型偵測 model.SetBlendShapeValue(BlendShapePreset.A, currentFaceData.MouthOpenY);

		}

        public override void SetLiveMode(LiveMode newMode)
        {
            liveMode = LiveMode.FaceOnly;
        }

        public override void CalibrateModel()
        {
            if (!faceDataInterpolator.HasInputData || isPause) return;
            FaceData currentFaceData = faceDataInterpolator.GetCurrentData();
            calibrationFaceData = -currentFaceData;
        }

        //called from thread
        public void OnFaceDataOutput(FaceData data)
        {
            faceDataInterpolator.UpdateData(data);
        }

    }
}