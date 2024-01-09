using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInfoPanel : ScreenBase
{
    [SerializeField] private PlayerInfo player1Info;
    [SerializeField] private PlayerInfo player2Info;

    public override void Open()
    {
        switch (GameController.Instance.GetGameType())
        {
            case GameType.PlayerVsPlayer:
                player1Info.SetPlayerType(PlayerType.PhysicalPlayer);
                player2Info.SetPlayerType(PlayerType.PhysicalPlayer);
                break;

            case GameType.PlayerVsComputer:
                player1Info.SetPlayerType(PlayerType.PhysicalPlayer);
                player2Info.SetPlayerType(PlayerType.ComputerPlayer);
                break;

            case GameType.ComputerVsComputer:
                player1Info.SetPlayerType(PlayerType.ComputerPlayer);
                player2Info.SetPlayerType(PlayerType.ComputerPlayer);
                break;
        }

        base.Open();
    }

    public void StartGame()
    {
        PlayerData[] playersData = new PlayerData[2];

        playersData[0] = player1Info.GetPlayerData();
        playersData[0].PlayerNumber = 1;

        playersData[1] = player2Info.GetPlayerData();
        playersData[1].PlayerNumber = 2;

        GameEvents.SetPlayersData(playersData);
        GameEvents.StartGame();
    }
}