using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuScript : MonoBehaviour {
    public void change(string scenex)
    {
        if (scenex == "BallGame") {
            StartCoroutine("changeScene");
        }
    } 

    public IEnumerator changeScene() {
            string bundurl = "file:///C:/Users/tft/Desktop/realGame%20-%20Copy/AssetBundles/thirdpersoncontrolorbitcamscene";
            WWW www = WWW.LoadFromCacheOrDownload(bundurl, 1);
            if (www != null) { Debug.Log(www); }
            Debug.Log("2");
            yield return www;
            AssetBundle bundle = www.assetBundle;
            if (bundle != null)
            {
                string[] scenePath = bundle.GetAllScenePaths();
                Debug.Log("scenepath: " + scenePath[0]);
                SceneManager.LoadScene(scenePath[0], LoadSceneMode.Single);
            }
           // bundle.Unload(false);
           // www.Dispose();
    }
}
