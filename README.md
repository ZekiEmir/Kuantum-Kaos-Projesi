# Kuantum Kaos Yönetimi
Bu proje, Omega Sektöründeki Kuantum Veri Ambarı'nın simülasyonunu içeren bir projedir. 
# Proje Hakkında Bu simülasyon, evrenin en kararsız seçimleri (Veri Paketleri, Karanlık Madde ve Anti Madde) analiz eden ve stabilitelerini yönetilen bir otomasyon sistemidir. 

Proje aşağıdaki 4 farklı programlama ayrıntıları ayrı ayrı kodlanmıştır: 
1. C#
2. Java
3. Python
4. JavaScript (Node.js)

# Teknik Detaylar ve Mimari
Ödev özelliklerine uygun olarak aşağıdaki OOP kuralları eksiksiz olarak eklenmiştir: 
* Abstract Class : Kuantum Nesnesi sınıfı temel yapı olarak tasarlanmış ve hızlandırılma ile stabilite değerleri belirlenirtur.
* Arayüz: Tehlikeli maddeler için IKritik bileşeni oluşturulmuş ve sadece bu maddelere "Acil Durum Soğutma" yeteneği verilmiştir.
* Polimorfizm: Her nesne türü (Veri Paketi, Karanlık Madde, Anti Madde) AnalizEt() yönteminin kendi özelliklerine göre ezerek (geçersiz kılma) farklı davranışları sergilemektedir.
* Özel İstisna: Stabilite seviyesi 0'ın altında düştüğünde sistem bir hata yerine KuantumCokusuException fırlatarak simülasyonu sonlandırılmaktadır.

# Dosya Yapısı
* Program.cs: C# Çözümü
* KuantumKaosYonetimi.java: Java Çözümü
* KuantumKaosYonetimi.py: Python Çözümü
* KuantumKaosYonetimi.js: JavaScript Çözümü
