using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScreensManager : MonoBehaviour
{
    static ScreensManager instance;
    public static ScreensManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ScreensManager>();
            }
            return instance;
        }
    }

    public ScreenBase openOnStart;
    public List<ScreenTitlePair> screensCollections = new List<ScreenTitlePair>();

    private Stack<ScreenBase> screensStack = new Stack<ScreenBase>();


    public void Start()
    {
        foreach (var screen in screensCollections)
        {
            screen.screen.Close();
        }

        screensStack.Push(openOnStart);
        openOnStart.Open();
    }

    public void Open(string screenId)
    {
        ScreenBase screen = screensCollections.FirstOrDefault(x => x.title == screenId)?.screen;
        if (screen != null)
        {
            screensStack.Peek().Close();
            screensStack.Push(screen);
            screen.Open();
        }
    }
    public void Back()
    {
        screensStack.Pop().Close();
        screensStack.Peek().Open();
    }
}