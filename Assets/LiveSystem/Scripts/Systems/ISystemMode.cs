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
using Mediapipe;
using Mediapipe.Unity;
using Cysharp.Threading.Tasks;

namespace LiveSystem
{
    public interface ISystemMode
    {
        public async UniTask InitSubSystem(Solution solution, ModelData modelData, ModelController modelController, List<Calculator> calculaters)
        {
            
        }
    }
}