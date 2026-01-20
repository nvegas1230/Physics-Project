using UnityEngine;

public class Disclaimer : MonoBehaviour
{
    public GameObject DisclaimerUI;
    public static bool clickedYet = false;

    private void Awake()
    {
        DisclaimerUI = transform.Find("FormulaSheet").gameObject;
        if (clickedYet == false)
            DisclaimerUI.SetActive(true);
        else
            DisclaimerUI.SetActive(false);
    }

    public void OpenUI()
    {
        DisclaimerUI.SetActive(false);
        clickedYet = true;
    }
}
