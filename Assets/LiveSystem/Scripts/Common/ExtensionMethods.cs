/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using UnityEngine;
using Mediapipe;

namespace LiveSystem.ExtensionMethods
{
    internal static class MediapipeExtensionMethods
    {
        public static NormalizedLandmark Round(this NormalizedLandmark landmark, int digits)
        {
            landmark.X = (float)Math.Round(landmark.X, digits);
            landmark.Y = (float)Math.Round(landmark.Y, digits);
            landmark.Z = (float)Math.Round(landmark.Z, digits);
            return landmark;
        }
    }
}

