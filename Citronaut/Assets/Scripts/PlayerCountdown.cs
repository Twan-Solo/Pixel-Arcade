using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerCountdown : MonoBehaviour
{
    public StarterAssets.StarterAssetsInputs input;
    public float countdownTime = 3f;
    public Text countdownText;

    private void Start()
    {
        input.enabled = false; // lock input at start
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        float remaining = countdownTime;
        while (remaining > 0f)
        {
            if (countdownText != null)
                countdownText.text = Mathf.Ceil(remaining).ToString();

            remaining -= Time.deltaTime;
            yield return null;
        }

        if (countdownText != null)
            countdownText.text = "";

        input.enabled = true; // unlock input
    }
}