using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private Image medalImage;
    [SerializeField] private Color[] medalColors = new Color[3];
    
    public void SetData(int rank, string username, long points)
    {
        rankText.text = $"#{rank}";
        nameText.text = username;
        pointsText.text = points.ToString("N0");
        
        // تعيين ألوان الميداليات
        if (rank == 1)
        {
            medalImage.color = new Color(1f, 0.84f, 0f); // Gold
            rankText.text = "🥇 " + rankText.text;
        }
        else if (rank == 2)
        {
            medalImage.color = new Color(0.75f, 0.75f, 0.75f); // Silver
            rankText.text = "🥈 " + rankText.text;
        }
        else if (rank == 3)
        {
            medalImage.color = new Color(0.8f, 0.5f, 0.2f); // Bronze
            rankText.text = "🥉 " + rankText.text;
        }
    }
}
