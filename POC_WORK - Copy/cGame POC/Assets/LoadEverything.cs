using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadEverything : MonoBehaviour
{  public AssetBundle towerAssets;
    IEnumerator Start()
    {
        WWW x = new WWW("file:///C:/Users/tft/Desktop/CGame/AssetBundles/Windows/newbuild");
        yield return x;
        towerAssets = x.assetBundle;
    }
}
