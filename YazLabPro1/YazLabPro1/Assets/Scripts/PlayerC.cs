using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerC : MonoBehaviour, PlayerInterface
{
    public int ToplamAdimSayisi = 0;
    public int ToplananAltinSayisi = 0;
    public int ToplamHarcananAltinsayisi = 0;

    public int Gold;

    public int HedefBelirlemeMaliyeti;

    public int HamleMaliyeti;

    public int MatrisKonumZ;

    public int MatrisKonumX;

    public int HedefKonumZ;

    public int HedefKonumX;

    public int enYakinKonumZ = 1000;

    public int enYakinKonumX = 1000;

    public int HamleSayisi = 0;

    public int GüncelHamle = 0;

    public int TümAltinSayisi;

    public int maliyet;

    public int enazmaliyet=10000;

    public int enazmaliyetZ, enazmaliyetX;

    public int mesafe;

    public int enYakinIndex;
    int i, j;
    int toplam;

    public GameObject Board;
    public List<GizliAltinBilgileri> EnYakinGizliAltinListe = new List<GizliAltinBilgileri>();

    string DosyaYolu;
    FileStream fs;
    StreamWriter sw;
    private void Awake()
    {

        
        Board = GameObject.FindGameObjectWithTag("Board");//Starta koyunca olmuyor
        HedefKonumX = -10;//En başta hedefi yok
        HedefKonumZ = -10;//En başta hedefi yok
        Gold = PlayerPrefs.GetInt("C_Gold");
        HedefBelirlemeMaliyeti = PlayerPrefs.GetInt("C_HBM");
        HamleMaliyeti = PlayerPrefs.GetInt("C_HM");
        HamleSayisi = PlayerPrefs.GetInt("_HS");
        enYakinIndex = 0;
        DosyaYolu = "OyuncuCFile.txt";

        fs = new FileStream(DosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
        sw = new StreamWriter(fs);

        sw.Flush();
        sw.Close();
        fs.Close();
    }


    public void HamleYap()
    {
        if (Gold <= 0)
        {
            Board.GetComponent<CreateGameBoard>().OyuncuCElendiMi = true;
            MatrisKonumZ = 1000;
            MatrisKonumX = 1000;
            this.gameObject.transform.position = new Vector3(Board.GetComponent<CreateGameBoard>().KareSayisiX + 1f, 1f, Board.GetComponent<CreateGameBoard>().KareSayisiZ-1f);
        }
        else
        {
            //Debug.Log("Hamle Yapin içi");
            GüncelHamle = 0;
            HedefeUlasildimi();



            //Debug.Log("MatrisKonumZ:" + MatrisKonumZ + " MatrisKonumX:" + MatrisKonumX);
            //Debug.Log("HedefKonumZ:" + HedefKonumZ + " HedefKonumX:" + HedefKonumX);
            //Debug.Log("GüncelHamle:" + GüncelHamle);
            while ((MatrisKonumZ != HedefKonumZ || MatrisKonumX != HedefKonumX) && GüncelHamle < HamleSayisi) //Aradaki işareti hem z hem x i farklı olsun yapmışız veya olacak..!
            {
                //Debug.Log("While ın içi");
                if (MatrisKonumX < HedefKonumX && GüncelHamle < HamleSayisi)
                {
                    SagaGit();
                }
                else if (MatrisKonumX > HedefKonumX && GüncelHamle < HamleSayisi)
                {
                    SolaGit();
                }
                if (MatrisKonumZ < HedefKonumZ && GüncelHamle < HamleSayisi)
                {
                    YukariGit();
                }
                else if (MatrisKonumZ > HedefKonumZ && GüncelHamle < HamleSayisi)
                {
                    AsagiGit();
                }
            }
            Gold -= HamleMaliyeti;
            ToplamHarcananAltinsayisi += HamleMaliyeti;
            //Debug.Log("While bitti");
        }
        if (Gold <= 0)
        {
            Board.GetComponent<CreateGameBoard>().OyuncuCElendiMi = true;
            MatrisKonumZ = 1000;
            MatrisKonumX = 1000;
            this.gameObject.transform.position = new Vector3(Board.GetComponent<CreateGameBoard>().KareSayisiX + 1f, 1f, Board.GetComponent<CreateGameBoard>().KareSayisiZ-1f);
        }


    }
    public void GizliAltinlariTespitEt(int[,] GizliAltinMatris, int AltinSayisii, int Z, int X)// en yakin 2 gizli altini tespit eder
    {
       
        for (int i = 0; i < Z; i++)
        {
            for (int j = 0; j < X; j++)
            {
                if (GizliAltinMatris[i, j] != 0)
                {
                    EnYakinGizliAltinListe.Add(new GizliAltinBilgileri { Zi = i, Xj = j ,Mesafe=Mathf.Abs(i-MatrisKonumZ)+ Mathf.Abs(j - MatrisKonumX) });
                   
                }
            }
        }

        for (int i = 0; i < EnYakinGizliAltinListe.Count; i++)
        {
            //Gizli Altin Sayisi ve Count 8 olmasına rağmen bazen eksik oluyor burada yazdırırken
            //Debug.Log("MESAFELER İLK HALİ " + EnYakinGizliAltinListe[i].Mesafe);
        }

        for (int i=0; i<EnYakinGizliAltinListe.Count;i++)
        {
            for (int j=0; j<EnYakinGizliAltinListe.Count;j++)
            {
                //if (EnYakinGizliAltinListe[i].Mesafe > EnYakinGizliAltinListe[j].Mesafe) Demin böyleydi,böyle olunca büyükten küçüğe doğru oluyor liste o yüzden en uzaktakileri açığa çıkarıyordu
               
                if (EnYakinGizliAltinListe[i].Mesafe < EnYakinGizliAltinListe[j].Mesafe)//Böyle düzeltildi de ?
                {
                    //Debug.Log("Değiştiliyor küçük olan:" + EnYakinGizliAltinListe[i].Mesafe);
                    //Debug.Log("Değiştiliyor büyük olan:" + EnYakinGizliAltinListe[j].Mesafe);
                    GizliAltinBilgileri Gizli_altin_gecici = EnYakinGizliAltinListe[i];
                    EnYakinGizliAltinListe[i] = EnYakinGizliAltinListe[j];
                    EnYakinGizliAltinListe[j] = Gizli_altin_gecici;
                }
            }
        }
        ////Debug.Log("--> " + EnYakinGizliAltinListe.Count);
        //for (int i = 0; i < EnYakinGizliAltinListe.Count; i++)
        //{
        //    //Gizli Altin Sayisi ve Count 8 olmasına rağmen bazen eksik oluyor burada yazdırırken
        //    //Debug.Log("MESAFE " + EnYakinGizliAltinListe[i].Mesafe);
        //}
        
        if (EnYakinGizliAltinListe.Count < 2)
        {
            enYakinIndex = 1;
        }
        else
        {
            enYakinIndex = 2;
        }
        for (int i = 0; i < enYakinIndex; i++)
        {
            if (EnYakinGizliAltinListe.Count > 0 && Board.GetComponent<CreateGameBoard>().GizliAltinMatris[EnYakinGizliAltinListe[i].Zi, EnYakinGizliAltinListe[i].Xj] != 0)
            {
                Board.GetComponent<CreateGameBoard>().GoldMatris[EnYakinGizliAltinListe[i].Zi, EnYakinGizliAltinListe[i].Xj] = Board.GetComponent<CreateGameBoard>().GizliAltinMatris[EnYakinGizliAltinListe[i].Zi, EnYakinGizliAltinListe[i].Xj];
                Board.GetComponent<CreateGameBoard>().GizliAltinMatris[EnYakinGizliAltinListe[i].Zi, EnYakinGizliAltinListe[i].Xj] = 0;
                //Debug.Log("Gizli altin normale çevrildi konumu:" + EnYakinGizliAltinListe[i].Zi + " "+ EnYakinGizliAltinListe[i].Xj);
                GameObject Gold_Obj = Instantiate(Board.GetComponent<CreateGameBoard>().GoldPrefab, new Vector3(EnYakinGizliAltinListe[i].Xj, 1f, EnYakinGizliAltinListe[i].Zi), Quaternion.Euler(40f, 0f, 0f));
                Board.GetComponent<CreateGameBoard>().AllGoldPrefabs[EnYakinGizliAltinListe[i].Zi, EnYakinGizliAltinListe[i].Xj] = Gold_Obj;
                Board.GetComponent<CreateGameBoard>().GizliAltinSayisi--;
            }
        }
        EnYakinGizliAltinListe.Clear();



    }

    public void HedefBelirle(int Z, int X)
    {
        //Debug.Log("MatrisKonumZ:" + MatrisKonumZ + " MatrisKonumX" + MatrisKonumX);
        TümAltinSayisi = Board.GetComponent<CreateGameBoard>().AltinSayisi;
        TümAltinSayisi -= Board.GetComponent<CreateGameBoard>().GizliAltinSayisi;
        if (TümAltinSayisi > 0)
        {
            //Debug.Log("Temel ife giriyor");
            for (int i = 0; i < Z; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    if (Board.GetComponent<CreateGameBoard>().GoldMatris[i, j] != 0) //Eğer altın varsa
                    {

                        mesafe = Mathf.Abs(i - MatrisKonumZ) + Mathf.Abs(j - MatrisKonumX);
                        //Debug.Log("MatrisKonumZ:" + MatrisKonumZ+" uzaktaki i:"+i);
                        //Debug.Log("Z Mesafesi:" + Mathf.Abs(i - MatrisKonumZ));
                        //Debug.Log("MatrisKonumX:" + MatrisKonumX + " uzaktaki j:" + j);
                        //Debug.Log("X Mesafesi:" + Mathf.Abs(j - MatrisKonumX));
                        //Debug.Log("Mesafe:" + mesafe);
                        if (mesafe % HamleSayisi == 0)
                        {
                            maliyet = (HamleMaliyeti * (mesafe / HamleSayisi)) - Board.GetComponent<CreateGameBoard>().GoldMatris[i, j];
                            //Debug.Log("Altın sayısı:" + Board.GetComponent<CreateGameBoard>().GoldMatris[i, j]);
                            //Debug.Log("1 maliyet:" + maliyet);
                        }
                        else
                        {

                            maliyet = ((HamleMaliyeti * (mesafe / HamleSayisi)) - Board.GetComponent<CreateGameBoard>().GoldMatris[i, j]) + HamleMaliyeti; //+HamleMaliyeti 1 hamlelik maliyet ekliyor. 
                                                                                                                     //7 ise sol taraftan 2 hamlenin maliyeti ekleniyor çünkü.En sağda 3.hamlenin maliyeti ekleniyor.
                                                                                                                     //Debug.Log("Altın sayısı:" + Board.GetComponent<CreateGameBoard>().GoldMatris[i, j]);
                                                                                                                     //Debug.Log("2 maliyet:" + maliyet);
                        }

                        if (maliyet <= enazmaliyet)
                        {
                            enazmaliyet = maliyet;
                            enazmaliyetZ = i;
                            enazmaliyetX = j;
                            //Debug.Log("Altın sayısı:" + Board.GetComponent<CreateGameBoard>().GoldMatris[i, j]);
                            //Debug.Log("Mesafe:" + mesafe);
                            //Debug.Log("En az maliyet:" + maliyet);
                            //Debug.Log("Güncel maaliyetli Z si:" + enazmaliyetZ + " X i:" + enazmaliyetX);

                        }
                        TümAltinSayisi--;

                    }

                }
            }
            HedefKonumZ = enazmaliyetZ;
            HedefKonumX = enazmaliyetX;
            //Debug.Log("En az maliyet:" + enazmaliyet);
            //Debug.Log("En son en az maaliyetli Z si:" + HedefKonumZ + " X i:" + HedefKonumX);
            Gold -= HedefBelirlemeMaliyeti;
            ToplamHarcananAltinsayisi += HedefBelirlemeMaliyeti;
            if (Gold <= 0)
            {
                Board.GetComponent<CreateGameBoard>().OyuncuCElendiMi = true;
                MatrisKonumZ = 1000;
                MatrisKonumX = 1000;
                this.gameObject.transform.position = new Vector3(Board.GetComponent<CreateGameBoard>().KareSayisiX + 1f, 1f, Board.GetComponent<CreateGameBoard>().KareSayisiZ - 1f);
            }
        }


        //en yakını bulma kısmı..
        //en yakini bulduktan sonra
        
    }

    public void HedefeUlasildimi()
    {
        if (MatrisKonumZ == HedefKonumZ && MatrisKonumX == HedefKonumX)
        {
            if (Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX] == 0) //Eğer zaten alınmışsa bu sorunu düzeltti artık aynı altını almıyorlar
            {
                //Debug.Log("Altin zaten alinmiş ama ulaştım");
                Destroy(Board.GetComponent<CreateGameBoard>().AllGoldPrefabs[HedefKonumZ, HedefKonumX].gameObject);
                GüncelHamle = HamleSayisi;// Yani bir daha hamle yapmasını engelliyoruz.
                enazmaliyet = 10000;
                HedefKonumX = -10;//Null yerine
                HedefKonumZ = -10;//Null yerine
                enYakinKonumZ = 1000;//Tekrar en yakını bulması için büyük bir değer giriyoruz
                enYakinKonumX = 1000;//Tekrar en yakını bulması için büyük bir değer giriyoruz
                
            }
            else
            {
                //Debug.Log("B hedefe ulaştı konumu:Z" + MatrisKonumZ + " X:" + MatrisKonumX);
                //Debug.Log("Altın sıfırlanıyor konumu:" + MatrisKonumZ + " " + MatrisKonumX);
                Gold += Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX];
                ToplananAltinSayisi += Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX];
                Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX] = 0;
                Board.GetComponent<CreateGameBoard>().AltinSayisi--;
                //Debug.Log("Altın sayısı azaltılıyor ve altın sıfırlanıyor konumu:" + MatrisKonumZ + " " + MatrisKonumX);
                Destroy(Board.GetComponent<CreateGameBoard>().AllGoldPrefabs[HedefKonumZ, HedefKonumX].gameObject);
                GüncelHamle = HamleSayisi;// Yani bir daha hamle yapmasını engelliyoruz.
                enazmaliyet = 10000;
                HedefKonumX = -10;//Null yerine
                HedefKonumZ = -10;//Null yerine
                enYakinKonumZ = 1000;//Tekrar en yakını bulması için büyük bir değer giriyoruz
                enYakinKonumX = 1000;//Tekrar en yakını bulması için büyük bir değer giriyoruz
                
            }
            if (Board.GetComponent<CreateGameBoard>().GizliAltinSayisi > 0)
            {
                GizliAltinlariTespitEt(Board.GetComponent<CreateGameBoard>().GizliAltinMatris, 80, Board.GetComponent<CreateGameBoard>().KareSayisiZ, Board.GetComponent<CreateGameBoard>().KareSayisiX);
            }
            HedefBelirle(Board.GetComponent<CreateGameBoard>().KareSayisiZ, Board.GetComponent<CreateGameBoard>().KareSayisiX);
            


        }
    }



    public void SagaGit()
    {
        //Debug.Log("C Sağa gitti");
        this.MatrisKonumX++;
        this.gameObject.transform.position = new Vector3(this.MatrisKonumX, 1f, this.MatrisKonumZ);
        ToplamAdimSayisi++;
        fs = new FileStream(DosyaYolu, FileMode.Append, FileAccess.Write);
        sw = new StreamWriter(fs);
        sw.WriteLine(this.MatrisKonumZ.ToString() + " " + this.MatrisKonumX.ToString() + " Saga 1 Adim Gitti");
        sw.Flush();
        sw.Close();
        fs.Close();
        GizliAltinKontrol();

        GüncelHamle++;
        HedefeUlasildimi();
        if (MatrisKonumX < HedefKonumX && GüncelHamle < HamleSayisi)
        {
            SagaGit();
        }
        return;
    }

    public void SolaGit()
    {
        //Debug.Log("C Sola gitti");
        this.MatrisKonumX--;
        this.gameObject.transform.position = new Vector3(this.MatrisKonumX, 1f, this.MatrisKonumZ);
        ToplamAdimSayisi++;
        fs = new FileStream(DosyaYolu, FileMode.Append, FileAccess.Write);
        sw = new StreamWriter(fs);
        sw.WriteLine(this.MatrisKonumZ.ToString() + " " + this.MatrisKonumX.ToString() + " Sola 1 Adim Gitti");
        sw.Flush();
        sw.Close();
        fs.Close();
        GizliAltinKontrol();

        GüncelHamle++;
        HedefeUlasildimi();
        if (MatrisKonumX > HedefKonumX && GüncelHamle < HamleSayisi)
        {
            SolaGit();
        }

        return;



    }

    public void AsagiGit()
    {
        //Debug.Log("C Asagi gitti");
        this.MatrisKonumZ--;
        this.gameObject.transform.position = new Vector3(this.MatrisKonumX, 1f, this.MatrisKonumZ);
        ToplamAdimSayisi++;
        fs = new FileStream(DosyaYolu, FileMode.Append, FileAccess.Write);
        sw = new StreamWriter(fs);
        sw.WriteLine(this.MatrisKonumZ.ToString() + " " + this.MatrisKonumX.ToString() + " Asagi 1 Adim Gitti");
        sw.Flush();
        sw.Close();
        fs.Close();
        GizliAltinKontrol();
        GüncelHamle++;
        HedefeUlasildimi();
        if (MatrisKonumZ > HedefKonumZ && GüncelHamle < HamleSayisi)
        {
            AsagiGit();
        }

        return;



    }

    public void YukariGit()
    {
        //Debug.Log("C Yukari gitti");
        this.MatrisKonumZ++;
        this.gameObject.transform.position = new Vector3(this.MatrisKonumX, 1f, this.MatrisKonumZ);
        ToplamAdimSayisi++;
        fs = new FileStream(DosyaYolu, FileMode.Append, FileAccess.Write);
        sw = new StreamWriter(fs);
        sw.WriteLine(this.MatrisKonumZ.ToString() + " " + this.MatrisKonumX.ToString() + " Yukari 1 Adim Gitti");
        sw.Flush();
        sw.Close();
        fs.Close();
        GizliAltinKontrol();

        GüncelHamle++;
        HedefeUlasildimi();
        if (MatrisKonumZ < HedefKonumZ && GüncelHamle < HamleSayisi)
        {
            YukariGit();
        }

        return;


    }
    public void GizliAltinKontrol()
    {
        if (!Board.GetComponent<CreateGameBoard>().OyuncuCElendiMi)
        {
            if (Board.GetComponent<CreateGameBoard>().GizliAltinMatris[MatrisKonumZ, MatrisKonumX] != 0)
            {
                Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX] = Board.GetComponent<CreateGameBoard>().GizliAltinMatris[MatrisKonumZ, MatrisKonumX];
                Board.GetComponent<CreateGameBoard>().GizliAltinMatris[MatrisKonumZ, MatrisKonumX] = 0;
                GameObject Gold_Obj = Instantiate(Board.GetComponent<CreateGameBoard>().GoldPrefab, new Vector3(MatrisKonumX, 1f, MatrisKonumZ), Quaternion.Euler(40f, 0f, 0f));
                Board.GetComponent<CreateGameBoard>().AllGoldPrefabs[MatrisKonumZ, MatrisKonumX] = Gold_Obj;
                Board.GetComponent<CreateGameBoard>().GizliAltinSayisi--;
            }
        }

    }

    public void KonumuAyarla(int i, int j)
    {
        this.MatrisKonumZ = i;
        this.MatrisKonumX = j;
    }
}
public class GizliAltinBilgileri
{
   public int Zi, Xj,Mesafe;


}
