using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInputLock : MonoBehaviour
{
    public ThirdPersonController controller;   // reference controller
    public StarterAssetsInputs input;
    public float countdownTime = 3f;
    public Text countdownText;

    private void Start()
    {
        controller.canMove = false;
        input.enabled = false;

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

        //Full UNlock
        input.enabled = true;
        controller.canMove = true;
    }
}