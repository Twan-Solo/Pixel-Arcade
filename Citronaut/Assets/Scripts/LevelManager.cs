using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Array of level scene names in order
    public string[] levels;
    public string mainMenuScene = "MainMenu";

    private int currentLevelIndex = 0;

    // Call this to load the next level
    public void LoadNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
            // No more levels, go back to main menu
            SceneManager.LoadScene(mainMenuScene);
        }
        else
        {
            SceneManager.LoadScene(levels[currentLevelIndex]);
        }
    }

    // Optional: Load a specific level by index
    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levels.Length)
        {
            SceneManager.LoadScene(mainMenuScene);
        }
        else
        {
            currentLevelIndex = index;
            SceneManager.LoadScene(levels[currentLevelIndex]);
        }
    }

    // Optional: Restart current level
    public void RestartLevel()
    {
        SceneManager.LoadScene(levels[currentLevelIndex]);
    }
    public void ResetCamera()
    {
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            // Force the camera to look straight ahead (or at your side-scroller angle)
            // Adjust the (0, 0, 0) to whatever rotation your camera needs for 2D
            mainCam.transform.rotation = Quaternion.Euler(0, 0, 0);

            // Optional: If your camera has a "Smooth Follow" script, disable/enable it 
            // to reset its internal target variables.
        }
    }
}

