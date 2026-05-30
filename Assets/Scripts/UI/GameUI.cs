using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameUI : MonoBehaviour
{
    [Header("Top Bar")]
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button leaderboardButton;
    
    [Header("Slot Machine")]
    [SerializeField] private TextMeshProUGUI[] reelTexts = new TextMeshProUGUI[5];
    [SerializeField] private Image[] reelBackgrounds = new Image[5];
    [SerializeField] private ParticleSystem winParticles;
    [SerializeField] private ParticleSystem bigWinParticles;
    
    [Header("Game Controls")]
    [SerializeField] private InputField betInputField;
    [SerializeField] private Slider betSlider;
    [SerializeField] private TextMeshProUGUI betAmountText;
    [SerializeField] private Button spinButton;
    [SerializeField] private Button maxBetButton;
    [SerializeField] private Button minBetButton;
    [SerializeField] private Button dailyChallengeButton;
    
    [Header("Result Panel")]
    [SerializeField] private Image resultPanel;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI winAmountText;
    [SerializeField] private Animator resultAnimator;
    
    [Header("Bottom Menu")]
    [SerializeField] private Button gameTabButton;
    [SerializeField] private Button leaderboardTabButton;
    [SerializeField] private Button profileTabButton;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject profilePanel;
    
    [Header("Audio")]
    [SerializeField] private AudioSource spinSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource bigWinSound;
    [SerializeField] private AudioSource clickSound;
    
    private bool isSpinning = false;
    private int minBet = 10;
    private int maxBet = 1000;
    private string[] symbols = { "🍒", "🔔", "7️⃣", "💎", "🍀" };

    private void Start()
    {
        SetupListeners();
        UpdateDisplay();
        InitializeBetSlider();
    }

    private void SetupListeners()
    {
        spinButton.onClick.AddListener(OnSpinClicked);
        maxBetButton.onClick.AddListener(SetMaxBet);
        minBetButton.onClick.AddListener(SetMinBet);
        dailyChallengeButton.onClick.AddListener(OnDailyChallengeClicked);
        
        leaderboardButton.onClick.AddListener(() => ShowPanel(leaderboardPanel));
        settingsButton.onClick.AddListener(ShowSettingsPanel);
        
        gameTabButton.onClick.AddListener(() => ShowPanel(gamePanel));
        leaderboardTabButton.onClick.AddListener(() => ShowPanel(leaderboardPanel));
        profileTabButton.onClick.AddListener(() => ShowPanel(profilePanel));
        
        betInputField.onEndEdit.AddListener(OnBetInputChanged);
        betSlider.onValueChanged.AddListener(OnBetSliderChanged);
    }

    private void InitializeBetSlider()
    {
        betSlider.minValue = minBet;
        betSlider.maxValue = maxBet;
        betSlider.value = 100;
    }

    private void UpdateDisplay()
    {
        if (GameManager.Instance != null)
        {
            playerNameText.text = GameManager.Instance.username;
            pointsText.text = GameManager.Instance.currentPoints.ToString("N0");
            levelText.text = CalculateLevel().ToString();
        }
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void OnSpinClicked()
    {
        if (isSpinning) return;

        int betAmount = GetBetAmount();

        if (GameManager.Instance.currentPoints < betAmount)
        {
            ShowResultMessage("❌ نقاط غير كافية!", Color.red);
            return;
        }

        PlaySound(clickSound);
        isSpinning = true;
        spinButton.interactable = false;
        
        StartCoroutine(SpinAnimation());
        GameManager.Instance.SpinSlots(betAmount, OnSpinResult);
    }

    private void OnSpinResult(string[] reels, string result, int winAmount, long newBalance)
    {
        StartCoroutine(DisplayResult(reels, result, winAmount, newBalance));
    }

    private IEnumerator SpinAnimation()
    {
        PlaySound(spinSound);
        float spinDuration = 1.5f;
        float elapsed = 0;

        while (elapsed < spinDuration)
        {
            for (int i = 0; i < 5; i++)
            {
                reelTexts[i].text = symbols[Random.Range(0, symbols.Length)];
                reelBackgrounds[i].color = Color.Lerp(Color.white, new Color(0.7f, 0.7f, 1f), Mathf.Sin(elapsed * 10));
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator DisplayResult(string[] reels, string result, int winAmount, long newBalance)
    {
        // Stop animation and show final reels
        for (int i = 0; i < 5; i++)
        {
            reelTexts[i].text = reels[i];
            reelBackgrounds[i].color = Color.white;
        }

        yield return new WaitForSeconds(0.5f);

        // Show result
        if (result == "BIG_WIN")
        {
            resultText.text = "🎉 فوز كبير! 🎉";
            resultText.color = new Color(1f, 0.84f, 0f); // Gold
            PlaySound(bigWinSound);
            bigWinParticles.Play();
        }
        else if (result == "WIN")
        {
            resultText.text = "✨ فوز! ✨";
            resultText.color = new Color(0.06f, 0.85f, 0.63f); // Green
            PlaySound(winSound);
            winParticles.Play();
        }
        else
        {
            resultText.text = "😔 حظاً أفضل في المرة القادمة";
            resultText.color = Color.red;
        }

        winAmountText.text = $"+{winAmount} نقطة";
        resultAnimator.SetTrigger("Show");

        yield return new WaitForSeconds(2f);
        resultAnimator.SetTrigger("Hide");

        isSpinning = false;
        spinButton.interactable = true;
    }

    private void OnDailyChallengeClicked()
    {
        PlaySound(clickSound);
        dailyChallengeButton.interactable = false;
        GameManager.Instance.CompleteDailyChallenge(OnChallengeComplete);
    }

    private void OnChallengeComplete(int bonusPoints)
    {
        ShowResultMessage($"✅ تحدي اليوم أكمل! +{bonusPoints} نقطة", Color.green);
        dailyChallengeButton.interactable = true;
    }

    private void SetMaxBet()
    {
        betSlider.value = maxBet;
        PlaySound(clickSound);
    }

    private void SetMinBet()
    {
        betSlider.value = minBet;
        PlaySound(clickSound);
    }

    private void OnBetInputChanged(string input)
    {
        if (int.TryParse(input, out int bet))
        {
            betSlider.value = Mathf.Clamp(bet, minBet, maxBet);
        }
    }

    private void OnBetSliderChanged(float value)
    {
        int betAmount = (int)value;
        betInputField.text = betAmount.ToString();
        betAmountText.text = $"الرهان: {betAmount} نقطة";
    }

    private int GetBetAmount()
    {
        return int.TryParse(betInputField.text, out int bet) ? Mathf.Clamp(bet, minBet, maxBet) : 100;
    }

    private void ShowResultMessage(string message, Color color)
    {
        resultText.text = message;
        resultText.color = color;
        resultAnimator.SetTrigger("Show");
        StartCoroutine(HideResultAfterDelay(2f));
    }

    private IEnumerator HideResultAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        resultAnimator.SetTrigger("Hide");
    }

    private void ShowPanel(GameObject panel)
    {
        gamePanel.SetActive(panel == gamePanel);
        leaderboardPanel.SetActive(panel == leaderboardPanel);
        profilePanel.SetActive(panel == profilePanel);
        PlaySound(clickSound);
    }

    private void ShowSettingsPanel()
    {
        // يمكن إضافة لوحة الإعدادات هنا
        PlaySound(clickSound);
    }

    private void PlaySound(AudioSource audioSource)
    {
        if (audioSource != null)
            audioSource.PlayOneShot(audioSource.clip);
    }

    private int CalculateLevel()
    {
        return (int)(GameManager.Instance.currentPoints / 5000) + 1;
    }
}
