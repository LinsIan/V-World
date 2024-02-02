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
using UniRx;

namespace VWorld
{
    [DefaultExecutionOrder(2)]
    public abstract class LiveSystem : IStartable, ITickable, IDisposable
    {
        // 由子類別去聲明需要注入的calculater、modelController、graph，和定義要怎麼Init
        // calculator和modelcontroller需要去聲明需要注入的資料(modelData、keypoints)，這樣liveSystem就不需要持有這些資料的類別
        protected ModelData modelData;
        protected ModelController modelController;
        protected ITaskApiRunner runner;
        protected CompositeDisposable disposables = new CompositeDisposable();
        protected bool isRunning = false;

        public LiveSystem(ModelData modelData, ModelController modelController, ITaskApiRunner runner)
        {
            this.modelData = modelData;
            this.modelController = modelController;
            this.runner = runner;
        }

        public virtual async void Start()
        {
            isRunning = false;
            await InitSubSystem();
            isRunning = true;
        }

        protected virtual void Pause()
        {
            runner.Pause();
            isRunning = false;
        }

        protected virtual void Resume()
        {
            runner.Resume();
            isRunning = true;
        }

        protected virtual void Stop()
        {
            runner.Stop();
            isRunning = false;
        }

        public void Tick()
        {
            if (!isRunning)
            {
                return;
            }

            modelController.UpdateModel();
        }

        protected virtual async UniTask InitSubSystem()
        {
            await modelController.Init();
        }

        public async void SetModelData(ModelData newData)
        {
            Pause();
            await modelController.SetModelData(newData);
            Resume();
        }

        public async void ChangeModel(int index)
        {
            Pause();
            await modelController.ChangeModel(index);
            Resume();
        }

        public void SetLiveMode(LiveMode mode)
        {
            modelController.SetLiveMode(mode);
        }

        public void Dispose()
        {
            disposables.Dispose();
        }
    }
}