/**
 * Copyright (c) 2023 LinsIan
 * 
 * Use of this source code is governed by an MIT-style license that can be
 * found in the LICENSE file or at https://opensource.org/licenses/MIT.
 */

namespace VWorld.Common
{
    public enum ModelType
    {
        Live2D = 0,
        Home3D
    }

    public enum LiveMode
    {
        FaceOnly = 0,
        UpperBody,
        Holistic
    }

    public enum ParamId
    {
        ParamAngleX = 0,
        ParamAngleY,
        ParamAngleZ,
        ParamEyeLOpen,
        ParamEyeROpen,
        ParamEyeBallX,
        ParamEyeBallY,
        ParamMouthOpenY,
        ParamBodyAngleX,
        ParamBodyAngleY,
        ParamBodyAngleZ,
        ParamBreath,
        ParamNeck,
        ParamSpine,
        ParamRightEye,
        ParamLeftEye
    }

    public enum Direction
    {
        Up = 0,
        Down,
        Left,
        Right,
        Mid
    }
}