using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class Credits : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Scrolling Credits")]
    [SerializeField] private RectTransform creditsText;
    [SerializeField] private float scrollSpeed = 50f;
    [SerializeField] private float startY = -600f;
    [SerializeField] private float endY = 600f;

    [Header("Bounce Animator")]
    [SerializeField] private Animator bounceAnimator;
    [SerializeField] private GameObject bounceObject;
    [SerializeField] private string bounceTriggerName = "PlayBounce";

    [Header("Timing")]
    [SerializeField] private float timeAfterBounce = 3f;

    private bool scrollingFinished = false;
    private bool bounceStarted = false;

    void Start()
    {
        // Hide the bounce object at start
        if (bounceObject != null)
            bounceObject.SetActive(false);

        // Set starting position of credits text
        Vector2 pos = creditsText.anchoredPosition;
        pos.y = startY;
        creditsText.anchoredPosition = pos;
    }

    void Update()
    {
        if (!scrollingFinished)
            ScrollCredits();

        // Optional skip (works with old or new Input System)
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
#else
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
#endif
    }

    private void ScrollCredits()
    {
        creditsText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (creditsText.anchoredPosition.y >= endY)
        {
            scrollingFinished = true;
            StartBounceAndTimer();
        }
    }

    private void StartBounceAndTimer()
    {
        if (bounceStarted) return;

        bounceStarted = true;

        // Show the bounce object
        if (bounceObject != null)
            bounceObject.SetActive(true);

        // Start the bounce animation
        if (bounceAnimator != null)
            bounceAnimator.SetTrigger(bounceTriggerName);

        StartCoroutine(GoToMenuAfterDelay());
    }

    private IEnumerator GoToMenuAfterDelay()
    {
        yield return new WaitForSeconds(timeAfterBounce);
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
