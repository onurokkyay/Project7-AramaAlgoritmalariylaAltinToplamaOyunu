using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerD : MonoBehaviour, PlayerInterface
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

    public int mesafe;

    public int enazmaliyet = 10000;

    public int enazmaliyetZ, enazmaliyetX;

    public int[,] GoldMatrisD;
    public GameObject OyuncuA_D;
    public GameObject OyuncuB_D;
    public GameObject OyuncuC_D;

    public int GoldMatrisDAltinSayisi;

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
        Gold = PlayerPrefs.GetInt("D_Gold");
        HedefBelirlemeMaliyeti = PlayerPrefs.GetInt("D_HBM");
        HamleMaliyeti = PlayerPrefs.GetInt("D_HM");
        HamleSayisi = PlayerPrefs.GetInt("_HS");
        GoldMatrisD = new int[Board.GetComponent<CreateGameBoard>().KareSayisiZ, Board.GetComponent<CreateGameBoard>().KareSayisiX];
        DosyaYolu = "OyuncuDFile.txt";
        fs = new FileStream(DosyaYolu, FileMode.OpenOrCreate, FileAccess.Write);
        sw = new StreamWriter(fs);

        sw.Flush();
        sw.Close();
        fs.Close();
    }
    private void Start()
    {
        OyuncuA_D = Board.GetComponent<CreateGameBoard>().OyuncuA;
        OyuncuB_D = Board.GetComponent<CreateGameBoard>().OyuncuB;
        OyuncuC_D = Board.GetComponent<CreateGameBoard>().OyuncuC;
    }



    public void HamleYap()
    {
        if (Gold <= 0)
        {
            Board.GetComponent<CreateGameBoard>().OyuncuDElendiMi = true;
            MatrisKonumZ = 1000;
            MatrisKonumX = 1000;
            this.gameObject.transform.position = new Vector3(-1f, 1f, Board.GetComponent<CreateGameBoard>().KareSayisiZ-1);
            
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
            Board.GetComponent<CreateGameBoard>().OyuncuDElendiMi = true;
            MatrisKonumZ = 1000;
            MatrisKonumX = 1000;
            this.gameObject.transform.position = new Vector3(-1f, 1f, Board.GetComponent<CreateGameBoard>().KareSayisiZ - 1);
        }


    }
    public void DigerHamleleriKontrolEt()
    {
        
            GoldMatrisDAltinSayisi = Board.GetComponent<CreateGameBoard>().AltinSayisi;
            for (int i = 0; i < Board.GetComponent<CreateGameBoard>().KareSayisiZ; i++)
            {
                for (int j = 0; j < Board.GetComponent<CreateGameBoard>().KareSayisiX; j++)
                {
                    GoldMatrisD[i, j] = Board.GetComponent<CreateGameBoard>().GoldMatris[i, j];
                }
            }
            //Debug.Log("Hedef "+OyuncuA_D.GetComponent<PlayerA>().HedefKonumZ +" "+OyuncuA_D.GetComponent<PlayerA>().HedefKonumX);
            //Debug.Log("Konum "+OyuncuA_D.GetComponent<PlayerA>().MatrisKonumZ + " " + OyuncuA_D.GetComponent<PlayerA>().MatrisKonumX);
            int mesafeX = Mathf.Abs(OyuncuA_D.GetComponent<PlayerA>().HedefKonumX - OyuncuA_D.GetComponent<PlayerA>().MatrisKonumX);
            int mesafeZ = Mathf.Abs(OyuncuA_D.GetComponent<PlayerA>().HedefKonumZ - OyuncuA_D.GetComponent<PlayerA>().MatrisKonumZ);
            //Debug.Log(mesafeZ + " " + mesafeX);
            int Mesafe = mesafeZ + mesafeX;
            //Debug.Log(Mesafe);


            int MesafeD = Mathf.Abs(MatrisKonumX - OyuncuA_D.GetComponent<PlayerA>().HedefKonumX) + Mathf.Abs(MatrisKonumZ - OyuncuA_D.GetComponent<PlayerA>().HedefKonumZ);
            int KacHamle;
            if (Mesafe % HamleSayisi == 0)
            {
                KacHamle = Mesafe / HamleSayisi;
            }
            else
            {
                KacHamle = (Mesafe / HamleSayisi) + 1;
            }
            int KacHamleD;
            if (MesafeD % HamleSayisi == 0)
            {
                KacHamleD = MesafeD / HamleSayisi;
            }
            else
            {
                KacHamleD = (MesafeD / HamleSayisi) + 1;
            }
            KacHamleD--;
            //Debug.Log("AMesafe" + Mesafe);
            //Debug.Log("AKacHamle" + KacHamle);
            //Debug.Log("KacHamleD" + KacHamleD);

            if (KacHamle <= KacHamleD)
            {
                if (OyuncuA_D.GetComponent<PlayerA>().HedefKonumZ != -10 && OyuncuA_D.GetComponent<PlayerA>().HedefKonumX != -10 && OyuncuA_D.GetComponent<PlayerA>().HedefKonumZ != 1000 && OyuncuA_D.GetComponent<PlayerA>().HedefKonumX != 1000)
                {
                //Debug.Log("A_Z " + OyuncuA_D.GetComponent<PlayerA>().MatrisKonumZ + " X " + OyuncuA_D.GetComponent<PlayerA>().MatrisKonumX);
                //Debug.Log("D_Z " + MatrisKonumZ + " X " + MatrisKonumX);
                //Debug.Log("Hedef Z "+OyuncuA_D.GetComponent<PlayerA>().HedefKonumZ+" X "+ OyuncuA_D.GetComponent<PlayerA>().HedefKonumX);
                GoldMatrisD[OyuncuA_D.GetComponent<PlayerA>().HedefKonumZ, OyuncuA_D.GetComponent<PlayerA>().HedefKonumX] = 0;
                GoldMatrisDAltinSayisi--;
                }
                
            }
            else
            {
                //Debug.Log("Bunu Alabilirim" + "A_Z " + OyuncuA_D.GetComponent<PlayerA>().HedefKonumZ + " X " + OyuncuA_D.GetComponent<PlayerA>().HedefKonumX);
            }

            Mesafe = Mathf.Abs(OyuncuB_D.GetComponent<PlayerB>().HedefKonumX - OyuncuB_D.GetComponent<PlayerB>().MatrisKonumX) + Mathf.Abs(OyuncuB_D.GetComponent<PlayerB>().HedefKonumZ - OyuncuB_D.GetComponent<PlayerB>().MatrisKonumZ);

            MesafeD = Mathf.Abs(MatrisKonumX - OyuncuB_D.GetComponent<PlayerB>().HedefKonumX) + Mathf.Abs(MatrisKonumZ - OyuncuB_D.GetComponent<PlayerB>().HedefKonumZ);

            if (Mesafe % HamleSayisi == 0)
            {
                KacHamle = Mesafe / HamleSayisi;
            }
            else
            {
                KacHamle = (Mesafe / HamleSayisi) + 1;
            }

            if (MesafeD % HamleSayisi == 0)
            {
                KacHamleD = MesafeD / HamleSayisi;
            }
            else
            {
                KacHamleD = (MesafeD / HamleSayisi) + 1;
            }
            KacHamleD--;
            //Debug.Log("BMesafe" + Mesafe);
            //Debug.Log("BKacHamle" + KacHamle);
            //Debug.Log("KacHamleD" + KacHamleD);

            if (KacHamle <= KacHamleD)
            {
            
            if (OyuncuB_D.GetComponent<PlayerB>().HedefKonumZ != -10 && OyuncuB_D.GetComponent<PlayerB>().HedefKonumX != -10 && OyuncuB_D.GetComponent<PlayerB>().HedefKonumZ != 1000 && OyuncuB_D.GetComponent<PlayerB>().HedefKonumX != 1000)
            {
                //Debug.Log("B_Z " + OyuncuB_D.GetComponent<PlayerB>().MatrisKonumZ + " X " + OyuncuB_D.GetComponent<PlayerB>().MatrisKonumX);
                //Debug.Log("D_Z " + MatrisKonumZ + " X " + MatrisKonumX);
                //Debug.Log("Hedef Z " + OyuncuB_D.GetComponent<PlayerB>().HedefKonumZ + " X " + OyuncuB_D.GetComponent<PlayerB>().HedefKonumX);
                GoldMatrisD[OyuncuB_D.GetComponent<PlayerB>().HedefKonumZ, OyuncuB_D.GetComponent<PlayerB>().HedefKonumX] = 0;
                GoldMatrisDAltinSayisi--;
                
            }

        }
            else
            {
                //Debug.Log("Bunu Alabilirim" + "B_Z " + OyuncuB_D.GetComponent<PlayerB>().HedefKonumZ + " X " + OyuncuB_D.GetComponent<PlayerB>().HedefKonumX);
            }

            Mesafe = Mathf.Abs(OyuncuC_D.GetComponent<PlayerC>().HedefKonumX - OyuncuC_D.GetComponent<PlayerC>().MatrisKonumX) + Mathf.Abs(OyuncuC_D.GetComponent<PlayerC>().HedefKonumZ - OyuncuC_D.GetComponent<PlayerC>().MatrisKonumZ);

            MesafeD = Mathf.Abs(MatrisKonumX - OyuncuC_D.GetComponent<PlayerC>().HedefKonumX) + Mathf.Abs(MatrisKonumZ - OyuncuC_D.GetComponent<PlayerC>().HedefKonumZ);

            if (Mesafe % HamleSayisi == 0)
            {
                KacHamle = Mesafe / HamleSayisi;
            }
            else
            {
                KacHamle = (Mesafe / HamleSayisi) + 1;
            }

            if (MesafeD % HamleSayisi == 0)
            {
                KacHamleD = MesafeD / HamleSayisi;
            }
            else
            {
                KacHamleD = (MesafeD / HamleSayisi) + 1;
            }
            KacHamleD--;
            //Debug.Log("CMesafe" + Mesafe);
            //Debug.Log("CKacHamle" + KacHamle);
            //Debug.Log("KacHamleD" + KacHamleD);

            if (KacHamle <= KacHamleD)
            {
                if(OyuncuC_D.GetComponent<PlayerC>().HedefKonumZ!=-10 && OyuncuC_D.GetComponent<PlayerC>().HedefKonumX != -10 && OyuncuC_D.GetComponent<PlayerC>().HedefKonumZ !=1000 && OyuncuC_D.GetComponent<PlayerC>().HedefKonumX !=1000)
                {
                //Debug.Log("C_Z " + OyuncuC_D.GetComponent<PlayerC>().MatrisKonumZ + " X " + OyuncuC_D.GetComponent<PlayerC>().MatrisKonumX);
                //Debug.Log("D_Z " + MatrisKonumZ + " X " + MatrisKonumX);
                //Debug.Log("Hedef Z " + OyuncuC_D.GetComponent<PlayerC>().HedefKonumZ + " X " + OyuncuC_D.GetComponent<PlayerC>().HedefKonumX);
                GoldMatrisD[OyuncuC_D.GetComponent<PlayerC>().HedefKonumZ, OyuncuC_D.GetComponent<PlayerC>().HedefKonumX] = 0;
                GoldMatrisDAltinSayisi--;
                }
                
                
            }
            else
            {

                //Debug.Log("Bunu Alabilirim" +"C_Z " + OyuncuC_D.GetComponent<PlayerC>().HedefKonumZ + " X " + OyuncuC_D.GetComponent<PlayerC>().HedefKonumX);
            }
        

    }


    public void HedefBelirle(int Z, int X)
    {
        //Debug.Log("MatrisKonumZ:" + MatrisKonumZ + " MatrisKonumX" + MatrisKonumX);
        DigerHamleleriKontrolEt();
        TümAltinSayisi = GoldMatrisDAltinSayisi;
        TümAltinSayisi -= Board.GetComponent<CreateGameBoard>().GizliAltinSayisi;
        //Debug.Log(TümAltinSayisi + "" + Board.GetComponent<CreateGameBoard>().GizliAltinSayisi);
        if (TümAltinSayisi > 0)
        {
            //Debug.Log("Temel ife giriyor");
            for (int i = 0; i < Z; i++)
            {
                for (int j = 0; j < X; j++)
                {
                    if (GoldMatrisD[i, j] != 0) //Eğer altın varsa
                    {

                        mesafe = Mathf.Abs(i - MatrisKonumZ) + Mathf.Abs(j - MatrisKonumX);
                        //Debug.Log("MatrisKonumZ:" + MatrisKonumZ+" uzaktaki i:"+i);
                        //Debug.Log("Z Mesafesi:" + Mathf.Abs(i - MatrisKonumZ));
                        //Debug.Log("MatrisKonumX:" + MatrisKonumX + " uzaktaki j:" + j);
                        //Debug.Log("X Mesafesi:" + Mathf.Abs(j - MatrisKonumX));
                        //Debug.Log("Mesafe:" + mesafe);
                        if (mesafe % HamleSayisi == 0)
                        {
                            maliyet = (HamleMaliyeti * (mesafe / HamleSayisi)) - GoldMatrisD[i, j];
                            //Debug.Log("Altın sayısı:" + Board.GetComponent<CreateGameBoard>().GoldMatris[i, j]);
                            //Debug.Log("1 maliyet:" + maliyet);
                        }
                        else
                        {

                            maliyet = ((HamleMaliyeti * (mesafe / HamleSayisi)) - GoldMatrisD[i, j]) + HamleMaliyeti; //+HamleMaliyeti 1 hamlelik maliyet ekliyor. 
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
                Board.GetComponent<CreateGameBoard>().OyuncuDElendiMi = true;
                MatrisKonumZ = 1000;
                MatrisKonumX = 1000;
                this.gameObject.transform.position = new Vector3(-1f, 1f, Board.GetComponent<CreateGameBoard>().KareSayisiZ - 1);
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
            //HedefBelirle(Board.GetComponent<CreateGameBoard>().KareSayisiZ, Board.GetComponent<CreateGameBoard>().KareSayisiX);
            

        }

    }

   

    public void SagaGit()
    {
        //Debug.Log("D Sağa gitti");
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
        //Debug.Log("D Sola gitti");
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
        //Debug.Log("D Asagi gitti");
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
        //Debug.Log("D Yukari gitti");
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
        if (!Board.GetComponent<CreateGameBoard>().OyuncuDElendiMi)
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
