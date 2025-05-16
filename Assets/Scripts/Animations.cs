using System.Collections;
using UnityEngine;

public class Animations : MonoBehaviour
{
    public static Animations instance;
    [SerializeField] private GameObject player;
    [SerializeField] private float blinkDuration = 2f; // Total blink duration
    [SerializeField] private float blinkInterval = 0.2f; // Interval between blinks
    private SpriteRenderer spriteRenderer;
    private Coroutine blinkCoroutine; // Reference to the active coroutine

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        if (player != null)
        {
            spriteRenderer = player.GetComponent<SpriteRenderer>();
        }
    }

    public void PlayerOnOff()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine); // Stop the previous coroutine if running
        }

        if (player != null)
        {
            blinkCoroutine = StartCoroutine(BlinkEffect()); // Start and track the coroutine
        }
    }

    private IEnumerator BlinkEffect()
    {
        if (player != null && spriteRenderer != null)
        {
            float elapsedTime = 0f;
            bool isVisible = true;

            while (elapsedTime < blinkDuration)
            {
                if (player == null || spriteRenderer == null)
                {
                    yield break; // Exit the coroutine if player or spriteRenderer is null
                }

                spriteRenderer.enabled = isVisible;
                isVisible = !isVisible;
                elapsedTime += blinkInterval;
                yield return new WaitForSeconds(blinkInterval);
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true; // Ensure player is visible at the end
            }
        }
    }
}