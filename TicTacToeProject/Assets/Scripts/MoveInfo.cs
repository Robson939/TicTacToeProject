using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MoveInfo
{
    public SignType signType;
    public (byte, byte) coordinates;

    public MoveInfo(byte moveNumber, SignType signType, (byte, byte) coordinates)
    {
        this.signType = signType;
        this.coordinates = coordinates;
    }
}