using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour

{
    public FadeTransition fadeTransition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Loads the first level (by build index)
    public void StartGame()
    {
        fadeTransition.FadeToScene(1);
    }

    // Update is called once per frame
    public void ExitGame()
    {
        Debug.Log("Exit Game pressed");
        Application.Quit();
    }
}
