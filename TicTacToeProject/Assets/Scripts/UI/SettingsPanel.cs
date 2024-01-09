using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : ScreenBase
{
    [SerializeField] private InputField reskinNameInput;
    [SerializeField] private Text logText;


    public override void Open()
    {
        reskinNameInput.text = "";
        var loadedAssetBundles = AssetBundle.GetAllLoadedAssetBundles();

        logText.text = loadedAssetBundles.Count() > 0 ? $"Loaded AssetBundle: {loadedAssetBundles.ElementAt(0).name}" : "No Asset Bundle loaded";

        base.Open();
    }

    public void Reskin()
    {
        string assetBundleName = reskinNameInput.text;
        string combinedPath = $"{Application.streamingAssetsPath}/{assetBundleName}";

        if (!File.Exists(combinedPath))
        {
            logText.text = $"Asset Bundle with name {assetBundleName} does not exist!";
            return;
        }

        if (AssetBundle.GetAllLoadedAssetBundles().Any(x => x.name.Contains(assetBundleName)))
        {
            logText.text = $"Asset Bundle with name {assetBundleName} is already loaded!";
            return;

        }

        AssetBundle.UnloadAllAssetBundles(true);
        AssetBundle loadedAssetBundle = AssetBundle.LoadFromFile(combinedPath);
        GameController.Instance.loadedAssetBundle = loadedAssetBundle;

        logText.text = $"Loaded AssetBundle: {reskinNameInput.text}";
    }
}