/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;
using VContainer;
using LiveSystem;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using Cysharp.Threading.Tasks.CompilerServices;
using System.IO;

public class LauncherEP : IStartable, ITickable
{

    [Inject] private FaceLandmarkKeyPoints faceLandmarkKeyPoints;
    [Inject] private IConnector connector;

    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private CancellationTokenSource cancellationTokenSource2 = new CancellationTokenSource();
    private CancellationTokenSource linkedTokenSource;

    private UniTaskCompletionSource source = new UniTaskCompletionSource();

    private async void OnClick()
    {
       var isCancel =  await UniTask.NextFrame(cancellationTokenSource.Token).SuppressCancellationThrow();
    }

    private void Cancel()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        cancellationTokenSource = new CancellationTokenSource();

        cancellationTokenSource2.CancelAfterSlim(TimeSpan.FromSeconds(1));

        linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token);
    }

    public async void Start()
    {
        await UniTask.Delay(1000);
        await UniTask.Yield(PlayerLoopTiming.Update);

        linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationTokenSource.Token, cancellationTokenSource2.Token);

        connector.Connect();
        Debug.Log($"faceLandmarkKeyPoints: {faceLandmarkKeyPoints.FaceMeshCount}");
        
        waitforResult().Forget();
        await DelayOneSecond();
        RunOnThread().Forget();
    }

    private async UniTaskVoid waitforResult()
    {
        await source.Task;
        Debug.Log("waitforResult");
    }

    public async UniTask DelayOneSecond()
    {
        source.TrySetResult();
        await UniTask.Delay(1000);
        Debug.Log("Delay");
    }

    public async UniTask ReadFile()
    {
        var path = Application.streamingAssetsPath + "/test.txt";
        await UniTask.SwitchToThreadPool();
        var text = await File.ReadAllTextAsync(path);
        await UniTask.Yield(PlayerLoopTiming.Update);
        Debug.Log(text);
    }

    public async UniTask RunOnThread()
    {
        int result = 0;
        await UniTask.RunOnThreadPool(() =>
        {
            for (int i = 0; i < 100000000; i++)
            {
                result += i;
            }
        });
        await UniTask.SwitchToMainThread();
        Debug.Log($"result: {result}");
    }

    public void Tick()
    {
    }
}
