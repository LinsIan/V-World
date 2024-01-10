// Copyright (c) 2023 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections;
using UnityEngine;
using Mediapipe.Unity.Sample;
using VContainer;
using Stopwatch = System.Diagnostics.Stopwatch;
using VContainer.Unity;
using Cysharp.Threading.Tasks;

namespace VWorld
{
    public abstract class TaskApiRunner<TTask> : IStartable where TTask : Mediapipe.Tasks.Vision.Core.BaseVisionTaskApi
    {
        protected Bootstrap bootstrap;
        protected bool isPaused;
        private readonly Stopwatch _stopwatch = new();

        [Inject]
        public TaskApiRunner(Bootstrap bootstrap)
        {
            this.bootstrap = bootstrap;
        }

        public virtual async void Start()
        {
            await UniTask.WaitUntil(() => bootstrap.isFinished);
            Play();
        }

        /// <summary>
        ///   Start the main program from the beginning.
        /// </summary>
        public virtual void Play()
        {
            isPaused = false;
            _stopwatch.Restart();
        }

        /// <summary>
        ///   Pause the main program.
        /// <summary>
        public virtual void Pause()
        {
            isPaused = true;
        }

        /// <summary>
        ///    Resume the main program.
        ///    If the main program has not begun, it'll do nothing.
        /// </summary>
        public virtual void Resume()
        {
            isPaused = false;
        }

        /// <summary>
        ///   Stops the main program.
        /// </summary>
        public virtual void Stop()
        {
            isPaused = true;
            _stopwatch.Stop();
        }

        protected long GetCurrentTimestampMillisec() => _stopwatch.IsRunning ? _stopwatch.ElapsedTicks / TimeSpan.TicksPerMillisecond : -1;
    }
}
