using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneController>();
            }
            return instance;
        }
    }
    private static SceneController instance;

    public SceneSO[] scenes;


    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadScene(SceneEnum scene)
    {
        string sceneName = scenes.ToList().First(x => x.sceneEnum == scene).sceneName;

        SceneManager.LoadScene(sceneName);
    }
}