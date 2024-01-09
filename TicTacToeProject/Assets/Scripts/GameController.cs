using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public SimulationSO[] DifficultyDatas => difficultyDatas;
    public AssetBundle loadedAssetBundle = null;

    [SerializeField] private SimulationSO[] difficultyDatas = new SimulationSO[3];
    [SerializeField] private SimulationSO hintData;
    [SerializeField] private string defaultAssetBundle = "defaultpackage";
    private GameType gameType = GameType.None;
    private PlayerData[] playersData;

    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameController>();
            }
            return instance;
        }
    }
    private static GameController instance;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void OnEnable()
    {
        GameEvents.OnGameTypeSelect += GameEvents_OnGameTypeSelect;
        GameEvents.OnSetPlayersData += GameEvents_OnSetPlayersData;
        GameEvents.OnGetGameData += GameEvents_OnGetGameData;
        GameEvents.OnStartGame += GameEvents_OnStartGame;
    }
    public void OnDisable()
    {
        GameEvents.OnGameTypeSelect -= GameEvents_OnGameTypeSelect;
        GameEvents.OnSetPlayersData -= GameEvents_OnSetPlayersData;
        GameEvents.OnGetGameData -= GameEvents_OnGetGameData;
        GameEvents.OnStartGame -= GameEvents_OnStartGame;
    }
    public void Start()
    {
        loadedAssetBundle = AssetBundle.LoadFromFile($"{Application.streamingAssetsPath}/{defaultAssetBundle}");

        SceneController.Instance.LoadScene(SceneEnum.Menu);
    }

    #region events

    private void GameEvents_OnGameTypeSelect(GameType gameType) => this.gameType = gameType;
    private void GameEvents_OnSetPlayersData(PlayerData[] playersData) => this.playersData = playersData;   
    private (GameType, PlayerData[]) GameEvents_OnGetGameData() => (gameType, playersData);
    private void GameEvents_OnStartGame() => SceneController.Instance.LoadScene(SceneEnum.MainGame);
    
    #endregion
    
    public GameType GetGameType() => gameType;
    public SimulationSO GetSimDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                return difficultyDatas[0];

            case Difficulty.Medium:
                return difficultyDatas[1];

            case Difficulty.Hard:
                return difficultyDatas[2];
        }

        return null;
    }
    public SimulationSO GetHintData() => hintData;
}