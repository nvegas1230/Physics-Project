using UnityEngine;

public class FormulaSheet : MonoBehaviour
{
    public static FormulaSheet Instance;
    public GameObject FormulaSheetUI;

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

        FormulaSheetUI = transform.Find("FormulaSheet").gameObject;
        FormulaSheetUI.SetActive(false);
    }

    public void OpenUI()
    {
        FormulaSheetUI.SetActive(!FormulaSheetUI.activeInHierarchy);
    }
}
