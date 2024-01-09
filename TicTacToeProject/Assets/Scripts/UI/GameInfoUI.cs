using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoUI : MonoBehaviour
{
    [SerializeField] private Text currentPlayerText;

    private void OnEnable()
    {
        GameEvents.OnSetCurrentPlayer += GameEvents_OnSetCurrentPlayer;
    }
    private void OnDisable()
    {
        GameEvents.OnSetCurrentPlayer -= GameEvents_OnSetCurrentPlayer;
    }

    private void GameEvents_OnSetCurrentPlayer(Player player)
    {
        currentPlayerText.text = $"Current player:\n{player.playerName}";
    }
}