using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents
{
    public delegate void SpaceSelectAction((byte, byte) coordinates, SignType signType);
    public static event SpaceSelectAction OnSpaceSelect;
    public static void SpaceSelect((byte, byte) coordinates, SignType signType) => OnSpaceSelect?.Invoke(coordinates, signType);


    public delegate void SpaceAction((byte, byte) coordinates);
    public static event SpaceAction OnAfterSpaceSelect;
    public static void AfterSpaceSelect((byte, byte) coordinates) => OnAfterSpaceSelect?.Invoke(coordinates);
    public static event SpaceAction OnSpaceHint;
    public static void SpaceHint((byte, byte) coordinates) => OnSpaceHint?.Invoke(coordinates);




    public delegate void GameTypeAction(GameType gameType);
    public static event GameTypeAction OnGameTypeSelect;
    public static void GameTypeSelect(GameType gameType) => OnGameTypeSelect?.Invoke(gameType);


    public delegate void PlayerAction(Player player);
    public static event PlayerAction OnEndGame;
    public static void EndGame(Player player) => OnEndGame?.Invoke(player);
    public static event PlayerAction OnSetCurrentPlayer;
    public static void SetCurrentPlayer(Player player) => OnSetCurrentPlayer?.Invoke(player);



    public delegate void PlayersAction(PlayerData[] playersData);
    public static event PlayersAction OnSetPlayersData;
    public static void SetPlayersData(PlayerData[] playersData) => OnSetPlayersData?.Invoke(playersData);


    public delegate (GameType, PlayerData[]) GameDataAction();
    public static event GameDataAction OnGetGameData;
    public static (GameType, PlayerData[]) GetGameData() => OnGetGameData.Invoke();


    public delegate void GameAction();
    public static event GameAction OnStartGame;
    public static void StartGame() => OnStartGame?.Invoke();
    public static event GameAction OnPhysicalPlayerTurn;
    public static void PhysicalPlayerTurn() => OnPhysicalPlayerTurn?.Invoke();

}