/**
 * Copyright (c) 2023 LinsIan
undefined
undefinedUse of this source code is governed by an MIT-style license that can be
undefinedfound in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using LiveSystem;
using Mediapipe;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TestLifeTimeScope : LifetimeScope
{
    [SerializeField] private FaceLandmarkKeyPoints faceLandmarkKeyPoints;
    [SerializeField] Launcher launcher;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterInstance(faceLandmarkKeyPoints);
        builder.Register<Connector>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();
        builder.RegisterComponent(launcher);
        builder.RegisterEntryPoint<LauncherEP>();
    }
}
