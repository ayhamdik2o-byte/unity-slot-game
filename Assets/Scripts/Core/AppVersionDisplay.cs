using UnityEngine;
using TMPro;

public class AppVersionDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;

    private void Start()
    {
        if (versionText != null)
        {
            versionText.text = $"Queensati v{AppSettings.APP_VERSION}";
        }
        
        Debug.Log($"📱 تطبيق: {AppSettings.APP_NAME}");
        Debug.Log($"📦 الحزمة: {AppSettings.PACKAGE_NAME}");
        Debug.Log($"🔧 الخادم: {AppSettings.BACKEND_URL}");
    }
}
