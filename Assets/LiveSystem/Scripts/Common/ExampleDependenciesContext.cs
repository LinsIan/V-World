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

public struct Dependency
{
    // 物件的類型
    public Type Type { get; set; }
    // 該物件的生成方式
    public Func<object> Factory { get; set; }
    // 是否為單例
    public bool IsSingleton { get; set; }
}

public class DependenciesCollection
{
    //註冊表
    private Dictionary<Type, Dependency> dependencies = new Dictionary<Type, Dependency>();
    //單例
    private Dictionary<Type, object> singletons = new Dictionary<Type, object>();
    //註冊
    public void Add(Dependency dependency) => dependencies.Add(dependency.Type, dependency);
    
    //取得實例
    public object Get(Type type)
    {
        if (!dependencies.ContainsKey(type))
        {   
            throw new ArgumentException("Type is not a dependency: " + type.FullName);
        }
        
        var dependency = dependencies[type];
        
        //該依賴為單例
        if (dependency.IsSingleton)
        {
            if (!singletons.ContainsKey(type))
            {
                singletons.Add(type, dependency.Factory());
            }
            return singletons[type];
        }
        
        //不是單例則生成新的實例
        return dependency.Factory();
    }
    
    public T Get<T>()
    {
        return (T)Get(typeof(T));
    }
}

public static class DependenciesContainer
{
    public static DependenciesCollection Dependencies { get; } = new DependenciesCollection();
}

//組合根
public class ExampleDependenciesContext : MonoBehaviour
{
    [SerializeField]
    private ExampleDependencyMonoBehaviour exampleDependency = default;
    
    private void Awake() {
        DependenciesContainer.Dependencies.Add(new Dependency{
            Type = typeof(ExampleDependencyMonoBehaviour),
            Factory = () => Instantiate(exampleDependency).GetComponent<ExampleDependencyMonoBehaviour>(),
            IsSingleton = true
        });
    }
}
