/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VWorld
{
    public interface ITaskApiRunner
    {
        public void Play();
        public void Pause();
        public void Resume();
        public void Stop();
    }
}
