using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SprintButtonPopAndActivate : MonoBehaviour
{
    private Vector3 originalScale;
    private Coroutine popRoutine;

    [SerializeField] private float popMultiplier = 1.2f; // 20% increase
    [SerializeField] private float smoothTime = 0.1f;

    public static SprintButtonPopAndActivate Instance { get; private set; }

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

    public void ChangeColor(bool isSprinting)
    {
        Image image = GetComponent<Image>();
        if (isSprinting)
        {
            image.color = Color.green;
        }
        else
        {
            // Your custom Orange color
            image.color = new Color32(255, 219, 104, 255);
        }
    }
}