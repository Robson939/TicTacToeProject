using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private Text playerName;
    [SerializeField] private GameObject physicalPlayerPanel;
    [SerializeField] private GameObject computerPlayerPanel;
    private PlayerData playerData;

    public void SetPlayerType(PlayerType playerType)
    {
        switch (playerType)
        {
            case PlayerType.PhysicalPlayer:
                physicalPlayerPanel.SetActive(true);
                computerPlayerPanel.SetActive(false);
                playerData.PlayerType = PlayerType.PhysicalPlayer;
                break;

            case PlayerType.ComputerPlayer:
                physicalPlayerPanel.SetActive(false);
                computerPlayerPanel.SetActive(true);
                playerData.PlayerType = PlayerType.ComputerPlayer;
                playerData.CompPlayerDifficulty = Difficulty.Easy;
                break;
        }
    }

    public PlayerData GetPlayerData() => playerData;

    #region events 
    public void OnDropdownValueChanged(int index)
    {
        switch (index) 
        { 
            case 0:
                playerData.CompPlayerDifficulty = Difficulty.Easy;
                break;
            case 1:
                playerData.CompPlayerDifficulty = Difficulty.Medium;
                break;
            case 2:
                playerData.CompPlayerDifficulty = Difficulty.Hard;
                break;
        }
    }
    #endregion
}