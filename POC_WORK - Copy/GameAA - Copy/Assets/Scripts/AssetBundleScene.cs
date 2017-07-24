using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using LitJson;
using UnityEngine.SceneManagement;

public static class ButtonExtension
{
    public static void AddEventListener<T>(this Button button, T param, Action<T> OnClick)
    {
        button.onClick.AddListener(delegate {
            OnClick(param);
        });
    }
}
public class AssetBundleScene : MonoBehaviour
{
    // original link file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/game-scene
    //public string url = "file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/game-scene";
    string urlofscene = "http://203.110.85.165:9999/unity_tower_defence_Android/game-scene";
   // public string url = "http://"+ urlofscene;
    private AssetBundle bundle1;
    [Header("UI Stuff")]
    public Transform rootContainer;
    public Button prefab;
    public Text labelText;

    AssetBundle assetBundle;

    public IEnumerator Start()
    {

        using (WWW www = new WWW(urlofscene))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error);
                yield break;
            }
            assetBundle = www.assetBundle;
            //assetBundle.Unload(false);
            string[] scenes = assetBundle.GetAllScenePaths();
            foreach (string sceneName in scenes)
            {
                //Debug.Log(Path.GetFileNameWithoutExtension(sceneName));
                if (Path.GetFileNameWithoutExtension(sceneName) == "Level1")
                {
                    //Path.GetFileNameWithoutExtension(sceneName)
                    labelText.text = "TowerDefence2D";
                    var clone = Instantiate(prefab.gameObject) as GameObject;
                    clone.GetComponent<Button>().AddEventListener(sceneName, loadAssetBundleScene);
                    clone.SetActive(true);
                    clone.transform.SetParent(rootContainer);
                }
            }
        }
    }

    public void loadAssetBundleScene(string sceneName)
    {
        //Debug.Log("gettttttttttttttttttt here       all the timeeeeeeeeeeeeeeeeeeeee");
        GameObject G1 = GameObject.Find("Manager");
        if (G1)
        {
            DontDestroyOnLoad(G1);
        }
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

    }

    public void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {

        GameObject G1 = GameObject.Find("Manager");
        DontDestroyOnLoad(G1);
        GameObject gg = GameObject.Find("DataManager");
        if (gg == null)
        {
            gg = new GameObject("DataManager");

        }
        DontDestroyOnLoad(gg);
       // Debug.Log("Level Loaded");
       // Debug.Log(scene.name);
       // Debug.Log(mode);
        //print("done with scene");
        /*if (!GameObject.Find("DataManager"))
        {
            GameObject DataManager = new GameObject();
            DontDestroyOnLoad(DataManager);
           // Debug.Log("created");
        }
        else {
           // Debug.Log("WE HAVE IT");
        }
        */
        // attacher xx = new attacher();
        StartCoroutine(BinaryAttacher());

    }

    public IEnumerator BinaryAttacher()
    {

        string newUrl = "";
       // Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.ToString() == "MainMenu")
        {
            //http://203.110.85.165:9999/unity_tower_defence_Android/
            newUrl = "http://203.110.85.165:9999/unity_tower_defence_Android/data.json";
        }
        else if (SceneManager.GetActiveScene().name.ToString() == "LevelChoose")
        {
            //newUrl = "file:///E:/data_LevelChoose.json";
            newUrl = "http://203.110.85.165:9999/unity_tower_defence_Android/data_LevelChoose.json";
        }
        else if (SceneManager.GetActiveScene().name.ToString() == "LevelUI")
        {
            //newUrl = "file:///E:/data_LeveUI.json";
            newUrl = "http://203.110.85.165:9999/unity_tower_defence_Android/data_LeveUI.json";
        }
        else
        {
            //newUrl = "file:///E:/data_Level.json";
            newUrl = "http://203.110.85.165:9999/unity_tower_defence_Android/data_Level.json";
        }
       // Debug.Log(newUrl);
        WWW www1 = new WWW(newUrl);
        yield return www1;
        if (www1.error == null)
        {
            string jsonString = www1.text;
           // Debug.Log(jsonString);
            JsonData itemData = JsonMapper.ToObject(jsonString);
            int inc = 0;
            if (bundle1 == null)
            {
                string scriptUrl = "http://203.110.85.165:9999/unity_tower_defence_Android/textassets";
                WWW www2 = new WWW(scriptUrl);
                yield return www2;
                bundle1 = www2.assetBundle;
            }
            TextAsset txt = bundle1.LoadAsset("ByteTextData.bytes") as TextAsset;
            var assembly = System.Reflection.Assembly.Load(txt.bytes);
            //if (assembly != null) {// Debug.Log(assembly + "is not null"); }
            // Debug.Log(itemData["attachData"][1]["e1"].ToString());
            while (itemData["attachData"][inc]["e1"].ToString() != "")
            {
                if (itemData["attachData"][inc]["e1"].ToString() == "maincamera" && itemData["attachData"][inc]["e2"].ToString() == "notgameobject")
                {
                    var type = assembly.GetType(itemData["attachData"][inc]["e4"].ToString());
                   // Debug.Log(itemData["attachData"][inc]["e4"].ToString());
                    if (!Camera.main.gameObject.GetComponent(type))
                    {
                        Camera.main.gameObject.AddComponent(type);
                    }
                }
                else if (itemData["attachData"][inc]["e2"].ToString() == "gameobject" && itemData["attachData"][inc]["e3"].ToString() == "notag")
                {
                   // Debug.Log(itemData["attachData"][inc]["e1"].ToString());
                    GameObject g1 = GameObject.Find(itemData["attachData"][inc]["e1"].ToString());
                    if (g1 != null)
                    {
                      //  Debug.Log(itemData["attachData"][inc]["e4"].ToString() + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                      //  Debug.Log(itemData["attachData"][inc]["e1"].ToString());
                        var type = assembly.GetType(itemData["attachData"][inc]["e4"].ToString());
                        var cc = g1.GetComponent(type);
                        if (!cc)
                        {
                            g1.AddComponent(type);
                        }
                    }
                }
                else if (itemData["attachData"][inc]["e2"].ToString() == "gameobject" && itemData["attachData"][inc]["e3"].ToString() != "notag")
                {
                    GameObject[] gos = GameObject.FindGameObjectsWithTag(itemData["attachData"][inc]["e3"].ToString());
                    foreach (GameObject go in gos)
                    {
                        var type = assembly.GetType(itemData["attachData"][inc]["e4"].ToString());
                        if (go != null)
                        {
                            var xx = go.GetComponent(type);
                            if (!xx)
                            {
                                go.AddComponent(type);
                            }
                        }
                    }

                }
                inc++;
            }
        }
        else
        {
            Debug.LogError("got errrrrrrrrrrrrrrrrrrrrr");
        }
    }
}
