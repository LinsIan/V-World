/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VWorld.Data;
using VWorld.Common;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.HarmonicMotion;
using Cysharp.Threading.Tasks;
using VContainer;

namespace VWorld
{
    public class Live2DModelController : ModelController
    {
        protected CubismModel cubismModel;
        protected CubismHarmonicMotionController motionController;
        protected Dictionary<ParamId, CubismParameter> parameters;
        protected FaceData defaultFaceData;
        protected FaceData calibrationFaceData;
        protected Interpolator<FaceData> interpolator;

        [Inject]
        public Live2DModelController(ModelData modelData) : base(modelData)
        {
        }

        public override async UniTask Init()
        {
            await base.Init();
            interpolator = new Interpolator<FaceData>(FaceData.Lerp);
            cubismModel = modelObj.GetComponent<CubismModel>();
            motionController = modelObj.GetComponent<CubismHarmonicMotionController>();
            InitParameters();
            SetMotionRate();
            isPause = false;
            defaultFaceData = new FaceData(
                parameters[ParamId.ParamAngleX].DefaultValue,
                parameters[ParamId.ParamAngleY].DefaultValue,
                parameters[ParamId.ParamAngleZ].DefaultValue,
                parameters[ParamId.ParamEyeLOpen].DefaultValue,
                parameters[ParamId.ParamEyeROpen].DefaultValue,
                parameters[ParamId.ParamEyeBallX].DefaultValue, 
                parameters[ParamId.ParamEyeBallY].DefaultValue,
                parameters[ParamId.ParamMouthOpenY].DefaultValue,
                parameters[ParamId.ParamBodyAngleX].DefaultValue,
                parameters[ParamId.ParamBodyAngleY].DefaultValue,
                parameters[ParamId.ParamBodyAngleZ].DefaultValue
           );
        }

        public override void UpdateModel()
        {
            if (isPause || !interpolator.HasInputData) return;

            FaceData currentFaceData = interpolator.GetCurrentData();

            UpdateParamter(ParamId.ParamAngleX, currentFaceData.AngleX, calibrationFaceData.AngleX);
            UpdateParamter(ParamId.ParamAngleY, currentFaceData.AngleY, calibrationFaceData.AngleY);
            UpdateParamter(ParamId.ParamAngleZ, currentFaceData.AngleZ, calibrationFaceData.AngleZ);
            UpdateParamter(ParamId.ParamEyeLOpen, currentFaceData.EyeLOpen, calibrationFaceData.EyeLOpen);
            UpdateParamter(ParamId.ParamEyeROpen, currentFaceData.EyeROpen, calibrationFaceData.EyeROpen);
            UpdateParamter(ParamId.ParamEyeBallX, currentFaceData.EyeBallX);
            UpdateParamter(ParamId.ParamEyeBallY, currentFaceData.EyeBallY);
            UpdateParamter(ParamId.ParamMouthOpenY, currentFaceData.MouthOpenY);
            UpdateParamter(ParamId.ParamBodyAngleX, currentFaceData.BodyAngleX, calibrationFaceData.BodyAngleX);
            UpdateParamter(ParamId.ParamBodyAngleY, currentFaceData.BodyAngleY, calibrationFaceData.BodyAngleY);
            UpdateParamter(ParamId.ParamBodyAngleZ, currentFaceData.BodyAngleZ, calibrationFaceData.BodyAngleZ);


            for (int i = 0; i < modelData.Sensitivities.Count; i++)
            {
                var sensitivity = modelData.Sensitivities[i];

                for (int j = 0; j < sensitivity.EffectedParamIds.Count; j++)
                {
                    if (parameters.ContainsKey(sensitivity.EffectedParamIds[j]))
                    {
                        var id = sensitivity.EffectedParamIds[j];
                        ApplySensitivity(id, ref parameters[id].Value, sensitivity.Value);
                    }
                }
            }

            cubismModel.ForceUpdateNow();
        }

        public override void CalibrateModel()
        {
            if (!interpolator.HasInputData || isPause) return;
            FaceData currentFaceData = interpolator.GetCurrentData();
            calibrationFaceData = defaultFaceData - currentFaceData;
        }

        public override void SetLiveMode(LiveMode newMode)
        {
            liveMode = LiveMode.FaceOnly;
        }

        //called from thread
        public void OnFaceDataOutput(FaceData data)
        {
            interpolator?.UpdateData(data);
        }

        public void SetMotionRate()
        {
            if (motionController == null) return;
            
            for (int i = 0; i < modelData.MotionRates.Count; i++)
            {
                motionController.ChannelTimescales[i] = modelData.MotionRates[i].Value;
            }
        }

        protected void InitParameters()
        {
            parameters = new Dictionary<ParamId, CubismParameter>();
            var modelParamteters = cubismModel.Parameters;
            var values = (ParamId[])Enum.GetValues(typeof(ParamId));
            foreach (ParamId item in values)
            {
                string id = Enum.GetName(typeof(ParamId), item);
                var param = modelParamteters.FindById(id);
                if (param != null)
                {
                    parameters.Add(item, modelParamteters.FindById(id));
                }
            }
        }

        protected void UpdateParamter(ParamId id, float currentValue, float calibrationValue = 0)
        {
            parameters[id].Value = currentValue + calibrationValue;
        }
    }

}

