using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public InputField HamleSayisi;
    public InputField KareSayisiM;
    public InputField KareSayisiN;
    public InputField ihtimal;
    public InputField Gizliihtimal;
    public GameObject playerA;
    public GameObject playerB;
    public GameObject playerC;
    public GameObject playerD;
    public GameObject errorText;
    public GameObject errorText2;
    public int errorCount;

    public void Start()
    {
        PlayerPrefs.SetInt("_GAO", 10);
        PlayerPrefs.SetInt("_HS",3);
        PlayerPrefs.SetInt("_M",20);//Z
        PlayerPrefs.SetInt("_N",20);//X
        PlayerPrefs.SetInt("ihtimal",20);// 20/100
        PlayerPrefs.SetInt("A_Gold", 200);
        PlayerPrefs.SetInt("B_Gold", 200);
        PlayerPrefs.SetInt("C_Gold", 200);
        PlayerPrefs.SetInt("D_Gold", 200);
        PlayerPrefs.SetInt("A_HBM", 5);
        PlayerPrefs.SetInt("B_HBM", 10);
        PlayerPrefs.SetInt("C_HBM", 15);
        PlayerPrefs.SetInt("D_HBM", 20);
        PlayerPrefs.SetInt("A_HM",5);
        PlayerPrefs.SetInt("B_HM", 5);
        PlayerPrefs.SetInt("C_HM", 5);
        PlayerPrefs.SetInt("D_HM", 5);
        errorText.SetActive(false);
        errorText2.SetActive(false);
        errorCount=0;
    }

    public void Basla()
    {
        if (PlayerPrefs.GetInt("_M") <= 0 || PlayerPrefs.GetInt("_N")<=0)
        {
            errorText.SetActive(true);
            errorCount++;
        }
        if(PlayerPrefs.GetInt("_HS")<=0)
        {
            errorText2.SetActive(true);
            errorCount++;
        }
        if (errorCount == 0)
        {
            errorText.SetActive(false);
            errorText2.SetActive(false);
            SceneManager.LoadScene("GameLevel");
        }
        errorCount = 0;
        
    }
   
    public void GizliAltinOrani()
    {

        PlayerPrefs.SetInt("_GAO", Convert.ToInt32(Gizliihtimal.text));

    }

    public void HamleSayisiniAyarla()
    {

        PlayerPrefs.SetInt("_HS", Convert.ToInt32(HamleSayisi.text));

    }

    public void KareSayisiniAyarlaM()
    {

        PlayerPrefs.SetInt("_M", Convert.ToInt32(KareSayisiM.text));

    }
    public void KareSayisiniAyarlaN()
    {

        PlayerPrefs.SetInt("_N", Convert.ToInt32(KareSayisiN.text));

    }
    public void İhtimaliAyarla()
    {

        PlayerPrefs.SetInt("ihtimal", Convert.ToInt32(ihtimal.text));

    }
    public void AltiniAyarlaA()
    {
       
        PlayerPrefs.SetInt("A_Gold", Convert.ToInt32(playerA.transform.GetChild(0).GetComponent<InputField>().text));
        
    }
   public void AltiniAyarlaB()
    {
        PlayerPrefs.SetInt("B_Gold", Convert.ToInt32(playerB.transform.GetChild(0).GetComponent<InputField>().text));
    }
    public void AltiniAyarlaC()
    {
        PlayerPrefs.SetInt("C_Gold", Convert.ToInt32(playerC.transform.GetChild(0).GetComponent<InputField>().text));
    }
   public  void AltiniAyarlaD()
    {
        PlayerPrefs.SetInt("D_Gold", Convert.ToInt32(playerD.transform.GetChild(0).GetComponent<InputField>().text));
    }

    public void HedefMaliyetiBelirleA()
    {

        PlayerPrefs.SetInt("A_HBM", Convert.ToInt32(playerA.transform.GetChild(1).GetComponent<InputField>().text));

    }
    public void HedefMaliyetiBelirleB()
    {

        PlayerPrefs.SetInt("B_HBM", Convert.ToInt32(playerB.transform.GetChild(1).GetComponent<InputField>().text));

    }
    public void HedefMaliyetiBelirleC()
    {

        PlayerPrefs.SetInt("C_HBM", Convert.ToInt32(playerC.transform.GetChild(1).GetComponent<InputField>().text));

    }
    public void HedefMaliyetiBelirleD()
    {

        PlayerPrefs.SetInt("D_HBM", Convert.ToInt32(playerD.transform.GetChild(1).GetComponent<InputField>().text));

    }

    public void HamleMaliyetiA()
    {

        PlayerPrefs.SetInt("A_HM", Convert.ToInt32(playerA.transform.GetChild(2).GetComponent<InputField>().text));

    }

    public void HamleMaliyetiB()
    {

        PlayerPrefs.SetInt("B_HM", Convert.ToInt32(playerB.transform.GetChild(2).GetComponent<InputField>().text));

    }
    public void HamleMaliyetiC()
    {

        PlayerPrefs.SetInt("C_HM", Convert.ToInt32(playerC.transform.GetChild(2).GetComponent<InputField>().text));

    }
    public void HamleMaliyetiD()
    {

        PlayerPrefs.SetInt("D_HM", Convert.ToInt32(playerD.transform.GetChild(2).GetComponent<InputField>().text));

    }


}
