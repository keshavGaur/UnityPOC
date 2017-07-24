using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Main menu operate.
/// </summary>
public class MainMenu : MonoBehaviour
{
    // Name of scene to start on click
    public string startSceneName = "LevelChoose";

    void Awake()
    {
       // Debug.Log("Awake does work In main menu");
        GameObject.Find("MainMenu").transform.Find("BG").transform.Find("Buttons").transform.Find("Start").transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            Camera.main.GetComponent<MainMenu>().NewGame();
        });
        GameObject.Find("MainMenu").transform.Find("BG").transform.Find("Buttons").transform.Find("Start").transform.Find("Button").GetComponent<Button>().onClick.AddListener(() =>
        {
            Camera.main.GetComponent<MainMenu>().Quit();
        });
    }

    // Start new game
    public void NewGame()
    {
        SceneManager.LoadScene(startSceneName);
    }

    /// <summary>
    /// Close application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
