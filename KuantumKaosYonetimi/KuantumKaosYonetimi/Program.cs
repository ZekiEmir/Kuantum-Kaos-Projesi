using System;
using System.Collections.Generic;

namespace KuantumKaosYonetimi
{
    // ---------------------------------------------------------
    // 1. BÖLÜM: ÖZEL HATA YÖNETİMİ (CUSTOM EXCEPTION)
    // ---------------------------------------------------------
    // Standart 'Exception' sınıfından türeterek kendi özel "Patlama" hatamızı tanımlıyoruz.
    public class KuantumCokusuException : Exception
    {
        public KuantumCokusuException(string nesneID)
            : base($"DİKKAT! {nesneID} kimlikli nesne çöktü! Patlama gerçekleşti.")
        {
        }
    }

    // ---------------------------------------------------------
    // 2. BÖLÜM: ARAYÜZ (INTERFACE)
    // ---------------------------------------------------------
    // Sadece "soğutulabilir" (tehlikeli) nesnelerin sahip olacağı yetenekleri belirtir.
    public interface IKritik
    {
        void AcilDurumSogutmasi();
    }

    // ---------------------------------------------------------
    // 3. BÖLÜM: TEMEL YAPI (ABSTRACT CLASS & ENCAPSULATION) 
    // ---------------------------------------------------------
    // Diğer tüm nesnelerin atasıdır. Tek başına üretilemez (abstract).
    public abstract class KuantumNesnesi
    {
        public string ID { get; set; }
        public int TehlikeSeviyesi { get; set; }

        // Kapsülleme (Encapsulation): _stabilite değişkenini koruma altına alıyoruz.
        private double _stabilite;

        public double Stabilite
        {
            get { return _stabilite; }
            set
            {
                // Değer 100'ü aşarsa sınıra sabitliyoruz.
                if (value > 100)
                    _stabilite = 100;
                // Değer 0 veya altına düşerse sistemi patlatıyoruz (Hata fırlatıyoruz).
                else if (value <= 0)
                {
                    _stabilite = 0;
                    throw new KuantumCokusuException(ID);
                }
                // Sorun yoksa değeri normal atıyoruz.
                else
                {
                    _stabilite = value;
                }
            }
        }

        // Alt sınıfların kendi yöntemleriyle doldurması gereken gövdesiz metot.
        public abstract void AnalizEt();

        // Nesnenin o anki durumunu yazı olarak veren yardımcı metot.
        public string DurumBilgisi()
        {
            return $"[{ID}] Stabilite: %{Stabilite:F2} | Tehlike: {TehlikeSeviyesi}";
        }
    }

    // ---------------------------------------------------------
    // 4. BÖLÜM: NESNE ÇEŞİTLERİ (INHERITANCE)
    // ---------------------------------------------------------

    // A. Veri Paketi (Sıradan Nesne)
    // IKritik arayüzünü almadığı için soğutulamaz, sadece analiz edilebilir.
    public class VeriPaketi : KuantumNesnesi
    {
        public VeriPaketi(string id)
        {
            ID = id;
            Stabilite = 100;
            TehlikeSeviyesi = 1;
        }

        // Analiz edilince stabiliteyi az düşürür (-5).
        public override void AnalizEt()
        {
            Stabilite -= 5;
            Console.WriteLine($"{ID} içeriği okundu. (Stabilite -5)");
        }
    }

    // B. Karanlık Madde (Tehlikeli Nesne)
    // KuantumNesnesi'nden miras alır VE IKritik arayüzünü uygular (Soğutulabilir).
    public class KaranlikMadde : KuantumNesnesi, IKritik
    {
        public KaranlikMadde(string id)
        {
            ID = id;
            Stabilite = 100;
            TehlikeSeviyesi = 5;
        }

        // Analiz edilince stabiliteyi orta seviye düşürür (-15).
        public override void AnalizEt()
        {
            Stabilite -= 15;
            Console.WriteLine($"{ID} analiz edildi. Karanlık enerji yayılıyor! (Stabilite -15)");
        }

        // IKritik arayüzünden gelen soğutma özelliği.
        public void AcilDurumSogutmasi()
        {
            Stabilite += 50; // Stabiliteyi artırır (Setter 100'ü geçirmemeyi garantiler).
            Console.WriteLine($"{ID} soğutuldu. Stabilite yenilendi.");
        }
    }

    // C. Anti Madde (Çok Tehlikeli Nesne)
    public class AntiMadde : KuantumNesnesi, IKritik
    {
        public AntiMadde(string id)
        {
            ID = id;
            Stabilite = 100;
            TehlikeSeviyesi = 10;
        }

        // Analiz edilince stabiliteyi çok düşürür (-25). Patlama riski yüksek.
        public override void AnalizEt()
        {
            Console.WriteLine("UYARI: Evrenin dokusu titriyor...");
            Stabilite -= 25;
            Console.WriteLine($"{ID} analiz edildi. (Stabilite -25)");
        }

        public void AcilDurumSogutmasi()
        {
            Stabilite += 50;
            Console.WriteLine($"{ID} soğutuldu. Kritik seviye düşürüldü.");
        }
    }

    // ---------------------------------------------------------
    // 5. BÖLÜM: OYNANIŞ DÖNGÜSÜ (MAIN LOOP)
    // ---------------------------------------------------------
    class Program
    {
        static void Main(string[] args)
        {
            // Polimorfizm: Farklı türdeki nesneleri tek bir 'KuantumNesnesi' listesinde tutuyoruz.
            List<KuantumNesnesi> envanter = new List<KuantumNesnesi>();
            Random rnd = new Random();
            int sayac = 1;

            Console.WriteLine("--- OMEGA SEKTÖRÜ KUANTUM VERİ AMBARI BAŞLATILIYOR ---");
            Console.WriteLine("Vardiya Amiri Girişi Yapıldı...\n");

            // Hata yakalama bloğu (try-catch). Sistem patlarsa program kapanacak.
            try
            {
                while (true)
                {
                    Console.WriteLine("\n=== KUANTUM AMBARI KONTROL PANELİ ===");
                    Console.WriteLine("1. Yeni Nesne Ekle (Rastgele)");
                    Console.WriteLine("2. Tüm Envanteri Listele");
                    Console.WriteLine("3. Nesneyi Analiz Et");
                    Console.WriteLine("4. Acil Durum Soğutması Yap");
                    Console.WriteLine("5. Çıkış");
                    Console.Write("Seçiminiz: ");
                    string secim = Console.ReadLine();

                    if (secim == "1")
                    {
                        // 1 ile 3 arasında rastgele sayı üretip, şansa göre nesne ekliyoruz.
                        int zar = rnd.Next(1, 4);
                        string yeniId = "NESNE-" + sayac++;

                        if (zar == 1) envanter.Add(new VeriPaketi(yeniId));
                        else if (zar == 2) envanter.Add(new KaranlikMadde(yeniId));
                        else envanter.Add(new AntiMadde(yeniId));

                        Console.WriteLine($"{yeniId} ambara kabul edildi.");
                    }
                    else if (secim == "2")
                    {
                        Console.WriteLine("\n--- ENVANTER DURUMU ---");
                        if (envanter.Count == 0) Console.WriteLine("Ambar boş.");

                        // Listedeki her bir nesneyi gezerek durumunu yazdırıyoruz.
                        foreach (var nesne in envanter)
                        {
                            Console.WriteLine(nesne.DurumBilgisi());
                        }
                    }
                    else if (secim == "3")
                    {
                        Console.Write("Analiz edilecek Nesne ID'si: ");
                        string girilenId = Console.ReadLine();

                        // ID'si eşleşen ilk nesneyi listeden bulur.
                        var hedef = envanter.Find(x => x.ID == girilenId);

                        if (hedef != null)
                        {
                            // Analiz metodu çalışır, stabilite düşer. 0 olursa patlar!
                            hedef.AnalizEt();
                        }
                        else
                        {
                            Console.WriteLine("Nesne bulunamadı!");
                        }
                    }
                    else if (secim == "4")
                    {
                        Console.Write("Soğutulacak Nesne ID'si: ");
                        string girilenId = Console.ReadLine();
                        var hedef = envanter.Find(x => x.ID == girilenId);

                        if (hedef != null)
                        {
                            // 'is' anahtar kelimesi ile nesnenin soğutulabilir (IKritik) olup olmadığına bakıyoruz.
                            if (hedef is IKritik kritikNesne)
                            {
                                kritikNesne.AcilDurumSogutmasi();
                            }
                            else
                            {
                                Console.WriteLine("HATA: Bu nesne soğutulamaz! (Tehlikeli değil)");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nesne bulunamadı!");
                        }
                    }
                    else if (secim == "5")
                    {
                        Console.WriteLine("Sistem kapatılıyor...");
                        break; // Döngüyü kırar ve programı sonlandırır.
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz seçim.");
                    }
                }
            }
            // Stabilite 0'a inip hata fırlatıldığında kod buraya atlar.
            catch (KuantumCokusuException ex)
            {
                Console.WriteLine("\n**************************************");
                Console.WriteLine("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
                Console.WriteLine($"Sebep: {ex.Message}");
                Console.WriteLine("**************************************");
            }
            // Öngörülemeyen başka hatalar için genel yakalama bloğu.
            catch (Exception ex)
            {
                Console.WriteLine($"Beklenmedik bir hata: {ex.Message}");
            }

            Console.WriteLine("Simülasyon Bitti. Çıkmak için bir tuşa basın.");
            Console.ReadKey();
        }
    }
}