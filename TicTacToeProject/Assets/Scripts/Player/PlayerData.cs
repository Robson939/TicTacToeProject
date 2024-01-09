using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerData
{
    private bool playerNumber;
    private PlayerType playerType;
    private Difficulty compPlayerDifficulty;

    public byte PlayerNumber { get => (byte)(!playerNumber ? 1 : 2); set => playerNumber = value == 2; }
    public PlayerType PlayerType { get => playerType; set => playerType = value; }
    public Difficulty CompPlayerDifficulty { get => compPlayerDifficulty; set => compPlayerDifficulty = value; }
}