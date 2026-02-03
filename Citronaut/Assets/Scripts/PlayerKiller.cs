using UnityEngine;

public class PlayerKiller : MonoBehaviour
{
    public void KillPawnInstantly()
    {
        if (PlayerData.Instance != null)
        {
            GameObject playerObj = PlayerData.Instance.gameObject;

            // 1. Clear the reference first
            PlayerData.Instance = null;

            // 2. Use standard Destroy (Safe for physics triggers)
            Destroy(playerObj);
        }
        else
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) Destroy(player);
        }
    }
}