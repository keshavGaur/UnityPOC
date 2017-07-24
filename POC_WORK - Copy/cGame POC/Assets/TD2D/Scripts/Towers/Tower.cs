using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tower building and operation.
/// </summary>
public class Tower : MonoBehaviour
{
    // Prefab for building tree
    public GameObject buildingTreePrefab;
    // Visualisation of attack range for this tower
    public GameObject rangeImage;
    //AssetBundle towerAsset;
    GameObject newTower;
    // User interface manager
    private UiManager uiManager;
    // Level UI canvas for building tree display
    private Canvas canvas;
    // Collider of this tower
    private Collider2D bodyCollider;
    // Displayed building tree
    private BuildingTree activeBuildingTree;
    [HideInInspector]
    public AssetBundle towerprefabAsset;
    //AssetBundle buildTreeAsset;
    //public AssetBundle buildTreeAsset;
    static int count = 0;
    Texture2D tex;
    //[HideInInspector]
    //public AssetBundle menuPrefabsAsset;
    //GameObject newGame;
    // public WWW TowerPrefabs;
    GameObject Cmanager;
    public GameObject RangedAttack;
    /// <summary>
    /// Raises the enable event.
    /// </summary>
    void OnEnable()
    {
        bodyCollider = GetComponent<Collider2D>();
        EventManager.StartListening("GamePaused", GamePaused);
        EventManager.StartListening("UserClick", UserClick);
        EventManager.StartListening("UserUiClick", UserClick);
    }

    /// <summary>
    /// Raises the disable event.
    /// </summary>
    void OnDisable()
    {
        EventManager.StopListening("GamePaused", GamePaused);
        EventManager.StopListening("UserClick", UserClick);
        EventManager.StopListening("UserUiClick", UserClick);
    }

    /// <summary>
    /// Atart this instance.
    /// </summary>
    void Start()
    {
        //Debug.Log("IN TOWER'S START");
        GameObject[] gots = GameObject.FindGameObjectsWithTag("Tower");
        foreach (GameObject got in gots)
        {
            // Debug.Log("IN TOWER'S foreach in START");
            got.transform.Find("Plate").GetComponent<SpriteRenderer>().sortingOrder = 0;
            got.transform.Find("Floor").GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
        uiManager = FindObjectOfType<UiManager>();
        // Debug.Log("DONE WITH TOWER'S foreach in START");
        if (uiManager != null)
        {
            GameObject x = GameObject.Find("UIManager");
            if (!x.GetComponent<UiManager>())
            {
                uiManager = x.AddComponent<UiManager>();
            }
        }
        //if (uiManager!=null) { Debug.Log("1:  uiManager is not null"); }
        canvas = GameObject.FindGameObjectWithTag("LevelUI").GetComponent<Canvas>();
        // if (canvas) { Debug.Log("2:  canvas is not null"); }
        bodyCollider = GetComponent<Collider2D>();
        // if (bodyCollider != null) { Debug.Log("3:  bodyCollider is not null"); }
        //   Debug.Log("4:  called init things");
        StartCoroutine("initThings");
        //  Debug.Log("OUTOF init THINGS ##############~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        Cmanager = GameObject.Find("Cmanager");
        //   Debug.Log("Out of TOWER'S start");
        // This canvas wiil use to place building tree UI
        //Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        /*foreach (Canvas canv in canvases)
        {
            if (canv.CompareTag("LevelUI"))
            {
                canvas = canv;
                break;
            }
        }
        */
    }
    IEnumerator initThings()
    {
        Debug.Log("IN initThings ");
        //Debug.Log("xxxxx in here Tower one xxxxx");
        //GameObject.Find("SpawnPoint").GetComponent<SpawnPoint>().
        if (Cmanager == null)
        {
            Cmanager = GameObject.Find("CManager");
        }
        if (!Cmanager.GetComponent<attachTower>())
        {
            Cmanager.AddComponent<attachTower>();
        }
        if (Cmanager.GetComponent<attachTower>().towerAssets == null)
        {
            WWW x = new WWW("file:///C:/Users/tft/Desktop/CGame/AssetBundles/Windows/newbuild");
            yield return x;
            Cmanager.GetComponent<attachTower>().towerAssets = x.assetBundle;
        }

        //Debug.Log(towerAsset);
        //doing stuff
        //  Debug.Log("TOWERASSET IS NOT NULL");
        GameObject xx = Cmanager.GetComponent<attachTower>().towerAssets.LoadAsset("BuildingTree") as GameObject;
        if (xx != null) Debug.Log("xx Not NULL XXXXXXXXXXXXXXXXX");
        Debug.Log("CALLED imagevala");
        yield return StartCoroutine(imagevala());
        Sprite xxxx = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        xx.transform.Find("BG").GetComponent<Image>().sprite = xxxx;
        xx.AddComponent(typeof(BuildingTree));
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Tower");
        // Debug.Log("value of xx is :" + xx.ToString());
        foreach (GameObject g in gos)
        {
            //Debug.Log("In TOWER FOR LOOP");
            {
                if (xx != null)
                {
                    Debug.Log("XX IS NOT NULL ");
                    g.GetComponent<Tower>().buildingTreePrefab = xx;
                }
            }
        }
    }
    void Update()
    {
        if (count == 0)
        {
            StartCoroutine("initThings");
            count++;
        }
        if (newTower != null)
        {
            newTower.transform.Find("Body").GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }
    /// <summary>
    /// Opens the building tree.
    /// </summary>
    private void OpenBuildingTree()
    {
        //file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/towermakeoverprefabs
        Debug.Log("IN OPEN BUILDING TREE");
        // Create building tree
        // if (!buildingTreePrefab)
        // {
        // Debug.Log("buildingTreePrefab IS NOT PRESENT HERE");
        //}
        //if (buildingTreePrefab)
        // {
        // Debug.Log(buildingTreePrefab + "buildingTreePrefab IS PRESENT SOME OTHER PROBLEM");
        // }
        //start things
        uiManager = FindObjectOfType<UiManager>();
        canvas = GameObject.FindGameObjectWithTag("LevelUI").GetComponent<Canvas>();
        bodyCollider = GetComponent<Collider2D>();
        Debug.Assert(uiManager && canvas && bodyCollider, "Wrong initial parameters");
        //if (buildingTreePrefab.transform.Find("BG").GetComponent<Image>() != null)
        //{
        //  Debug.Log("buildingTreePrefab HAS IMAGE");
        //}
        //else Debug.Log("buildingTreePrefab HAS NO IMAGE");

        // Debug.Log("GOING TO INSTANTIATE @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
        activeBuildingTree = Instantiate(buildingTreePrefab, canvas.transform).GetComponent<BuildingTree>();
        // Debug.Log("INSTANTIATED buildingTreePrefab");
        // Set it over the tower

        GameObject v = activeBuildingTree.transform.Find("Bowman").gameObject;
        v.SetActive(true);
        // StartCoroutine("attachThings",);
        v = activeBuildingTree.transform.Find("Magic").gameObject;
        v.SetActive(true);
        v = activeBuildingTree.transform.Find("Barrack").gameObject;
        v.SetActive(true);
        v = activeBuildingTree.transform.Find("Mortar").gameObject;
        v.SetActive(true);
        // Debug.Log("ACTIVATING GAMEOBJECTS");
        v = activeBuildingTree.transform.Find("Ballista").gameObject;
        v.SetActive(true);
        //  Debug.Log("STARTING COROUTINE");
        StartCoroutine("attachThings");
        activeBuildingTree.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        activeBuildingTree.myTower = this;
        // Disable tower raycastv, 
        bodyCollider.enabled = false;

    }

    IEnumerator imagevala()
    {
        Debug.Log("In image download part ");
        if (!Cmanager) Cmanager = GameObject.Find("CManager");
        if (Cmanager.GetComponent<attachTower>().towerImagesAssets == null)
        {
            WWW TowerImageswww = new WWW("file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/ring");
            yield return TowerImageswww;
            Cmanager.GetComponent<attachTower>().towerImagesAssets = TowerImageswww.assetBundle;
            tex = Cmanager.GetComponent<attachTower>().towerImagesAssets.LoadAsset("image.jpg") as Texture2D;
            //Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            //tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f
        }
    }
    IEnumerator attachThings()
    {
        /// Debug.Log(obj.name + "THIS IS GAMEOBJECTS NAME");
        /// 

        //GameObject x = Cmanager.GetComponent<attachTower>().towerImagesAssets.LoadAsset("Building_ring_01") as GameObject;
        // if (Cmanager.GetComponent<attachTower>().towerImagesAssets.LoadAsset("Building_ring_01") as Sprite != null) { Debug.Log("SPRITE IS NOT NULL"); }
        //activeBuildingTree.transform.Find("BG").gameObject.GetComponent<Image>().sprite = Cmanager.GetComponent<attachTower>().towerImagesAssets.LoadAsset("Building_ring_01_05") as Sprite;

        // Debug.Log("IN ATTACHTHINGS");

        if (Cmanager.GetComponent<attachTower>().towerprefabAssets == null)
        {
            // Debug.Log("DOWNLOADING STUFF");
            WWW TowerPrefabs = new WWW("file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/towerprefab");
            yield return TowerPrefabs;
            Cmanager.GetComponent<attachTower>().towerprefabAssets = TowerPrefabs.assetBundle;
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Tower");

            foreach (GameObject G in gos)
            {
                G.GetComponent<Tower>().towerprefabAsset = TowerPrefabs.assetBundle;
            }
            towerprefabAsset = TowerPrefabs.assetBundle;
        }

        towerprefabAsset = Cmanager.GetComponent<attachTower>().towerprefabAssets;
        // Debug.Log(" IN REAL ATTACH THINGS");
        GameObject go = GameObject.Find("Bowman");
        //  Debug.Log("Adding building Icon");
        if (!go.GetComponent<BuildingIcon>())
        {
            go.AddComponent<BuildingIcon>();
        }
        ///   Debug.Log("Added building Icon");
        //if (go.name == "Barrack") obj.name = "Barracks"; 
        GameObject BowmanL1 = towerprefabAsset.LoadAsset("BowmanL1") as GameObject;

        if (!BowmanL1.GetComponent<AiStateIdle>())
        {
            BowmanL1.AddComponent<AiStateIdle>();
        }
        AiStateIdle bowmanaistate = BowmanL1.transform.gameObject.GetComponent<AiStateIdle>();
        if (!BowmanL1.GetComponent<AiStateAttack>())
        {
            BowmanL1.AddComponent<AiStateAttack>();
        }
        BowmanL1.GetComponent<AiStateAttack>().useTargetPriority = true;
        BowmanL1.GetComponent<AiStateAttack>().passiveAiState = bowmanaistate;


        AiStateAttack x = BowmanL1.GetComponent<AiStateAttack>();
        AiState.AiTransaction[] aitransactionArray = { new AiState.AiTransaction() { trigger = AiState.Trigger.TriggerStay, newState = x } };
        BowmanL1.GetComponent<AiStateIdle>().specificTransactions = aitransactionArray;

        if (!BowmanL1.GetComponent<SpriteSorting>())
        {
            BowmanL1.AddComponent(typeof(SpriteSorting));

            BowmanL1.GetComponent<SpriteSorting>().isStatic = true;
        }
        if (!BowmanL1.GetComponent<Price>())
        {
            BowmanL1.AddComponent(typeof(Price));

            BowmanL1.GetComponent<Price>().price = 5;
        }
        if (!BowmanL1.GetComponent<UnitInfo>())
        {
            BowmanL1.AddComponent(typeof(UnitInfo));
            BowmanL1.GetComponent<UnitInfo>().unitName = "Archer tower";
            BowmanL1.GetComponent<UnitInfo>().primaryText = "1.0";
            //todo for primary and secondary icon
            BowmanL1.GetComponent<UnitInfo>().secondaryText = "1";
        }

        if (!BowmanL1.GetComponent<Tower>())
        {
            BowmanL1.AddComponent(typeof(Tower));

            //  Debug.Log("Going TO LOAD TOWER FOR BOWMAN  NOW !!!!!!!!!!!");
            if (Cmanager.GetComponent<attachTower>().buildTreeAssetS == null)
            {
                WWW buildTree = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/buildingtreechildprefabs");
                yield return buildTree;
                Cmanager.GetComponent<attachTower>().buildTreeAssetS = buildTree.assetBundle;

            }
            GameObject BowmanBuild = Cmanager.GetComponent<attachTower>().buildTreeAssetS.LoadAsset("BuildingTreeBowman") as GameObject;
            //  Debug.Log("LOADED TOWER ALSO");

            BowmanBuild.AddComponent(typeof(BuildingTree));
            if (BowmanBuild)
            {
                //     Debug.Log("BOWMAN BUILD IS NOT NULL");
                BowmanL1.GetComponent<Tower>().buildingTreePrefab = BowmanBuild;
                BowmanL1.GetComponent<Tower>().rangeImage = BowmanL1.transform.Find("RangedAttack").transform.Find("Range").gameObject;
            }
        }
        if (!BowmanL1.GetComponent<AiBehavior>())
        {
            BowmanL1.AddComponent<AiBehavior>();
            BowmanL1.GetComponent<AiBehavior>().defaultState = bowmanaistate;
        }

        RangedAttack = BowmanL1.transform.Find("RangedAttack").gameObject;
        GetRangedAttack(RangedAttack, BowmanL1);
        //  Debug.Log("1 NEARLY COMPLETED");
        go.SetActive(true);
        go.GetComponent<BuildingIcon>().towerPrefab = BowmanL1;



        go = GameObject.Find("Magic");
        //  Debug.Log("IN MAGIC");
        if (!go.GetComponent<BuildingIcon>())
        {
            go.AddComponent<BuildingIcon>();
        }
        GameObject MagicL1 = towerprefabAsset.LoadAsset("MagicL1") as GameObject;
        if (!MagicL1.GetComponent<AiStateIdle>())
        {
            MagicL1.AddComponent<AiStateIdle>();
        }

        if (!MagicL1.GetComponent<AiStateAttack>())
        {
            MagicL1.AddComponent<AiStateAttack>();
            MagicL1.GetComponent<AiStateAttack>().useTargetPriority = true;
            MagicL1.GetComponent<AiStateAttack>().passiveAiState = MagicL1.transform.gameObject.GetComponent<AiStateIdle>();
        }
        AiStateAttack magicaistate = MagicL1.transform.gameObject.GetComponent<AiStateAttack>();
        AiState.AiTransaction[] aitransactionArrayMagic = { new AiState.AiTransaction() { trigger = AiState.Trigger.TriggerStay, newState = magicaistate } };
        MagicL1.GetComponent<AiStateIdle>().specificTransactions = aitransactionArrayMagic;
        if (!MagicL1.GetComponent<SpriteSorting>())
        {
            MagicL1.AddComponent(typeof(SpriteSorting));
            MagicL1.GetComponent<SpriteSorting>().isStatic = true;
        }
        if (!MagicL1.GetComponent<Price>())
        {
            MagicL1.AddComponent(typeof(Price));
            MagicL1.GetComponent<Price>().price = 6;
        }

        if (MagicL1.GetComponent<UnitInfo>())
        {
            MagicL1.AddComponent(typeof(UnitInfo));
            MagicL1.GetComponent<UnitInfo>().unitName = "Magic tower";
            MagicL1.GetComponent<UnitInfo>().primaryText = "2.2";
            //todo for primary and secondary icon
            MagicL1.GetComponent<UnitInfo>().secondaryText = "2";
        }

        if (!MagicL1.GetComponent<Tower>())
        {
            MagicL1.AddComponent(typeof(Tower));
        }
        //   Debug.Log("bUILDING TREE FOR MAGIC");
        GameObject MagicBuild = Cmanager.GetComponent<attachTower>().buildTreeAssetS.LoadAsset("BuildingTreeMagic") as GameObject;
        MagicBuild.AddComponent(typeof(BuildingTree));
        MagicL1.GetComponent<Tower>().buildingTreePrefab = MagicBuild;
        MagicL1.GetComponent<Tower>().rangeImage = MagicL1.transform.Find("RangedAttack").transform.Find("Range").gameObject;

        if (!MagicL1.GetComponent<AiBehavior>())
        {
            MagicL1.AddComponent<AiBehavior>();
            MagicL1.GetComponent<AiBehavior>().defaultState = MagicL1.transform.gameObject.GetComponent<AiStateIdle>();
        }
        RangedAttack = MagicL1.transform.Find("RangedAttack").gameObject;
        GetRangedAttack(RangedAttack, MagicL1);
        go.SetActive(true);
        go.GetComponent<BuildingIcon>().towerPrefab = MagicL1;

        go = GameObject.Find("Barrack");
        if (!go.GetComponent<BuildingIcon>())
        {
            go.AddComponent<BuildingIcon>();
        }
        GameObject BarracksL1 = towerprefabAsset.LoadAsset("BarracksL1") as GameObject;
        if (!BarracksL1.GetComponent<SpriteSorting>())
        {
            BarracksL1.AddComponent(typeof(SpriteSorting));
            BarracksL1.GetComponent<SpriteSorting>().isStatic = true;
        }
        if (!BarracksL1.GetComponent<Price>())
        {
            BarracksL1.AddComponent(typeof(Price));
            BarracksL1.GetComponent<Price>().price = 4;
        }
        if (!BarracksL1.GetComponent<UnitInfo>())
        {
            BarracksL1.AddComponent(typeof(UnitInfo));
            BarracksL1.GetComponent<UnitInfo>().unitName = "Barracks";
            BarracksL1.GetComponent<UnitInfo>().primaryText = "10.0";
            //todo for primary and secondary icon
            BarracksL1.GetComponent<UnitInfo>().secondaryText = "1";
        }

        if (!BarracksL1.GetComponent<Tower>())
        {
            BarracksL1.AddComponent(typeof(Tower));
            GameObject barracksbuild = Cmanager.GetComponent<attachTower>().buildTreeAssetS.LoadAsset("BuildingTreeBarracks") as GameObject;
            barracksbuild.AddComponent(typeof(BuildingTree));
            BarracksL1.GetComponent<Tower>().buildingTreePrefab = barracksbuild;
        }

        if (!BarracksL1.GetComponent<DefendersSpawner>())
        {
            BarracksL1.AddComponent<DefendersSpawner>();
            BarracksL1.GetComponent<DefendersSpawner>().cooldown = 10;
            BarracksL1.GetComponent<DefendersSpawner>().maxNum = 1;
            WWW defendprefabwww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/defenderprefab");
            yield return defendprefabwww;
            Cmanager.GetComponent<attachTower>().defenderPrefab = defendprefabwww.assetBundle;
            GameObject swordMan = Cmanager.GetComponent<attachTower>().defenderPrefab.LoadAsset("Swordsman") as GameObject;
            if (!swordMan.GetComponent<SpriteSorting>())
            {
                swordMan.AddComponent<SpriteSorting>();
            }
            if (!swordMan.GetComponent<NavAgent>())
            {
                swordMan.AddComponent<NavAgent>();
            }
            if (!swordMan.GetComponent<DamageTaker>())
            {
                swordMan.AddComponent<DamageTaker>();
                swordMan.GetComponent<DamageTaker>().hitpoints = 10;
                swordMan.GetComponent<DamageTaker>().damageDisplayTime = 0.2f;
                swordMan.GetComponent<DamageTaker>().healthBar = swordMan.transform.Find("HealthBar");
            }
            if (!swordMan.GetComponent<UnitInfo>())
            {
                swordMan.AddComponent<UnitInfo>();
                swordMan.GetComponent<UnitInfo>().unitName = "Swordsman";
                swordMan.GetComponent<UnitInfo>().primaryText = "10";
                swordMan.GetComponent<UnitInfo>().secondaryText = "1";
            }

            if (!swordMan.GetComponent<AiStateIdle>())
            {
                swordMan.AddComponent<AiStateIdle>();
            }

            if (!swordMan.GetComponent<AiStateAttack>())
            {
                swordMan.AddComponent<AiStateAttack>();
                swordMan.GetComponent<AiStateAttack>().useTargetPriority = true;
                swordMan.GetComponent<AiStateAttack>().passiveAiState = swordMan.transform.gameObject.GetComponent<AiStateIdle>();
            }
            AiStateAttack swordaistate = swordMan.transform.gameObject.GetComponent<AiStateAttack>();
            AiState.AiTransaction[] aitransactionArraysword = { new AiState.AiTransaction() { trigger = AiState.Trigger.TriggerStay, newState = swordaistate } };
            swordMan.GetComponent<AiStateIdle>().specificTransactions = aitransactionArraysword;
            if (!swordMan.GetComponent<AiStateMove>())
            {
                swordMan.AddComponent<AiStateMove>();
                swordMan.GetComponent<AiStateMove>().passiveAiState = swordMan.transform.gameObject.GetComponent<AiStateIdle>();
            }

            if (!swordMan.GetComponent<AiBehavior>())
            {
                swordMan.AddComponent<AiBehavior>();
                swordMan.GetComponent<AiBehavior>().defaultState = swordMan.transform.gameObject.GetComponent<AiStateMove>();
            }

            BarracksL1.GetComponent<DefendersSpawner>().prefab = swordMan;
            BarracksL1.GetComponent<DefendersSpawner>().spawnPoint = BarracksL1.transform.Find("SpawnPoint").GetComponent<Transform>();
        }
        go.SetActive(true);
        go.GetComponent<BuildingIcon>().towerPrefab = BarracksL1;



        go = GameObject.Find("Mortar");
        if (!go.GetComponent<BuildingIcon>())
        {
            go.AddComponent<BuildingIcon>();
        }
        GameObject MortarL1 = towerprefabAsset.LoadAsset("MortarL1") as GameObject;
        if (!MortarL1.GetComponent<AiStateIdle>())
        {
            MortarL1.AddComponent<AiStateIdle>();
        }

        if (!MortarL1.GetComponent<AiStateAttack>())
        {
            MortarL1.AddComponent<AiStateAttack>();
            MortarL1.GetComponent<AiStateAttack>().useTargetPriority = true;
            MortarL1.GetComponent<AiStateAttack>().passiveAiState = MortarL1.transform.gameObject.GetComponent<AiStateIdle>();
        }
        AiStateAttack mortaraistate = MortarL1.transform.gameObject.GetComponent<AiStateAttack>();
        AiState.AiTransaction[] aitransactionArrayMortar = { new AiState.AiTransaction() { trigger = AiState.Trigger.TriggerStay, newState = mortaraistate } };
        MortarL1.GetComponent<AiStateIdle>().specificTransactions = aitransactionArrayMortar;
        if (!MortarL1.GetComponent<SpriteSorting>())
        {
            MortarL1.AddComponent(typeof(SpriteSorting));
            MortarL1.GetComponent<SpriteSorting>().isStatic = true;
        }
        if (!MortarL1.GetComponent<Price>())
        {
            MortarL1.AddComponent(typeof(Price));
            MortarL1.GetComponent<Price>().price = 10;
        }
        if (!MortarL1.GetComponent<UnitInfo>())
        {
            MortarL1.AddComponent(typeof(UnitInfo));
            MortarL1.GetComponent<UnitInfo>().unitName = "Mortar";
            MortarL1.GetComponent<UnitInfo>().primaryText = "3.0";
            //todo for primary and secondary icon
            MortarL1.GetComponent<UnitInfo>().secondaryText = "3";
        }

        if (!MortarL1.GetComponent<Tower>())
        {
            MortarL1.AddComponent(typeof(Tower));
            GameObject mortarbuild = Cmanager.GetComponent<attachTower>().buildTreeAssetS.LoadAsset("BuildingTreeMortar") as GameObject;
            mortarbuild.AddComponent(typeof(BuildingTree));
            MortarL1.GetComponent<Tower>().buildingTreePrefab = mortarbuild;
            MortarL1.GetComponent<Tower>().rangeImage = MortarL1.transform.Find("RangedAttack").transform.Find("Range").gameObject;
        }

        if (!MortarL1.GetComponent<AiBehavior>())
        {
            MortarL1.AddComponent<AiBehavior>();
            MortarL1.GetComponent<AiBehavior>().defaultState = MortarL1.transform.gameObject.GetComponent<AiStateIdle>();
        }

        RangedAttack = MortarL1.transform.Find("RangedAttack").gameObject;
        GetRangedAttack(RangedAttack, MortarL1);

        go.SetActive(true);
        go.GetComponent<BuildingIcon>().towerPrefab = MortarL1;


        go = GameObject.Find("Ballista");
        if (!go.GetComponent<BuildingIcon>())
        {
            go.AddComponent<BuildingIcon>();
        }
        GameObject BallistaL1 = towerprefabAsset.LoadAsset("BallistaL1") as GameObject;
        if (!BallistaL1.GetComponent<AiStateIdle>())
        {
            BallistaL1.AddComponent<AiStateIdle>();
        }

        if (!BallistaL1.GetComponent<AiStateAttack>())
        {
            BallistaL1.AddComponent<AiStateAttack>();
            BallistaL1.GetComponent<AiStateAttack>().useTargetPriority = true;
            BallistaL1.GetComponent<AiStateAttack>().passiveAiState = BallistaL1.transform.gameObject.GetComponent<AiStateIdle>();
        }
        AiStateAttack ballistaaistate = BallistaL1.transform.gameObject.GetComponent<AiStateAttack>();
        AiState.AiTransaction[] aitransactionArrayBallista = { new AiState.AiTransaction() { trigger = AiState.Trigger.TriggerStay, newState = ballistaaistate } };
        BallistaL1.GetComponent<AiStateIdle>().specificTransactions = aitransactionArrayBallista;
        if (!BallistaL1.GetComponent<SpriteSorting>())
        {
            BallistaL1.AddComponent(typeof(SpriteSorting));
            BallistaL1.GetComponent<SpriteSorting>().isStatic = true;
        }
        if (!BallistaL1.GetComponent<Price>())
        {
            BallistaL1.AddComponent(typeof(Price));
            BallistaL1.GetComponent<Price>().price = 15;
        }
        if (!BallistaL1.GetComponent<UnitInfo>())
        {
            BallistaL1.AddComponent(typeof(UnitInfo));
            BallistaL1.GetComponent<UnitInfo>().unitName = "Ballista";
            BallistaL1.GetComponent<UnitInfo>().primaryText = "3.0";
            //todo for primary and secondary icon
            BallistaL1.GetComponent<UnitInfo>().secondaryText = "5";
        }

        if (!BallistaL1.GetComponent<Tower>())
        {
            BallistaL1.AddComponent(typeof(Tower));
            GameObject ballistabuild = Cmanager.GetComponent<attachTower>().buildTreeAssetS.LoadAsset("BuildingTreeBallista") as GameObject;
            ballistabuild.AddComponent(typeof(BuildingTree));
            BallistaL1.GetComponent<Tower>().buildingTreePrefab = ballistabuild;
            BallistaL1.GetComponent<Tower>().rangeImage = BallistaL1.transform.Find("RangedAttack").transform.Find("Range").gameObject;
        }

        if (!BallistaL1.GetComponent<AiBehavior>())
        {
            BallistaL1.AddComponent<AiBehavior>();
            BallistaL1.GetComponent<AiBehavior>().defaultState = BallistaL1.transform.gameObject.GetComponent<AiStateIdle>();
        }

        RangedAttack = BallistaL1.transform.Find("RangedAttack").gameObject;
        GetRangedAttack(RangedAttack, BallistaL1);

        go.SetActive(true);
        go.GetComponent<BuildingIcon>().towerPrefab = BallistaL1;

    }





    void GetRangedAttack(GameObject RA, GameObject appliedOn)
    {
        //  Debug.Log("IN Getranged attack function " + appliedOn.name);
        if (RA != null)
        {
            RA.AddComponent<AiColliderTrigger>();
            List<string> ab = new List<string> { "Enemy" };
            RA.GetComponent<AiColliderTrigger>().tags = ab;


            RA.AddComponent<AttackRanged>();
            if (appliedOn.name == "BallistaL1")
            {
                //appliedOn.transform.Find("Body").GetComponent<SpriteRenderer>().sortingOrder = 0;
                RangedAttack.GetComponent<AttackRanged>().damage = 5;
                RangedAttack.GetComponent<AttackRanged>().cooldown = 3;
                //Arrow prefab need to be set
                StartCoroutine("addBallistaArrow", RangedAttack);
                RangedAttack.GetComponent<AttackRanged>().firePoint = appliedOn.transform.Find("FirePoint").GetComponent<Transform>();
            }
            if (appliedOn.name == "MortarL1")
            {
                RangedAttack.GetComponent<AttackRanged>().damage = 3;
                RangedAttack.GetComponent<AttackRanged>().cooldown = 3;
                //Arrow prefab need to be set
                StartCoroutine("addMortarArrow", RangedAttack);
                RangedAttack.GetComponent<AttackRanged>().firePoint = appliedOn.transform.Find("FirePoint").GetComponent<Transform>();
            }

            if (appliedOn.name == "MagicL1")
            {
                RangedAttack.GetComponent<AttackRanged>().damage = 2;
                RangedAttack.GetComponent<AttackRanged>().cooldown = 2.2f;
                //Arrow prefab need to be set
                StartCoroutine("addMagicArrow", RangedAttack);
                RangedAttack.GetComponent<AttackRanged>().firePoint = appliedOn.transform.Find("FirePoint").GetComponent<Transform>();
            }

            if (appliedOn.name == "BowmanL1")
            {
                RangedAttack.GetComponent<AttackRanged>().damage = 1;
                RangedAttack.GetComponent<AttackRanged>().cooldown = 1;
                //Arrow prefab need to be set
                StartCoroutine("addBowmanArrow", RangedAttack);
                RangedAttack.GetComponent<AttackRanged>().firePoint = appliedOn.transform.Find("FirePoint").GetComponent<Transform>();
            }

        }


    }

    //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

    /// <summary>
    /// Closes the building tree.
    /// </summary>
    private void CloseBuildingTree()
    {
        if (activeBuildingTree != null)
        {
            Destroy(activeBuildingTree.gameObject);
            // Enable tower raycast
            bodyCollider.enabled = true;
        }
    }

    /// <summary>
    /// Builds the tower.
    /// </summary>
    /// <param name="towerPrefab">Tower prefab.</param>
    public void BuildTower(GameObject towerPrefab)
    {
        // Close active building tree
        CloseBuildingTree();
        Price price = towerPrefab.GetComponent<Price>();
        // If anough gold
        if (uiManager.SpendGold(price.price) == true)
        {
            // Create new tower and place it on same position
            newTower = Instantiate<GameObject>(towerPrefab, transform.parent);
            newTower.transform.position = transform.position;
            newTower.transform.rotation = transform.rotation;
            //newTower.transform.Find("Body").GetComponent<SpriteRenderer>().sortingOrder = 0;
            //newTower.GetComponent<AiBehavior>().defaultState = newTower.transform.gameObject.GetComponent<AiStateAttack>();
            AiStateAttack newToweraistate = newTower.transform.gameObject.GetComponent<AiStateAttack>();
            AiState.AiTransaction[] aitransactionArrayNewTower = { new AiState.AiTransaction() { trigger = AiState.Trigger.TriggerStay, newState = newToweraistate } };
            newTower.GetComponent<AiStateIdle>().specificTransactions = aitransactionArrayNewTower;
            // newTower.GetComponent<AiStateAttack>().rangedAttack = GetComponentInChildren<AttackRanged>() as IAttack;
            // newTower.GetComponent<AiStateAttack>().meleeAttack = GetComponentInChildren<AttackMelee>() as IAttack;
            //newTower.GetComponent<AiStateAttack>().rangedAttack = GetComponentInChildren<AttackRanged>() as IAttack;
            /* GameObject RangedAttack = newTower.transform.Find("RangedAttack").gameObject;
             if (RangedAttack != null)
             {
                 RangedAttack.AddComponent<AiColliderTrigger>();
                 List<string> ab = new List<string> { "Enemy" };
                 RangedAttack.GetComponent<AiColliderTrigger>().tags = ab;


                 RangedAttack.AddComponent<AttackRanged>();
                 if (newTower.name == "BallistaL1(Clone)")
                 {
                     newTower.transform.Find("Body").GetComponent<SpriteRenderer>().sortingOrder = 0;
                     RangedAttack.GetComponent<AttackRanged>().damage = 5;
                     RangedAttack.GetComponent<AttackRanged>().cooldown = 3;
                     //Arrow prefab need to be set
                     StartCoroutine("addBallistaArrow", RangedAttack);
                     RangedAttack.GetComponent<AttackRanged>().firePoint = newTower.transform.Find("FirePoint").GetComponent<Transform>();
                 }
                 if (newTower.name == "MortarL1(Clone)")
                 {
                     RangedAttack.GetComponent<AttackRanged>().damage = 3;
                     RangedAttack.GetComponent<AttackRanged>().cooldown = 3;
                     //Arrow prefab need to be set
                     StartCoroutine("addMortarArrow", RangedAttack);
                     RangedAttack.GetComponent<AttackRanged>().firePoint = newTower.transform.Find("FirePoint").GetComponent<Transform>();
                 }

                 if (newTower.name == "MagicL1(Clone)")
                 {
                     RangedAttack.GetComponent<AttackRanged>().damage = 2;
                     RangedAttack.GetComponent<AttackRanged>().cooldown = 2.2f;
                     //Arrow prefab need to be set
                     StartCoroutine("addMagicArrow", RangedAttack);
                     RangedAttack.GetComponent<AttackRanged>().firePoint = newTower.transform.Find("FirePoint").GetComponent<Transform>();
                 }

                 if (newTower.name == "BowmanL1(Clone)")
                 {
                     RangedAttack.GetComponent<AttackRanged>().damage = 1;
                     RangedAttack.GetComponent<AttackRanged>().cooldown = 1;
                     //Arrow prefab need to be set
                     StartCoroutine("addBowmanArrow", RangedAttack);
                     RangedAttack.GetComponent<AttackRanged>().firePoint = newTower.transform.Find("FirePoint").GetComponent<Transform>();
                 }

             }*/
            // Destroy old tower
            Destroy(gameObject);
        }
    }
    IEnumerator addBallistaArrow(GameObject game)
    {
        //Debug.Log("addBallistaArrow ADDING ARROW STUFF HERE~~~~~~`````````````````````````````````````!!!");
        if (!Cmanager)
        {
            Cmanager = GameObject.Find("CManager");
        }
        if (Cmanager.GetComponent<attachTower>().ArrowsAssets == null)
        {
            //  Debug.Log(" going to DOWNLOAD arrowswww");
            WWW arrowswww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/arrowsforall");
            //yield return arrowswww;
            //  Debug.Log("DOWNLOADING arrowswww");
            Cmanager.GetComponent<attachTower>().ArrowsAssets = arrowswww.assetBundle;
        }
        if (Cmanager.GetComponent<attachTower>().ExplosionAssets == null)
        {
            // Debug.Log(" going to DOWNLOAD explosionswww");
            WWW explosionswww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/explosionall");
            // yield return explosionswww;
            //  Debug.Log("dOWNLOADING explosionswww");
            Cmanager.GetComponent<attachTower>().ExplosionAssets = explosionswww.assetBundle;
        }
        //Debug.Log("GOING TO FIND ARROWS NOW");
        GameObject arrow = Cmanager.GetComponent<attachTower>().ArrowsAssets.LoadAsset("Bold") as GameObject;
        //if (!arrow) { Debug.Log("NOT ABLE TO GET ARROW"); }
        if (arrow)
        {
            //Debug.Log("ARROW IS NOT NULL");
            if (!arrow.GetComponent<BulletBold>())
            {
                arrow.AddComponent<BulletBold>();
                arrow.GetComponent<BulletBold>().lifeTime = 5;
                arrow.GetComponent<BulletBold>().speed = 5;
                arrow.GetComponent<BulletBold>().speedUpOverTime = 0.5f;
                arrow.GetComponent<BulletBold>().hitDistance = 0.1f;
                arrow.GetComponent<BulletBold>().ballisticOffset = 0.1f;
                arrow.GetComponent<BulletBold>().penetrationRatio = 0.3f;
            }
            if (!arrow.GetComponent<AOE>())
            {
                arrow.AddComponent<AOE>();
                arrow.GetComponent<AOE>().aoeDamageRate = 1;
                arrow.GetComponent<AOE>().radius = 0;
                arrow.GetComponent<AOE>().explosion = Cmanager.GetComponent<attachTower>().ExplosionAssets.LoadAsset("DustFX") as GameObject;
            }
        }
        game.GetComponent<AttackRanged>().arrowPrefab = arrow;
        //Debug.Log("DOne WITH TOWERS");
        yield return null;

    }


    IEnumerator addMortarArrow(GameObject game)
    {
        // Debug.Log("addMortarArrow ADDING ARROW STUFF HERE~~~~~~`````````````````````````````````````!!!");
        if (!Cmanager)
        {
            Cmanager = GameObject.Find("CManager");
        }
        if (Cmanager.GetComponent<attachTower>().ArrowsAssets == null)
        {
            //  Debug.Log(" going to DOWNLOAD arrowswww");
            WWW arrowswww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/arrowsforall");
            //yield return arrowswww;
            // Debug.Log("DOWNLOADING arrowswww");
            Cmanager.GetComponent<attachTower>().ArrowsAssets = arrowswww.assetBundle;
        }
        if (Cmanager.GetComponent<attachTower>().ExplosionAssets == null)
        {
            //// Debug.Log(" going to DOWNLOAD explosionswww");
            WWW explosionswww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/explosionall");
            // yield return explosionswww;
            //  Debug.Log("dOWNLOADING explosionswww");
            Cmanager.GetComponent<attachTower>().ExplosionAssets = explosionswww.assetBundle;
        }
        // Debug.Log("GOING TO FIND ARROWS NOW");
        GameObject arrow = Cmanager.GetComponent<attachTower>().ArrowsAssets.LoadAsset("BombAlly") as GameObject;
        // if (!arrow) { Debug.Log("NOT ABLE TO GET ARROW"); }
        if (arrow)
        {
            //   Debug.Log("ARROW IS NOT NULL");
            if (!arrow.GetComponent<BulletBold>())
            {
                arrow.AddComponent<BulletBold>();
                arrow.GetComponent<BulletBold>().lifeTime = 5;
                arrow.GetComponent<BulletBold>().speed = 5;
                arrow.GetComponent<BulletBold>().speedUpOverTime = 0.5f;
                arrow.GetComponent<BulletBold>().hitDistance = 0.1f;
                arrow.GetComponent<BulletBold>().ballisticOffset = 0.1f;
                arrow.GetComponent<BulletBold>().penetrationRatio = 0.3f;
            }
            if (!arrow.GetComponent<AOE>())
            {
                arrow.AddComponent<AOE>();
                arrow.GetComponent<AOE>().aoeDamageRate = 1;
                arrow.GetComponent<AOE>().radius = 0;
                arrow.GetComponent<AOE>().explosion = Cmanager.GetComponent<attachTower>().ExplosionAssets.LoadAsset("DustFX") as GameObject;
            }
        }
        game.GetComponent<AttackRanged>().arrowPrefab = arrow;
        // Debug.Log("DOne WITH TOWERS");
        yield return null;

    }


    IEnumerator addMagicArrow(GameObject game)
    {
        // Debug.Log("addMortarArrow ADDING ARROW STUFF HERE~~~~~~`````````````````````````````````````!!!");
        if (!Cmanager)
        {
            Cmanager = GameObject.Find("CManager");
        }
        if (Cmanager.GetComponent<attachTower>().ArrowsAssets == null)
        {
            //   Debug.Log(" going to DOWNLOAD arrowswww");
            WWW arrowswww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/arrowsforall");
            //yield return arrowswww;
            //    Debug.Log("DOWNLOADING arrowswww");
            Cmanager.GetComponent<attachTower>().ArrowsAssets = arrowswww.assetBundle;
        }
        if (Cmanager.GetComponent<attachTower>().ExplosionAssets == null)
        {
            //    Debug.Log(" going to DOWNLOAD explosionswww");
            WWW explosionswww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/explosionall");
            // yield return explosionswww;
            //  Debug.Log("dOWNLOADING explosionswww");
            Cmanager.GetComponent<attachTower>().ExplosionAssets = explosionswww.assetBundle;
        }
        //  Debug.Log("GOING TO FIND ARROWS NOW");
        GameObject arrow = Cmanager.GetComponent<attachTower>().ArrowsAssets.LoadAsset("MagicMissile") as GameObject;
        //  if (!arrow) { Debug.Log("NOT ABLE TO GET ARROW"); }
        if (arrow)
        {
            // Debug.Log("ARROW IS NOT NULL");
            if (!arrow.GetComponent<BulletArrow>())
            {
                arrow.AddComponent<BulletArrow>();
                arrow.GetComponent<BulletArrow>().lifeTime = 5;
                arrow.GetComponent<BulletArrow>().speed = 4;
                arrow.GetComponent<BulletArrow>().speedUpOverTime = 0.5f;
                arrow.GetComponent<BulletArrow>().hitDistance = 0.2f;
                arrow.GetComponent<BulletArrow>().ballisticOffset = 0;
                arrow.GetComponent<BulletArrow>().freezeRotation = true;
            }
        }
        game.GetComponent<AttackRanged>().arrowPrefab = arrow;
        // Debug.Log("DOne WITH TOWERS");
        yield return null;

    }

    IEnumerator addBowmanArrow(GameObject game)
    {
        // Debug.Log("addMortarArrow ADDING ARROW STUFF HERE~~~~~~`````````````````````````````````````!!!");
        if (!Cmanager)
        {
            Cmanager = GameObject.Find("CManager");
        }
        if (Cmanager.GetComponent<attachTower>().ArrowsAssets == null)
        {
            //    Debug.Log(" going to DOWNLOAD arrowswww");
            WWW arrowswww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/arrowsforall");
            //yield return arrowswww;
            //    Debug.Log("DOWNLOADING arrowswww");
            Cmanager.GetComponent<attachTower>().ArrowsAssets = arrowswww.assetBundle;
        }
        if (Cmanager.GetComponent<attachTower>().ExplosionAssets == null)
        {
            //   Debug.Log(" going to DOWNLOAD explosionswww");
            WWW explosionswww = new WWW("file:///C:/Users/tft/Desktop/NewTD2D/AssetBundles/Windows/explosionall");
            // yield return explosionswww;
            //  Debug.Log("dOWNLOADING explosionswww");
            Cmanager.GetComponent<attachTower>().ExplosionAssets = explosionswww.assetBundle;
        }
        // Debug.Log("GOING TO FIND ARROWS NOW");
        GameObject arrow = Cmanager.GetComponent<attachTower>().ArrowsAssets.LoadAsset("Arrow") as GameObject;
        // if (!arrow) { Debug.Log("NOT ABLE TO GET ARROW"); }
        if (arrow)
        {
            //  Debug.Log("ARROW IS NOT NULL");
            if (!arrow.GetComponent<BulletArrow>())
            {
                arrow.AddComponent<BulletArrow>();
                arrow.GetComponent<BulletArrow>().lifeTime = 5;
                arrow.GetComponent<BulletArrow>().speed = 6;
                arrow.GetComponent<BulletArrow>().speedUpOverTime = 0.5f;
                arrow.GetComponent<BulletArrow>().hitDistance = 0.1f;
                arrow.GetComponent<BulletArrow>().ballisticOffset = 0.5f;
            }
        }
        game.GetComponent<AttackRanged>().arrowPrefab = arrow;
        // Debug.Log("DOne WITH TOWERS");
        yield return null;

    }




    /// <summary>
    /// Disable tower raycast and close building tree on game pause.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void GamePaused(GameObject obj, string param)
    {
        if (param == bool.TrueString) // Paused
        {
            //CloseBuildingTree();
            bodyCollider.enabled = false;
        }
        else // Unpaused
        {
            bodyCollider.enabled = true;
        }
    }

    /// <summary>
    /// On user click.
    /// </summary>
    /// <param name="obj">Object.</param>
    /// <param name="param">Parameter.</param>
    private void UserClick(GameObject obj, string param)
    {
        if (obj == gameObject) // This tower is clicked
        {
            //     Debug.Log(obj);
            // Show attack range
            ShowRange(true);
            if (activeBuildingTree == null)
            {
                // Open building tree if it is not
                OpenBuildingTree();
            }
        }
        else // Other click
        {
            // Hide attack range
            ShowRange(false);
            // Close active building tree
            CloseBuildingTree();
        }
    }

    /// <summary>
    /// Display tower's attack range.
    /// </summary>
    /// <param name="condition">If set to <c>true</c> condition.</param>
    private void ShowRange(bool condition)
    {
        if (rangeImage != null)
        {
            rangeImage.SetActive(condition);
        }
    }
}
