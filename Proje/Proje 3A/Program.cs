using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace Proje_3
{
    class TreeNode
    {
        public Sirket sirketNode;
        public TreeNode leftChild;
        public TreeNode rightChild;
        public void displayNode()
        { Console.Write(sirketNode.sirketAdi + " "); }
    }

    class Tree
    {
        private TreeNode root;
        public ArrayList dugumler = new ArrayList();
        public TreeNode[] dugumlerDizi;
        public Tree() { root = null; }
        public TreeNode getRoot()
        {
            return root;
        }

        public void preOrder(TreeNode localRoot)
        {
            if (localRoot != null)
            {
                localRoot.displayNode();
                preOrder(localRoot.leftChild);
                preOrder(localRoot.rightChild);
            }
        }

        public void inOrder(TreeNode localRoot)
        {
            if (localRoot != null)
            {
                inOrder(localRoot.leftChild);
                localRoot.displayNode();
                inOrder(localRoot.rightChild);
            }
        }

        public void ucreteGoreYazdir(TreeNode localRoot, int min, int max)
        {
            if (localRoot != null)                                         // inorder siralama kullanilarak istenilen degerler arasinda yer alan ucreti veren ilanlar bulunur.
            {
                ucreteGoreYazdir(localRoot.leftChild, min, max);
                foreach (Ilan i in localRoot.sirketNode.ilanlar)
                    if (i.ucret > min && i.ucret < max)
                    {
                        Console.WriteLine("Sirket Adi                : " + i.sirketAdi);
                        Console.WriteLine("Is Tanimi                 : " + i.isTanimi);
                        Console.WriteLine("Yetenekler ve Uzmanliklar : " + i.yetenek);
                        Console.WriteLine("Aday Tanimi               : " + i.adayTanimi);
                        Console.WriteLine("Pozisyon Bilgileri        : " + i.pozisyonBilgileri);
                        Console.WriteLine("Ucret                     : " + i.ucret);
                        Console.WriteLine("--------------------------");
                    }
                ucreteGoreYazdir(localRoot.rightChild, min, max);
            }
        }

        public void yetenekleriArrayListeEkle(TreeNode localRoot, ref ArrayList kelimeler)
        {                                                                         // inorder siralama ile ilanlardaki yetenek bilgileri sirayla ArrayList'e eklenir.
            if (localRoot != null)
            {
                yetenekleriArrayListeEkle(localRoot.leftChild, ref kelimeler);
                foreach (Ilan i in localRoot.sirketNode.ilanlar)
                {
                    char[] ayrac = { ' ' };
                    string[] kelimelerDizi = i.yetenek.Split(ayrac);
                    for (int j = 0; j < kelimelerDizi.Length - 2; j++)
                    {
                        if (!kelimeler.Contains(kelimelerDizi[j]))
                            kelimeler.Add(kelimelerDizi[j]);
                    }
                }
                yetenekleriArrayListeEkle(localRoot.rightChild, ref kelimeler);
            }
        }

        public void ilanlariArraylisteEkle(TreeNode localRoot, ref ArrayList ilanlar)
        {
            if (localRoot != null)                                             // inorder siralama ile ilanlar ArrayList'e eklenir.
            {
                ilanlariArraylisteEkle(localRoot.leftChild, ref ilanlar);
                foreach (Ilan i in localRoot.sirketNode.ilanlar)
                    ilanlar.Add(i);
                ilanlariArraylisteEkle(localRoot.rightChild, ref ilanlar);
            }
        }

        public void postOrder(TreeNode localRoot)
        {
            if (localRoot != null)
            {
                postOrder(localRoot.leftChild);
                postOrder(localRoot.rightChild);
                localRoot.displayNode();
            }
        }

        public void dugumleriDiziyeTasi(TreeNode localRoot)
        {
            int index = 0;
            if (localRoot == null)                                    // Agacta yer alan dugumleri diziye ekler.
                return;
            dugumleriDiziyeTasi(localRoot.leftChild);
            dugumler.Add(localRoot);
            dugumleriDiziyeTasi(localRoot.rightChild);
            dugumlerDizi = new TreeNode[dugumler.Count];
            foreach (TreeNode dugum in dugumler)
            {
                dugumlerDizi[index] = dugum;
                index++;
            }
        }

        public void balanceRecursive(int low, int high, TreeNode[] dugumDizisi)
        {
            if (low == high)
                return;
            int midpoint = (low + high) / 2;
            TreeNode eleman = (TreeNode)dugumDizisi[midpoint];                 // Agacin dengelenmesini saglar.
            insert(eleman.sirketNode);
            balanceRecursive(midpoint + 1, high, dugumDizisi);
            balanceRecursive(low, midpoint, dugumDizisi);
        }

        public void insert(Sirket yeniSirket)
        {
            TreeNode newNode = new TreeNode();
            newNode.sirketNode = yeniSirket;
            if (root == null)
                root = newNode;
            else
            {
                TreeNode current = root;
                TreeNode parent;
                while (true)
                {
                    parent = current;
                    if (string.Compare(yeniSirket.sirketAdi, current.sirketNode.sirketAdi, true) == -1)
                    {
                        current = current.leftChild;
                        if (current == null)
                        {
                            parent.leftChild = newNode;
                            return;
                        }
                    }
                    else
                    {
                        current = current.rightChild;
                        if (current == null)
                        {
                            parent.rightChild = newNode;
                            return;
                        }
                    }
                }
            }
        }

        public void düzeyListele(TreeNode etkin, int d)
        {
            if (etkin != null)
            {
                d = d + 1;
                düzeyListele(etkin.leftChild, d);
                Console.WriteLine(etkin.sirketNode.sirketAdi + " " + d + ". düzeyde");
                düzeyListele(etkin.rightChild, d);
            }
        }

        public int bul(TreeNode localRoot, string aranan)
        {
            if (localRoot == null)                                                   // Bir sirketin bulunup bulunmadigini geri dondurur.
                return -1;
            if (string.Compare(localRoot.sirketNode.sirketAdi, aranan, true) == 0)
                return 1;
            if (bul(localRoot.rightChild, aranan) == 1)
                return 1;
            if (bul(localRoot.leftChild, aranan) == 1)
                return 1;
            return -1;
        }

        public TreeNode bul2(TreeNode localRoot, string aranan)                          // Aranan sirketin oldugu dugum geri dondurulur.
        {
            if (localRoot == null)
                return null;
            if (string.Compare(localRoot.sirketNode.sirketAdi, aranan, true) == 0)
                return localRoot;
            if (bul(localRoot.rightChild, aranan) == 1)
                return localRoot.rightChild;
            if (bul(localRoot.leftChild, aranan) == 1)
                return localRoot.leftChild;
            return null;
        }

        public int dugumSayisiBul(TreeNode localRoot)
        {
            int h1 = 0, h2 = 0;

            if (localRoot == null)
                return 0;
            if (localRoot.leftChild != null)
                h1 = dugumSayisiBul(localRoot.leftChild);
            if (localRoot.rightChild != null)
                h2 = dugumSayisiBul(localRoot.rightChild);
            return h1 + h2 + 1;
        }
    }

    class Heap
    {
        const int MAX = 10;
        private Ilan[] veri = new Ilan[MAX];
        private int ES;

        public Heap()
        {
            ES = 0;
        }

        public int solCocukGetir(int i)
        {
            return 2 * i + 1;
        }

        public int sagCocukGetir(int i)
        {
            return 2 * i + 2;
        }

        public int ebeveynGetir(int i)
        {
            return (i - 1) / 2;
        }

        public bool getir(ref Ilan e)
        {
            if (ES == 0)
                return false;
            e = veri[0];
            veri[0] = veri[ES - 1];
            ES--;
            heapifyDown(0);
            return true;
        }

        public bool heapOlustur(Ilan[] a, int es)
        {
            if (ES > MAX)
                return false;
            this.ES = es;
            for (int i = 0; i < es; i++)
            {
                veri[i] = a[i];
            }
            int k = (es - 1) / 2;
            for (int i = k; i >= 0; i--)
            {
                heapifyDown(i);
            }
            return true;
        }

        private void heapifyUp(int i)
        {
            if (i == 0)
                return;
            int eb = ebeveynGetir(i);
            if (veri[eb].ucret < veri[i].ucret)
            {
                Ilan temp = veri[i];
                veri[i] = veri[eb];
                veri[eb] = temp;
                heapifyUp(eb);
            }
        }
        private void heapifyDown(int i)
        {
            int Sol = solCocukGetir(i);
            int Sag = sagCocukGetir(i);
            int min;
            if (Sag >= ES)
            {
                if (Sol >= ES)
                    return;
                else min = Sol;
            }
            else
            {
                if (veri[Sol].ucret > veri[Sag].ucret)
                    min = Sol;
                else
                    min = Sag;
            }
            if (veri[min].ucret > veri[i].ucret)
            {
                Ilan temp = veri[i];
                veri[i] = veri[min];
                veri[min] = temp;
                heapifyDown(min);
            }
        }
    }

    class Kategori
    {
        public string kategoriAdi;
        public Tree agac;
        public Kategori(string ad)
        {
            kategoriAdi = ad;
            agac = new Tree();
        }
    }

    class Sirket
    {
        public string sirketAdi;
        public ArrayList ilanlar;
        public Sirket()
        {
            ilanlar = new ArrayList();
        }
    }

    class Ilan
    {
        public string sirketAdi;
        public string isTanimi;
        public string yetenek;
        public string adayTanimi;
        public string pozisyonBilgileri;
        public int ucret;
        public ArrayList basvuruListesi;
        public Ilan()
        {
            basvuruListesi = new ArrayList();
        }
    }

    class Aday
    {
        public string adSoyad;
        public int yas;
        public string okul;
        public string yetenek;
    }

    class Program
    {
        static void Main(string[] args)
        {
            ArrayList kategoriler = new ArrayList(); int secim, secim1, secim2; Heap maasYigin = new Heap(); Hashtable hashTablosu = new Hashtable();
            baslangicKategoriOlustur(kategoriler); char cevap;                                         // Once hazir veriler dosyadan okunur.
            do
            {
                secim = menuGoruntule();                                                                // Aday ve personel icin program iki module ayrilir.
                switch (secim)
                {
                    case 1:
                        secim1 = personelAltMenuGoruntule();
                        switch (secim1)
                        {
                            case 1:
                                yeniKategoriEkle(kategoriler);
                                break;
                            case 2:
                                yeniIlanEkle(kategoriler);
                                break;
                            case 3:
                                belirliBirSirketinBelirliKategoridekiIlanlariniGoruntuleVeSil(kategoriler);
                                break;
                            case 4:
                                ilanBilgilerindeDegisiklikYapma(kategoriler);
                                break;
                            case 5:
                                adayIseAl(kategoriler);
                                break;
                            case 6:
                                oneriSistemi(kategoriler);
                                break;
                        }
                        break;
                    case 2:
                        secim2 = adayAltMenuGoruntule();
                        switch (secim2)
                        {
                            case 1:
                                birSirketinTumIlanlariniListele(kategoriler);
                                break;
                            case 2:
                                belirtilenUcretlerArasindakTumSirketleriListele(kategoriler);
                                break;
                            case 3:
                                Console.WriteLine(toplamIlanSayisiBul(kategoriler));
                                break;
                            case 4:
                                agacListeleme(kategoriler);
                                break;
                            case 5:
                                isIlaninaBasvuruYap(kategoriler);
                                break;
                            case 6:
                                hashTableOlustur(kategoriler, hashTablosu);
                                break;
                            case 7:
                                maasHeapOlustur(kategoriler, maasYigin);
                                break;
                        }
                        break;
                }
                Console.WriteLine("\nYeni bir islem yapmak ister misiniz? (E/e/H/h)  ");
                cevap = cevapAl();
            } while (cevap == 'E' || cevap == 'e');
            Console.ReadKey();
        }

        static int menuGoruntule()
        {
            int secim;
            Console.WriteLine("Personel Girisi      (1)");
            Console.WriteLine("Aday Girisi          (2)\n");
            do
            {
                Console.Write("Lutfen seciminizi yapiniz: ");
                secim = int.Parse(Console.ReadLine());
            } while (secim < 1 || secim > 2);
            Console.WriteLine("------------------------------------------------");
            return secim;
        }

        static char cevapAl()
        {
            char c;
            c = Console.ReadKey().KeyChar;                                                      // Tekrar islem yapilmak istenip istenmedigini sorar.
            Console.WriteLine();
            while (c != 'E' && c != 'e' && c != 'H' && c != 'h')
            {
                Console.Write("Lutfen gecerli bir deger giriniz: ");
                c = Console.ReadKey().KeyChar;
                Console.WriteLine();
            }
            Console.WriteLine();
            return c;
        }

        static int personelAltMenuGoruntule()
        {
            int secim;
            Console.WriteLine("\n1. Yeni Kategori Eklenmesi");
            Console.WriteLine("2. Yeni Ilan Girisi");
            Console.WriteLine("3. Sirket Ilanlarini Goruntuleme Ve Silme");
            Console.WriteLine("4. Ilan Bilgilerinde Degisiklik Yapma");
            Console.WriteLine("5. Aday Ise Alma");
            Console.WriteLine("6. Oneri Sistemi");
            do
            {
                Console.Write("\nLutfen seciminizi yapiniz: ");
                secim = int.Parse(Console.ReadLine());
            } while (secim < 1 || secim > 6);
            Console.WriteLine("\n------------------------------------------------");
            return secim;
        }

        static int adayAltMenuGoruntule()
        {
            int secim;
            Console.WriteLine("1. Adi Verilen Bir Sirketin Tum Ilanlarini Listeleme");
            Console.WriteLine("2. Belirtilen Ucretler Arasindaki Ilanlari Listeleme");
            Console.WriteLine("3. Kategorilerdeki Toplam Ilan Sayilarinin Belirtilmesi");
            Console.WriteLine("4. Bir Kategorideki Tum Ilanlari Listeleme (Inorder, Preorder, Postorder), Derinlik Ve Eleman Sayisi Yazdirma");
            Console.WriteLine("5. Bir Is Ilanina Basvuru Yapilmasi");
            Console.WriteLine("6. Verilen Bır Kelimenin Gectigi Is Ilanlarina Hash Table Ile Erisilmesi");
            Console.WriteLine("7. Heap Olusturarak Bir Kategorideki En Yuksek Maasli N Adet Ilanin Listelenmesi");
            {
                Console.Write("Lutfen seciminizi yapiniz: ");
                secim = int.Parse(Console.ReadLine());
            } while (secim < 1 || secim > 7) ;
            Console.WriteLine("------------------------------------------------");
            return secim;
        }
        private static void baslangicKategoriOlustur(ArrayList kategorilerArrayList)
        {
            StreamReader dosyaOku; Sirket yeniSirket; Ilan yeniIlan; TreeNode node = new TreeNode();
            string yazi, kategoriAdi, sirketAdi; int ilanSayisi = 6;
            string[] baslangictakiKategoriler = { "Yazilim Muhendisi", "Ag Yoneticisi", "Veritabani Yoneticisi", "Mobil Yazilim Gelistirme Uzmani", "Backend Developer", "Web Developer" };

            for (int i = 0; i < baslangictakiKategoriler.Length; i++)
            {
                Kategori kategori = new Kategori(baslangictakiKategoriler[i]);
                kategorilerArrayList.Add(kategori);
            }
            dosyaOku = File.OpenText("C:/Users/Pc/Desktop/Veriler.dat");
            yazi = dosyaOku.ReadLine();
            for (int i = 1; i <= ilanSayisi; i++)
            {
                yeniSirket = new Sirket(); yeniIlan = new Ilan();                       // Programda ornek olarak kullanilacak olan veriler dosyadan okunur.
                kategoriAdi = yazi; yazi = dosyaOku.ReadLine();
                yeniSirket.sirketAdi = yazi; yeniIlan.sirketAdi = yazi; sirketAdi = yazi; yazi = dosyaOku.ReadLine();
                yeniIlan.isTanimi = yazi; yazi = dosyaOku.ReadLine();
                yeniIlan.yetenek = yazi; yeniIlan.yetenek = yeniIlan.yetenek.Insert(yeniIlan.yetenek.Length, " "); yazi = dosyaOku.ReadLine();
                yeniIlan.adayTanimi = yazi; yazi = dosyaOku.ReadLine();
                yeniIlan.pozisyonBilgileri = yazi; yazi = dosyaOku.ReadLine();
                yeniIlan.ucret = Convert.ToInt32(yazi); yazi = dosyaOku.ReadLine();
                if (i != 6)
                    yeniSirket.ilanlar.Add(yeniIlan);
                foreach (Kategori o in kategorilerArrayList)
                    if (string.Compare(kategoriAdi, o.kategoriAdi, true) == 0)
                        if (i != 6)
                            o.agac.insert(yeniSirket);
                if (i == 6)
                    ((Kategori)kategorilerArrayList[0]).agac.bul2(((Kategori)kategorilerArrayList[0]).agac.getRoot(), sirketAdi).sirketNode.ilanlar.Add(yeniIlan);
            }
            dosyaOku.Close();
        }

        static string kategoriSec(ArrayList kategorilerArrayList)                                         
        {
            ArrayList kategori = new ArrayList(); int secim, i = 1;
            Console.WriteLine();
            foreach (Kategori k in kategorilerArrayList)
            {
                Console.Write(i++ + ". "); Console.WriteLine(k.kategoriAdi);
                kategori.Add(k.kategoriAdi);
            }
            secim = int.Parse(Console.ReadLine());
            return ((string)kategori[secim - 1]);
        }
    
        private static void yeniKategoriEkle(ArrayList kategorilerArrayList)
        {
            Kategori yeniKategori;
            Console.WriteLine("\nLutfen eklemek istediginiz kategoriyi giriniz: ");
            yeniKategori = new Kategori(Console.ReadLine());
            kategorilerArrayList.Add(yeniKategori);
            Console.WriteLine("\nYeni kategori eklenmistir.");
        }

        private static void yeniIlanEkle(ArrayList kategorilerArrayList)
        {
            string sirketAdi, kategoriAdi; Ilan yeniIlan = new Ilan();

            Console.WriteLine("\nLutfen ilan kategorisini giriniz:");
            kategoriAdi = kategoriSec(kategorilerArrayList);
            Console.WriteLine("Lutfen sirket adi giriniz:");
            sirketAdi = Console.ReadLine(); yeniIlan.sirketAdi = sirketAdi;
            Console.WriteLine("Lutfen is tanimini giriniz:");
            yeniIlan.isTanimi = Console.ReadLine();
            Console.WriteLine("Lutfen yetenek ve uzmanliklari giriniz:");
            yeniIlan.yetenek = Console.ReadLine();
            Console.WriteLine("Lutfen aday tanimini giriniz:");
            yeniIlan.adayTanimi = Console.ReadLine();
            Console.WriteLine("Lutfen pozisyon bilgilerini giriniz:");
            yeniIlan.pozisyonBilgileri = Console.ReadLine();
            Console.WriteLine("Lutfen ucreti giriniz:");                                             // Ilani veren sirket daha onceden ilan verdiyse ayni sirkete ilan bilgileri eklenir, yoksa yeni bir dugum olusturulur.
            yeniIlan.ucret = int.Parse(Console.ReadLine());
            foreach (Kategori o in kategorilerArrayList)
                if (string.Compare(kategoriAdi, o.kategoriAdi, true) == 0)
                {
                    if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                        o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar.Add(yeniIlan);
                    else if (o.agac.bul(o.agac.getRoot(), sirketAdi) != 1)
                    {
                        Sirket yeniSirket = new Sirket();
                        yeniSirket.sirketAdi = sirketAdi;
                        yeniSirket.ilanlar.Add(yeniIlan);
                        o.agac.insert(yeniSirket);
                    }
                }
            for (int i = 0; i < kategorilerArrayList.Count; i++)
            {
                ((Kategori)kategorilerArrayList[i]).agac.dugumleriDiziyeTasi(((Kategori)kategorilerArrayList[i]).agac.getRoot());
                TreeNode[] yeniDugumler = ((Kategori)kategorilerArrayList[i]).agac.dugumlerDizi;
                if (((Kategori)kategorilerArrayList[i]).agac.dugumSayisiBul(((Kategori)kategorilerArrayList[i]).agac.getRoot()) == 0)
                    continue;
                Tree yeniAgac = new Tree();
                yeniAgac.balanceRecursive(0, ((Kategori)kategorilerArrayList[i]).agac.dugumlerDizi.Length, yeniDugumler);
                ((Kategori)kategorilerArrayList[i]).agac = yeniAgac;
            }
            Console.WriteLine("\nIlan eklendi.");
        }

        private static void belirliBirSirketinBelirliKategoridekiIlanlariniGoruntuleVeSil(ArrayList kategorilerArrayList)
        {
            string kategoriAdi, sirketAdi; int counter = 1, silinecekIlanNo;
            Console.WriteLine("\nLutfen tum ilanlarini goruntuleyip istediginiz ilanini silmek istediginiz sirketin bulundugu kategorinin adini giriniz: ");
            kategoriAdi = kategoriSec(kategorilerArrayList); 
            Console.Write("\nLutfen sirketin adiniz giriniz: ");
            sirketAdi = Console.ReadLine(); Console.WriteLine();
            foreach (Kategori o in kategorilerArrayList)
                if (string.Compare(kategoriAdi, o.kategoriAdi, true) == 0)
                {
                    if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                    {
                        foreach (Ilan i in o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar)
                        {
                            Console.WriteLine(counter++ + ".");
                            Console.WriteLine("Is Tanimi                 : " + i.isTanimi);                                 
                            Console.WriteLine("Yetenekler ve Uzmanliklar : " + i.yetenek);
                            Console.WriteLine("Aday Tanimi               : " + i.adayTanimi);
                            Console.WriteLine("Pozisyon Bilgileri        : " + i.pozisyonBilgileri);
                            Console.WriteLine("Ucret                     : " + i.ucret);
                            Console.WriteLine("--------------------------");
                        }
                        if (o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar.Count != 0)
                        {
                            Console.Write("Lutfen silmek istediginiz ilanin numarasini giriniz: ");
                            silinecekIlanNo = int.Parse(Console.ReadLine());
                            o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar.RemoveAt(silinecekIlanNo - 1);
                            Console.WriteLine("Ilan silindi.");
                        }
                    }
                }
        }

        static int belirliBirSirketinIlanlariniGoruntule(ArrayList kategorilerArrayList, string sirketAdi)
        {
            int counter = 1, sirketeAitIlanSayisi = 0;

            foreach (Kategori o in kategorilerArrayList)
                if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                {
                    foreach (Ilan i in o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar)
                    {
                        Console.WriteLine(counter++ + ".");
                        Console.WriteLine("Is Tanimi                 : " + i.isTanimi);                                 
                        Console.WriteLine("Yetenekler ve Uzmanliklar : " + i.yetenek);
                        Console.WriteLine("Aday Tanimi               : " + i.adayTanimi);
                        Console.WriteLine("Pozisyon Bilgileri        : " + i.pozisyonBilgileri);
                        Console.WriteLine("Ucret                     : " + i.ucret);
                        sirketeAitIlanSayisi++;
                    }
                }
            return sirketeAitIlanSayisi;
        }

        private static void ilanBilgilerindeDegisiklikYapma(ArrayList kategorilerArrayList)
        {
            string kategoriAdi, sirketAdi; int sirketeAitIlanSayisi, ilanNo, degistirilecekVeriNo;

            Console.WriteLine("\nLutfen degisiklik yapmak istediginiz ilanin bulundugu kategoriyi giriniz:");  
            kategoriAdi = kategoriSec(kategorilerArrayList); ;
            Console.WriteLine("\nLutfen degisiklik yapmak istediginiz ilani veren sirketin adini giriniz:");
            sirketAdi = Console.ReadLine();
            sirketeAitIlanSayisi = belirliBirSirketinIlanlariniGoruntule(kategorilerArrayList, sirketAdi);
            if (sirketeAitIlanSayisi > 1)
            {
                Console.WriteLine("Lutfen degisiklik yapmak istediginiz ilanin numarasini giriniz: ");
                ilanNo = int.Parse(Console.ReadLine());
            }
            else ilanNo = 1;
            foreach (Kategori o in kategorilerArrayList)                                        
                if (string.Compare(kategoriAdi, o.kategoriAdi, true) == 0)
                {
                    if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                    {
                        Console.WriteLine("Hangi sahada degisiklik yapmak istersiniz? Numarasini giriniz. (Sirket Adi (1)/Is Tanimi (2)/Yetenek ve Uzmanliklar (3)/Aday Tanimi (4)/Pozisyon Bilgileri (5)/Ucret (6))");
                        degistirilecekVeriNo = int.Parse(Console.ReadLine());
                        if (degistirilecekVeriNo == 1)
                        {
                            Console.Write("Sirket Adi: ");
                            ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).sirketAdi = Console.ReadLine();
                        }
                        if (degistirilecekVeriNo == 2)
                        {
                            Console.Write("Is Tanimi: ");
                            ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).isTanimi = Console.ReadLine();
                        }
                        if (degistirilecekVeriNo == 3)
                        {
                            Console.Write("Yetenek ve Uzmanliklar: ");
                            ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).yetenek = Console.ReadLine();
                        }
                        if (degistirilecekVeriNo == 4)
                        {
                            Console.Write("Aday Tanimi: ");
                            ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).adayTanimi = Console.ReadLine();
                        }
                        if (degistirilecekVeriNo == 5)
                        {
                            Console.Write("Pozisyon Bilgileri: ");
                            ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).pozisyonBilgileri = Console.ReadLine();
                        }
                        if (degistirilecekVeriNo == 6)
                        {
                            Console.Write("Ucret: ");
                            ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).ucret = int.Parse(Console.ReadLine());
                        }
                    }
                }
            Console.WriteLine("Degisiklikler kaydedildi.");
        }

        private static void adayIseAl(ArrayList kategorilerArrayList)
        {
            string kategoriAdi, sirketAdi; int sirketeAitIlanSayisi, ilanNo;

            Console.WriteLine("\nHangi kategoride aday ise almak istiyorsunuz?");                         
            kategoriAdi = kategoriSec(kategorilerArrayList); 
            Console.WriteLine("Hangi sirkete ait ilan icin aday ise almak istiyorsunuz?");
            sirketAdi = Console.ReadLine();
            sirketeAitIlanSayisi = belirliBirSirketinIlanlariniGoruntule(kategorilerArrayList, sirketAdi);
            if (sirketeAitIlanSayisi > 1)
            {
                Console.WriteLine("Aday ise almak istediginiz ilanin numarasini giriniz: ");
                ilanNo = int.Parse(Console.ReadLine());
            }
            else ilanNo = 1;
            foreach (Kategori o in kategorilerArrayList)
                if (string.Compare(kategoriAdi, o.kategoriAdi, true) == 0)
                {
                    if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                    {
                        Console.WriteLine(((Aday)(((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).basvuruListesi[0])).adSoyad + " ise alindi.");
                        ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).basvuruListesi.RemoveAt(0);
                    }
                }
        }

        private static void birSirketinTumIlanlariniListele(ArrayList kategorilerArrayList)
        {
            string sirketAdi; int counter = 1;
            Console.Write("\nLutfen tum ilanlarini goruntulemek istediginiz sirketin adiniz giriniz: ");
            sirketAdi = Console.ReadLine(); Console.WriteLine();
            foreach (Kategori o in kategorilerArrayList)
                if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                {
                    foreach (Ilan i in o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar)
                    {
                        Console.WriteLine(counter++ + ".");
                        Console.WriteLine("Is Tanimi                 : " + i.isTanimi);                                
                        Console.WriteLine("Yetenekler ve Uzmanliklar : " + i.yetenek);
                        Console.WriteLine("Aday Tanimi               : " + i.adayTanimi);
                        Console.WriteLine("Pozisyon Bilgileri        : " + i.pozisyonBilgileri);
                        Console.WriteLine("Ucret                     : " + i.ucret);
                        Console.WriteLine("--------------------------");
                    }
                }
        }

        private static void belirtilenUcretlerArasindakTumSirketleriListele(ArrayList kategorilerArrayList)
        {
            int minUcret, maxUcret;
            Console.Write("Deger araligini giriniz:\nMin ucret: ");            
            minUcret = int.Parse(Console.ReadLine());
            Console.Write("Max ucret: ");
            maxUcret = int.Parse(Console.ReadLine());
            Console.WriteLine();
            foreach (Kategori o in kategorilerArrayList)
            {
                o.agac.ucreteGoreYazdir(o.agac.getRoot(), minUcret, maxUcret);
            }
        }

        static int toplamIlanSayisiBul(ArrayList kategorilerArrayList)
        {
            int ilanSayisi = 0;                                                   
            foreach (Kategori k in kategorilerArrayList)
            {
                ilanSayisi += k.agac.dugumSayisiBul(k.agac.getRoot());
            }
            return ilanSayisi;
        }

        private static void agacListeleme(ArrayList kategorilerArrayList)
        {
            string kategoriAdi;
            Console.WriteLine("Lutfen kategori adini giriniz:");
            kategoriAdi = kategoriSec(kategorilerArrayList); ;
            foreach (Kategori k in kategorilerArrayList)
            {
                if (string.Compare(kategoriAdi, k.kategoriAdi, true) == 0)
                {
                    k.agac.preOrder(k.agac.getRoot());
                    Console.WriteLine("\n");                                   
                    Console.WriteLine("Inorder:");
                    k.agac.inOrder(k.agac.getRoot());
                    Console.WriteLine("\n");
                    Console.WriteLine("Postorder:");
                    k.agac.postOrder(k.agac.getRoot());
                    Console.WriteLine("\n");
                    k.agac.düzeyListele(k.agac.getRoot(), 0);
                    Console.WriteLine();
                    Console.WriteLine("Ilan sayisi: " + k.agac.dugumSayisiBul(k.agac.getRoot()));
                    Console.WriteLine("*************************************");
                }
            }
        }

        private static void isIlaninaBasvuruYap(ArrayList kategorilerArrayList)
        {
            string kategoriAdi, sirketAdi; int sirketeAitIlanSayisi, ilanNo; Aday yeniAday = new Aday();

            Console.WriteLine("\nBasvuru yapmak istediginiz ilanin kategorisini giriniz:");                
            kategoriAdi = kategoriSec(kategorilerArrayList); ;
            Console.WriteLine("Lutfen basvuru yapmak istediginiz ilani veren sirketin adini giriniz:");
            sirketAdi = Console.ReadLine();
            Console.WriteLine("Lutfen adinizi ve soyadinizi giriniz:");
            yeniAday.adSoyad = Console.ReadLine();
            Console.WriteLine("Lutfen yasinizi giriniz:");
            yeniAday.yas = int.Parse(Console.ReadLine());
            Console.WriteLine("Lutfen mezun oldugunuz okulu giriniz:");
            yeniAday.okul = Console.ReadLine();
            Console.WriteLine("Lutfen yeteneklerinizi ve uzmanliklarinizi giriniz:");
            yeniAday.yetenek = Console.ReadLine();
            sirketeAitIlanSayisi = belirliBirSirketinIlanlariniGoruntule(kategorilerArrayList, sirketAdi);
            if (sirketeAitIlanSayisi > 1)
            {
                Console.WriteLine("Lutfen basvuru yapmak istediginiz ilanin numarasini giriniz: ");
                ilanNo = int.Parse(Console.ReadLine());
            }
            else ilanNo = 1;
            foreach (Kategori o in kategorilerArrayList)
                if (string.Compare(kategoriAdi, o.kategoriAdi, true) == 0)
                {
                    if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                    {
                        ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).basvuruListesi.Add(yeniAday);
                    }
                }
            Console.WriteLine("Basvuru gerceklesti.");
        }

        private static void hashTableOlustur(ArrayList kategorilerArrayList, Hashtable hashTablosu)
        {
            ArrayList kelimelerArrayList = new ArrayList(); ArrayList kelimelerSonHalArrayList = new ArrayList();
            ArrayList ilanlarArrayList = new ArrayList(); string kelime;
            foreach (Kategori o in kategorilerArrayList)
            {
                o.agac.yetenekleriArrayListeEkle(o.agac.getRoot(), ref kelimelerArrayList);
            }
            foreach (string s in kelimelerArrayList)
                if (!kelimelerSonHalArrayList.Contains(s))
                    kelimelerSonHalArrayList.Add(s);
            string[] kelimelerDizi = new string[kelimelerSonHalArrayList.Count];
            for (int i = 0; i < kelimelerDizi.Length; i++)
            {
                kelimelerDizi[i] = ((string)kelimelerSonHalArrayList[i]);                     // Yetenekler bir diziye alinir.
            }
            foreach (Kategori o in kategorilerArrayList)
                o.agac.ilanlariArraylisteEkle(o.agac.getRoot(), ref ilanlarArrayList);               
            for (int j = 0; j < kelimelerDizi.Length; j++)
            {
                ArrayList hashIlanlar = new ArrayList();
                string aranacakKelime = kelimelerDizi[j];
                string aranacakKelime1 = aranacakKelime.Insert(aranacakKelime.Length, " ");
                foreach (Ilan i in ilanlarArrayList)
                {
                    if (i.yetenek.IndexOf(aranacakKelime1) != -1)                                  // Is ilanindaki bir yetenegin bulundugu ilanlar ArrayList'e eklenir.
                    {
                        hashIlanlar.Add(i);
                    }
                }
                hashTablosu.Add(kelimelerDizi[j], hashIlanlar);
            }
            Console.Write("Lutfen aratmak istediginiz kelimeyi giriniz:  ");                            
            kelime = Console.ReadLine();
            foreach (Ilan i in ((ArrayList)hashTablosu[kelime]))
            {
                Console.WriteLine("Sirket Adi                : " + i.sirketAdi);
                Console.WriteLine("Is Tanimi                 : " + i.isTanimi);
                Console.WriteLine("Yetenekler ve Uzmanliklar : " + i.yetenek);
                Console.WriteLine("Aday Tanimi               : " + i.adayTanimi);
                Console.WriteLine("Pozisyon Bilgileri        : " + i.pozisyonBilgileri);
                Console.WriteLine("Ucret                     : " + i.ucret);
                Console.WriteLine("--------------------------");
            }
        }

        private static void maasHeapOlustur(ArrayList kategorilerArrayList, Heap maasYigin)
        {
            ArrayList ilanlar = new ArrayList(); int listelenecekIlanSayisi;
            foreach (Kategori o in kategorilerArrayList)
                o.agac.ilanlariArraylisteEkle(o.agac.getRoot(), ref ilanlar);
            Ilan[] ilanlarDizi = new Ilan[ilanlar.Count];
            for (int i = 0; i < ilanlarDizi.Length; i++)
                ilanlarDizi[i] = ((Ilan)ilanlar[i]);
            maasYigin.heapOlustur(ilanlarDizi, ilanlarDizi.Length);
            Console.WriteLine("Lutfen N degerini giriniz:");
            listelenecekIlanSayisi = int.Parse(Console.ReadLine());
            for (int i = 0; i < listelenecekIlanSayisi; i++)
            {
                Ilan temp = new Ilan();
                maasYigin.getir(ref temp);
                Console.WriteLine("Sirket Adi                : " + temp.sirketAdi);
                Console.WriteLine("Is Tanimi                 : " + temp.isTanimi);
                Console.WriteLine("Yetenekler ve Uzmanliklar : " + temp.yetenek);
                Console.WriteLine("Aday Tanimi               : " + temp.adayTanimi);
                Console.WriteLine("Pozisyon Bilgileri        : " + temp.pozisyonBilgileri);
                Console.WriteLine("Ucret                     : " + temp.ucret);
                Console.WriteLine("--------------------------");
            }
        }

        public static void oneriSistemi(ArrayList kategorilerArrayList)
        {
            string sirketAdi, kategoriAdi; int sirketeAitIlanSayisi, ilanNo; ArrayList kelimeler = new ArrayList(); string[] kelimelerDizi;
            ArrayList uygunAdaylar = new ArrayList();
            Console.WriteLine("\nLutfen kategori adini giriniz:");               
            kategoriAdi = kategoriSec(kategorilerArrayList);
            Console.WriteLine("Lutfen sirket adini giriniz:");
            sirketAdi = Console.ReadLine();
            sirketeAitIlanSayisi = belirliBirSirketinIlanlariniGoruntule(kategorilerArrayList, sirketAdi);
            if (sirketeAitIlanSayisi > 1)
            {
                Console.WriteLine("Lutfen aday onerilerini gormek istediginiz ilanin numarasini giriniz: ");
                ilanNo = int.Parse(Console.ReadLine());
            }
            else ilanNo = 1;
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine("\nBu ilan icin aday önerileri:\n");
            foreach (Kategori o in kategorilerArrayList)
                if (string.Compare(kategoriAdi, o.kategoriAdi, true) == 0)
                {
                    if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                    {
                        char[] ayrac = { ' ' };
                        string[] kelimelerDizisi = ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).yetenek.Split(ayrac);
                        for (int j = 0; j < kelimelerDizisi.Length - 2; j++)
                        {
                            if (!kelimeler.Contains(kelimelerDizisi[j]))                 
                                kelimeler.Add(kelimelerDizisi[j]);
                        }
                    }
                }
            kelimelerDizi = new string[kelimeler.Count];                                     
            for (int i = 0; i < kelimelerDizi.Length; i++)
                kelimelerDizi[i] = ((string)kelimeler[i]);                                   // Ilandaki yetenek bilgileri diziye alindi.
            for (int j = 0; j < kelimelerDizi.Length; j++)
            {
                string aranacakKelime = kelimelerDizi[j];
                foreach (Kategori o in kategorilerArrayList)
                    if (string.Compare(kategoriAdi, o.kategoriAdi, true) == 0)
                    {
                        if (o.agac.bul(o.agac.getRoot(), sirketAdi) == 1)
                        {
                            foreach (Aday a in ((Ilan)o.agac.bul2(o.agac.getRoot(), sirketAdi).sirketNode.ilanlar[ilanNo - 1]).basvuruListesi)
                            {
                                if (a.yetenek.IndexOf(aranacakKelime) != -1)
                                    uygunAdaylar.Add(a);                                          // Ilandaki bir yetenek bir adayda varsa bu aday onerilmek uzere ArrayList'te tutuluyor.
                            }
                        }
                    }
            }
            foreach (Aday a in uygunAdaylar)
            {
                Console.WriteLine("Ad soyad               :" + a.adSoyad);
                Console.WriteLine("Yas                    :" + a.yas);
                Console.WriteLine("Mezun olunan okul      :" + a.okul);
                Console.WriteLine("Yetenek ve uzmanliklar :" + a.yetenek);
                Console.WriteLine("------------------------------------------------");
            }
        }
    }
}

