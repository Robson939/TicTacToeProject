using System;
using UnityEngine;
using System.Linq;
using System.IO;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;

public class AssetBundleCreatorEW : EditorWindow
{
    private string assetBundleName;
    private Sprite background;
    private Sprite symbolX;
    private Sprite symbolO;
    private string logs;

    [MenuItem("Asset Bundles/Create Asset Bundle")]
    public static void OpenWindow()
    {
        GetWindow(typeof(AssetBundleCreatorEW));   
    }
    
    private void OnGUI()
    {
        EditorGUILayout.Space(5);
        GUILayout.Label("Create Asset Bundle", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);
        assetBundleName = EditorGUILayout.TextField("Name", assetBundleName);
        symbolX = (Sprite)EditorGUILayout.ObjectField("Symbol X", symbolX, typeof(Sprite), false);
        symbolO = (Sprite)EditorGUILayout.ObjectField("Symbol O", symbolO, typeof(Sprite), false);
        background = (Sprite)EditorGUILayout.ObjectField("Background", background, typeof(Sprite), false);

        if (GUILayout.Button("Build"))
        {        
            logs = "";

            if (string.IsNullOrWhiteSpace(assetBundleName) || background == null || symbolX == null || symbolO == null)
            {
                logs = "All fields must be filled!";
            }
            else if (assetBundleName.Count() > 50)
            {
                logs = "Name of Asset Bundle is too long";
            }
            else if (Directory.GetFiles(Application.streamingAssetsPath).Select(x => Path.GetFileName(x).ToLower()).ToList().Any(y => y.Contains(assetBundleName.ToLower())))
            {
                logs = $"Asset Bundle with name {assetBundleName} already exists"; ;
            }
            else
            {
                try
                {
                    foreach (UnityEngine.Object newObj in new UnityEngine.Object[3] { symbolX, symbolO, background })
                    {
                        var assetImporterObj = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(newObj));
                        assetImporterObj.assetBundleName = assetBundleName;                        
                    }

                    BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

                    AssetDatabase.Refresh();

                    foreach (UnityEngine.Object newObj in new UnityEngine.Object[3] { symbolX, symbolO, background })
                    {
                        var assetImporterObj = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(newObj));
                        assetImporterObj.assetBundleName = string.Empty;
                    }

                    logs = $"{assetBundleName} Bundle created successfully!";
                }
                catch (Exception e)
                {
                    logs = e.Message;
                }
            }
        }
        
        GUILayout.Label(logs, EditorStyles.wordWrappedLabel);
    }
}

#endif