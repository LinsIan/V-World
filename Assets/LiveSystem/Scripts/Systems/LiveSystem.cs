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
        // 由子類別去聲明需要注入的calculater、modelController、graph，和定義要怎麼Init
        // calculator和modelcontroller需要去聲明需要注入的資料(modelData、keypoints)，這樣liveSystem就不需要持有這些資料的類別
        // 由組合根去抓graph然後註冊
        [InjectField] protected Solution solution;
        [InjectField] protected ModelData modelData;
        [InjectField] protected ModelController modelController;

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