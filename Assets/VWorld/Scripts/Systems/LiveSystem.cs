/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections;
using Mediapipe.Tasks.Vision.Core;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using VContainer;
using VWorld.Common;
using VContainer.Unity;

namespace VWorld
{
    [DefaultExecutionOrder(2)]
    public abstract class LiveSystem : ITickable, IStartable
    {
        // 由子類別去聲明需要注入的calculater、modelController、graph，和定義要怎麼Init
        // calculator和modelcontroller需要去聲明需要注入的資料(modelData、keypoints)，這樣liveSystem就不需要持有這些資料的類別
        protected ModelData modelData;
        protected ModelController modelController;
        protected ITaskApiRunner runner;

        protected List<Calculator> calculaters = new List<Calculator>();

        public LiveSystem(ModelData modelData, ModelController modelController, ITaskApiRunner runner)
        {
            this.modelData = modelData;
            this.modelController = modelController;
            this.runner = runner;
        }

        public virtual async void Start()
        {
            await InitSubSystem();
        }

        protected virtual void Pause()
        {
            runner.Pause();
        }

        protected virtual void Resume()
        {
            runner.Resume();
        }

        protected virtual void Stop()
        {
            runner.Stop();
        }

        public void Tick()
        {
            modelController.UpdateModel();
        }

        protected virtual async UniTask InitSubSystem()
        {
            await modelController.Init();
        }

        public async void SetModelData(ModelData newData, Action callback = null)
        {
            await modelController.SetModelData(newData);
            callback?.Invoke();
        }

        public async void ChangeModel(int index, Action callback = null)
        {
            await modelController.ChangeModel(index);
            callback?.Invoke();
        }

        public void SetLiveMode(LiveMode mode)
        {
            modelController.SetLiveMode(mode);
        }

    }
}