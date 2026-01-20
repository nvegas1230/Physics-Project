using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application has quit.");
    }

    public void ObjectVisible(GameObject targetUIElement)
    {
        if (targetUIElement.activeInHierarchy)
        {
            targetUIElement.SetActive(false);
        }
        else
        {
            targetUIElement.SetActive(true);
        }
    }

    public void StartKinematicsPracticeProblem(string Type)
    {
        SceneDataTransfer.PhysicsUnit = "Kinematics";
        SceneDataTransfer.ProblemType = Type;

        GoToScene("PracticeProblems");
    }
}
