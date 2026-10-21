# ByeByeDPI Android

<div align="center">
  <h3>Android İçin Yerel Derin Paket İnceleme (DPI) Sansür Aşma Aracı</h3>
  <p>VPN Sunucusu Olmadan • Hız Kaybı Yaşamadan • Root İzni Gerektirmeden</p>
</div>

---

## 📱 Genel Bakış

**ByeByeDPI Android**, [GoodbyeDPI](https://github.com/ValdikSS/GoodbyeDPI) ve [ByeByeDPI Desktop](https://github.com/alisariaslan/ByeByeDPI) mantığını Android platformuna taşıyan yerel bir DPI atlatma aracıdır.

> [!NOTE]
> Bu uygulama klasik bir VPN **değildir**. Trafiğinizi uzak bir sunucuya veya yabancı bir ülkeye aktarmaz, IP adresinizi değiştirmez. Android'in `VpnService` API'sini yalnızca cihaz içindeki trafiği yakalamak ve yerel paket manipülasyonu (TLS ClientHello parçalama, HTTP başlık modifikasyonu, TCP paket bölme) uygulamak için kullanır. Bu sayede internet bağlantı hızınızda hiçbir kayıp veya gecikme (ping artışı) yaşanmaz.

---

## ✨ Özellikler

* ⚡ **Tek Tıkla Bağlantı**: Animasyonlu ve şık neon buton ile anında koruma.
* 🇹🇷 **Türkiye Servis Sağlayıcıları İçin Optimize Modlar**:
  * **Türkiye Standart**: TurkNet, Türk Telekom ve genel ağlar için dengeli parçalama (`--split 1 --disoob 1 --auto=torst`).
  * **Türkiye Agresif**: Superonline ve derin sansür uygulayan ağlar için 2 baytlık ClientHello bölme (`--split 2 --disoob 2 --tlsrec 1+s`).
  * **Discord & Voice Özel**: Discord ses kanalları, metin akışı ve medya problemleri için optimize mod.
  * **Özel Parametreler**: Kendi komut satırı argümanlarınızı elle girebilme desteği.
* 🔘 **Hızlı Ayarlar Kutucuğu (Quick Settings Tile)**: Android bildirim çubuğunu aşağı kaydırarak uygulamayı açmadan tek tıkla başlatıp durdurma.
* 🔀 **Bölünmüş Tünelleme (Split Tunneling)**:
  * İstediğiniz uygulamaları (örneğin bankacılık uygulamaları) tünel dışına çıkarma (Baypas).
  * Veya yalnızca seçtiğiniz uygulamaların tüneli kullanmasını sağlama.
* 🌐 **Güvenli DNS Yönetimi**: Cloudflare (1.1.1.1), Google (8.8.8.8), AdGuard DNS veya özel DNS IP seçeneği.
* 🚀 **Açılışta Otomatik Başlatma**: Telefon yeniden başlatıldığında arka planda otomatik devreye girme.
* 📜 **Canlı Konsol Kayıtları**: DPI motorunun ürettiği tüm ağ ve paket tanılarını gerçek zamanlı izleme ve panoya kopyalama.
* 🎨 **Modern Cyber-Dark Tasarım**: Android 14/15 uyumlu Jetpack Compose (Material 3) arayüzü.

---

## 🏗️ Mimari Şema

```
[Android Uygulamaları / Tarayıcı]
               │
               ▼
   [Android VpnService (tun0)]
               │
               ▼
   [ByeByeDPI Servis Katmanı]
               │
               ▼
[DPI Manipülasyon Motoru (ciadpi)]
 ├── TLS ClientHello Bölme (SNI Splitting)
 ├── Sahte TCP/HTTP İstekleri (Fake Packets)
 └── HTTP Başlık Modifikasyonları
               │
               ▼
     [İnternet / ISS Ağ Geçidi]
```

---

## 🛠️ Derleme ve Kurulum (Android Studio)

### Gereksinimler
* Android Studio (Ladybug / Koala / Jellyfish veya üstü)
* JDK 21
* Android SDK 35 / 36

### Adımlar
1. Android Studio'yu açın.
2. **Open** seçeneğiyle bu klasörü (`android/` veya `ByeByeDPI-Android`) seçin.
3. Gradle senkronizasyonunun tamamlanmasını bekleyin.
4. Cihazınızı veya emülatörü bağlayıp **Run 'app'** (`Shift + F10`) tuşuna basın.

---

## 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) altında dağıtılmaktadır.
Geliştirici: [Ali Sarıaslan](https://github.com/alisariaslan)
Masaüstü Versiyon: [alisariaslan/ByeByeDPI](https://github.com/alisariaslan/ByeByeDPI)
