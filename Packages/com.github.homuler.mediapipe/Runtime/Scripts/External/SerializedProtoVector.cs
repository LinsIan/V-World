// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using pb = Google.Protobuf;

namespace Mediapipe
{
  [StructLayout(LayoutKind.Sequential)]
  internal readonly struct SerializedProtoVector
  {
    private readonly IntPtr _data;
    private readonly int _size;

    public void Dispose()
    {
      UnsafeNativeMethods.mp_api_SerializedProtoArray__delete(_data, _size);
    }

    public List<T> Deserialize<T>(pb::MessageParser<T> parser) where T : pb::IMessage<T>
    {
      var protos = new List<T>(_size);

      Deserialize(parser, protos);

      return protos;
    }

    /// <summary>
    ///   Deserializes the data as a list of <typeparamref name="T" />.
    /// </summary>
    /// <param name="protos">A list of <typeparamref name="T" /> to populate</param>
    public void Deserialize<T>(pb::MessageParser<T> parser, List<T> protos) where T : pb::IMessage<T>
    {
      protos.Clear();

      unsafe
      {
        var protoPtr = (SerializedProto*)_data;

        for (var i = 0; i < _size; i++)
        {
          var serializedProto = Marshal.PtrToStructure<SerializedProto>((IntPtr)protoPtr++);
          protos.Add(serializedProto.Deserialize(parser));
        }
      }
    }
  }
}
