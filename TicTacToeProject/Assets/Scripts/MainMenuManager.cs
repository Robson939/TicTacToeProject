using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void SetPvsPGame()
    {
        GameEvents.GameTypeSelect(GameType.PlayerVsPlayer);
        ScreensManager.Instance.Open("PlayersInfo");
    }

    public void SetPvsCGame()
    {
        GameEvents.GameTypeSelect(GameType.PlayerVsComputer);
        ScreensManager.Instance.Open("PlayersInfo");
    }

    public void SetCvsCGame()
    {
        GameEvents.GameTypeSelect(GameType.ComputerVsComputer);
        ScreensManager.Instance.Open("PlayersInfo");
    }

    public void CloseApp()
    {
        Application.Quit();
    }
}