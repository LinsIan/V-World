/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;

namespace VWorld
{
    public interface ICalculator
    {
        public abstract void OnLandmarkDetectionOutput(IReadOnlyList<NormalizedLandmark> landmarks);
    }
}
