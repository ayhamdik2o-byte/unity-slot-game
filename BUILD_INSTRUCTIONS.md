# 🎮 Queensati - SlotFun Game

## خطوات بناء APK

### المتطلبات:
- ✅ Unity 2021 LTS أو أحدث
- ✅ Android SDK
- ✅ NDK
- ✅ JDK

### الخطوات الأساسية:

#### 1️⃣ في Unity
```
File → Build Settings → Android → Switch Platform
```

#### 2️⃣ إعدادات البناء
```
File → Build Settings → Player Settings

تعيين:
- Company Name: Queensati Games
- Product Name: Queensati
- Package Name: com.queensati.slotgame
- Version: 1.0
- Bundle Version Code: 1
```

#### 3️⃣ استخدام أداة البناء التلقائية (اختياري)
```
في Unity:
Assets/Editor/BuildAPK.cs

Menu → Build → Build Queensati APK
```

#### 4️⃣ بناء يدوي
```
File → Build Settings
اختر Android
اضغط Build
اختر مكان الحفظ
```

#### 5️⃣ تثبيت على الهاتف
```bash
# باستخدام ADB
adb install -r Queensati.apk

# أو انسخ الملف يدوياً وثبّته
```

### إعدادات الـ Backend

عدّل URL في `AppSettings.cs`:

```csharp
// للخادم المحلي (نفس الشبكة)
public static readonly string BACKEND_URL = "http://192.168.1.100:3000/api";

// للخادم الحقيقي
public static readonly string BACKEND_URL = "http://your-domain.com:3000/api";
```

### حجم APK المتوقع
- التطبيق الأساسي: ~50-80 MB

### ملاحظات مهمة
⚠️ تأكد من:
- [ ] الاتصال بالإنترنت متاح
- [ ] Backend Server يعمل
- [ ] URL الخادم صحيحة
- [ ] الصلاحيات مفعّلة في AndroidManifest.xml

---

**تم البناء بنجاح! 🎉**
