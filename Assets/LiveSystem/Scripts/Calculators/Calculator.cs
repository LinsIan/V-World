/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;

namespace LiveSystem
{
    public abstract class Calculator
    {
        public virtual void OnLandmarksOutput(object sender, OutputEventArgs<NormalizedLandmarkList> data)
        {
        }

        public virtual void OnMultiLandmarksOutput(object sender, OutputEventArgs<List<NormalizedLandmarkList>> data)
        {
        }

    }
}
