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
using UnityEngine.ResourceManagement.AsyncOperations;
using VWorld.Data;
using VWorld.Common;
using Cysharp.Threading.Tasks;
using VContainer;

namespace VWorld
{

    public abstract class ModelController
    {
        [Inject] protected ModelData modelData;

        protected readonly float SensitivityConstant = 0.5f;
        protected GameObject modelObj;
        protected AssetReferenceGameObject modelRef;
        protected LiveMode liveMode;
        protected bool isPause = true;

        public ModelController()
        {
        }

        public virtual async UniTask Init()
        {
            isPause = true;
            if (modelObj != null)
            {
                ReleaseModel();
            }
            modelObj = await InstantiateModel();
        }

        public virtual void UpdateModel()
        {
        }

        public virtual void CalibrateModel()
        {
        }

        public virtual void SetLiveMode(LiveMode newMode)
        {
        }

        public async UniTask SetModelData(ModelData newData)
        {
            modelData = newData;
            await Init();
        }

        public async UniTask ChangeModel(int index)
        {
            modelData.CurrentAsset = index;
            await Init();
        }

        protected async UniTask<GameObject> InstantiateModel()
        {
            modelRef = modelData.Assets[modelData.CurrentAsset].PrefabRef;
            var handle = await modelRef.InstantiateAsync();
            return handle;
        }

        protected void ReleaseModel()
        {
            modelRef.ReleaseInstance(modelObj);
        }

        protected void ApplySensitivity(ParamId id, ref float value, float sensitivity)
        {
            switch (id)
            {
                case ParamId.ParamEyeLOpen:
                case ParamId.ParamEyeROpen:
                case ParamId.ParamMouthOpenY:
                    var constantRate = (sensitivity >= 1) ? (sensitivity - 1) : 0;
                    value = Mathf.Clamp01((sensitivity * value) - (constantRate * SensitivityConstant));
                    break;
                default:
                    value *= sensitivity;
                    break;
            }

            
        }
    }
}