using UnityEngine;
using System.Collections;

public class ResultAnimator : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
        
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(ShowAnimation());
    }

    public void Hide()
    {
        StartCoroutine(HideAnimation());
    }

    private IEnumerator ShowAnimation()
    {
        canvasGroup.alpha = 0;
        rectTransform.anchoredPosition = originalPosition + Vector3.up * 100;

        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            rectTransform.anchoredPosition = Vector3.Lerp(
                originalPosition + Vector3.up * 100,
                originalPosition,
                elapsed / duration
            );
            yield return null;
        }

        canvasGroup.alpha = 1;
        rectTransform.anchoredPosition = originalPosition;
    }

    private IEnumerator HideAnimation()
    {
        float duration = 0.5f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);
            rectTransform.anchoredPosition = Vector3.Lerp(
                originalPosition,
                originalPosition + Vector3.up * 100,
                elapsed / duration
            );
            yield return null;
        }

        canvasGroup.alpha = 0;
        gameObject.SetActive(false);
    }
}
