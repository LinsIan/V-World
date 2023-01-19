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
using Mediapipe.Unity;

namespace LiveSystem
{
    public abstract class LiveSystem : MonoBehaviour
    {
        // 這裡應該改成注入live mode的工廠，然後根據切換模式的時候可以去拿到對應的initializer
        // 再由這個initializer去創建solution、modelController、calculaters
        // 注入：modeldata facelandmarkkeypoints factory
        // 透過工廠生成：solution

        [SerializeField] protected Solution solution;
        [SerializeField] protected ModelData modelData;
        [SerializeField] protected FaceLandmarkKeyPoints keyPoints;
        protected ModelController modelController;
        protected List<Calculator> calculaters = new List<Calculator>();

        protected virtual IEnumerator Start()
        {
            yield return InitSubSystem();
        }

        protected virtual void Pause()
        {
            solution.Pause();
        }

        protected virtual void Stop()
        {
        }

        protected virtual void Update()
        {
            modelController.UpdateModel();
        }

        protected virtual IEnumerator InitSubSystem()
        {
            yield return modelController.Init();
        }

        public IEnumerator SetModelData(ModelData newData, Action callback = null)
        {
            yield return modelController.SetModelData(newData);
            callback?.Invoke();
        }

        public IEnumerator ChangeModel(int index, Action callback = null)
        {
            yield return modelController.ChangeModel(index);
            callback?.Invoke();
        }

        public void SetLiveMode(LiveMode mode)
        {
            modelController.SetLiveMode(mode);
        }   
    }
}