const readline = require('readline'); // Konsoldan veri okumak için kütüphane

// =============================================================
// 1. BÖLÜM: ÖZEL HATA YÖNETİMİ
// =============================================================
// JS'de 'Error' sınıfından miras alıyoruz.
//
class KuantumCokusuError extends Error {
    constructor(nesneID) {
        super(`DİKKAT! ${nesneID} kimlikli nesne çöktü! Patlama gerçekleşti.`);
        this.name = "KuantumCokusuError";
    }
}

// =============================================================
// 2. BÖLÜM: TEMEL YAPI (ABSTRACT CLASS SİMÜLASYONU)
// =============================================================
// JS'de 'abstract' kelimesi yoktur. Bunu 'new.target' ile kontrol ederiz.
//
class KuantumNesnesi {
    constructor(id) {
        // Eğer birisi "new KuantumNesnesi()" derse hata veriyoruz.
        if (new.target === KuantumNesnesi) {
            throw new Error("KuantumNesnesi soyut bir sınıftır, doğrudan üretilemez!");
        }
        this.id = id;
        this.tehlikeSeviyesi = 0;
        this._stabilite = 100.0; // Gizli değişken (Convention olarak _ kullanılır)
    }

    // KAPSÜLLEME (ENCAPSULATION)
    //
    get stabilite() {
        return this._stabilite;
    }

    set stabilite(value) {
        if (value > 100) {
            this._stabilite = 100;
        } else if (value <= 0) {
            this._stabilite = 0;
            // Hata fırlatma
            //
            throw new KuantumCokusuError(this.id);
        } else {
            this._stabilite = value;
        }
    }

    // Soyut Metot Simülasyonu
    // Alt sınıflar bunu ezmezse (override) hata veririz.
    //
    analizEt() {
        throw new Error("analizEt() metodu alt sınıf tarafından uygulanmalıdır!");
    }

    // Durum Bilgisi
    //
    durumBilgisi() {
        // toFixed(2) virgülden sonra 2 basamak gösterir
        return `[${this.id}] Stabilite: %${this.stabilite.toFixed(2)} | Tehlike: ${this.tehlikeSeviyesi}`;
    }
}

// =============================================================
// 3. BÖLÜM: NESNE ÇEŞİTLERİ
// =============================================================

// A. Veri Paketi
//
class VeriPaketi extends KuantumNesnesi {
    constructor(id) {
        super(id);
        this.tehlikeSeviyesi = 1;
    }

    analizEt() {
        this.stabilite -= 5;
        console.log(`${this.id} içeriği okundu. (Stabilite -5)`);
    }
}

// B. Karanlık Madde
class KaranlikMadde extends KuantumNesnesi {
    constructor(id) {
        super(id);
        this.tehlikeSeviyesi = 5;
    }

    analizEt() {
        this.stabilite -= 15;
        console.log(`${this.id} analiz edildi. Karanlık enerji yayılıyor! (Stabilite -15)`);
    }

    // IKritik Arayüzü Metodu (JS'de interface olmadığı için direkt yazıyoruz)
    acilDurumSogutmasi() {
        this.stabilite += 50;
        console.log(`${this.id} soğutuldu. Stabilite yenilendi.`);
    }
}

// C. Anti Madde
class AntiMadde extends KuantumNesnesi {
    constructor(id) {
        super(id);
        this.tehlikeSeviyesi = 10;
    }

    analizEt() {
        console.log("UYARI: Evrenin dokusu titriyor...");
        this.stabilite -= 25;
        console.log(`${this.id} analiz edildi. (Stabilite -25)`);
    }

    // IKritik Arayüzü Metodu
    acilDurumSogutmasi() {
        this.stabilite += 50;
        console.log(`${this.id} soğutuldu. Kritik seviye düşürüldü.`);
    }
}

// =============================================================
// 4. BÖLÜM: OYNANIŞ DÖNGÜSÜ (MAIN LOOP)
// =============================================================

// Konsol okuma arayüzü
const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

const envanter = []; // Listemiz
let sayac = 1;

console.log("--- OMEGA SEKTÖRÜ KUANTUM VERİ AMBARI (JS) ---");

// Menüyü tekrar tekrar göstermek için fonksiyon
function menuGoster() {
    console.log("\n=== KUANTUM AMBARI KONTROL PANELİ ===");
    console.log("1. Yeni Nesne Ekle");
    console.log("2. Tüm Envanteri Listele");
    console.log("3. Nesneyi Analiz Et");
    console.log("4. Acil Durum Soğutması Yap");
    console.log("5. Çıkış");
    
    rl.question("Seçiminiz: ", (secim) => {
        try {
            islemYap(secim);
        } catch (error) {
            //
            if (error instanceof KuantumCokusuError) {
                console.log("\n**************************************");
                console.log("SİSTEM ÇÖKTÜ! TAHLİYE BAŞLATILIYOR...");
                console.log(`Sebep: ${error.message}`);
                console.log("**************************************");
                rl.close(); // Programı kapat
                process.exit(1);
            } else {
                console.log(`Beklenmedik hata: ${error.message}`);
                menuGoster(); // Menüye dön
            }
        }
    });
}

function islemYap(secim) {
    if (secim === "1") {
        const zar = Math.floor(Math.random() * 3) + 1; // 1-3 arası sayı
        const yeniId = `NESNE-${sayac++}`;

        if (zar === 1) envanter.push(new VeriPaketi(yeniId));
        else if (zar === 2) envanter.push(new KaranlikMadde(yeniId));
        else envanter.push(new AntiMadde(yeniId));

        console.log(`${yeniId} ambara kabul edildi.`);
        menuGoster();
    } 
    else if (secim === "2") {
        console.log("\n--- ENVANTER DURUMU ---");
        if (envanter.length === 0) console.log("Ambar boş.");
        
        envanter.forEach(nesne => {
            console.log(nesne.durumBilgisi());
        });
        menuGoster();
    } 
    else if (secim === "3") {
        rl.question("Analiz edilecek ID: ", (girilenId) => {
            try {
                const hedef = envanter.find(x => x.id === girilenId);
                if (hedef) {
                    hedef.analizEt();
                } else {
                    console.log("Nesne bulunamadı!");
                }
            } catch (error) {
                // Analiz sırasında patlama olursa hatayı yukarı fırlat
                if (error instanceof KuantumCokusuError) throw error; 
            }
            menuGoster();
        });
    } 
    else if (secim === "4") {
        rl.question("Soğutulacak ID: ", (girilenId) => {
            const hedef = envanter.find(x => x.id === girilenId);

            if (hedef) {
                // Type Checking (Duck Typing)
                // "Eğer acilDurumSogutmasi diye bir fonksiyonu varsa, bu kritiktir."
                //
                if (typeof hedef.acilDurumSogutmasi === 'function') {
                    hedef.acilDurumSogutmasi();
                } else {
                    console.log("Bu nesne soğutulamaz! (IKritik değil)");
                }
            } else {
                console.log("Nesne bulunamadı!");
            }
            menuGoster();
        });
    } 
    else if (secim === "5") {
        console.log("Çıkış yapılıyor...");
        rl.close();
    } 
    else {
        console.log("Geçersiz seçim.");
        menuGoster();
    }
}

// Başlat
menuGoster();