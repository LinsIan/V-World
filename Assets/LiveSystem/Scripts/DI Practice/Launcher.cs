/**
*Copyright (c) 2023 LinsIan

Use of this source code is governed by an MIT-style license that can be
found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using LiveSystem;
using UnityEngine;
using VContainer;
using VContainer.Unity;


public class Launcher : MonoBehaviour
{
    [Inject] private FaceLandmarkKeyPoints faceLandmarkKeyPoints;
    private IConnector connector;
    

    [Inject]
    public void Constrdduct(IConnector connector)
    {
        this.connector = connector;
    }



    public void Awake()
    {
        // connector.Connect();
        // Debug.Log($"faceLandmarkKeyPoints: {faceLandmarkKeyPoints.FaceMeshCount}");
    }
}
