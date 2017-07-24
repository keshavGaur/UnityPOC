using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Show unit info on special sheet.
/// </summary>
public class ShowInfo : MonoBehaviour
{
	// Name of unit
    public Text unitName;
	// Primary icon for displaing
    public Image primaryIcon;
	// Primary text for displaing
    public Text primaryText;
	// Secondary icon for displaing
    public Image secondaryIcon;
	// Secondary text for displaing
    public Text secondaryText;

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    void OnDestroy()
    {
		EventManager.StopListening("UserClick", UserClick);
    }

	/// <summary>
	/// Awake this instance.
	/// </summary>
    void Awake()
    {
       /* if (SceneManager.GetActiveScene().name == "Level1") {
            GameObject UnitInfo = GameObject.FindGameObjectWithTag("LevelUI").transform.Find("UnitInfo").gameObject;
            if (UnitInfo) {
                UnitInfo.GetComponent<ShowInfo>().unitName = UnitInfo.transform.Find("Content").transform.Find("UnitName").GetComponent<Text>();
                UnitInfo.GetComponent<ShowInfo>().primaryIcon = UnitInfo.transform.Find("Content").transform.Find("Image").GetComponent<Image>();
                UnitInfo.GetComponent<ShowInfo>().primaryText = UnitInfo.transform.Find("Content").transform.Find("Text").GetComponent<Text>();
                UnitInfo.GetComponent<ShowInfo>().secondaryIcon = UnitInfo.transform.Find("Content").transform.Find("Image").GetComponent<Image>();
                UnitInfo.GetComponent<ShowInfo>().secondaryText = UnitInfo.transform.Find("Content").transform.Find("Text").GetComponent<Text>();
            }
        }
        */
        Debug.Assert(unitName && primaryIcon && primaryText && secondaryIcon && secondaryText, "Wrong intial settings");
    }

	/// <summary>
	/// Start this instance.
	/// </summary>
    void Start()
    {
		EventManager.StartListening("UserClick", UserClick);
        HideUnitInfo();
    }

	/// <summary>
	/// Shows the unit info.
	/// </summary>
	/// <param name="info">Info.</param>
    public void ShowUnitInfo(UnitInfo info)
    {
		gameObject.SetActive(true);
        unitName.text = info.unitName;
        primaryText.text = info.primaryText;
        secondaryText.text = info.secondaryText;
        if (info.primaryIcon != null)
        {
            primaryIcon.sprite = info.primaryIcon;
            primaryIcon.gameObject.SetActive(true);
        }
        if (info.secondaryIcon != null)
        {
            secondaryIcon.sprite = info.secondaryIcon;
            secondaryIcon.gameObject.SetActive(true);
        }
    }

	/// <summary>
	/// Hides the unit info.
	/// </summary>
    public void HideUnitInfo()
    {
        unitName.text = primaryText.text = secondaryText.text = "";
        primaryIcon.gameObject.SetActive(false);
        secondaryIcon.gameObject.SetActive(false);
		gameObject.SetActive(false);
    }

	/// <summary>
	/// User click handler.
	/// </summary>
	/// <param name="obj">Object.</param>
	/// <param name="param">Parameter.</param>
    private void UserClick(GameObject obj, string param)
    {
        HideUnitInfo();
        if (obj != null)
        {
			// Cliced object has info for displaing
            UnitInfo unitInfo = obj.GetComponent<UnitInfo>();
            if (unitInfo != null)
            {
                ShowUnitInfo(unitInfo);
            }
        }
    }
}
