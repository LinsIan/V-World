/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

using System;

namespace LiveSystem
{
    //用來注入的屬性，繼承Attribute的話就可以寫在[]中
    [UnityEngine.Scripting.Preserve]
    public class InjectFieldAttribute : Attribute 
    {
    }
}
