using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerB :MonoBehaviour, PlayerInterface
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

    int i, j;
    int toplam;

    public GameObject Board;

    string DosyaYolu;
    FileStream fs;
    StreamWriter sw;
    private void Awake()
    {


        Board = GameObject.FindGameObjectWithTag("Board");//Starta koyunca olmuyor
        HedefKonumX = -10;//En başta hedefi yok
        HedefKonumZ = -10;//En başta hedefi yok
        Gold = PlayerPrefs.GetInt("B_Gold");
        HedefBelirlemeMaliyeti = PlayerPrefs.GetInt("B_HBM");
        HamleMaliyeti = PlayerPrefs.GetInt("B_HM");
        HamleSayisi = PlayerPrefs.GetInt("_HS");
        DosyaYolu = "OyuncuBFile.txt";

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
            Board.GetComponent<CreateGameBoard>().OyuncuBElendiMi = true;
            MatrisKonumZ = 1000;
            MatrisKonumX = 1000;
            this.gameObject.transform.position = new Vector3(Board.GetComponent<CreateGameBoard>().KareSayisiX +1f , 1f, 0f);
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
            Board.GetComponent<CreateGameBoard>().OyuncuBElendiMi = true;
            MatrisKonumZ = 1000;
            MatrisKonumX = 1000;
            this.gameObject.transform.position = new Vector3(Board.GetComponent<CreateGameBoard>().KareSayisiX + 1f, 1f, 0f);
        }


    }

    public void HedefBelirle( int Z, int X)
    {
        //Debug.Log(Board.GetComponent<CreateGameBoard>().KareSayisiZ);
        //Debug.Log(Board.GetComponent<CreateGameBoard>().KareSayisiX);
        //Debug.Log(Z);
        //Debug.Log(X);
        //Debug.Log("MatrisKonumZ:" + MatrisKonumZ + " MatrisKonumX" + MatrisKonumX);
        TümAltinSayisi = Board.GetComponent<CreateGameBoard>().AltinSayisi;
        TümAltinSayisi -= Board.GetComponent<CreateGameBoard>().GizliAltinSayisi;
        if (TümAltinSayisi > 0)
        {
            //Debug.Log("Temel ife giriyor");
            for (int i = 0; i < Z; i++)
            {
                for (int j = 0; j< X; j++)
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
            //en yakını bulma kısmı..
            //en yakini bulduktan sonra
            HedefKonumZ = enazmaliyetZ;
            HedefKonumX = enazmaliyetX;
            //Debug.Log("En az maliyet:" + enazmaliyet);
            //Debug.Log("En son en az maaliyetli Z si:" + HedefKonumZ + " X i:" + HedefKonumX);
            Gold -= HedefBelirlemeMaliyeti;
            ToplamHarcananAltinsayisi += HedefBelirlemeMaliyeti;
            if (Gold <= 0)
            {
                Board.GetComponent<CreateGameBoard>().OyuncuBElendiMi = true;
                MatrisKonumZ = 1000;
                MatrisKonumX = 1000;
                this.gameObject.transform.position = new Vector3(Board.GetComponent<CreateGameBoard>().KareSayisiX + 1f, 1f, 0f);
            }
        }
               

    
    }
    public void HedefeUlasildimi()
    {
        //Debug.Log("En baş hedefe ulaştı mı kontrolü");
        if (MatrisKonumZ == HedefKonumZ && MatrisKonumX == HedefKonumX)
        {
            if(Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX] == 0) //Eğer zaten alınmışsa bu sorunu düzeltti artık aynı altını almıyorlar
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
                ToplananAltinSayisi+= Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX];
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
            HedefBelirle(Board.GetComponent<CreateGameBoard>().KareSayisiZ, Board.GetComponent<CreateGameBoard>().KareSayisiX);


        }
        //else Debug.Log("B hala hedefe ulaşamadı");
    }

   

    public void SagaGit()
    {
        //Debug.Log("B Sağa gitti");
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
        //Debug.Log("B Sola gitti");
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
        //Debug.Log("B Asagi gitti");
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
        //Debug.Log("B Yukari gitti");
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
        if (!Board.GetComponent<CreateGameBoard>().OyuncuBElendiMi)
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
