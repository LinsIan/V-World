/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */


using System;
using System.Reflection;
using System.Collections.Generic;

namespace LiveSystem
{
    public class DependenciesProvider
    {
        //註冊表
        private Dictionary<Type, Dependency> dependencies = new Dictionary<Type, Dependency>();
        //單例表
        private Dictionary<Type, object> singletons = new Dictionary<Type, object>();

        public DependenciesProvider(DependenciesCollection dependencies)
        {
            foreach (var dependency in dependencies)
            {
                this.dependencies.Add(dependency.Type, dependency);
            }
        }

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

        // 幫傳入的類別做注入的動作(屬性注入)
        public object Inject(object dependant)
        {
            Type type = dependant.GetType();
            while (type != null)
            {
                // 找出該類別所有成員
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic
                    | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                // 遍歷其成員
                foreach (var field in fields)
                {
                    // 找出繼承InjectFieldAttribute的成員
                    if (field.GetCustomAttribute<InjectFieldAttribute>(false) == null)
                    {
                        continue;
                    }
                    // 產生or取得實例並注入
                    field.SetValue(dependant, Get(field.FieldType));
                }
                // 往父類別查有沒有其他需要注入的成員
                type = type.BaseType;
            }
            return dependant;
        }

        //TODO:嘗試做方法注入，使用GetMethods和GetParameters找到有註冊依賴的類別然後注入實例
    }
}
