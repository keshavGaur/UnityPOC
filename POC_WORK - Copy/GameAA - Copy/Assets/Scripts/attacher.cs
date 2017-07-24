using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using LitJson;
using UnityEngine.SceneManagement;

public class attacher : MonoBehaviour
{
    private AssetBundle bundle1;

    public IEnumerator Start()
    {
       // SceneManager.activeSceneChanged += MyMethod; // subscribe
        string newUrl = "";
        Debug.Log(SceneManager.GetActiveScene().name.ToString());
        if (SceneManager.GetActiveScene().name.ToString() == "MainMenu")
        {
            newUrl = "http://192.168.108.185:8080/projects/unity_tower_defence/data.json";
        }
        else if (SceneManager.GetActiveScene().name.ToString() == "LevelChoose")
        {
            newUrl = "http://192.168.108.185:8080/projects/unity_tower_defence/data_LevelChoose.json";
        }
        else if (SceneManager.GetActiveScene().name.ToString() == "LevelUI")
        {
            newUrl = "http://192.168.108.185:8080/projects/unity_tower_defence/data_LeveUI.json";
        }
        else
        {
            newUrl = "http://192.168.108.185:8080/projects/unity_tower_defence/data_Level.json";
        }
        Debug.Log(newUrl);
        WWW www1 = new WWW(newUrl);
        yield return www1;
        if (www1.error == null)
        {
            string jsonString = www1.text;
            Debug.Log(jsonString);
            JsonData itemData = JsonMapper.ToObject(jsonString);
            int inc = 0;
            if (bundle1 == null)
            {
                string scriptUrl = "http://192.168.108.185:8080/projects/unity_tower_defence/textassets";
                WWW www2 = new WWW(scriptUrl);
                bundle1 = www2.assetBundle;
            }
            TextAsset txt = bundle1.LoadAsset("ByteTextData.bytes") as TextAsset;
            var assembly = System.Reflection.Assembly.Load(txt.bytes);
            if (assembly != null) { Debug.Log(assembly + "is not null"); }
            // Debug.Log(itemData["attachData"][1]["e1"].ToString());
            while (itemData["attachData"][inc]["e1"].ToString() != "")
            {
                if (itemData["attachData"][inc]["e1"].ToString() == "maincamera" && itemData["attachData"][inc]["e2"].ToString() == "notgameobject")
                {
                    var type = assembly.GetType(itemData["attachData"][inc]["e4"].ToString());
                    Camera.main.gameObject.AddComponent(type);
                }
                else if (itemData["attachData"][inc]["e2"].ToString() == "gameobject" && itemData["attachData"][inc]["e3"].ToString() == "notag")
                {
                    Debug.Log(itemData["attachData"][inc]["e1"].ToString());
                    GameObject g1 = GameObject.Find(itemData["attachData"][inc]["e1"].ToString());
                    if (g1 != null)
                    {
                        Debug.Log(itemData["attachData"][inc]["e4"].ToString() + "!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                        Debug.Log(itemData["attachData"][inc]["e1"].ToString());
                        var type = assembly.GetType(itemData["attachData"][inc]["e4"].ToString());
                        g1.AddComponent(type);
                    }
                    else {
                        Debug.Log(itemData["attachData"][inc]["e1"].ToString() + "NOT FOUND @@@@@@");
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
                            go.AddComponent(type);
                        }
                    }

                }
                inc++;
            }
          //  SceneManager.activeSceneChanged += MyMethod;
        }
        else
        {
            Debug.LogError("got errrrrrrrrrrrrrrrrrrrrr");
        }
    }
    public void MyMethod(Scene scene, LoadSceneMode mode) {
        Debug.Log(scene);
    }
}



