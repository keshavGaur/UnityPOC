using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Position for defenders.
/// </summary>
public class DefendPoint : MonoBehaviour
{
	// Prefab for defend point
	public GameObject defendPointPrefab;

	// List with defend places for this defend point
	private List<Transform> defendPlaces = new List<Transform>();
    [HideInInspector]
    public AssetBundle defendptasset;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
	{
        StartCoroutine("initThings");
        Debug.Log("DONE WITH INITthings in defendpt ");
        Debug.Assert(defendPointPrefab, "defendPointPrefab is NULL Wrong initial settings");
		// Get defend places from defend point prefab and place it on scene
		foreach (Transform defendPlace in defendPointPrefab.transform)
		{
            Debug.Log(defendPlace);
            Instantiate(defendPlace.gameObject, transform);

        }
		// Create defend places list
		foreach (Transform child in transform)
		{
			defendPlaces.Add(child);
		}
	}

    IEnumerator initThings() {
        Debug.Log("in here            xxxxx");
        //GameObject.Find("SpawnPoint").GetComponent<SpawnPoint>().
        WWW x = new WWW("file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/defend");
        yield return x;
        defendptasset = x.assetBundle;
        Debug.Log(defendptasset);
        if (defendptasset != null)
        {
            Debug.Log("NOT NULLLLLLLLLLLLLLLLLLL DEFENDPT     YO");
            GameObject[] go = GameObject.FindGameObjectsWithTag("defendpt");
            foreach (GameObject g in go) {
                g.GetComponent<DefendPoint>().defendPointPrefab = defendptasset.LoadAsset("DefendPoint") as GameObject;
            }
        }
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
