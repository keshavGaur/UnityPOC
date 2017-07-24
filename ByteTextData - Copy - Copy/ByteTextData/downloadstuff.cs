using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
[DefaultExecutionOrder(1)]
class downloadstuff:MonoBehaviour
{
    string prefabsPath = "http://203.110.85.165:9999/unity_tower_defence_Android/Level1.json";
    JsonData itemData2;
    int increment1 = 0;
    public static Hashtable myHashtable = new Hashtable();
    void OnEnable()
    {
        StartCoroutine("initDownload");
    }
    IEnumerator initDownload() {
        Debug.Log("First Script++++++++++++++++++++++++++++++++++++++++++++++++");
        //Debug.Log(prefabsPath);
        WWW prefabswww = new WWW(prefabsPath);
        yield return prefabswww;
        if (prefabswww == null)
        {
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
        AssetBundleScene.myHashtable = myHashtable;
    }

}
