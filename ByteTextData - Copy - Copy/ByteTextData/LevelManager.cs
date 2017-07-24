using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Level control script.
/// </summary>
public class LevelManager : MonoBehaviour
{
    static int count = 0;
    // UI scene. Load on level start
    public string levelUiSceneName = "LevelUI";
    // Gold amount for this level
    public int goldAmount = 25;
    // How many times enemies can reach capture point before defeat
    public int defeatAttempts = 1;
    // List with allowed randomly generated enemy for this level
    public List<GameObject> allowedEnemies = new List<GameObject>();

    // User interface manager
    private UiManager uiManager;
    // Nymbers of enemy spawners in this level
    private int spawnNumbers;
    // Current loose counter
    private int beforeLooseCounter = 1;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        // Load UI scene
        //SceneManager.LoadScene(levelUiSceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        //yield return new WaitUntil(() => AssetBundleScene.myHashtable.Count != 0);
        if (count == 0)
        {
            SceneManager.LoadScene(levelUiSceneName, LoadSceneMode.Additive);
            count++;
        }
        uiManager = FindObjectOfType<UiManager>();
        SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
        spawnNumbers = spawnPoints.Length;
        if (spawnNumbers <= 0)
        {
          //  Debug.Log(spawnNumbers);
            Debug.LogError("Have no spawners");
        }
        // Set random enemies list for each spawner
        foreach (SpawnPoint spawnPoint in spawnPoints)
        {
            //Debug.Log("IN spawnPoint'S FOREACH");
            spawnPoint.randomEnemiesList = allowedEnemies;
        }
        Debug.Assert(uiManager, "Wrong initial parameters");
        // Set gold amount for this level
        if (uiManager)
        {
            uiManager.SetGold(goldAmount);
            beforeLooseCounter = defeatAttempts;
            uiManager.SetDefeatAttempts(beforeLooseCounter);
        }
        
    }

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("Captured", Captured);
        EventManager.StartListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("Captured", Captured);
        EventManager.StopListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    /// <summary>
    /// Enemy reached capture point.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void Captured(GameObject obj, string param)
    {
       // Debug.Log(obj.name);
        //Debug.Log("IN CAPTURED");
        if (beforeLooseCounter > 0)
        {
            beforeLooseCounter--;
            GameObject.Find("UIManager").GetComponent<UiManager>().SetDefeatAttempts(beforeLooseCounter);
            if (beforeLooseCounter <= 0)
            {
                // Defeat
                GameObject.Find("UIManager").GetComponent<UiManager>().GoToDefeatMenu();
            }
        }
    }

    /// <summary>
    /// All enemies are dead.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void AllEnemiesAreDead(GameObject obj, string param)
    {
        Debug.Log("IN AllEnemiesAreDead");
        --spawnNumbers;
        Debug.Log(spawnNumbers + ":  These are spawnNUMBERS");
        // Enemies dead at all spawners
        if (spawnNumbers <= 2)
        {
            Debug.Log("spawnno. 0");
            // Victory
            GameObject.Find("UIManager").GetComponent<UiManager>().GoToVictoryMenu();
        }
    }
}
