using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/SceneData", order = 1)]
public class SceneSO : ScriptableObject
{
    public SceneEnum sceneEnum;
    public string sceneName;
}