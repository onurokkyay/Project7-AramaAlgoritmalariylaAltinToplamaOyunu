using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerD : MonoBehaviour, PlayerInterface
{
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

    int i, j;
    int toplam;

    public GameObject Board;
    private void Awake()
    {

        Board = GameObject.FindGameObjectWithTag("Board");//Starta koyunca olmuyor
        HedefKonumX = -10;//En başta hedefi yok
        HedefKonumZ = -10;//En başta hedefi yok
        Gold = PlayerPrefs.GetInt("D_Gold");
        HedefBelirlemeMaliyeti = PlayerPrefs.GetInt("D_HBM");
        HamleMaliyeti = PlayerPrefs.GetInt("D_HM");
        HamleSayisi = PlayerPrefs.GetInt("_HS");

    }


    public void HamleYap()
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
        //Debug.Log("While bitti");

    }

    public void HedefBelirle(int[,] goldMatris, int AltinSayisi, int X, int Z)
    {
        TümAltinSayisi = Board.GetComponent<CreateGameBoard>().AltinSayisi;
        TümAltinSayisi -= Board.GetComponent<CreateGameBoard>().GizliAltinSayisi;


        while (AltinSayisi > 0)
        {
            for (int i = 0; i < Z; i++)
            {
                for (int j = 0; i < X; i++)
                {
                    if (goldMatris[i, j] == 1) //Eğer altın varsa
                    {

                        if (Mathf.Abs(i - MatrisKonumZ) + Mathf.Abs(j - MatrisKonumX) < Mathf.Abs(HedefKonumZ - MatrisKonumZ) + Mathf.Abs(HedefKonumX - MatrisKonumX))
                        {

                            HedefKonumZ = i;
                            HedefKonumX = j;

                        }
                        AltinSayisi--;
                    }

                }
            }
            //Debug.Log("en yakin hedefin Z si:" + i + " X i:" + j);

        }

        //en yakını bulma kısmı..
        //en yakini bulduktan sonra
        HedefKonumZ = i;
        HedefKonumX = j;
        Gold -= HedefBelirlemeMaliyeti;
    }

    public void HedefeUlasildimi()
    {

    }

    public void OyundanElendiMi()
    {

    }

    public void SagaGit()
    {
        //Debug.Log("D Sağa gitti");
        this.MatrisKonumX++;
        this.gameObject.transform.position = new Vector3(this.MatrisKonumX, 1f, this.MatrisKonumZ);
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
        if (Board.GetComponent<CreateGameBoard>().GizliAltinMatris[MatrisKonumZ, MatrisKonumX] != 0)
        {
            Board.GetComponent<CreateGameBoard>().GoldMatris[MatrisKonumZ, MatrisKonumX] = Board.GetComponent<CreateGameBoard>().GizliAltinMatris[MatrisKonumZ, MatrisKonumX];
            Board.GetComponent<CreateGameBoard>().GizliAltinMatris[MatrisKonumZ, MatrisKonumX] = 0;
            GameObject Gold_Obj = Instantiate(Board.GetComponent<CreateGameBoard>().GoldPrefab, new Vector3(MatrisKonumX, 1f, MatrisKonumZ), Quaternion.Euler(40f, 0f, 0f));
            Board.GetComponent<CreateGameBoard>().AllGoldPrefabs[MatrisKonumZ, MatrisKonumX] = Gold_Obj;
            Board.GetComponent<CreateGameBoard>().GizliAltinSayisi--;
        }

    }

    public void KonumuAyarla(int i, int j)
    {
        this.MatrisKonumZ = i;
        this.MatrisKonumX = j;
    }
}
