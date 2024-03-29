/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace VWorld.Data
{
    [Serializable]
    public class SettingValue
    {
        public static readonly float Default = 1;
        public static readonly float Min = 0;
        public static readonly float Max = 3;

        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField]
        public List<Common.ParamId> EffectedParamIds { get; private set; }

        [SerializeField]
        private float basicRatio = 1;

        [SerializeField, Range(0, 3)]
        private float value = Default;

        public float Value
        {
            get => value * basicRatio;
            set => this.value = Mathf.Clamp(value, Min, Max);
        }

        public SettingValue()
        {
            Reset();
        }

        public void Reset()
        {
            Value = Default;
        }
    }
}
