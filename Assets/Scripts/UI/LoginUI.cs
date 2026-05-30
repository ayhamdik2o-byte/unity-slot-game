using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    
    [Header("Login Panel")]
    [SerializeField] private InputField emailInput;
    [SerializeField] private InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button toggleRegisterButton;
    [SerializeField] private TextMeshProUGUI loginMessageText;
    
    [Header("Register Panel")]
    [SerializeField] private InputField regUsernameInput;
    [SerializeField] private InputField regEmailInput;
    [SerializeField] private InputField regPasswordInput;
    [SerializeField] private InputField regConfirmPasswordInput;
    [SerializeField] private Button registerButton;
    [SerializeField] private Button toggleLoginButton;
    [SerializeField] private TextMeshProUGUI registerMessageText;
    
    [Header("UI Elements")]
    [SerializeField] private Image loadingSpinner;
    [SerializeField] private CanvasGroup canvasGroup;
    
    private bool isProcessing = false;

    private void Start()
    {
        // Login Panel Listeners
        loginButton.onClick.AddListener(OnLoginClicked);
        toggleRegisterButton.onClick.AddListener(ShowRegisterPanel);
        
        // Register Panel Listeners
        registerButton.onClick.AddListener(OnRegisterClicked);
        toggleLoginButton.onClick.AddListener(ShowLoginPanel);
        
        // Show login panel by default
        ShowLoginPanel();
    }

    private void OnLoginClicked()
    {
        if (isProcessing) return;
        
        string email = emailInput.text.Trim();
        string password = passwordInput.text;

        if (!ValidateLoginInputs(email, password))
            return;

        isProcessing = true;
        loginButton.interactable = false;
        StartCoroutine(ShowLoadingSpinner());
        
        GameManager.Instance.Login(email, password, OnLoginSuccess, OnLoginFailed);
    }

    private void OnRegisterClicked()
    {
        if (isProcessing) return;
        
        string username = regUsernameInput.text.Trim();
        string email = regEmailInput.text.Trim();
        string password = regPasswordInput.text;
        string confirmPassword = regConfirmPasswordInput.text;

        if (!ValidateRegisterInputs(username, email, password, confirmPassword))
            return;

        isProcessing = true;
        registerButton.interactable = false;
        StartCoroutine(ShowLoadingSpinner());
        
        GameManager.Instance.Register(username, email, password, OnRegisterSuccess, OnRegisterFailed);
    }

    private bool ValidateLoginInputs(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            ShowLoginError("❌ أدخل البريد الإلكتروني");
            return false;
        }

        if (!email.Contains("@"))
        {
            ShowLoginError("❌ البريد الإلكتروني غير صحيح");
            return false;
        }

        if (string.IsNullOrEmpty(password))
        {
            ShowLoginError("❌ أدخل كلمة المرور");
            return false;
        }

        if (password.Length < 6)
        {
            ShowLoginError("❌ كلمة المرور ي��ب أن تكون 6 أحرف على الأقل");
            return false;
        }

        return true;
    }

    private bool ValidateRegisterInputs(string username, string email, string password, string confirmPassword)
    {
        if (string.IsNullOrEmpty(username))
        {
            ShowRegisterError("❌ أدخل اسم المستخدم");
            return false;
        }

        if (username.Length < 3)
        {
            ShowRegisterError("❌ اسم المستخدم يجب أن يكون 3 أحرف على الأقل");
            return false;
        }

        if (string.IsNullOrEmpty(email))
        {
            ShowRegisterError("❌ أدخل البريد الإلكتروني");
            return false;
        }

        if (!email.Contains("@"))
        {
            ShowRegisterError("❌ البريد الإلكتروني غير صحيح");
            return false;
        }

        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
        {
            ShowRegisterError("❌ أدخل كلمة المرور");
            return false;
        }

        if (password.Length < 6)
        {
            ShowRegisterError("❌ كلمة المرور يجب أن تكون 6 أحرف على الأقل");
            return false;
        }

        if (password != confirmPassword)
        {
            ShowRegisterError("❌ كلمات المرور غير متطابقة");
            return false;
        }

        return true;
    }

    private void OnLoginSuccess()
    {
        isProcessing = false;
        loginButton.interactable = true;
        ShowLoginMessage("✅ تم تسجيل الدخول بنجاح! جاري التحميل...", Color.green);
        StartCoroutine(LoadGameSceneDelay());
    }

    private void OnLoginFailed(string errorMessage)
    {
        isProcessing = false;
        loginButton.interactable = true;
        ShowLoginError("❌ " + errorMessage);
    }

    private void OnRegisterSuccess()
    {
        isProcessing = false;
        registerButton.interactable = true;
        ShowRegisterMessage("✅ تم التسجيل بنجاح! جاري التحميل...", Color.green);
        StartCoroutine(LoadGameSceneDelay());
    }

    private void OnRegisterFailed(string errorMessage)
    {
        isProcessing = false;
        registerButton.interactable = true;
        ShowRegisterError("❌ " + errorMessage);
    }

    private void ShowLoginPanel()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        ClearLoginInputs();
    }

    private void ShowRegisterPanel()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        ClearRegisterInputs();
    }

    private void ShowLoginMessage(string message, Color color)
    {
        loginMessageText.text = message;
        loginMessageText.color = color;
        loginMessageText.gameObject.SetActive(true);
    }

    private void ShowLoginError(string message)
    {
        ShowLoginMessage(message, Color.red);
    }

    private void ShowRegisterMessage(string message, Color color)
    {
        registerMessageText.text = message;
        registerMessageText.color = color;
        registerMessageText.gameObject.SetActive(true);
    }

    private void ShowRegisterError(string message)
    {
        ShowRegisterMessage(message, Color.red);
    }

    private void ClearLoginInputs()
    {
        emailInput.text = "";
        passwordInput.text = "";
        loginMessageText.gameObject.SetActive(false);
    }

    private void ClearRegisterInputs()
    {
        regUsernameInput.text = "";
        regEmailInput.text = "";
        regPasswordInput.text = "";
        regConfirmPasswordInput.text = "";
        registerMessageText.gameObject.SetActive(false);
    }

    private IEnumerator ShowLoadingSpinner()
    {
        loadingSpinner.gameObject.SetActive(true);
        while (isProcessing)
        {
            loadingSpinner.transform.Rotate(0, 0, -5);
            yield return null;
        }
        loadingSpinner.gameObject.SetActive(false);
    }

    private IEnumerator LoadGameSceneDelay()
    {
        yield return new WaitForSeconds(1.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
}
