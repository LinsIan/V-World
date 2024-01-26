/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using Mediapipe.Tasks.Components.Containers;
using UnityEngine;

namespace VWorld.Common
{
    internal static class MediapipeExtensionMethods
    {
        public static Vector3 Round(this NormalizedLandmark landmark, int digits)
        {
            var x = (float)Math.Round(landmark.x, digits);
            var y = (float)Math.Round(landmark.y, digits);
            var z = (float)Math.Round(landmark.z, digits);
            return new Vector3(x, y, z);
        }

        public static Vector3 ToVector3(this NormalizedLandmark landmark)
        {
            return new Vector3(landmark.x, landmark.y, landmark.z);
        }
    }
}

