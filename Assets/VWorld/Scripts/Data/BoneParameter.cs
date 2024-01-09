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

namespace VWorld.Data
{
    [Serializable]
    public class BoneParameter
    {
        [field: SerializeField]
        public Transform Bone { get; private set; }

        [field: SerializeField]
        public Vector3 MaxRotation { get; private set; }

        [field: SerializeField]
        public Vector3 MinRoataion { get; private set; }

        public void SetBoneRotation(Vector3 rotation)
        {
            Bone.eulerAngles = new Vector3(
                Mathf.Clamp(rotation.x, MinRoataion.x, MaxRotation.x),
                Mathf.Clamp(rotation.y, MinRoataion.y, MaxRotation.y),
                Mathf.Clamp(rotation.z, MinRoataion.z, MaxRotation.z)
            );
        }
        
    }
}