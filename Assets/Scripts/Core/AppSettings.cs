using UnityEngine;

public class AppSettings : MonoBehaviour
{
    public static readonly string APP_NAME = "Queensati";
    public static readonly string APP_VERSION = "1.0";
    public static readonly string COMPANY_NAME = "Queensati Games";
    public static readonly string PACKAGE_NAME = "com.queensati.slotgame";
    
    // Backend URL (عدّل حسب خادمك)
    public static readonly string BACKEND_URL = "http://your-server.com:3000/api";
    
    // أو استخدم Localhost على نفس الشبكة
    public static readonly string LOCALHOST_URL = "http://192.168.1.100:3000/api";
}
