using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BundleBuilder : Editor
{

    [MenuItem("Assets/Build AssetBundle")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}