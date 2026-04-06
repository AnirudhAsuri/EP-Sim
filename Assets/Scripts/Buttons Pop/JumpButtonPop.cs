using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButtonPop : MonoBehaviour
{
    private Vector3 originalScale;
    private Coroutine popRoutine;

    [SerializeField] private float popMultiplier = 1.2f; // 20% increase
    [SerializeField] private float smoothTime = 0.1f;

    public static JumpButtonPop Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        originalScale = transform.localScale;
    }

    public void ButtonPop()
    {
        if (popRoutine != null) StopCoroutine(popRoutine);
        popRoutine = StartCoroutine(PopSequence());
    }

    private IEnumerator PopSequence()
    {
        Vector3 targetScale = originalScale * popMultiplier;
        float elapsed = 0;

        // Scale UP
        while (elapsed < smoothTime)
        {
            elapsed += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / smoothTime);
            yield return null;
        }

        // Scale BACK
        elapsed = 0;
        while (elapsed < smoothTime)
        {
            elapsed += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / smoothTime);
            yield return null;
        }

        transform.localScale = originalScale;
    }
}