using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player
{
    public string playerName;
    public SignType signType;
    public byte playerNumber;


    public abstract void MakeMove();
    public abstract void SetTurn();
    public Player(PlayerData playerData)
    {
        playerName = $"Player {playerData.PlayerNumber}";
        playerNumber = playerData.PlayerNumber;
    }
}