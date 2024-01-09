using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Space[,] grid = new Space[3, 3];

    public Sprite xSprite;
    public Sprite oSprite;
    public Image background;

    private Stack<MoveInfo> moveInfoList = new Stack<MoveInfo>();

    public Player CurrentPlayer { get; set; }
    private Player player1;
    private Player player2;

    [SerializeField] private GameObject hintButton;
    [SerializeField] private GameObject undoButton;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    private static GameManager instance;


    public void OnEnable()
    {
        GameEvents.OnSpaceSelect += GameEvents_OnSpaceSelect;
        GameEvents.OnAfterSpaceSelect += GameEvents_OnAfterSpaceSelect;
        GameEvents.OnPhysicalPlayerTurn += GameEvents_OnPhysicalPlayerTurn;
    }
    public void OnDisable()
    {
        GameEvents.OnSpaceSelect -= GameEvents_OnSpaceSelect;
        GameEvents.OnAfterSpaceSelect -= GameEvents_OnAfterSpaceSelect;
        GameEvents.OnPhysicalPlayerTurn += GameEvents_OnPhysicalPlayerTurn;
    }
    public void Start()
    {
        LoadAssets();
        SetGrid(grid);
        GameStart(GameEvents.GetGameData());
    }

    private void LoadAssets()
    {
        xSprite = GameController.Instance.loadedAssetBundle.LoadAsset<Sprite>("ExTarget");
        oSprite = GameController.Instance.loadedAssetBundle.LoadAsset<Sprite>("CircleTarget");
        background.sprite = GameController.Instance.loadedAssetBundle.LoadAsset<Sprite>("EmptyBG");
    }
    public void SetGrid(Space[,] grid)
    {
        for (byte i = 0; i < 3; i++)
        {
            for (byte j = 0; j < 3; j++)
            {
                grid[i, j] = new Space() { coordinates = (i, j), currentSignType = SignType.None };
            }
        }
    }
    private void GameStart((GameType, PlayerData[]) gameData)
    {
        switch (gameData.Item1)
        {
            case GameType.PlayerVsPlayer:
                player1 = new PhysicalPlayer(gameData.Item2[0]);
                player2 = new PhysicalPlayer(gameData.Item2[1]);
                break;

            case GameType.PlayerVsComputer:
                player1 = new PhysicalPlayer(gameData.Item2[0]);
                player2 = new ComputerPlayer(gameData.Item2[1]);
                break;

            case GameType.ComputerVsComputer:
                player1 = new ComputerPlayer(gameData.Item2[0]);
                player2 = new ComputerPlayer(gameData.Item2[1]);

                break;
        }

        List<Player> players = new List<Player>() { player1, player2 };
        int firstPlayer = Random.Range(0, 2);

        players[firstPlayer].signType = SignType.X;
        players[1 - firstPlayer].signType = SignType.O;
        CurrentPlayer = players[firstPlayer];

        SetButtonsForPhysicalPlayer();
        SetTurnForCurrentPlayer();
    }

    #region events

    private void GameEvents_OnPhysicalPlayerTurn()
    {
        SetButtonsForPhysicalPlayer();
    }
    private void GameEvents_OnSpaceSelect((byte, byte) coordinates, SignType signType) => grid[coordinates.Item1, coordinates.Item2].Select(signType);
    private void GameEvents_OnAfterSpaceSelect((byte, byte) coordinates)
    {
        moveInfoList.Push(new MoveInfo()
        {
            signType = CurrentPlayer.signType,
            coordinates = coordinates
        });

        SignType winningSignType = GetWinningSignType(grid);
        CurrentPlayer = CurrentPlayer == player1 ? player2 : player1;

        if (moveInfoList.Count == 9)
        {
            GameEvents.EndGame(null);
        }

        if (winningSignType == SignType.None)
        {
            SetTurnForCurrentPlayer();
        }
        else
        {
            GameEvents.EndGame(player1.signType == winningSignType ? player1 : player2);
        }
    }

    #endregion

    private void SetButtonsForPhysicalPlayer()
    {
        bool shouldEnableButton =
            GameController.Instance.GetGameType() == GameType.PlayerVsComputer &&
            CurrentPlayer.GetType() == typeof(PhysicalPlayer);

        Instance.hintButton.SetActive(shouldEnableButton);
        Instance.undoButton.SetActive(shouldEnableButton && moveInfoList.Count > 1);
    }
    private void SetTurnForCurrentPlayer()
    {
        if (CurrentPlayer.GetType() == typeof(ComputerPlayer))
        {
            Invoke(nameof(CurrentPlayerSetTurn), 1f);
        }
        else
        {
            CurrentPlayerSetTurn();
        }

        GameEvents.SetCurrentPlayer(CurrentPlayer);
    }
    private void CurrentPlayerSetTurn()
    {
        CurrentPlayer.SetTurn();
    }

    #region button actions

    public void Undo()
    {
        MoveInfo move = moveInfoList.Pop();
        GameEvents.SpaceSelect((move.coordinates.Item1, move.coordinates.Item2), SignType.None);
        move = moveInfoList.Pop();
        GameEvents.SpaceSelect((move.coordinates.Item1, move.coordinates.Item2), SignType.None);

        if (moveInfoList.Count < 2)
        {
            Instance.undoButton.SetActive(false);
        }
    }
    public void Hint()
    {
        MoveInfo result = SimulationManager.Sim(grid, CurrentPlayer.signType, GameController.Instance.GetHintData());
        GameEvents.SpaceHint(result.coordinates);
    }
    public void RestartGame()
    {
        SceneController.Instance.LoadScene(SceneEnum.MainGame);
    }

    #endregion

    public static SignType GetWinningSignType(Space[,] grid)
    {
        byte pointsX = 0;
        byte pointsO = 0;

        //vertical
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                SetScore(ref pointsX, ref pointsO, grid[i, j].currentSignType);
            }

            if (pointsX == 3) 
            {
                return SignType.X;
            }
            else if (pointsO == 3)
            {
                return SignType.O;
            }

            pointsX = 0;
            pointsO = 0;
        }

        //horizontal
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < 3; i++)
            {
                SetScore(ref pointsX, ref pointsO, grid[i, j].currentSignType);
            }

            if (pointsX == 3)
            {
                return SignType.X;
            }
            else if (pointsO == 3)
            {
                return SignType.O;
            }


            pointsX = 0;
            pointsO = 0;
        }

        //diagonal1
        for (int i=0; i < 3; i++)
        {
            SetScore(ref pointsX, ref pointsO, grid[i, i].currentSignType);
        }

        if (pointsX == 3)
        {
            return SignType.X;
        }
        else if (pointsO == 3)
        {
            return SignType.O;
        }

        pointsX = 0;
        pointsO = 0;


        //diagonal2
        for (int i=0; i<3; i++)
        {
            SetScore(ref pointsX, ref pointsO, grid[i, 2 - i].currentSignType);
        }

        if (pointsX == 3)
        {
            return SignType.X;
        }
        else if (pointsO == 3)
        {
            return SignType.O;
        }

        return SignType.None;
    }
    private static void SetScore(ref byte pointsX, ref byte pointsO, SignType currentSignType)
    {
        if (currentSignType == SignType.X)
        {
            pointsX++;
        }

        if (currentSignType == SignType.O)
        {
            pointsO++;
        }
    }
}