/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using UnityEngine;

namespace VWorld.Data
{
    [Serializable]
    public class LandmarkPoint
    {
        [field: SerializeField]
        public Common.Direction Direction { get; private set; }

        [field: SerializeField]
        public int Index { get; private set; }
    }
}