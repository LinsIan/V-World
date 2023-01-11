// Copyright (c) 2021 Lins Ian
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file

using System.Collections.Generic;
using Mediapipe;
using Mediapipe.Unity;

namespace LiveSystem
{
    public abstract class Calculater
    {
        public virtual void OnLandmarksOutput(object sender, OutputEventArgs<NormalizedLandmarkList> data)
        {
        }

        public virtual void OnMultiLandmarksOutput(object sender, OutputEventArgs<List<NormalizedLandmarkList>> data)
        {
        }

    }
}
