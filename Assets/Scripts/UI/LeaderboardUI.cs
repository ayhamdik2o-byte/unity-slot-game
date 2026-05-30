using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private Transform leaderboardContainer;
    [SerializeField] private GameObject leaderboardEntryPrefab;
    [SerializeField] private Button refreshButton;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private AudioSource clickSound;
    
    private List<LeaderboardEntry> leaderboardData = new List<LeaderboardEntry>();

    private void Start()
    {
        refreshButton.onClick.AddListener(RefreshLeaderboard);
        RefreshLeaderboard();
    }

    private void RefreshLeaderboard()
    {
        loadingText.text = "جاري التحميل...";
        loadingText.gameObject.SetActive(true);
        
        ClearLeaderboard();
        GameManager.Instance.GetLeaderboard(OnLeaderboardReceived);
        
        PlaySound();
    }

    private void OnLeaderboardReceived(LeaderboardEntry[] entries)
    {
        loadingText.gameObject.SetActive(false);
        DisplayLeaderboard(entries);
    }

    private void DisplayLeaderboard(LeaderboardEntry[] entries)
    {
        for (int i = 0; i < entries.Length; i++)
        {
            GameObject newEntry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            LeaderboardEntryUI entryUI = newEntry.GetComponent<LeaderboardEntryUI>();
            
            if (entryUI != null)
            {
                entryUI.SetData(i + 1, entries[i].username, entries[i].points);
            }
        }
        
        // Scroll to top
        scrollRect.verticalNormalizedPosition = 1f;
    }

    private void ClearLeaderboard()
    {
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void PlaySound()
    {
        if (clickSound != null)
            clickSound.PlayOneShot(clickSound.clip);
    }
}

[System.Serializable]
public class LeaderboardEntry
{
    public int rank;
    public string username;
    public long points;
}
