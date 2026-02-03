using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    public LevelManager levelManager;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelManager.LoadNextLevel();
        }
    }

}
