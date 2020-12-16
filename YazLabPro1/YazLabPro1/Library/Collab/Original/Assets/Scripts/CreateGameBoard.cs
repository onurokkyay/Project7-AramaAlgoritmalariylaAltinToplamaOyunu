using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateGameBoard : MonoBehaviour
{
    public GameObject GoldPrefab;
    public GameObject[,] AllGoldPrefabs;
    public GameObject cubePrefab1;
    public GameObject cubePrefab2;
    public GameObject[,] CubeMatris;
    public int[,] GizliAltinMatris;
    public GameObject playerAprefab;
    public GameObject playerBprefab;
    public GameObject playerCprefab;
    public GameObject playerDprefab;
    public GameObject OyuncuA;
    public GameObject OyuncuB;
    public GameObject OyuncuC;
    public GameObject OyuncuD;
    public bool TurnOyuncuA;
    public bool TurnOyuncuB;
    public bool TurnOyuncuC;
    public bool TurnOyuncuD;
    public int GizliAltinIhtimal;

    //GameManagerden gelenler
    public Camera maincamera;
    public int[,] GoldMatris;
    public GameObject[,] PlayerMatris;
    public int z, x;
    public int AltinSayisi = 0;
    public int GizliAltinSayisi=0;
    public float ihtimal = 0;


    public int Counter;
    public int KareSayisiZ=20;
    public int KareSayisiX = 20;
    public int turSayisi;


    private void Awake()
    {
        OyunuHazirla();
    }
    void Start()
    {
        //Debug.Log("Hedef Belirleniyor");
        //OyuncuA.GetComponent<PlayerA>().HedefBelirle(GoldMatris, 80, KareSayisiZ, KareSayisiX); //A HEDEF BELİRLİYOR 80 AltinSayisi

        //Debug.Log("A harekete geçiyor");
        //OyuncuA.GetComponent<PlayerA>().HamleYap();


        //Debug.Log("AltinSayisi:" +AltinSayisi);

        //while (AltinSayisi > 0)
        //{
        //    if (OyuncuA.GetComponent<PlayerA>().HedefKonumX == -10 && OyuncuA.GetComponent<PlayerA>().HedefKonumZ == -10)//Hedefi yoksa
        //    {
        //        Debug.Log("Hedef Belirleniyor");
        //        OyuncuA.GetComponent<PlayerA>().HedefBelirle(GoldMatris, 80, KareSayisiZ, KareSayisiX); //A HEDEF BELİRLİYOR 80 AltinSayisi
        //    }
        //    OyuncuA.GetComponent<PlayerA>().HamleYap();

        //    Debug.Log("Hamle Yapildi");


        //}

        //Debug.Log("Altin bitti.Altin Sayisi:" + AltinSayisi);
        StartCoroutine(StartedGame());
        //OyuncuB.GetComponent<PlayerB>().HedefBelirle(GoldMatris, 80, KareSayisiZ, KareSayisiX); //A HEDEF BELİRLİYOR 80 AltinSayisi
    }
    IEnumerator StartedGame()
    {
        while (AltinSayisi-GizliAltinSayisi > 0)
        {
            //Bazen son altınlarda sıkıntı çıkıyor 1 veya 2 tane altın kalıyor bazen ama altın sayısı 0 oluyor.Aynı altını aldıkları için sanırım.
            //yield return new WaitForSeconds(0.02f);
            //Debug.Log(turSayisi/4);
            //yield return new WaitForSeconds(0.001f);
            yield return new WaitForSeconds(0.05f);
            if (TurnOyuncuA)
            {
                PlayerAPlaying();
                TurnOyuncuA = false;
                TurnOyuncuB = true;
            }
            else if (TurnOyuncuB)
            {
                PlayerBPlaying();
                TurnOyuncuB = false;
                TurnOyuncuC = true;
            }
            else if (TurnOyuncuC)
            {
                PlayerCPlaying();
                TurnOyuncuC = false;
                TurnOyuncuD = true;
            }
            else if (TurnOyuncuD)
            {
                PlayerDPlaying();
                TurnOyuncuD = false;
                TurnOyuncuA = true;
            }
            turSayisi++;



        }     
        
        Debug.Log("Altin bitti.Altin Sayisi:" + AltinSayisi);
        Debug.Log("Altin bitti.GizliAltin Sayisi:" + GizliAltinSayisi);
        //StartCoroutine(WaitLoadScene());
    }
    IEnumerator WaitLoadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameLevel");
    }
    void PlayerDPlaying()
    {
        if (OyuncuD.GetComponent<PlayerD>().HedefKonumX == -10 && OyuncuD.GetComponent<PlayerD>().HedefKonumZ == -10)//Hedefi yoksa
        {
            // Debug.Log("B nin hedefi yok Hedef Belirleniyor");
            OyuncuD.GetComponent<PlayerD>().HedefBelirle( KareSayisiZ, KareSayisiX); //A HEDEF BELİRLİYOR 80 AltinSayisi
        }
        else if (GoldMatris[OyuncuD.GetComponent<PlayerD>().HedefKonumZ, OyuncuD.GetComponent<PlayerD>().HedefKonumX] == 0)//Hedeflediğini başkası almışsa 
        {
            //   Debug.Log("B nin hedefini başkası almış yeniden Hedef Belirleniyor");
            OyuncuD.GetComponent<PlayerD>().HedefBelirle( KareSayisiZ, KareSayisiX);
        }
        OyuncuD.GetComponent<PlayerD>().HamleYap();

        //Debug.Log("Hamle Yapildi");
    }
    void PlayerCPlaying()
    {
       //Her hedef belirlediğinde gizli altınları açığa çıkartıyor.
        if (OyuncuC.GetComponent<PlayerC>().HedefKonumX == -10 && OyuncuC.GetComponent<PlayerC>().HedefKonumZ == -10)//Hedefi yoksa
        {
            if (GizliAltinSayisi > 0)
            {
                OyuncuC.GetComponent<PlayerC>().GizliAltinlariTespitEt(GizliAltinMatris, 80, KareSayisiZ, KareSayisiX);
            }
            //Debug.Log("B nin hedefi yok Hedef Belirleniyor");
            OyuncuC.GetComponent<PlayerC>().HedefBelirle( KareSayisiZ, KareSayisiX); //A HEDEF BELİRLİYOR 80 AltinSayisi
        }
        else if (GoldMatris[OyuncuC.GetComponent<PlayerC>().HedefKonumZ, OyuncuC.GetComponent<PlayerC>().HedefKonumX] == 0)//Hedeflediğini başkası almışsa 
        {
            if (GizliAltinSayisi > 0)
            {
                OyuncuC.GetComponent<PlayerC>().GizliAltinlariTespitEt(GizliAltinMatris, 80, KareSayisiZ, KareSayisiX);
            }
            //Debug.Log("B nin hedefini başkası almış yeniden Hedef Belirleniyor");
            OyuncuC.GetComponent<PlayerC>().HedefBelirle( KareSayisiZ, KareSayisiX);
        }
        OyuncuC.GetComponent<PlayerC>().HamleYap();

        //Debug.Log("Hamle Yapildi");
    }
    void PlayerBPlaying()
    {
        if (OyuncuB.GetComponent<PlayerB>().HedefKonumX == -10 && OyuncuB.GetComponent<PlayerB>().HedefKonumZ == -10)//Hedefi yoksa
        {
            // Debug.Log("B nin hedefi yok Hedef Belirleniyor");
            OyuncuB.GetComponent<PlayerB>().HedefBelirle(KareSayisiZ, KareSayisiX); //A HEDEF BELİRLİYOR 80 AltinSayisi
        }
        else if (GoldMatris[OyuncuB.GetComponent<PlayerB>().HedefKonumZ, OyuncuB.GetComponent<PlayerB>().HedefKonumX] == 0)//Hedeflediğini başkası almışsa 
        {
            //   Debug.Log("B nin hedefini başkası almış yeniden Hedef Belirleniyor");
            OyuncuB.GetComponent<PlayerB>().HedefBelirle(KareSayisiZ, KareSayisiX);
        }
        OyuncuB.GetComponent<PlayerB>().HamleYap();

        //Debug.Log("Hamle Yapildi");
    }
    void PlayerAPlaying()
    {
        //Debug.Log("1 " + OyuncuA.GetComponent<PlayerA>().HedefKonumZ + "  " + OyuncuA.GetComponent<PlayerA>().HedefKonumX);
        if (OyuncuA.GetComponent<PlayerA>().HedefKonumX == -10 && OyuncuA.GetComponent<PlayerA>().HedefKonumZ == -10)//Hedefi yoksa
        {

            //Debug.Log("A nın hedefi yok Hedef Belirleniyor");
            OyuncuA.GetComponent<PlayerA>().HedefBelirle( KareSayisiZ, KareSayisiX); //A HEDEF BELİRLİYOR 80 AltinSayisi
           // Debug.Log("2 " + OyuncuA.GetComponent<PlayerA>().HedefKonumZ + "  " + OyuncuA.GetComponent<PlayerA>().HedefKonumX);
        }  
        else if (GoldMatris[OyuncuA.GetComponent<PlayerA>().HedefKonumZ, OyuncuA.GetComponent<PlayerA>().HedefKonumX] == 0) //Hedeflenen konumdakini başkası aldıysa
        {
            //Debug.Log("3 " + OyuncuA.GetComponent<PlayerA>().HedefKonumZ + "  " + OyuncuA.GetComponent<PlayerA>().HedefKonumX);
            //Debug.Log("A nın altınını başkası almış yeniden Hedef Belirleniyor");
            
            OyuncuA.GetComponent<PlayerA>().HedefBelirle( KareSayisiZ, KareSayisiX);
        }
        OyuncuA.GetComponent<PlayerA>().HamleYap();

        //Debug.Log("Hamle Yapildi");
    }

    void OyunuHazirla()
    {
        turSayisi = 0;
        TurnOyuncuA = true;
        TurnOyuncuB = false;
        TurnOyuncuC = false;
        TurnOyuncuD = false;
        KareSayisiZ = PlayerPrefs.GetInt("_M");
        KareSayisiX = PlayerPrefs.GetInt("_N");
        ihtimal = PlayerPrefs.GetInt("ihtimal");
        GizliAltinIhtimal = PlayerPrefs.GetInt("_GAO");
        //Debug.Log("İhtimal:" + ihtimal);
        AltinSayisi = (int) (((KareSayisiX * KareSayisiZ) * ihtimal) / 100);
        GizliAltinSayisi = (int) ((AltinSayisi * GizliAltinIhtimal) / 100);
        GizliAltinMatris = new int[KareSayisiZ, KareSayisiX];
        //Debug.Log("AltinSayisi ihtimalden sonra:" + AltinSayisi);
        CubeMatris = new GameObject[KareSayisiZ, KareSayisiX];
        AllGoldPrefabs = new GameObject[KareSayisiZ, KareSayisiX];
        Counter = 0;
        createBoard();
        CreatePlayers();
        GoldMatris = new int[KareSayisiZ, KareSayisiX];
        maincamera.transform.position = new Vector3(KareSayisiX / 2, ((KareSayisiX + KareSayisiZ) / 2) * 1.5f, KareSayisiZ / 2);
        MatrisYazdir();//Matrise Altın Sayısı kadar 5-20 arasında bir değer atıldı.
        AltinSayisi = (int)(((KareSayisiX * KareSayisiZ) * ihtimal) / 100);
        GizliAltinSayisi = (int)((AltinSayisi * GizliAltinIhtimal) / 100);
        //Debug.Log(AltinSayisi);
        //Debug.Log(GizliAltinSayisi);
        //Debug.Log(GizliAltinIhtimal);
        //Debug.Log("Düzeltilmiş Altin Sayisi:"+AltinSayisi);
        PlayerMatris = new GameObject[KareSayisiZ, KareSayisiX];
        
        KonumlariAyarla();
    }
    void createBoard()
    {
        for (int i = 0; i < KareSayisiZ; i++)
        {

            for (int j = 0; j < KareSayisiX; j++)
            {
                
                if (Counter % 2 == 0)
                {
                    GameObject cubeObj = Instantiate(cubePrefab1, new Vector3(j, 0, i), Quaternion.identity);
                    CubeMatris[i, j] = cubeObj;
                }
                else
                {
                     GameObject cubeObj= Instantiate(cubePrefab2, new Vector3(j, 0, i), Quaternion.identity);
                    CubeMatris[i, j] = cubeObj;
                }
                


                Counter++;

            }
            if (KareSayisiX % 2 == 0)
            {
                Counter++;
            }
            


        }




    }
    void CreatePlayers()
    {

         OyuncuA =Instantiate(playerAprefab, new Vector3(0, 1, 0), Quaternion.identity)as GameObject;

         OyuncuB =Instantiate(playerBprefab, new Vector3(KareSayisiX-1, 1, 0), Quaternion.identity)as GameObject;
      
         OyuncuC = Instantiate(playerCprefab, new Vector3(KareSayisiX - 1, 1, KareSayisiZ - 1), Quaternion.identity)as GameObject;
         OyuncuD = Instantiate(playerDprefab, new Vector3(0, 1, KareSayisiZ - 1), Quaternion.identity)as GameObject;
         

    }

    void MatrisYazdir()
    {

        for (int i = 0; i < KareSayisiZ; i++)
        {
            for (int j = 0; j < KareSayisiX; j++)
            {
                GoldMatris[i, j] = 0;
            }
        }
        AltinSayisi = (int)(((KareSayisiX * KareSayisiZ) * ihtimal) / 100);
        

        for (int i = 0; i < KareSayisiZ; i++)
        {
            for (int j = 0; j < KareSayisiX; j++)
            {
                GizliAltinMatris[i, j] = 0;
            }
        }
        GizliAltinSayisi = (int)((AltinSayisi * GizliAltinIhtimal) / 100);
        //Debug.Log(GizliAltinSayisi);

        while (AltinSayisi > GizliAltinSayisi )
        {
           
            if (GoldMatris[z = Random.Range(0, KareSayisiZ), x = Random.Range(0, KareSayisiX)] == 0)
            {
                GameObject Gold_Obj=  Instantiate(GoldPrefab, new Vector3(x, 1f, z),Quaternion.Euler(40f,0f,0f)) as GameObject;
                
                AllGoldPrefabs[z, x] = Gold_Obj;

                GoldMatris[z, x] = Random.Range(1, 5) * 5;  //5-10-15-20 üretiyor
                
                AltinSayisi--;
                //Debug.Log("Altinin konumu Z:"+z+" X:"+x);
            }

        }
        while (GizliAltinSayisi > 0)
        {
            if(GoldMatris[z = Random.Range(0, KareSayisiZ), x = Random.Range(0, KareSayisiX)] == 0 && GizliAltinMatris[z, x] == 0)
            {
                //Debug.Log("Gizli altinin konumu Z:" + z + " X:" + x);
                GizliAltinMatris[z, x] = Random.Range(1, 5) * 5;
                GizliAltinSayisi--;
            }

        }

        //for (int i = 0; i < KareSayisiZ; i++)
        //{
        //    for (int j = 0; j < KareSayisiX; j++)
        //    {
        //        if(GizliAltinMatris[i, j] != 0)
        //        {
        //            Debug.Log("["+i+","+j+"]");
        //        }
        //    }
        //}

    }

    void KonumlariAyarla()
    {
        PlayerMatris[0, 0] = OyuncuA;
        OyuncuA.GetComponent<PlayerA>().KonumuAyarla(0, 0);


        PlayerMatris[0, KareSayisiX - 1] = OyuncuB;
        OyuncuB.GetComponent<PlayerB>().KonumuAyarla(0, KareSayisiX - 1);

        PlayerMatris[KareSayisiZ - 1, KareSayisiX - 1] = OyuncuC;
        OyuncuC.GetComponent<PlayerC>().KonumuAyarla(KareSayisiZ - 1, KareSayisiX - 1);

        PlayerMatris[KareSayisiZ - 1, 0] = OyuncuD;
        OyuncuD.GetComponent<PlayerD>().KonumuAyarla(KareSayisiZ - 1, 0);

    }



}
