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
    public class DependenciesCollection : IEnumerable<Dependency>
    {
        private List<Dependency> dependencies = new List<Dependency>();
        //註冊
        public void Add(Dependency dependency) => dependencies.Add(dependency);

        public IEnumerator<Dependency> GetEnumerator() => dependencies.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => dependencies.GetEnumerator();
    }
}
