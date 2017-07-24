using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position for defenders.
/// </summary>
[DefaultExecutionOrder(100)]
public class DefendPoint : MonoBehaviour
{
    // Prefab for defend point
    public GameObject defendPointPrefab;

    // List with defend places for this defend point
    private List<Transform> defendPlaces = new List<Transform>();
    GameObject manager;
    [HideInInspector]
    public AssetBundle defendptasset;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {

        StartCoroutine("initThings");
        // Debug.Log("DONE WITH INITthings in defendpt ");
        Debug.Assert(defendPointPrefab, "defendPointPrefab is NULL Wrong initial settings");
    }

    IEnumerator Start()
    {
        manager = GameObject.Find("Manager");
        yield return new WaitUntil(() => defendPointPrefab != null);
        // Debug.Log("IN DEFEND POINT'S START");
        // Get defend places from defend point prefab and place it on scene
        foreach (Transform defendPlace in defendPointPrefab.transform)
        {
            // Debug.Log("In DEFEND POINTS FOREACH");
            Instantiate(defendPlace.gameObject, transform);
        }
        // Create defend places list
        foreach (Transform child in transform)
        {
            // Debug.Log("In DEFEND POINTS FOREACH");
            defendPlaces.Add(child);
        }
    }

    IEnumerator initThings()
    {
        yield return new WaitUntil(() => AssetBundleScene.myHashtable.Count != 0);
        Debug.Log("in here            xxxxx");
        //GameObject.Find("SpawnPoint").GetComponent<SpawnPoint>().
        //http://localhost:8080/projects/unity_tower_defence/
        //file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/defendpointasset
        //WWW x = new WWW("http://203.110.85.165:9999/unity_tower_defence_Android/defendpointasset");
        // yield return x;
        // defendptasset = x.assetBundle;
        //if (manager.GetComponent<hashclass>())
        //{
        //    Debug.Log("MANAGER HAS HASHCLASS");
        //    if (hashclass.thehashtableforprefabs.Count != 0)
        //     {
        //          Debug.Log("thehashtableforprefabs DOES HAVE SOME DATA");
        //      }
        //      else {
        //          Debug.Log("thehashtableforprefabs IS NULL");
        //      }
        //  }
        // else {
        //     Debug.Log("MANAGER does't HAVE HASHCLASS ");
        // }
        if (AssetBundleScene.myHashtable.Count != 0)
        {
            defendptasset = (AssetBundle)AssetBundleScene.myHashtable["defendpointasset"];
        }
        //if()
        /*int ii = 0;
        for (ii = 0; ii < xa.Length; ii++)
        {
            Debug.Log(xa[ii]+ ":"+ii);
        }
        */
        //Debug.Log(arrayofcomps+ "THIS IS ARRAY OF COMPONENTS");
        if (defendptasset == null) {
            Debug.Log("DEFEND POISNT ASSET IS NULL");
        }
        if (defendptasset != null)
        {
            Debug.Log("NOT NULLLLLLLLLLLLLLLLLLL DEFENDPT     YO");
            GameObject[] go = GameObject.FindGameObjectsWithTag("defendpt");
            foreach (GameObject g in go)
            {
                g.GetComponent<DefendPoint>().defendPointPrefab = defendptasset.LoadAsset("DefendPoint") as GameObject;
            }
        }
        yield return null;
    }

    /// <summary>
    /// Gets the defend points list.
    /// </summary>
    /// <returns>The defend points.</returns>
    public List<Transform> GetDefendPoints()
    {
        return defendPlaces;
    }
}
