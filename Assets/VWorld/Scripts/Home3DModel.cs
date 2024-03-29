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


namespace VWorld
{
    //runtime home 3d model
    public class Home3DModel : MonoBehaviour
    {
        [SerializeField] private BoneParameter neck;
        [SerializeField] private BoneParameter spine;
        [SerializeField] private BoneParameter leftEye;
        [SerializeField] private BoneParameter rightEye;

        private Dictionary<ParamId, BoneParameter> parameters;

        private void Awake()
        {
            parameters = new Dictionary<ParamId, BoneParameter>()
            {
                { ParamId.ParamNeck, neck },
                { ParamId.ParamSpine, spine },
                { ParamId.ParamLeftEye, leftEye },
                { ParamId.ParamRightEye, rightEye }
            };
        }

        public void SetBoneRotation(ParamId paramId, Vector3 rotation)
        {
            parameters[paramId].SetBoneRotation(rotation);
        }

        public void SetBlendShapeValue(float value)
        {
            //TODO:設定BlendShape
        }

        public void SetBlendShapeValueSmoothly(float value)
        {
            //TODO:表情功能變化
        }
    }
}