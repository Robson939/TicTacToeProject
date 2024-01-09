using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalPlayer : Player
{
    public PhysicalPlayer(PlayerData playerData) : base(playerData)
    {
    }

    public override void MakeMove()
    {

    }

    public override void SetTurn()
    {
        GameEvents.PhysicalPlayerTurn();
    }
}