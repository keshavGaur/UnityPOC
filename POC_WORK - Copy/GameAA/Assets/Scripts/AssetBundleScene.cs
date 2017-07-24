using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Reflection;
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
    //file:///C:/Users/tft/Desktop/keep/GameList.json
    //http://203.110.85.165:9999/unity_tower_defence_Android/GameList.json
    string urlofGamesList = "http://203.110.85.165:9999/unity_tower_defence_Android/GameList.json";
   // public string url = "http://"+ urlofscene;
    private AssetBundle bundle1;
    [Header("UI Stuff")]
    public Transform rootContainer;
    public Button prefab;
    public Text labelText;
    int increment = 0;
    GameObject manager;
    JsonData itemData1;
    JsonData itemData2;
    static int count = 0;
    public Hashtable myHashtable = new Hashtable();
    string loadsceneurl;
    string prefabwwwxString;
    JsonData prefabwwwx;

    AssetBundle assetBundle;

    public IEnumerator Start()
    {
        Debug.Log("In Here");
        manager = GameObject.Find("Manager");
        if (!manager.GetComponent<attacher>()) {
            manager.AddComponent<attacher>();
        }
        using (WWW www = new WWW(urlofGamesList))
        {
            yield return www;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(www.error + "there is error");
                yield break;
            }
            //
            Debug.Log("1");
            Debug.Log(www.text+ "2");
            string jsonString1 = www.text;
            Debug.Log("3");
            // Debug.Log(jsonString);
            itemData1 = JsonMapper.ToObject(jsonString1);
            Debug.Log("4");
            Debug.Log(itemData1);
            Debug.Log(itemData1["gamesAvailable"][increment]["GameId"].ToString());
            //assetBundle = www.assetBundle;
            //assetBundle.Unload(false);
            /*
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
            }*/
            while (itemData1["gamesAvailable"][increment]["GameId"].ToString() != "") {
                Debug.Log(itemData1["gamesAvailable"][increment]["GameName"].ToString());
                labelText.text = itemData1["gamesAvailable"][increment]["GameName"].ToString();
                var clone = Instantiate(prefab.gameObject) as GameObject;
                clone.GetComponent<Button>().AddEventListener(itemData1["gamesAvailable"][increment]["GameName"].ToString(), loadAssetBundleScene);
                clone.SetActive(true);
                clone.transform.SetParent(rootContainer);
                increment++;
            }
        }
    }

    public void loadAssetBundleScene(string sceneName)
    {
        int i = 0;
        while (itemData1["gamesAvailable"][i]["GameName"].ToString() != sceneName) {
            i++;
        }
        loadsceneurl = itemData1["gamesAvailable"][i]["GameScenePath"].ToString();
        //Debug.Log("ValueType of i:::::" + i);
        //Debug.Log("THIS IS THE SCENENAME:-" + sceneName);
        GameObject G1 = GameObject.Find("Manager");
        if (G1)
        {
            DontDestroyOnLoad(G1);
        }
        Debug.Log("IN LOADASSETBUNDLESCENE");
        StartCoroutine("LoadDataForScenes", sceneName);
        //Debug.Log("gettttttttttttttttttt here       all the timeeeeeeeeeeeeeeeeeeeee");
        
        /*
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        */

    }

    IEnumerator LoadDataForScenes(String sceneName)
    {
        int increment1 = 0;
        if (manager == null) {
            manager = GameObject.Find("Manager");
        }
        if (!manager.GetComponent<attacher>()) {
            manager.AddComponent<attacher>();
        }
        Debug.Log("IN LoadDataForScenes" + sceneName);
        if (manager.GetComponent<attacher>().sceneBundle == null)
        {
            WWW loadDataUrlwww = new WWW(loadsceneurl);
            yield return loadDataUrlwww;
            manager.GetComponent<attacher>().sceneBundle = loadDataUrlwww.assetBundle;
        }
        //Loading prefabs
      /*  string prefabsPath = "http://203.110.85.165:9999/unity_tower_defence_Android/" + sceneName + ".json";
        Debug.Log(prefabsPath);
        WWW prefabswww = new WWW(prefabsPath);
        yield return prefabswww;
        if (prefabswww == null) {
            Debug.Log("PREFAB PATH RETURNED NULL");
        }
        string jsonString2 = prefabswww.text;
        Debug.Log(jsonString2);
       // Debug.Log("3");
        // Debug.Log(jsonString);
        itemData2 = JsonMapper.ToObject(jsonString2);
        Debug.Log(itemData2);
        while (itemData2["prefabPaths"][increment1].ToString() != "")
        {
           // Debug.Log("1");
            //Debug.Log(itemData2["prefabPaths"][increment1].ToString() + "this is the url");
            
            WWW wwwx = new WWW(itemData2["prefabPaths"][increment1].ToString());
            yield return wwwx;
            string[] results = itemData2["prefabPaths"][increment1].ToString().Split(new string[] { "http://203.110.85.165:9999/unity_tower_defence_Android/" }, StringSplitOptions.None);
            //Debug.Log(results[1]);
            myHashtable[results[1]] = wwwx.assetBundle;
           // Debug.Log(myHashtable.Count);

            increment1++;
        }
        */

            // assetBundle = www.assetBundle;
            string[] scenes = manager.GetComponent<attacher>().sceneBundle.GetAllScenePaths();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        // return loadDataUrlwww;
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
        //yield return new WaitForSeconds(1);
        if (manager == null) {
            manager = GameObject.Find("Manager");
        }
        if (!manager.GetComponent<attacher>()) {
            manager.AddComponent<attacher>();
        }
        //manager.
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
            if (manager.GetComponent<attacher>().scriptBundle == null && bundle1 == null)
            {
                //file:///C:/Users/tft/Desktop/POC_WORK%20-%20Copy/cGame%20POC/AssetBundles/Android/textassets1
                //http://203.110.85.165:9999/unity_tower_defence_Android/textassets1
                string scriptUrl = "http://203.110.85.165:9999/unity_tower_defence_Android/textassets1";
                WWW www2 = new WWW(scriptUrl);
                yield return www2;
                manager.GetComponent<attacher>().scriptBundle = www2.assetBundle;
            }
            if (bundle1 == null)
            {
                bundle1 = manager.GetComponent<attacher>().scriptBundle;
            }
            TextAsset txt = bundle1.LoadAsset("ByteTextData.bytes") as TextAsset;
            var assembly = System.Reflection.Assembly.Load(txt.bytes);
            //if (assembly != null) {// Debug.Log(assembly + "is not null"); }
            // Debug.Log(itemData["attachData"][1]["e1"].ToString());
            //typeof(MyType).SetField("MyField", anotherObject);
            //, BindingFlags.NonPublic | BindingFlags.Instance
            //var typeprefab = assembly.GetType("hashclass");
            //FieldInfo field =typeprefab.GetField("thehashtableforprefabs");
            //field.SetValue(typeprefab , myHashtable);

            //FieldInfo field1 = typeprefab.GetField("thehashtableforprefabss");
            //field.SetValue(typeprefab, myHashtable);

            //var typeofprefab = assembly.GetType("hashclass");
            //var typeofprefabhas = manager.GetComponent(typeofprefab);
            //var typeofprefab = assembly.GetType("hashclass");
           // if (!manager.GetComponent(typeprefab)) {
              //  manager.AddComponent(typeprefab);
            //}
            //manager.GetComponent(typeofprefab).get
            var typeofDownload = assembly.GetType("downloadstuff");
            var ifhasdownload = manager.GetComponent(typeofDownload);
            if (!ifhasdownload)
            {
                manager.AddComponent(typeofDownload);
            }


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
