using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// User interface and events manager.
/// </summary>
public class UiManager : MonoBehaviour
{
    // This scene will loaded after whis level exit
    public string exitSceneName;
    // Start screen canvas
    public GameObject startScreen;
    // Pause menu canvas
    public GameObject pauseMenu;
    // Defeat menu canvas
    public GameObject defeatMenu;
    // Victory menu canvas
    public GameObject victoryMenu;
    // Level interface
    public GameObject levelUI;
    // Avaliable gold amount
    public Text goldAmount;
    // Capture attempts before defeat
    public Text defeatAttempts;
    // Victory and defeat menu display delay
    public float menuDisplayDelay = 1f;
    GameObject Cmanager;
    //GameObject firestormFX;
    // Is game paused?
    AssetBundle Starfire;
    private bool paused;
    // Camera is dragging now
    private bool cameraIsDragged;
    // Origin point of camera dragging start
    private Vector3 dragOrigin = Vector3.zero;
    // Camera control component
    private CameraControl cameraControl;
    GameObject star;
    GameObject fire;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    IEnumerator Start()
    {
        //yield return new WaitUntil(() => AssetBundleScene.myHashtable.Count != 0);
        Cmanager = GameObject.Find("CManager");
        exitSceneName = "LevelChoose";
        GameObject g1 = GameObject.Find("StartScreen");
        startScreen = g1;
        startScreen.transform.Find("BG").transform.Find("Sheet").transform.Find("Resume").gameObject.AddComponent<ButtonHandler>();
        startScreen.transform.Find("BG").transform.Find("Sheet").transform.Find("Resume").GetComponent<Button>().onClick.AddListener(() =>
        {
            startScreen.transform.Find("BG").transform.Find("Sheet").transform.Find("Resume").GetComponent<ButtonHandler>().ButtonPressed("Resume");
        });

        g1 = GameObject.FindGameObjectWithTag("LevelUI");
        levelUI = g1;
        //onclick functions for VictoryMenu
        levelUI.transform.Find("Buttons").transform.Find("Pause").gameObject.AddComponent<ButtonHandler>();
        levelUI.transform.Find("Buttons").transform.Find("Pause").GetComponent<Button>().onClick.AddListener(() => {
            levelUI.transform.Find("Buttons").transform.Find("Pause").GetComponent<ButtonHandler>().ButtonPressed("Pause");
        });
        //Adding Timer stuff
        GameObject timergo = levelUI.transform.Find("Timer").gameObject;
        timergo.AddComponent<WavesTimer>();
        timergo.GetComponent<WavesTimer>().timeBar = GameObject.Find("Sand").GetComponent<Image>();
        GameObject.Find("CurrentWave").GetComponent<Text>().text = "0";
        timergo.GetComponent<WavesTimer>().currentWaveText = GameObject.Find("CurrentWave").GetComponent<Text>();
        GameObject.Find("TotalWaves").GetComponent<Text>().text = "6";
        timergo.GetComponent<WavesTimer>().maxWaveNumberText = GameObject.Find("TotalWaves").GetComponent<Text>();
        timergo.GetComponent<WavesTimer>().highlightedFX = timergo.transform.Find("Highlight").gameObject;
        timergo.GetComponent<WavesTimer>().highlightedTO = 0.5f;

        //inside LevelUI WORK FOR StarBurst and FireStorm
        GameObject startBurst = levelUI.transform.Find("UserActions").transform.Find("StarBurst").gameObject;
        startBurst.AddComponent<UserActionIcon>();
        startBurst.GetComponent<UserActionIcon>().cooldown = 30;
        //StartCoroutine("getStarBurstAndFireStormPrefabs");
        //
        //file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/starandfireprefabs
        //WWW starAndFire = new WWW("http://203.110.85.165:9999/unity_tower_defence_Android/starandfireprefabs");
        //yield return starAndFire;
        if (AssetBundleScene.myHashtable.Count == 0) {
            yield return new WaitUntil(() => AssetBundleScene.myHashtable.Count != 0);
        }
        Starfire = (AssetBundle)AssetBundleScene.myHashtable["starandfireprefabs"];
        if (Starfire)
        {
            star = Starfire.LoadAsset("StarBurst") as GameObject;
            if (star != null)
            {
               // Debug.Log("STAR IS NOT NULL");
            }
            fire = Starfire.LoadAsset("FireStorm") as GameObject;
            if (fire != null)
            {
                //Debug.Log("FIRE IS NOT NULL");
            }
        }
        if (!star.GetComponent<AirStrike>())
        {
            star.AddComponent<AirStrike>();
            star.GetComponent<AirStrike>().damage = 5;
            star.GetComponent<AirStrike>().radius = 1;
            //effectprefab is to be set
            StartCoroutine("stareffect",star);
            star.GetComponent<AirStrike>().effectDuration = 2;
        }
        startBurst.GetComponent<UserActionIcon>().userActionPrefab = star;
        startBurst.GetComponent<UserActionIcon>().highlightIcon = startBurst.transform.Find("Highlight").gameObject;
        startBurst.GetComponent<UserActionIcon>().cooldownIcon = startBurst.transform.Find("Cooldown").gameObject;
        startBurst.GetComponent<UserActionIcon>().cooldownText = startBurst.transform.Find("CooldownText").gameObject.GetComponent<Text>();
        startBurst.transform.Find("Cooldown").gameObject.SetActive(false);
        startBurst.transform.Find("CooldownText").gameObject.SetActive(false);



        GameObject FireStorm = levelUI.transform.Find("UserActions").transform.Find("FireStorm").gameObject;
        FireStorm.AddComponent<UserActionIcon>();
        FireStorm.GetComponent<UserActionIcon>().cooldown = 60;
        if (!fire.GetComponent<AirStrike>())
        {
            fire.AddComponent<AirStrike>();
            float[] a = { 0.5f, 0.5f, 0.5f };
            fire.GetComponent<AirStrike>().delaysBeforeDamage = a;
            fire.GetComponent<AirStrike>().damage = 5;
            fire.GetComponent<AirStrike>().radius = 1;
            //effectprefab is to be set
            fireeffect(fire);
            fire.GetComponent<AirStrike>().effectDuration = 4;
        }
        FireStorm.GetComponent<UserActionIcon>().userActionPrefab = fire;
        FireStorm.GetComponent<UserActionIcon>().highlightIcon = FireStorm.transform.Find("Highlight").gameObject;
        FireStorm.GetComponent<UserActionIcon>().cooldownIcon = FireStorm.transform.Find("Cooldown").gameObject;
        FireStorm.GetComponent<UserActionIcon>().cooldownText = FireStorm.transform.Find("CooldownText").gameObject.GetComponent<Text>();


        GameObject UnitInfo = GameObject.FindGameObjectWithTag("LevelUI").transform.Find("UnitInfo").gameObject;
        UnitInfo.AddComponent<ShowInfo>();
        if (UnitInfo)
        {
            UnitInfo.GetComponent<ShowInfo>().unitName = UnitInfo.transform.Find("Content").transform.Find("UnitName").GetComponent<Text>();
            UnitInfo.GetComponent<ShowInfo>().primaryIcon = UnitInfo.transform.Find("Content").transform.Find("Image").GetComponent<Image>();
            UnitInfo.GetComponent<ShowInfo>().primaryText = UnitInfo.transform.Find("Content").transform.Find("Text").GetComponent<Text>();
            UnitInfo.GetComponent<ShowInfo>().secondaryIcon = UnitInfo.transform.Find("Content").transform.Find("Image").GetComponent<Image>();
            UnitInfo.GetComponent<ShowInfo>().secondaryText = UnitInfo.transform.Find("Content").transform.Find("Text").GetComponent<Text>();
        }

        g1 = GameObject.Find("Everything");
        GameObject g2 = g1.transform.Find("PauseMenu").gameObject;
        pauseMenu = g2;

        //onclick functions for pausemenu
        pauseMenu.transform.Find("BG").transform.Find("Resume").gameObject.AddComponent<ButtonHandler>();
        pauseMenu.transform.Find("BG").transform.Find("Resume").GetComponent<Button>().onClick.AddListener(() => {
            pauseMenu.transform.Find("BG").transform.Find("Resume").GetComponent<ButtonHandler>().ButtonPressed("Resume");
        });

        pauseMenu.transform.Find("BG").transform.Find("Back").gameObject.AddComponent<ButtonHandler>();
        pauseMenu.transform.Find("BG").transform.Find("Back").GetComponent<Button>().onClick.AddListener(() => {
            pauseMenu.transform.Find("BG").transform.Find("Back").GetComponent<ButtonHandler>().ButtonPressed("Back");
        });




        //onclick functions for DefeatMenu
        g2 = g1.transform.Find("DefeatMenu").gameObject;
        defeatMenu = g2;
        defeatMenu.transform.Find("Sheet").transform.Find("Buttons").transform.Find("Restart").gameObject.AddComponent<ButtonHandler>();
        defeatMenu.transform.Find("Sheet").transform.Find("Buttons").transform.Find("Restart").GetComponent<Button>().onClick.AddListener(() =>
        {
            defeatMenu.transform.Find("Sheet").transform.Find("Buttons").transform.Find("Restart").GetComponent<ButtonHandler>().ButtonPressed("Restart");
        });

        defeatMenu.transform.Find("Sheet").transform.Find("Buttons").transform.Find("Back").gameObject.AddComponent<ButtonHandler>();
        defeatMenu.transform.Find("Sheet").transform.Find("Buttons").transform.Find("Back").GetComponent<Button>().onClick.AddListener(() =>
        {
            defeatMenu.transform.Find("Sheet").transform.Find("Buttons").transform.Find("Back").GetComponent<ButtonHandler>().ButtonPressed("Back");
        });



        //onclick functions for VictoryMenu
        g2 = g1.transform.Find("VictoryMenu").gameObject;
        victoryMenu = g2;
        victoryMenu.transform.Find("Sheet").transform.Find("Back").gameObject.AddComponent<ButtonHandler>();
        victoryMenu.transform.Find("Sheet").transform.Find("Back").GetComponent<Button>().onClick.AddListener(() => {
            victoryMenu.transform.Find("Sheet").transform.Find("Back").GetComponent<ButtonHandler>().ButtonPressed("Back");
        });


        
        GameObject.Find("GoldAmount").GetComponent<Text>().text = "25";
        GameObject.Find("Attempts").GetComponent<Text>().text = "1";
        
        goldAmount = GameObject.Find("GoldAmount").GetComponent<Text>();
        defeatAttempts = GameObject.Find("Attempts").GetComponent<Text>();

        cameraControl = FindObjectOfType<CameraControl>();
        Debug.Assert(cameraControl && startScreen && pauseMenu && defeatMenu && victoryMenu && levelUI && defeatAttempts && goldAmount, "Wrong initial parameters");
        PauseGame(true);
    }


    IEnumerator stareffect(GameObject game) {
        if (!Cmanager) {
            Cmanager = GameObject.Find("CManager");
        }
        if (Cmanager.GetComponent<attachTower>().ExplosionAssets == null)
        {
            // Debug.Log("IN NOT FOUND BLOCK");
            //
            //file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/explosionall
            //WWW starwww = new WWW("http://203.110.85.165:9999/unity_tower_defence_Android/explosionall");
            //yield return starwww;
            if (AssetBundleScene.myHashtable.Count == 0)
            {
                yield return new WaitUntil(() => AssetBundleScene.myHashtable.Count != 0);
            }
            Cmanager.GetComponent<attachTower>().ExplosionAssets = (AssetBundle)AssetBundleScene.myHashtable["explosionall"];
        }
        // Debug.Log("called stareffect");
        game.GetComponent<AirStrike>().effectPrefab = Cmanager.GetComponent<attachTower>().ExplosionAssets.LoadAsset("StarBurstFX") as GameObject;
        if (fire == null) {
            Debug.Log("fire is null");
        }
        if (!fire.GetComponent<AirStrike>()) {
            fire.AddComponent<AirStrike>();
        }
        fire.GetComponent<AirStrike>().effectPrefab =  Cmanager.GetComponent<attachTower>().ExplosionAssets.LoadAsset("FireStormFX") as GameObject;
    }

   void fireeffect(GameObject game)
    {/*
        if (!Cmanager)
        {
            Cmanager = GameObject.Find("CManager");
        }
        if (Cmanager.GetComponent<attachTower>().ExplosionAssets == null)
        {
            Debug.Log("IN NOT FOUND BLOCK");
            WWW firewww = new WWW("http://203.110.85.165:9999/unity_tower_defence_Android/explosionall");
            yield return firewww;
            Cmanager.GetComponent<attachTower>().ExplosionAssets = firewww.assetBundle;
           // game.GetComponent<AirStrike>().effectPrefab = Cmanager.GetComponent<attachTower>().ExplosionAssets.LoadAsset("FireStormFX") as GameObject;
        }
        Debug.Log(" @@@@@@@@@@@@I M IN fireeffect NOw");
        Debug.Log("called fireeffect");
        if (Cmanager.GetComponent<attachTower>().ExplosionAssets.LoadAsset("FireStormFX") as GameObject != null) Debug.Log("FFFFFFFFFFFFFFFFFFFFFFFFFFFF NOT NULL");
        */
     // game.GetComponent<AirStrike>().effectPrefab = firestormFX;
    }

    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        EventManager.StartListening("UnitKilled", UnitKilled);
        EventManager.StartListening("ButtonPressed", ButtonPressed);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("UnitKilled", UnitKilled);
        EventManager.StopListening("ButtonPressed", ButtonPressed);
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    /*void Start()
    {
		PauseGame(true);
    }
    */
    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update()
    {
        if (paused == false)
        {
            // User press mouse button
            if (Input.GetMouseButtonDown(0) == true)
            {
                // Check if pointer over UI components
                GameObject hittedObj = null;
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);
                if (results.Count > 0) // UI components on pointer
                {
                    // Search for Action Icon hit in results
                    foreach (RaycastResult res in results)
                    {
                        if (res.gameObject.CompareTag("ActionIcon"))
                        {
                            hittedObj = res.gameObject;
                            break;
                        }
                    }
                    // Send message with user click data on UI component
                    EventManager.TriggerEvent("UserUiClick", hittedObj, null);
                }
                else // No UI components on pointer
                {
                    // Check if pointer over colliders
                    RaycastHit2D[] hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.forward);
                    foreach (RaycastHit2D hit in hits)
                    {
                        // If this is allowed collider
                        if (hit.collider.gameObject.CompareTag("Tower")
                            || hit.collider.gameObject.CompareTag("Enemy")
                            || hit.collider.gameObject.CompareTag("Defender"))
                        {
                            hittedObj = hit.collider.gameObject;
                            break;
                        }
                    }
                    // Send message with user click data on game space
                    EventManager.TriggerEvent("UserClick", hittedObj, null);
                }
                // If there is no hitted object - start camera drag
                if (hittedObj == null)
                {
                    cameraIsDragged = true;
                    dragOrigin = Input.mousePosition;
                }
            }
            if (Input.GetMouseButtonUp(0) == true)
            {
                // Stop drag camera on mouse release
                cameraIsDragged = false;
            }
            if (cameraIsDragged == true)
            {
                Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                // Camera dragging (inverted)
                cameraControl.MoveX(-pos.x);
                cameraControl.MoveY(-pos.y);
            }
        }
    }

    /// <summary>
    /// Stop current scene and load new scene
    /// </summary>
    /// <param name="sceneName">Scene name.</param>
    private void LoadScene(string sceneName)
    {
        EventManager.TriggerEvent("SceneQuit", null, null);
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
	private void ResumeGame()
    {
        GoToLevel();
        PauseGame(false);
    }

    /// <summary>
    /// Gos to main menu.
    /// </summary>
	private void ExitFromLevel()
    {
        LoadScene(exitSceneName);
    }

    /// <summary>
    /// Closes all UI canvases.
    /// </summary>
    private void CloseAllUI()
    {
        startScreen.SetActive(false);
        pauseMenu.SetActive(false);
        defeatMenu.SetActive(false);
        victoryMenu.SetActive(false);
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    /// <param name="pause">If set to <c>true</c> pause.</param>
    private void PauseGame(bool pause)
    {
        paused = pause;
        // Stop the time on pause
        Time.timeScale = pause ? 0f : 1f;
        EventManager.TriggerEvent("GamePaused", null, pause.ToString());
    }

    /// <summary>
    /// Gos to pause menu.
    /// </summary>
	private void GoToPauseMenu()
    {
        PauseGame(true);
        CloseAllUI();
        pauseMenu.SetActive(true);
    }

    /// <summary>
    /// Gos to level.
    /// </summary>
    private void GoToLevel()
    {
        CloseAllUI();
        levelUI.SetActive(true);
        PauseGame(false);
    }

    /// <summary>
    /// Gos to defeat menu.
    /// </summary>
    public void GoToDefeatMenu()
    {
        StartCoroutine("DefeatCoroutine");
    }

    /// <summary>
    /// Display defeat menu after delay.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator DefeatCoroutine()
    {
        yield return new WaitForSeconds(menuDisplayDelay);
        PauseGame(true);
        CloseAllUI();
        defeatMenu.SetActive(true);
    }

    /// <summary>
    /// Gos to victory menu.
    /// </summary>
    public void GoToVictoryMenu()
    {
        StartCoroutine("VictoryCoroutine");
    }

    /// <summary>
    /// Display victory menu after delay.
    /// </summary>
    /// <returns>The coroutine.</returns>
    private IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(menuDisplayDelay);
        PauseGame(true);
        CloseAllUI();

        // --- Game progress autosaving ---
        // Get the name of completed level
        DataManager.instance.progress.lastCompetedLevel = SceneManager.GetActiveScene().name;
        // Check if this level have no completed before
        bool hit = false;
        foreach (string level in DataManager.instance.progress.openedLevels)
        {
            if (level == SceneManager.GetActiveScene().name)
            {
                hit = true;
                break;
            }
        }
        if (hit == false)
        {
            DataManager.instance.progress.openedLevels.Add(SceneManager.GetActiveScene().name);
        }
        // Save game progress
        DataManager.instance.SaveGameProgress();

        victoryMenu.SetActive(true);
    }

    /// <summary>
    /// Restarts current level.
    /// </summary>
	private void RestartLevel()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Gets current gold amount.
    /// </summary>
    /// <returns>The gold.</returns>
    private int GetGold()
    {
        int gold;
        int.TryParse(goldAmount.text, out gold);
        return gold;
    }

    /// <summary>
    /// Sets gold amount.
    /// </summary>
    /// <param name="gold">Gold.</param>
	public void SetGold(int gold)
    {
        goldAmount.text = gold.ToString();
    }

    /// <summary>
    /// Adds the gold.
    /// </summary>
    /// <param name="gold">Gold.</param>
    private void AddGold(int gold)
    {
        SetGold(GetGold() + gold);
    }

    /// <summary>
    /// Spends the gold if it is.
    /// </summary>
    /// <returns><c>true</c>, if gold was spent, <c>false</c> otherwise.</returns>
    /// <param name="cost">Cost.</param>
    public bool SpendGold(int cost)
    {
        bool res = false;
        int currentGold = GetGold();
        if (currentGold >= cost)
        {
            SetGold(currentGold - cost);
            res = true;
        }
        return res;
    }

    /// <summary>
    /// Sets the defeat attempts.
    /// </summary>
    /// <param name="attempts">Attempts.</param>
    public void SetDefeatAttempts(int attempts)
    {
        defeatAttempts.text = attempts.ToString();
    }

    /// <summary>
    /// On unit killed by other unit.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
	private void UnitKilled(GameObject obj, string param)
    {
        // If this is enemy
        if (obj.CompareTag("Enemy"))
        {
            Price price = obj.GetComponent<Price>();
            if (price != null)
            {
                // Add gold for enemy kill
                AddGold(price.price);
            }
        }
    }

    /// <summary>
    /// Buttons pressed handler.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void ButtonPressed(GameObject obj, string param)
    {
        switch (param)
        {
            case "Pause":
                GoToPauseMenu();
                break;
            case "Resume":
                GoToLevel();
                break;
            case "Back":
                ExitFromLevel();
                break;
            case "Restart":
                RestartLevel();
                break;
        }
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
