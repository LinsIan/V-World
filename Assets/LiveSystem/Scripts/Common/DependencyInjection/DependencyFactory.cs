/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Runtime.Serialization;

namespace LiveSystem
{
    public class DependencyFactory
    {
        public delegate object Delegate(DependenciesProvider dependencies);

        public static Delegate FromClass<T>() where T : class, new()
        {
            return (dependencies) =>
            {
                var type = typeof(T);
                var obj = FormatterServices.GetUninitializedObject(type);
                dependencies.Inject(obj);
                type.GetConstructor(Type.EmptyTypes).Invoke(obj, null);
                return (T)obj;
            };
        }

        //先把產生的物件注入完成再設為active，才會去觸發awake
        public static Delegate FromPrefab<T>(T prefab) where T : MonoBehaviour
        {
            return (dependencies) =>
            {
                bool wasActive = prefab.gameObject.activeSelf;
                //設為false才不會生成的時候馬上觸發awake
                prefab.gameObject.SetActive(false);
                var instance = GameObject.Instantiate(prefab);
                prefab.gameObject.SetActive(wasActive);
                //這邊或許可以用transform foreach去找
                var children = instance.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var child in children)
                {
                    dependencies.Inject(child);
                }
                //注入完畢，設為原本的狀態
                instance.gameObject.SetActive(wasActive);
                return instance.GetComponent<T>();
            };
        }

        public static Delegate FromGameObject<T>(T instance) where T : MonoBehaviour
        {
            return (dependencies) =>
            {
                var children = instance.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var child in children)
                {
                    dependencies.Inject(child);
                }
                return instance;
            };
        }

        // TODO:FromAddressables
    }
}

