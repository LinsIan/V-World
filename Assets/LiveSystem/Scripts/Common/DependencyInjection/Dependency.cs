/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;

namespace LiveSystem
{
    public struct Dependency
    {
        // 物件的類型
        public Type Type { get; set; }
        // 該物件的生成方式
        public DependencyFactory.Delegate Factory { get; set; }
        // 是否為單例
        public bool IsSingleton { get; set; }
    }
}
