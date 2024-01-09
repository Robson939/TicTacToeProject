using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanelUI : MonoBehaviour
{
    [SerializeField] private Text winnerText;
    [SerializeField] private GameObject panelGO;

    public void OnEnable()
    {
        GameEvents.OnEndGame += GameEvents_OnEndGame;
    }
    public void OnDisable()
    {
        GameEvents.OnEndGame -= GameEvents_OnEndGame;
    }

    private void GameEvents_OnEndGame(Player winningPlayer)
    {
        panelGO.SetActive(true);
        winnerText.text = winningPlayer != null ? $"{winningPlayer.playerName} wins!!!" : "Draw!";
    }

    public void BackToMenu()
    {
        SceneController.Instance.LoadScene(SceneEnum.Menu);
    }
}
