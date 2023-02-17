/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LiveSystem
{
    [DefaultExecutionOrder(-1)]
    public abstract class DependenciesContext : MonoBehaviour
    {
        protected DependenciesCollection dependenciesCollection = new DependenciesCollection();
        protected DependenciesProvider dependenciesProvider;

        private void Awake() {
            DontDestroyOnLoad(gameObject);
            Setup();
            dependenciesProvider = new DependenciesProvider(dependenciesCollection);

            var children = GetComponentsInChildren<MonoBehaviour>(true);
            foreach (var child in children)
            {
                dependenciesProvider.Inject(child);
            }

            Configure();
        }

        protected abstract void Setup();

        protected abstract void Configure();
    }
}

