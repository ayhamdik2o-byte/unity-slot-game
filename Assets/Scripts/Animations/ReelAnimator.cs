using UnityEngine;
using TMPro;
using System.Collections;

public class ReelAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI reelText;
    [SerializeField] private Image reelBackground;
    [SerializeField] private AudioSource spinSound;
    
    private string[] symbols = { "🍒", "🔔", "7️⃣", "💎", "🍀" };
    private Color originalColor;

    private void Start()
    {
        originalColor = reelBackground.color;
    }

    public IEnumerator SpinReel(float duration)
    {
        float elapsed = 0;
        
        while (elapsed < duration)
        {
            reelText.text = symbols[Random.Range(0, symbols.Length)];
            reelBackground.color = Color.Lerp(
                originalColor,
                new Color(0.7f, 0.7f, 1f),
                Mathf.Sin(elapsed * 20) * 0.5f + 0.5f
            );
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        reelBackground.color = originalColor;
    }

    public void SetSymbol(string symbol)
    {
        reelText.text = symbol;
    }

    public void PlayWinAnimation()
    {
        StartCoroutine(WinAnimation());
    }

    private IEnumerator WinAnimation()
    {
        for (int i = 0; i < 3; i++)
        {
            reelBackground.color = new Color(1f, 0.84f, 0f);
            yield return new WaitForSeconds(0.2f);
            reelBackground.color = originalColor;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
