using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerInterface 
{
    
    void HamleYap(); //Hareket etmesini sağlar
    void HedefBelirle(int Z,int X);//Kendi kuralına göre hedef belirler
    void SagaGit();
    void SolaGit();
    void YukariGit();
    void AsagiGit();
    void HedefeUlasildimi();

    void KonumuAyarla(int i,int j);


}
