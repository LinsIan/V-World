/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace VWorld.Common
{

    public class ScalarKalmanFilter
    {
        // 越相信測量值速度就越快，但是平滑度就越低
        private const float DefaultQ = 0.0001f; // 越大越相信測量值而不是預測值
        private const float DefaultR = 0.002f; // 越大越相信預測值而不是測量值
        private const float DefaultP = 1;

        public Vector3 value;  // 系統的狀態量
        public float k; // 卡爾曼增益
        public float q; // 預測過程噪聲協方差
        public float r; // 測量過程噪聲協方差
        public float p; // 估計誤差協方差
 
        public ScalarKalmanFilter(float predict_q = DefaultQ, float measured_r = DefaultR)
        {
            value = Vector3.zero;
            q = predict_q;
            r = measured_r;
            p = DefaultP;
        }

        public Vector3 Filt(Vector3 lastMeasurement)
        {
            var predictValue = value;

            var pminus = p + q;

            k = pminus / (pminus +r);

            value = predictValue + k * (lastMeasurement - predictValue);

            p = (1 - k) *pminus;

            return value;
        }

        public void Reset()
        {
            p = DefaultP;
            value = Vector3.zero;
            k = 0;
        }
    }
}

