using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Level choose scene manager.
/// </summary>
public class LevelChoose : MonoBehaviour
{
	// Scene to exit
    public string exitSceneName;
	// Choosen level
	public GameObject currentLevel;
	// All levels
	public List<GameObject> levelsPrefabs = new List<GameObject>();
	// Visual displaing for number of levels
	public Transform togglesFolder;
	// Active toggle prefab
	public Toggle activeTogglePrefab;
	// Inactive toggle prefab
	public Toggle inactiveTogglePrefab;
	// Next level button
	public Button nextLevelButton;
	// Previous level button
	public Button prevLevelButton;

	// Index of last allowed level for choosing
	private int maxActiveLevelIdx;
	// Index of current displayed level
	private int currentDisplayedLevelIdx;
	// List with active toggles
	private List<Toggle> activeToggles = new List<Toggle>();

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake()
    {
        exitSceneName = "MainMenu";
        if (GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Parchment").transform.Find("LevelDisplayArea").transform.Find("Level1").gameObject)
        {
            currentLevel = GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Parchment").transform.Find("LevelDisplayArea").transform.Find("Level1").gameObject;
        }
        else if (GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Parchment").transform.Find("LevelDisplayArea").transform.Find("Level2").gameObject)
        {
            currentLevel = GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Parchment").transform.Find("LevelDisplayArea").transform.Find("Level2").gameObject;
        }
        else if (GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Parchment").transform.Find("LevelDisplayArea").transform.Find("Level3").gameObject)
        {
            currentLevel = GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Parchment").transform.Find("LevelDisplayArea").transform.Find("Level3").gameObject;
        }
        togglesFolder = GameObject.Find("TogglesFolder").GetComponent<Transform>();
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
    IEnumerator Start()
    {
        WWW toggleswww = new WWW("file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/levelchoosetoggles");
        AssetBundle levelToggleasset = toggleswww.assetBundle;
        yield return levelToggleasset;
        if (levelToggleasset)
        {
            Debug.Log(levelToggleasset + ": levelToggleasset is NOT NULL");
            GameObject x = levelToggleasset.LoadAsset("ActiveToggle") as GameObject;
            Toggle xx = x.GetComponent<Toggle>();
            activeTogglePrefab = xx;
            x = levelToggleasset.LoadAsset("InactiveToggle") as GameObject;
            xx = x.GetComponent<Toggle>();
        }
        nextLevelButton = GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Buttons").transform.Find("NextLevel").GetComponent<Button>();
        prevLevelButton = GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Buttons").transform.Find("PrevLevel").GetComponent<Button>();
        maxActiveLevelIdx = -1;
        Debug.Assert(currentLevel && togglesFolder && activeTogglePrefab && inactiveTogglePrefab && nextLevelButton && prevLevelButton, "Wrong initial settings");
        WWW levelPrefabswww = new WWW("file:///C:/Users/tft/Desktop/cGame%20POC/AssetBundles/Windows/levelchooselevelprefabs");
        yield return levelPrefabswww;
        AssetBundle levelPrefabsAssets = levelPrefabswww.assetBundle;
        GameObject Level1 = levelPrefabsAssets.LoadAsset("Level1") as GameObject;
        GameObject Level2 = levelPrefabsAssets.LoadAsset("Level2") as GameObject;
        GameObject Level3 = levelPrefabsAssets.LoadAsset("Level3") as GameObject;
        List<GameObject> levelss = new List<GameObject>()
        {
            Level1,Level2,Level3
        };
        GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Buttons").transform.Find("PrevLevel").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject.Find("LevelChooser").GetComponent<LevelChoose>().DisplayPrevLevel();
        });

        GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Buttons").transform.Find("NextLevel").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject.Find("LevelChooser").GetComponent<LevelChoose>().DisplayNextLevel();
        });

        GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Buttons").transform.Find("Go").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject.Find("LevelChooser").GetComponent<LevelChoose>().GoToLevel();
        });


        GameObject.Find("LevelChooser").transform.Find("BG").transform.Find("Buttons").transform.Find("Exit").gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject.Find("LevelChooser").GetComponent<LevelChoose>().Exit();
        });


        int hitIdx = -1;
		int levelsCount = DataManager.instance.progress.openedLevels.Count;
		if (levelsCount > 0)
		{
			// Get name of last opened level from stored data
			string openedLevelName = DataManager.instance.progress.openedLevels[levelsCount - 1];

	        int idx;
			for (idx = 0; idx < levelsPrefabs.Count; ++idx)
	        {
				// Try to find last opened level in levels list
				if (levelsPrefabs[idx].name == openedLevelName)
	            {
	                hitIdx = idx;
	                break;
	            }
	        }
		}
		// Level found
		if (hitIdx >= 0)
		{
			if (levelsPrefabs.Count > hitIdx + 1)
			{
				maxActiveLevelIdx = hitIdx + 1;
			}
			else
			{
				maxActiveLevelIdx = hitIdx;
			}
		}
		// level does not found
		else
		{
			if (levelsPrefabs.Count > 0)
			{
				maxActiveLevelIdx = 0;
			}
			else
			{
				Debug.LogError("Have no levels prefabs!");
			}
		}
		if (maxActiveLevelIdx >= 0)
		{
			DisplayToggles();
			DisplayLevel(maxActiveLevelIdx);
		}
    }

	/// <summary>
	/// Visual displaing for number of levels
	/// </summary>
	private void DisplayToggles()
	{
		foreach (Toggle toggle in togglesFolder.GetComponentsInChildren<Toggle>())
		{
			Destroy(toggle.gameObject);
		}
		int cnt;
		for (cnt = 0; cnt < maxActiveLevelIdx + 1; cnt++)
		{
			GameObject toggle = Instantiate(activeTogglePrefab.gameObject, togglesFolder);
			activeToggles.Add(toggle.GetComponent<Toggle>());
		}
		if (maxActiveLevelIdx < levelsPrefabs.Count - 1)
		{
			Instantiate(inactiveTogglePrefab.gameObject, togglesFolder);
		}
	}

	/// <summary>
	/// Displaies choosen level.
	/// </summary>
	/// <param name="levelIdx">Level index.</param>
	private void DisplayLevel(int levelIdx)
	{
		Transform parentOfLevel = currentLevel.transform.parent;
		Vector3 levelPosition = currentLevel.transform.position;
		Quaternion levelRotation = currentLevel.transform.rotation;
		Destroy(currentLevel);
		currentLevel = Instantiate(levelsPrefabs[levelIdx], parentOfLevel);
		currentLevel.name = levelsPrefabs[levelIdx].name;
		currentLevel.transform.position = levelPosition;
		currentLevel.transform.rotation = levelRotation;
		currentDisplayedLevelIdx = levelIdx;
		foreach (Toggle toggle in activeToggles)
		{
			toggle.isOn = false;
		}
		activeToggles[levelIdx].isOn = true;
		UpdateButtonsVisible (levelIdx);
	}

	/// <summary>
	/// Updates the buttons visible.
	/// </summary>
	/// <param name="levelIdx">Level index.</param>
	private void UpdateButtonsVisible(int levelIdx)
	{
		prevLevelButton.interactable = levelIdx > 0 ? true : false;
		nextLevelButton.interactable = levelIdx < maxActiveLevelIdx ? true : false;
	}

	/// <summary>
	/// Displaies the next level.
	/// </summary>
	public void DisplayNextLevel()
	{
		if (currentDisplayedLevelIdx < maxActiveLevelIdx)
		{
			DisplayLevel(currentDisplayedLevelIdx + 1);
		}
	}

	/// <summary>
	/// Displaies the previous level.
	/// </summary>
	public void DisplayPrevLevel()
	{
		if (currentDisplayedLevelIdx > 0)
		{
			DisplayLevel (currentDisplayedLevelIdx - 1);
		}
	}

	/// <summary>
	/// Exit scene.
	/// </summary>
	public void Exit()
	{
		SceneManager.LoadScene(exitSceneName);
	}

	/// <summary>
	/// Go to choosen level.
	/// </summary>
	public void GoToLevel()
	{
		SceneManager.LoadScene(currentLevel.name);
	}
}
