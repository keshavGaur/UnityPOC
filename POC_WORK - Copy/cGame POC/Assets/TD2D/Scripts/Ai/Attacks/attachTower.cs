using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class attachTower : MonoBehaviour
{
    public AssetBundle towerAssets;
    public AssetBundle towerprefabAssets;
    public AssetBundle buildTreeAssetS;
    public AssetBundle ArrowsAssets;
    public AssetBundle ExplosionAssets;
    public AssetBundle towerImagesAssets;
    public AssetBundle defendptassets;
    public AssetBundle bundles;
    public AssetBundle defenderPrefab;
    public IAttack rangedAttacks;
    public string x;
    public AiState.AiTransaction[] specificTransaction;
    void Awake()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject go in gos)
        {

            go.AddComponent<SpriteSorting>();
            go.AddComponent<UnitInfo>();
            go.AddComponent<Tower>();
            //SpriteRenderer sprite = GetComponent<SpriteRenderer>().sortingOrder = 0;
            //go.transform.Find("Plate").GetComponent<SpriteRenderer>().sortingOrder = 0;
            //go.transform.Find("Floor").GetComponent<SpriteRenderer>().sortingOrder = 0;
            //go.SetActive(true);
        }

    }
}

