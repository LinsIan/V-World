/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VWorld.Data;
using VWorld.Common;

namespace VWorld
{
    [CreateAssetMenu(fileName = "ModelData", menuName = "ScriptableObjects/ModelData", order = 1)]
    public class ModelData : ScriptableObject
    {
        [field: SerializeField]
        public int ID { get; private set; }

        [field: SerializeField]
        public string Description { get; private set; }

        [field: SerializeField]
        public ModelType ModelType { get; private set; }

        [field: SerializeField]
        public List<ModelAsset> Assets { get; private set; }

        [field: SerializeField]
        public List<SettingValue> Sensitivities { get; private set; }

        [field: SerializeField]
        public List<SettingValue> MotionRates { get; private set; }

        [SerializeField]
        private int currentAsset = 0;

        public int CurrentAsset
        {
            get => currentAsset;
            set
            {
                if (value >= 0 && value < Assets.Count)
                {
                    currentAsset = value;
                }
            }
        }
    }
}

