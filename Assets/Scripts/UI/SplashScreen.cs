using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup splashCanvasGroup;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float waitDuration = 1f;

    private void Start()
    {
        StartCoroutine(ShowSplash());
    }

    private System.Collections.IEnumerator ShowSplash()
    {
        splashCanvasGroup.alpha = 1f;
        
        // انتظر
        yield return new WaitForSeconds(waitDuration);
        
        // اختفاء تدريجي
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            splashCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }
        
        splashCanvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
