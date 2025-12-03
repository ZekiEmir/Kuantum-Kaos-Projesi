import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.Scanner;

// 1. BÖLÜM: ÖZEL HATA YÖNETİMİ
class KuantumCokusuException extends RuntimeException {
    public KuantumCokusuException(String nesneID) {
        super("DİKKAT! " + nesneID + " kimlikli nesne çöktü! Patlama gerçekleşti.");
    }
}

// 2. BÖLÜM: ARAYÜZ (INTERFACE)
interface IKritik {
    void acilDurumSogutmasi();
}

// 3. BÖLÜM: TEMEL YAPI (ABSTRACT CLASS)
abstract class KuantumNesnesi {
    protected String id;
    protected int tehlikeSeviyesi;
    private double stabilite;

    // Kurucu Metot (Constructor)
    public KuantumNesnesi(String id) {
        this.id = id;
    }

    public String getId() {
        return id;
    }

    public int getTehlikeSeviyesi() {
        return tehlikeSeviyesi;
    }

    // Encapsulation (Kapsülleme)
    public double getStabilite() {
        return stabilite;
    }

    public void setStabilite(double value) {
        if (value > 100) {
            this.stabilite = 100;
        } else if (value <= 0) {
            this.stabilite = 0;
            // Stabilite 0 veya altına düşerse hata fırlat
            throw new KuantumCokusuException(this.id);
        } else {
            this.stabilite = value;
        }
    }

    // Abstract Metot
    public abstract void analizEt();

    // Durum Bilgisi Metodu 
    public String durumBilgisi() {
        // String.format ile virgülden sonra 2 basamak gösteriyoruz
        return String.format("[%s] Stabilite: %% %.2f | Tehlike: %d", id, stabilite, tehlikeSeviyesi);
    }
}

// 4. BÖLÜM: NESNE ÇEŞİTLERİ (INHERITANCE) 

// A. Veri Paketi 
class VeriPaketi extends KuantumNesnesi {
    public VeriPaketi(String id) {
        super(id);
        setStabilite(100);
        this.tehlikeSeviyesi = 1;
    }

    @Override
    public void analizEt() {
        setStabilite(getStabilite() - 5);
        System.out.println(getId() + " içeriği okundu. (Stabilite -5)");
    }
}

// B. Karanlık Madde 
class KaranlikMadde extends KuantumNesnesi implements IKritik {
    public KaranlikMadde(String id) {
        super(id);
        setStabilite(100);
        this.tehlikeSeviyesi = 5;
    }

    @Override
    public void analizEt() {
        setStabilite(getStabilite() - 15);
        System.out.println(getId() + " analiz edildi. Karanlık enerji yayılıyor! (Stabilite -15)");
    }

    @Override
    public void acilDurumSogutmasi() {
        setStabilite(getStabilite() + 50);
        System.out.println(getId() + " soğutuldu. Stabilite yenilendi.");
    }
}

// C. Anti Madde 
class AntiMadde extends KuantumNesnesi implements IKritik {
    public AntiMadde(String id) {
        super(id);
        setStabilite(100);
        this.tehlikeSeviyesi = 10;
    }

    @Override
    public void analizEt() {
        System.out.println("UYARI: Evrenin dokusu titriyor...");
        setStabilite(getStabilite() - 25);
        System.out.println(getId() + " analiz edildi. (Stabilite -25)");
    }

    @Override
    public void acilDurumSogutmasi() {
        setStabilite(getStabilite() + 50);
        System.out.println(getId() + " soğutuldu. Kritik seviye düşürüldü.");
    }
}

// 5. BÖLÜM: OYNANIŞ DÖNGÜSÜ (MAIN LOOP) 
public class KuantumKaosYonetimi {
    public static void main(String[] args) {
        List<KuantumNesnesi> envanter = new ArrayList<>(); // Generic List 
        Scanner scanner = new Scanner(System.in);
        Random rnd = new Random();
        int sayac = 1;

        System.out.println("--- OMEGA SEKTÖRÜ KUANTUM VERİ AMBARI (JAVA) ---");

        try {
            while (true) { // Sonsuz Döngü 
                System.out.println("\n=== KUANTUM AMBARI KONTROL PANELİ ===");
                System.out.println("1. Yeni Nesne Ekle");
                System.out.println("2. Tüm Envanteri Listele");
                System.out.println("3. Nesneyi Analiz Et");
                System.out.println("4. Acil Durum Soğutması Yap");
                System.out.println("5. Çıkış");
                System.out.print("Seçiminiz: ");

                String secim = scanner.nextLine();

                if (secim.equals("1")) {
                    int zar = rnd.nextInt(3) + 1; // 1, 2 veya 3
                    String yeniId = "NESNE-" + sayac++;

                    if (zar == 1) envanter.add(new VeriPaketi(yeniId));
                    else if (zar == 2) envanter.add(new KaranlikMadde(yeniId));
                    else envanter.add(new AntiMadde(yeniId));

                    System.out.println(yeniId + " ambara kabul edildi.");
                } 
                else if (secim.equals("2")) {
                    System.out.println("\n--- ENVANTER DURUMU ---");
                    if (envanter.isEmpty()) System.out.println("Ambar boş.");
                    
                    for (KuantumNesnesi nesne : envanter) {
                        System.out.println(nesne.durumBilgisi()); // Polimorfizm 
                    }
                } 
                else if (secim.equals("3")) {
                    System.out.print("Analiz edilecek ID: ");
                    String girilenId = scanner.nextLine();
                    
                    // Nesneyi bulma
                    KuantumNesnesi hedef = null;
                    for (KuantumNesnesi k : envanter) {
                        if (k.getId().equals(girilenId)) {
                            hedef = k;
                            break;
                        }
                    }

                    if (hedef != null) {
                        hedef.analizEt();
                    } else {
                        System.out.println("Nesne bulunamadı!");
                    }
                } 
                else if (secim.equals("4")) {
                    System.out.print("Soğutulacak ID: ");
                    String girilenId = scanner.nextLine();
                    
                    KuantumNesnesi hedef = null;
                    for (KuantumNesnesi k : envanter) {
                        if (k.getId().equals(girilenId)) {
                            hedef = k;
                            break;
                        }
                    }

                    if (hedef != null) {
                        // Type Checking (instanceof) 
                        if (hedef instanceof IKritik) {
                            ((IKritik) hedef).acilDurumSogutmasi();
                        } else {
                            System.out.println("Bu nesne soğutulamaz! (IKritik değil)");
                        }
                    } else {
                        System.out.println("Nesne bulunamadı!");
                    }
                } 
                else if (secim.equals("5")) {
                    System.out.println("Çıkış yapılıyor...");
                    break;
                }
            }
        } catch (KuantumCokusuException e) {
            // Game Over Yakalama 
            System.out.println("\n**************************************");
            System.out.println("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
            System.out.println("Sebep: " + e.getMessage());
            System.out.println("**************************************");
        } catch (Exception e) {
            System.out.println("Beklenmedik hata: " + e.getMessage());
        }
        
        scanner.close();
    }
}