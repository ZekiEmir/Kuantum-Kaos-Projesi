import random
from abc import ABC, abstractmethod # Soyut sınıf (Abstract) yapmak için gerekli kütüphane

# =============================================================
# 1. BÖLÜM: ÖZEL HATA YÖNETİMİ
# =============================================================
# Python'da hatalar "Exception" sınıfından türer.
class KuantumCokusuException(Exception):
    def __init__(self, nesne_id):
        self.message = f"DİKKAT! {nesne_id} kimlikli nesne çöktü! Patlama gerçekleşti."
        super().__init__(self.message)

# =============================================================
# 2. BÖLÜM: ARAYÜZ (INTERFACE)
# =============================================================
# Python'da "Interface" yoktur, bunu Soyut Sınıf (ABC) ile taklit ederiz.
# Bu sınıftan miras alanlar "acil_durum_sogutmasi" yapmak ZORUNDADIR.
class IKritik(ABC):
    @abstractmethod
    def acil_durum_sogutmasi(self):
        pass

# =============================================================
# 3. BÖLÜM: TEMEL YAPI (ABSTRACT CLASS)
# =============================================================
class KuantumNesnesi(ABC):
    def __init__(self, id_no):
        self.id = id_no            # ID özelliği
        self.tehlike_seviyesi = 0  # Tehlike seviyesi
        self._stabilite = 100.0    # Gizli özellik (önünde _ var)

    # KAPSÜLLEME (ENCAPSULATION) - Property
    @property
    def stabilite(self):
        return self._stabilite

    @stabilite.setter
    def stabilite(self, value):
        if value > 100:
            self._stabilite = 100
        elif value <= 0:
            self._stabilite = 0
            # Hata fırlatma anı!
            raise KuantumCokusuException(self.id)
        else:
            self._stabilite = value

    # Soyut Metot (Alt sınıflar doldurmak zorunda)
    @abstractmethod
    def analiz_et(self):
        pass

    # Durum Bilgisi
    def durum_bilgisi(self):
        return f"[{self.id}] Stabilite: %{self.stabilite:.2f} | Tehlike: {self.tehlike_seviyesi}"

# =============================================================
# 4. BÖLÜM: NESNE ÇEŞİTLERİ (INHERITANCE)
# =============================================================

# A. Veri Paketi
class VeriPaketi(KuantumNesnesi):
    def __init__(self, id_no):
        super().__init__(id_no) # Ata sınıfın kurucusunu çağır
        self.tehlike_seviyesi = 1
    
    def analiz_et(self):
        self.stabilite -= 5
        print(f"{self.id} içeriği okundu. (Stabilite -5)")

# B. Karanlık Madde (Hem KuantumNesnesi hem IKritik)
class KaranlikMadde(KuantumNesnesi, IKritik):
    def __init__(self, id_no):
        super().__init__(id_no)
        self.tehlike_seviyesi = 5

    def analiz_et(self):
        self.stabilite -= 15
        print(f"{self.id} analiz edildi. Karanlık enerji yayılıyor! (Stabilite -15)")

    def acil_durum_sogutmasi(self):
        self.stabilite += 50
        print(f"{self.id} soğutuldu. Stabilite yenilendi.")

# C. Anti Madde
class AntiMadde(KuantumNesnesi, IKritik):
    def __init__(self, id_no):
        super().__init__(id_no)
        self.tehlike_seviyesi = 10

    def analiz_et(self):
        print("UYARI: Evrenin dokusu titriyor...")
        self.stabilite -= 25
        print(f"{self.id} analiz edildi. (Stabilite -25)")

    def acil_durum_sogutmasi(self):
        self.stabilite += 50
        print(f"{self.id} soğutuldu. Kritik seviye düşürüldü.")

# =============================================================
# 5. BÖLÜM: OYNANIŞ DÖNGÜSÜ (MAIN LOOP)
# =============================================================
def main():
    envanter = [] # Listemizi oluşturduk
    sayac = 1

    print("--- OMEGA SEKTÖRÜ KUANTUM VERİ AMBARI (PYTHON) ---")

    try:
        while True: # Sonsuz Döngü
            print("\n=== KUANTUM AMBARI KONTROL PANELİ ===")
            print("1. Yeni Nesne Ekle")
            print("2. Tüm Envanteri Listele")
            print("3. Nesneyi Analiz Et")
            print("4. Acil Durum Soğutması Yap")
            print("5. Çıkış")
            
            secim = input("Seçiminiz: ")

            if secim == "1":
                zar = random.randint(1, 3)
                yeni_id = f"NESNE-{sayac}"
                sayac += 1

                if zar == 1: envanter.append(VeriPaketi(yeni_id))
                elif zar == 2: envanter.append(KaranlikMadde(yeni_id))
                else: envanter.append(AntiMadde(yeni_id))
                
                print(f"{yeni_id} ambara kabul edildi.")

            elif secim == "2":
                print("\n--- ENVANTER DURUMU ---")
                if not envanter:
                    print("Ambar boş.")
                for nesne in envanter:
                    print(nesne.durum_bilgisi()) # Polimorfizm

            elif secim == "3":
                girilen_id = input("Analiz edilecek ID: ")
                # Python'da listeyi tarayıp ID bulmanın kısa yolu (Next):
                hedef = next((x for x in envanter if x.id == girilen_id), None)

                if hedef:
                    hedef.analiz_et()
                else:
                    print("Nesne bulunamadı!")

            elif secim == "4":
                girilen_id = input("Soğutulacak ID: ")
                hedef = next((x for x in envanter if x.id == girilen_id), None)

                if hedef:
                    # Type Checking (Bu nesne IKritik mi?) 
                    if isinstance(hedef, IKritik):
                        hedef.acil_durum_sogutmasi()
                    else:
                        print("HATA: Bu nesne soğutulamaz! (IKritik değil)")
                else:
                    print("Nesne bulunamadı!")

            elif secim == "5":
                print("Sistem kapatılıyor...")
                break
            else:
                print("Geçersiz seçim.")

    except KuantumCokusuException as e:
        # Game Over Yakalama
        print("\n**************************************")
        print("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...")
        print(f"Sebep: {e}")
        print("**************************************")
    except Exception as e:
        print(f"Beklenmedik bir hata: {e}")

# Python'da kodu başlatmak için bu kalıp kullanılır:
if __name__ == "__main__":
    main()