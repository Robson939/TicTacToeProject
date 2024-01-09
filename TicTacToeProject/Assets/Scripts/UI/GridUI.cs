using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridUI : MonoBehaviour
{
    private SpaceUI[,] grid = new SpaceUI[3,3];

    public void OnEnable()
    {
        GameEvents.OnSpaceSelect += GameEvents_OnSpaceSelect;
        GameEvents.OnSpaceHint += GameEvents_OnSpaceHint;
    }
    public void OnDisable()
    {
        GameEvents.OnSpaceSelect -= GameEvents_OnSpaceSelect;
        GameEvents.OnSpaceHint -= GameEvents_OnSpaceHint;
    }
    public void Awake()
    {
        SpaceUI[] spaces = new SpaceUI[9];
        byte index = 0;

        for (int i = 0; i < 9; i++)
        {
            spaces[i] = transform.GetChild(i).GetComponent<SpaceUI>();
        }

        for (byte i = 0; i < 3; i++)
        {
            for (byte j = 0; j < 3; j++)
            {
                spaces[index].coordinates = (i, j);
                grid[i,j] = spaces[index];
                index++;
            }
        }
    }

    private void GameEvents_OnSpaceHint((byte, byte) coordinates)
    {
        grid[coordinates.Item1, coordinates.Item2].Highlight();
    }
    private void GameEvents_OnSpaceSelect((byte, byte) coordinates, SignType signType)
    {
        DOTween.Kill("highlightHint");

        grid[coordinates.Item1, coordinates.Item2].Select(signType);
    }
}