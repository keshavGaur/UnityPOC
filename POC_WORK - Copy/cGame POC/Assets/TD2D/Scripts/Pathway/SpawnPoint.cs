using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Enemy spawner.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Enemy wave structure.
    /// </summary>
    [System.Serializable]
    public class Wave
    {
        // Delay before wave run
        public float delayBeforeWave;
        // List of enemies in this wave
        public List<GameObject> enemies;
    }

    // Enemies will have different speed in specified interval
    public float speedRandomizer = 0.2f;
    // Delay between enemies spawn in wave
    public float unitSpawnDelay = 0.8f;
    // Waves list for this spawner
    //public List<Wave> waves;
    AssetBundle bundle;
    public static GameObject element0;
    public static GameObject element1;
    public static GameObject element2;
    public static GameObject element3;
    public static GameObject element4;
    public static GameObject element5;
    public static List<GameObject> enemy1 = new List<GameObject> { element0 };
    public static List<GameObject> enemy2 = new List<GameObject> { element0, element1 };
    public static List<GameObject> enemy3 = new List<GameObject> { element0, element1, element2 };
    public static List<GameObject> enemy4 = new List<GameObject> { element0, element1, element2, element3 };
    public static List<GameObject> enemy5 = new List<GameObject> { element0, element1, element2, element3, element4 };
    public static List<GameObject> enemy6 = new List<GameObject> { element0, element1, element2, element3, element4, element5 };

    public List<Wave> waves = new List<Wave>
            {
                new Wave() {delayBeforeWave=0f, enemies = enemy1},
                new Wave() {delayBeforeWave=0f, enemies = enemy2},
                new Wave() {delayBeforeWave=0f, enemies = enemy3},
                new Wave() {delayBeforeWave=0f, enemies = enemy4},
                new Wave() {delayBeforeWave=0f, enemies = enemy5},
                new Wave() {delayBeforeWave=0f, enemies = enemy6}
            };
    // This list is used for random enemy spawn
    [HideInInspector]
    public List<GameObject> randomEnemiesList = new List<GameObject>();

    // Enemies will move along this pathway
    private Pathway path;
    // Buffer with active spawned enemies
    private List<GameObject> activeEnemies = new List<GameObject>();
    // All enemies were spawned
    private bool finished = false;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        path = GetComponentInParent<Pathway>();
        Debug.Assert(path != null, "Wrong initial parameters");
        StartCoroutine("initiateThings");
    }

    public IEnumerator initiateThings()
    {
        string prefabUrl = "file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/prefab";
        WWW wwwPrefabs = new WWW(prefabUrl);
        yield return wwwPrefabs;
        bundle = wwwPrefabs.assetBundle;
        GameObject Dog = bundle.LoadAsset("Dog") as GameObject;
        GameObject Goblin = bundle.LoadAsset("Goblin") as GameObject;
        GameObject Ogre = bundle.LoadAsset("Ogre") as GameObject;
        GameObject Orc = bundle.LoadAsset("Orc") as GameObject;
        List<GameObject> allowedList = new List<GameObject> { Dog, Goblin, Ogre, Orc };

        randomEnemiesList = allowedList;

    }

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("UnitDie", UnitDie);
        EventManager.StartListening("WaveStart", WaveStart);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("UnitDie", UnitDie);
        EventManager.StopListening("WaveStart", WaveStart);
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        // If all spawned enemies are dead
        if ((finished == true) && (activeEnemies.Count <= 0))
        {
            EventManager.TriggerEvent("AllEnemiesAreDead", null, null);
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Runs the wave.
    /// </summary>
    /// <returns>The wave.</returns>
    private IEnumerator RunWave(int waveIdx)
    {
        if (waves.Count > waveIdx)
        {
            yield return new WaitForSeconds(waves[waveIdx].delayBeforeWave);
            foreach (GameObject enemy in waves[waveIdx].enemies)
            {
                Debug.Log("GOT the waves list");
                GameObject prefab = null;
                prefab = enemy;
                // If enemy prefab not specified - spawn random enemy
                if (prefab == null && randomEnemiesList.Count > 0)
                {
                    prefab = randomEnemiesList[Random.Range(0, randomEnemiesList.Count)];
                }
                if (prefab == null)
                {
                    Debug.LogError("Have no enemy prefab. Please specify enemies in Level Manager or in Spawn Point");
                }
                // Create enemy
                // GameObject newEnemy = Instantiate(prefab, transform.position, transform.rotation);

                //  GameObject newEnemy = Instantiate((GameObject)bundle.LoadAsset(randomEnemiesList[inc].name), transform.position, transform.rotation);
                GameObject newEnemy = Instantiate((GameObject)bundle.LoadAsset(prefab.name), transform.position, transform.rotation);
                // Set pathway
                newEnemy.AddComponent<NavAgent>();
                newEnemy.AddComponent<SpriteSorting>();
                newEnemy.AddComponent<Price>();
                newEnemy.AddComponent<DamageTaker>();
                newEnemy.AddComponent<UnitInfo>();
                newEnemy.AddComponent<AiBehavior>();
                newEnemy.AddComponent<AiStatePatrol>();

                if (newEnemy.name == "Orc(Clone)")
                {
                    newEnemy.AddComponent<AiStateAttack>();


                    newEnemy.GetComponent<DamageTaker>().hitpoints = 7;
                    newEnemy.GetComponent<DamageTaker>().healthBar = newEnemy.GetComponentInChildren<Transform>().Find("HealthBar");
                    newEnemy.GetComponent<NavAgent>().speed = 0.6f;
                    newEnemy.GetComponent<UnitInfo>().primaryText = "37";
                    newEnemy.GetComponent<UnitInfo>().unitName = "Orc";
                    newEnemy.GetComponent<AiBehavior>().defaultState = FindObjectOfType<AiStatePatrol>();
                    newEnemy.GetComponent<Price>().price = 1;
                    newEnemy.GetComponent<AiStateAttack>().passiveAiState = FindObjectOfType<AiStatePatrol>();

                }

                else if (newEnemy.name == "Ogre(Clone)")
                {
                    newEnemy.AddComponent<AiStateAttack>();
                    newEnemy.GetComponent<UnitInfo>().unitName = "Ogre";
                    newEnemy.GetComponent<UnitInfo>().primaryText = "3135";
                    newEnemy.GetComponent<UnitInfo>().secondaryText = "35";
                    newEnemy.GetComponent<AiBehavior>().defaultState = FindObjectOfType<AiStatePatrol>();
                    newEnemy.GetComponent<Price>().price = 2;
                    newEnemy.GetComponent<NavAgent>().speed = 0.5f;
                    newEnemy.GetComponent<AiStateAttack>().passiveAiState = FindObjectOfType<AiStatePatrol>();
                    newEnemy.GetComponent<DamageTaker>().hitpoints = 15;
                    newEnemy.GetComponent<DamageTaker>().healthBar = newEnemy.GetComponentInChildren<Transform>().Find("HealthBar");

                }
                else if (newEnemy.name == "Goblin(Clone)")
                {
                    newEnemy.AddComponent<AiStateAttack>();
                    newEnemy.GetComponent<UnitInfo>().primaryText = "34";
                    newEnemy.GetComponent<NavAgent>().speed = 0.8f;
                    newEnemy.GetComponent<Price>().price = 1;
                    newEnemy.GetComponent<AiBehavior>().defaultState = FindObjectOfType<AiStatePatrol>();
                    newEnemy.GetComponent<AiStateAttack>().passiveAiState = FindObjectOfType<AiStatePatrol>();
                    newEnemy.GetComponent<DamageTaker>().hitpoints = 4;
                    newEnemy.GetComponent<DamageTaker>().healthBar = newEnemy.GetComponentInChildren<Transform>().Find("HealthBar");
                    newEnemy.GetComponent<UnitInfo>().unitName = "Goblin";
                }
                else if (newEnemy.name == "Dog(Clone)")
                {
                    newEnemy.GetComponent<NavAgent>().speed = 1.0f;
                    newEnemy.GetComponent<Price>().price = 1;
                    newEnemy.GetComponent<AiBehavior>().defaultState = FindObjectOfType<AiStatePatrol>();
                    newEnemy.GetComponent<DamageTaker>().hitpoints = 4;
                    newEnemy.GetComponent<DamageTaker>().healthBar = newEnemy.GetComponentInChildren<Transform>().Find("HealthBar");
                    newEnemy.GetComponent<UnitInfo>().unitName = "Hungry dog";
                    newEnemy.GetComponent<UnitInfo>().primaryText = "4";
                }

                newEnemy.GetComponent<AiStatePatrol>().path = path;

                NavAgent agent = newEnemy.GetComponent<NavAgent>();
                // Set speed offset
                agent.speed = Random.Range(agent.speed * (1f - speedRandomizer), agent.speed * (1f + speedRandomizer));
                // Add enemy to list
                activeEnemies.Add(newEnemy);
                // Wait for delay before next enemy run
                yield return new WaitForSeconds(unitSpawnDelay);
            }
            if (waveIdx + 1 == waves.Count)
            {
                finished = true;
            }
        }
    }

    /// <summary>
    /// On unit die.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void UnitDie(GameObject obj, string param)
    {
        // If this is active enemy
        if (activeEnemies.Contains(obj) == true)
        {
            // Remove it from buffer
            activeEnemies.Remove(obj);
        }
    }

    // Wave start event received
    private void WaveStart(GameObject obj, string param)
    {
        int waveIdx;
        int.TryParse(param, out waveIdx);
        StartCoroutine("RunWave", waveIdx);
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}