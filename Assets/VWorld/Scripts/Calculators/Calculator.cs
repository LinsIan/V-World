/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;
using UniRx;

namespace VWorld
{
    public interface ICalculator<T>
    {
        public ReactiveProperty<T> LastestData { get; }
        public abstract void OnLandmarkDetectionOutput(IReadOnlyList<NormalizedLandmark> landmarks);
    }
}
