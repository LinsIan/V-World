/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace VWorld.UI
{
    public class StartMenu : MonoBehaviour
    {
        [SerializeField] private Dropdown modelTypeDropdown;
        [SerializeField] private Button startButton;

        private void Start()
        {
            foreach (var item in Enum.GetNames(typeof(Common.ModelType)))
            {
                modelTypeDropdown.options.Add(new Dropdown.OptionData(item));
            }
            modelTypeDropdown.RefreshShownValue();
            startButton.onClick.AddListener(() => { StartCoroutine(LoadSceneAsync(modelTypeDropdown.value + 1)); });
        }

        private IEnumerator LoadSceneAsync(int sceneBuildIndex)
        {
            var sceneLoadReq = SceneManager.LoadSceneAsync(sceneBuildIndex);
            yield return new WaitUntil(() => sceneLoadReq.isDone);
        }
    }
}
