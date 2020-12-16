using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerInterface 
{
    
    void HamleYap(); //Hareket etmesini sağlar
    void HedefBelirle();//Kendi kuralına göre hedef belirler
    void SagaGit();
    void SolaGit();
    void YukariGit();
    void AsagiGit();
    void HedefeUlasildimi();
    void OyundanElendiMi();


}
