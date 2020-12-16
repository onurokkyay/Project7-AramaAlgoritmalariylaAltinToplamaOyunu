using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerA : MonoBehaviour, PlayerInterface // : Implements
{

    public int ToplamAdimSayisi=0;
    public int ToplananAltinSayisi = 0;
    public int ToplamHarcananAltinsayisi = 0;

    public int Gold;

    public int HedefBelirlemeMaliyeti;

    public int HamleMaliyeti;

    public int MatrisKonumZ;

    public int MatrisKonumX;

    public int HedefKonumZ;

    public int HedefKonumX;

    public int enYakinKonumZ=1000;

    public int enYakinKonumX=1000;

    public int HamleSayisi = 0;

    public int GüncelHamle = 0;

    public int TümAltinSayisi;

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
        Gold = PlayerPrefs.GetInt("A_Gold");
        HedefBelirlemeMaliyeti = PlayerPrefs.GetInt("A_HBM");
        HamleMaliyeti = PlayerPrefs.GetInt("A_HM");
        HamleSayisi = PlayerPrefs.GetInt("_HS");
        // C:\Users\melih\Desktop\Melih\Universite\Yazılım Laboratuvarı Proje 1\YazLabPro1
         DosyaYolu = "OyuncuAFile.txt";

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
            Board.GetComponent<CreateGameBoard>().OyuncuAElendiMi = true;
            MatrisKonumZ = 1000;
            MatrisKonumX = 1000;
            this.gameObject.transform.position = new Vector3(-1f, 1f, 0);
        }
        else
        {
            //Debug.Log("Hamle Yapin içi");
            GüncelHamle = 0;
            HedefeUlasildimi();



            //Debug.Log("MatrisKonumZ:"+MatrisKonumZ+" MatrisKonumX:"+MatrisKonumX);
            //Debug.Log("HedefKonumZ:" + HedefKonumZ + " HedefKonumX:" + HedefKonumX);
            //Debug.Log("GüncelHamle:"+GüncelHamle);
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
            Board.GetComponent<CreateGameBoard>().OyuncuAElendiMi = true;
            MatrisKonumZ = 1000;
            MatrisKonumX = 1000;
            this.gameObject.transform.position = new Vector3(-1f, 1f, 0);
        }



    }

    public void HedefBelirle(int Z,int X)
    {
        //Debug.Log(Board.GetComponent<CreateGameBoard>().KareSayisiZ);
        //Debug.Log(Board.GetComponent<CreateGameBoard>().KareSayisiX);
        //Debug.Log(Z);
        //Debug.Log(X);
        //Debug.Log("Altin sayisi:hedefbelirlenin içi:" + Board.GetComponent<CreateGameBoard>().AltinSayisi);
        TümAltinSayisi = Board.GetComponent<CreateGameBoard>().AltinSayisi;
        TümAltinSayisi -= Board.GetComponent<CreateGameBoard>().GizliAltinSayisi;
        //Debug.Log(Board.GetComponent<CreateGameBoard>().GizliAltinSayisi);
        if (TümAltinSayisi > 0)
        {
            for (int i = 0; i < Z; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    if (Board.GetComponent<CreateGameBoard>().GoldMatris[i, j] != 0) //Eğer altın varsa
                    {

                        toplam = Mathf.Abs(enYakinKonumZ - MatrisKonumZ) + Mathf.Abs(enYakinKonumX - MatrisKonumX);
                        

                        if (Mathf.Abs(i - MatrisKonumZ) + Mathf.Abs(j - MatrisKonumX) <= toplam)
                        {

                            enYakinKonumZ = i;
                            enYakinKonumX = j;
                            //Debug.Log("en yakin hedefin Z si:" + enYakinKonumZ + " X i:" + enYakinKonumX);
                            //Debug.Log("goldMatris[i,j]="+goldMatris[i,j]);

                        }
                        TümAltinSayisi--;
                    }

                }
            }
            //en yakını bulma kısmı..
            //en yakini bulduktan sonra
            HedefKonumZ = enYakinKonumZ;
            HedefKonumX = enYakinKonumX;
            //Debug.Log("en yakin hedefin Z si:" + HedefKonumZ + " X i:" + HedefKonumX);
            Gold -= HedefBelirlemeMaliyeti;
            ToplamHarcananAltinsayisi += HedefBelirlemeMaliyeti;
            if (Gold <= 0)
            {
                Board.GetComponent<CreateGameBoard>().OyuncuAElendiMi = true;
                MatrisKonumZ = 1000;
                MatrisKonumX = 1000;
                this.gameObject.transform.position = new Vector3(-1f, 1f, 0);
            }
        }
            
        //Debug.Log("TÜM ALTIN SAYISI 0 OLMALI BAKALIM KAÇ:" + TümAltinSayisi);


      
        
        


        //MatrisKonumZ = HedefKonumZ;
        //MatrisKonumX = HedefKonumX;
        //HedefeUlasildimi(); Demek ki sıkıntı hareket fonksiyonunda.
    

    }

    public void HedefeUlasildimi()//Sıkıntı yok
    {
        //Debug.Log("En baş hedefe ulaştı mı kontrolü");
        if (MatrisKonumZ == HedefKonumZ && MatrisKonumX == HedefKonumX)
        {
                if(Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX] == 0) //Eğer zaten alınmışsa bu sorunu düzeltti artık aynı altını almıyorlar
            {
                //Debug.Log("Altin zaten alinmiş ama ulaştım");
                Destroy(Board.GetComponent<CreateGameBoard>().AllGoldPrefabs[HedefKonumZ, HedefKonumX].gameObject);
                GüncelHamle = HamleSayisi;// Yani bir daha hamle yapmasını engelliyoruz.
                HedefKonumX = -10;//Null yerine
                HedefKonumZ = -10;//Null yerine
                enYakinKonumZ = 1000;//Tekrar en yakını bulması için büyük bir değer giriyoruz
                enYakinKonumX = 1000;//Tekrar en yakını bulması için büyük bir değer giriyoruz
            }
            else
            {
                Gold += Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX];
                ToplananAltinSayisi += Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX];
                Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX] = 0;
                Board.GetComponent<CreateGameBoard>().AltinSayisi--;
                //Debug.Log("Altın sayısı azaltılıyor ve altın sıfırlanıyor konumu:" + MatrisKonumZ + " " + MatrisKonumX);
                Destroy(Board.GetComponent<CreateGameBoard>().AllGoldPrefabs[HedefKonumZ, HedefKonumX].gameObject);
                GüncelHamle = HamleSayisi;// Yani bir daha hamle yapmasını engelliyoruz.
                HedefKonumX = -10;//Null yerine
                HedefKonumZ = -10;//Null yerine
                enYakinKonumZ = 1000;//Tekrar en yakını bulması için büyük bir değer giriyoruz
                enYakinKonumX = 1000;//Tekrar en yakını bulması için büyük bir değer giriyoruz
            }
            HedefBelirle(Board.GetComponent<CreateGameBoard>().KareSayisiZ, Board.GetComponent<CreateGameBoard>().KareSayisiX);


        }
        //else Debug.Log("A hala hedefe ulaşamadı");
    }


    public void GizliAltinKontrol()
    {
        if (!Board.GetComponent<CreateGameBoard>().OyuncuAElendiMi)
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






    public void SagaGit()
    {        
        //Debug.Log("A Sağa gitti");
        this.MatrisKonumX++;
        this.gameObject.transform.position = new Vector3(this.MatrisKonumX, 1f, this.MatrisKonumZ);
        ToplamAdimSayisi++;

        fs = new FileStream(DosyaYolu, FileMode.Append, FileAccess.Write);
        sw = new StreamWriter(fs);
        sw.WriteLine(this.MatrisKonumZ.ToString() + " " + this.MatrisKonumX.ToString()+" Saga 1 Adim Gitti");
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
        //Debug.Log("A Sola gitti");
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
        //Debug.Log("A Asagi gitti");
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
        //Debug.Log("A Yukari gitti");
        this.MatrisKonumZ++;
        this.gameObject.transform.position = new Vector3(this.MatrisKonumX, 1f, this.MatrisKonumZ);
        ToplamAdimSayisi++;
        fs = new FileStream(DosyaYolu, FileMode.Append, FileAccess.Write, FileShare.None);
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

    public void KonumuAyarla(int i, int j)
    {
        this.MatrisKonumZ = i;
        this.MatrisKonumX = j;
    }



}
