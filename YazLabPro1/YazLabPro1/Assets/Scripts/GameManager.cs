using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject OyuncuA;
    public GameObject OyuncuB;
    public GameObject OyuncuC;
    public GameObject OyuncuD;
    public Text GoldA;
    public Text GoldB;
    public Text GoldC;
    public Text GoldD;
    public GameObject sonucPanel;
    public Text tasA;
    public Text hamA;
    public Text tamA;
    public Text tasB;
    public Text hamB;
    public Text tamB;
    public Text tasC;
    public Text hamC;
    public Text tamC;
    public Text tasD;
    public Text hamD;
    public Text tamD;
    public Text KasaAltinMiktari;
    public GameObject createGameBoard;
    private void Start()
    {
        sonucPanel.SetActive(false);
        OyuncuA = GameObject.Find("PlayerA(Clone)");
        OyuncuB = GameObject.Find("PlayerB(Clone)");
        OyuncuC = GameObject.Find("PlayerC(Clone)");
        OyuncuD = GameObject.Find("PlayerD(Clone)");
        GoldA.text = OyuncuA.GetComponent<PlayerA>().Gold.ToString();
        GoldB.text = OyuncuB.GetComponent<PlayerB>().Gold.ToString();
        GoldC.text = OyuncuC.GetComponent<PlayerC>().Gold.ToString();
        GoldD.text = OyuncuD.GetComponent<PlayerD>().Gold.ToString();
    }
    private void FixedUpdate()
    {
        GoldA.text = OyuncuA.GetComponent<PlayerA>().Gold.ToString();
        GoldB.text = OyuncuB.GetComponent<PlayerB>().Gold.ToString();
        GoldC.text = OyuncuC.GetComponent<PlayerC>().Gold.ToString();
        GoldD.text = OyuncuD.GetComponent<PlayerD>().Gold.ToString();
    }
    public void SetResultPanel()
    {
        tasA.text = OyuncuA.GetComponent<PlayerA>().ToplamAdimSayisi.ToString();
        hamA.text = OyuncuA.GetComponent<PlayerA>().ToplamHarcananAltinsayisi.ToString();
        tamA.text = OyuncuA.GetComponent<PlayerA>().ToplananAltinSayisi.ToString();

        tasB.text = OyuncuB.GetComponent<PlayerB>().ToplamAdimSayisi.ToString();
        hamB.text = OyuncuB.GetComponent<PlayerB>().ToplamHarcananAltinsayisi.ToString();
        tamB.text = OyuncuB.GetComponent<PlayerB>().ToplananAltinSayisi.ToString();

        tasC.text = OyuncuC.GetComponent<PlayerC>().ToplamAdimSayisi.ToString();
        hamC.text = OyuncuC.GetComponent<PlayerC>().ToplamHarcananAltinsayisi.ToString();
        tamC.text = OyuncuC.GetComponent<PlayerC>().ToplananAltinSayisi.ToString();

        tasD.text = OyuncuD.GetComponent<PlayerD>().ToplamAdimSayisi.ToString();
        hamD.text = OyuncuD.GetComponent<PlayerD>().ToplamHarcananAltinsayisi.ToString();
        tamD.text = OyuncuD.GetComponent<PlayerD>().ToplananAltinSayisi.ToString();

        KasaAltinMiktari.text = createGameBoard.GetComponent<CreateGameBoard>().KasadakiToplamAltin.ToString();

        sonucPanel.SetActive(true);
    }

}
