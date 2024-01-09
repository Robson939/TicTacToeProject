using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerPlayer : Player
{
    [SerializeField] private SimulationSO simulationSO;
    public void SetSimulationLevel(SimulationSO simulationSO) => this.simulationSO = simulationSO;

    public ComputerPlayer(PlayerData playerData) : base(playerData)
    {
        simulationSO = GameController.Instance.GetSimDifficulty(playerData.CompPlayerDifficulty);
    }

    public override void SetTurn()
    {
        MakeMove();
    }
    public override void MakeMove()
    {
        MoveInfo bestMove = SimulationManager.Sim(GameManager.Instance.grid, signType, simulationSO);

        GameEvents.SpaceSelect(bestMove.coordinates, signType);
        GameEvents.AfterSpaceSelect(bestMove.coordinates);
    }
}