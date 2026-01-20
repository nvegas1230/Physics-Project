using UnityEngine;
using TMPro;

public class Settings : MonoBehaviour
{
    public static Settings Instance;
    public GameObject SettingsContainer;
    GameObject SettingsButton;

    // Saves toggle settings and updates the text in settings menu
    public static void SaveToggleSetting(string setting, bool settingValue)
    {
        // Set the respective setting's text
        GameObject settingHolder = GameObject.Find(setting);
        GameObject buttonStatus = settingHolder.transform.Find("Button").Find("StatusText").gameObject;
        if (settingValue) { buttonStatus.GetComponent<TMP_Text>().text = "ON"; } else { buttonStatus.GetComponent<TMP_Text>().text = "OFF"; }

        // Set the actual value
        PlayerPrefs.SetInt(setting, settingValue ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Makes all of the statics have values and updates settings views
    public static void SettingsInit()
    {
        SolveForOne = SolveForOne;
        UseNegativeNumbers = UseNegativeNumbers;
        UseWholeNumbersOnly = UseWholeNumbersOnly;
    }

    // All settings will go here, default values are second parameter of .GetInt
    public static bool SolveForOne { get => PlayerPrefs.GetInt("SolveForOne", 0) == 1; set => Settings.SaveToggleSetting("SolveForOne", value); }
    public static bool UseNegativeNumbers { get => PlayerPrefs.GetInt("UseNegativeNumbers", 0) == 1; set => Settings.SaveToggleSetting("UseNegativeNumbers", value); }
    public static bool UseWholeNumbersOnly { get => PlayerPrefs.GetInt("UseWholeNumbersOnly", 0) == 1; set => Settings.SaveToggleSetting("UseWholeNumbersOnly", value); }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Make settings container update before making invisible
        SettingsContainer = Instance.transform.Find("SettingsContainer").gameObject;
        SettingsContainer.SetActive(true);
        SettingsInit();
        SettingsContainer.SetActive(false);
    }

    public void OpenSettings()
    {
        SettingsContainer.SetActive(!SettingsContainer.activeInHierarchy);
    }

    // Toggles settings
    // Use PlayerPrefs.SetInt("MusicToggle", isMusicOn ? 1 : 0); and PlayerPrefs.Save(); for saving settings to memory
    public void SettingToggled(string setting)
    {
        //*
        if (setting == "SolveForOne") { SolveForOne = !SolveForOne; }
        if (setting == "UseNegativeNumbers") { UseNegativeNumbers = !UseNegativeNumbers; }
        if (setting == "UseWholeNumbersOnly") { UseWholeNumbersOnly = !UseWholeNumbersOnly; }
        //*/
    }
}
