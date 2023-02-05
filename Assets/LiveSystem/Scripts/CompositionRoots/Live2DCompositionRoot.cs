/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using Mediapipe.Unity.IrisTracking;
using UnityEngine;
using Mediapipe.Unity;

namespace LiveSystem
{
    public class Live2DCompositionRoot : DependenciesContext
    {
        [SerializeField] Solution solution;
        [SerializeField] FaceLandmarkKeyPoints keyPoints;
        [SerializeField] ModelData modelData;

        protected override void Setup()
        {
            dependenciesCollection.Add(new Dependency { Type = typeof(Solution), Factory = DependencyFactory.FromNoInjectionObject(solution), IsSingleton = true });
            dependenciesCollection.Add(new Dependency { Type = typeof(FaceLandmarkKeyPoints), Factory = DependencyFactory.FromNoInjectionObject(keyPoints), IsSingleton = true });
            dependenciesCollection.Add(new Dependency { Type = typeof(ModelData), Factory = DependencyFactory.FromNoInjectionObject(modelData), IsSingleton = true });
            
            var graph = solution.GetComponent<IrisTrackingGraph>();
            dependenciesCollection.Add(new Dependency { Type = typeof(IrisTrackingGraph), Factory = DependencyFactory.FromGameObject(graph), IsSingleton = true });
            
            dependenciesCollection.Add(new Dependency { Type = typeof(Live2DFaceDataCalculator), Factory = DependencyFactory.FromClass<Live2DFaceDataCalculator>(), IsSingleton = true });
            dependenciesCollection.Add(new Dependency { Type = typeof(ModelController), Factory = DependencyFactory.FromClass<Live2DModelController>(), IsSingleton = true });
        }

        protected override void Configure()
        {
        }
    }
    
}

