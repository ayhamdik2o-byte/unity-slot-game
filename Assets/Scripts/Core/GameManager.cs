using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private string backendURL = "http://localhost:3000/api";
    
    public string playerToken = "";
    public int userId = 0;
    public string username = "";
    public long currentPoints = 10000;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Register
    public void Register(string username, string email, string password, 
        Action onSuccess, Action<string> onFailed)
    {
        StartCoroutine(RegisterCoroutine(username, email, password, onSuccess, onFailed));
    }

    private IEnumerator RegisterCoroutine(string username, string email, string password,
        Action onSuccess, Action<string> onFailed)
    {
        string jsonData = $"{{\"username\":\"{username}\",\"email\":\"{email}\",\"password\":\"{password}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest($"{backendURL}/auth/register", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            onSuccess?.Invoke();
        }
        else
        {
            onFailed?.Invoke("فشل التسجيل. حاول مرة أخرى.");
        }
    }

    // Login
    public void Login(string email, string password, 
        Action onSuccess, Action<string> onFailed)
    {
        StartCoroutine(LoginCoroutine(email, password, onSuccess, onFailed));
    }

    private IEnumerator LoginCoroutine(string email, string password,
        Action onSuccess, Action<string> onFailed)
    {
        string jsonData = $"{{\"email\":\"{email}\",\"password\":\"{password}\"}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest($"{backendURL}/auth/login", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            LoginResponse loginData = JsonUtility.FromJson<LoginResponse>(response);
            
            playerToken = loginData.token;
            userId = loginData.user.id;
            username = loginData.user.username;
            currentPoints = loginData.user.points;

            onSuccess?.Invoke();
        }
        else
        {
            onFailed?.Invoke("بيانات الدخول غير صحيحة");
        }
    }

    // Spin Slots
    public void SpinSlots(int betAmount, 
        Action<string[], string, int, long> onSpinComplete)
    {
        StartCoroutine(SpinCoroutine(betAmount, onSpinComplete));
    }

    private IEnumerator SpinCoroutine(int betAmount,
        Action<string[], string, int, long> onSpinComplete)
    {
        string jsonData = $"{{\"betAmount\":{betAmount}}}";
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest($"{backendURL}/game/spin", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {playerToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            SpinResponse spinData = JsonUtility.FromJson<SpinResponse>(response);
            
            currentPoints = spinData.newBalance;
            onSpinComplete?.Invoke(spinData.reels, spinData.result, spinData.winAmount, spinData.newBalance);
        }
    }

    // Daily Challenge
    public void CompleteDailyChallenge(Action<int> onComplete)
    {
        StartCoroutine(DailyChallengeCoroutine(onComplete));
    }

    private IEnumerator DailyChallengeCoroutine(Action<int> onComplete)
    {
        UnityWebRequest request = new UnityWebRequest($"{backendURL}/game/daily-challenge", "POST");
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", $"Bearer {playerToken}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            ChallengeResponse challengeData = JsonUtility.FromJson<ChallengeResponse>(response);
            currentPoints += challengeData.bonusPoints;
            onComplete?.Invoke(challengeData.bonusPoints);
        }
    }

    // Get Leaderboard
    public void GetLeaderboard(Action<LeaderboardEntry[]> onComplete)
    {
        StartCoroutine(GetLeaderboardCoroutine(onComplete));
    }

    private IEnumerator GetLeaderboardCoroutine(Action<LeaderboardEntry[]> onComplete)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{backendURL}/leaderboard/top");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string response = request.downloadHandler.text;
            // Parse JSON array
            LeaderboardResponse leaderboardResponse = new LeaderboardResponse();
            leaderboardResponse.entries = JsonHelper.FromJson<LeaderboardEntry>(response);
            
            onComplete?.Invoke(leaderboardResponse.entries);
        }
    }
}

// Response Models
[System.Serializable]
public class LoginResponse
{
    public string token;
    public UserData user;
}

[System.Serializable]
public class UserData
{
    public int id;
    public string username;
    public long points;
    public int rank;
}

[System.Serializable]
public class SpinResponse
{
    public string[] reels;
    public int betAmount;
    public int winAmount;
    public string result;
    public long newBalance;
}

[System.Serializable]
public class ChallengeResponse
{
    public string message;
    public int bonusPoints;
}

[System.Serializable]
public class LeaderboardResponse
{
    public LeaderboardEntry[] entries;
}

// JSON Helper for Array Parsing
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>("{ \"Items\": " + json + "}");
        return wrapper.Items;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
