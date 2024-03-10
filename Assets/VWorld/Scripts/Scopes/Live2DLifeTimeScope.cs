/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VWorld;
using VWorld.Data;
using Mediapipe.Unity.Sample;
using Mediapipe.Unity;


public class Live2DLifeTimeScope : LifetimeScope
{
    // TODO: mdoelData應該變成從使用者存檔的資料中讀取
    [SerializeField] private ModelData modelData;
    [SerializeField] private FaceLandmarkKeyPoints faceLandmarkKeyPoints;
    [SerializeField] private Mediapipe.Unity.Screen screen;
    [SerializeField] private Bootstrap bootstrapPrefab;
    [SerializeField] private FaceLandmarkerResultAnnotationController faceLandmarkerResultAnnotationController;
    

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponentInNewPrefab(bootstrapPrefab, Lifetime.Singleton);
        builder.RegisterInstance(screen);
        builder.RegisterInstance(faceLandmarkKeyPoints);
        builder.RegisterInstance(modelData);
        builder.RegisterInstance(faceLandmarkerResultAnnotationController);

        builder.Register<Live2DModelController>(Lifetime.Singleton).As<Live2DModelController, ModelController>();
        builder.Register<ICalculator<FaceData>, Live2DFaceDataCalculator>(Lifetime.Singleton);

        builder.RegisterEntryPoint<FaceLandmarkerRunner>().As<ITaskApiRunner, FaceLandmarkerRunner>();
        builder.RegisterEntryPoint<Live2DLiveSystem>();
    }
}
