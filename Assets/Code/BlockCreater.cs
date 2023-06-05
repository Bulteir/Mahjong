using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BlockCreater : MonoBehaviour
{
    public List<Texture> UVs;
    public Transform BlockParent;
    public GameObject StarController;

    [Tooltip("B�l�mde yer alan bloklara y�zdesel olarak ka� farkl� blok tipi atamas� yap�ls�n.")]
    [Range(0, 100)]
    public float complexityPercentage;

    // Start is called before the first frame update
    void Start()
    {
        CreateBlocks();
    }

    void CreateBlocks()
    {
        //sahnedeki block say�s� 0'dan b�y�k ve 144'ten k���k olmal� ayr�ca 3'e tam b�l�nmeli
        if (BlockParent.childCount > 0 && BlockParent.childCount <= 144 && BlockParent.childCount % 3 == 0)
        {

            List<int> avaibleBlockIndexes = new List<int>();
            for (int i = 0; i < BlockParent.childCount; i++)
            {
                avaibleBlockIndexes.Add(i);
            }

            //b�l�mde ka� farkl� uv kullan�laca�� verilen y�zdelik oranla hesaplan�r.
            int usableUVCount = Convert.ToInt32(((BlockParent.childCount / 3) * complexityPercentage / 100f));
            if (usableUVCount > 0)
            {
                //birden fazla list yap�s� kullanarak verilen oranda se�ilen blok tiplerinin random fonksiyonunda �st �ste se�ilmesini engelleyerek verilen oran� yakalamak i�in kullan�yoruz.
                List<int> avaibleUVIndexes = GetRandomUVIndexes(usableUVCount);
                List<int> avaibleUVIndexesForCorrection = new List<int>(avaibleUVIndexes);

                int randomUV = -1;
                for (int i = 0; i < BlockParent.childCount; i++)
                {
                    if (avaibleUVIndexesForCorrection.Count == 0)
                    {
                        avaibleUVIndexesForCorrection = new List<int>(avaibleUVIndexes);
                    }

                    //rastgele se�ilen uvlerin her birinin 3 ve katlar� kadar atanmas�n� sa�l�yor
                    if (i % 3 == 0)
                    {
                        randomUV = avaibleUVIndexesForCorrection[Random.Range(0, avaibleUVIndexesForCorrection.Count)];
                        avaibleUVIndexesForCorrection.Remove(randomUV);
                    }

                    int randomBlockIndex = Random.Range(0, avaibleBlockIndexes.Count);
                    CreateMaterial(BlockParent.GetChild(avaibleBlockIndexes[randomBlockIndex]), UVs[randomUV]);

                    //blo�un type'�na uv'nin indexini at�yoruz.
                    BlockParent.GetChild(avaibleBlockIndexes[randomBlockIndex]).GetComponent<BlockProperties>().blockType = randomUV;

                    //rastgele se�ilen bloklar tekrar se�ilmesin diye listeden ��kar�yoruz.
                    avaibleBlockIndexes.RemoveAt(randomBlockIndex);
                }

                StarController.GetComponent<InGame_StarController>().startBlockCount = BlockParent.childCount;
                //maximum ta� say�s� 144. Her blok tipinden en az 3 tane ta� olacak. En zor b�l�m 144 ta� ve 144/3=48 farkl� blok tipi yani %100 karma��kl�k oran� ile olu�ur.
                float levelDifficultyRate = (((BlockParent.childCount / 3) * complexityPercentage) / (float)((144 / 3) * 100)) * 100;
                Debug.Log("B�l�m " + BlockParent.childCount + " tane blok ve " + usableUVCount + " farkl� blok tipi kullan�larak olu�turuldu. \nB�l�m�n toplam zorluk oran�: %" + levelDifficultyRate.ToString("#.00"));
            }
            else
            {
                Debug.LogWarning("Blok tipi oran� yeterli de�il.");
            }
        }
        else
        {
            Debug.LogWarning("B�l�mdeki blok say�s� oyun i�in uygun de�il.");
        }
    }

    void CreateMaterial (Transform block,Texture uv)
    {
        Renderer rend = block.GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Mobile/VertexLitWithColor"));
        rend.material.mainTexture = uv;
        rend.material.color = UnityEngine.Color.white;
    }

    //b�l�m i�erisinde yer alan bloklara aray�zden verid�imiz kar��kl�k y�zdesine g�re ka� farkl� blok tipi olmas� gerekti�inin atamas�
    List<int> GetRandomUVIndexes(int usableUVCount)
    {
        List<int> UVIndex = new List<int>();
        //ge�ici olarak t�m uvlerin indexini al�yoruz.
        List<int> avaibleTextureIndexes = new List<int>();
        for (int i = 0; i < UVs.Count; i++)
        {
            avaibleTextureIndexes.Add(i);
        }

        //ka� farkl� uv se�ilmeli ise burada random �ekilde se�tiriyoruz. Se�ilenin tekrar se�ilmemesini garanti ediyoruz.
        for (int i = 0; i < usableUVCount; i++)
        {
            int randomUVIndex = Random.Range(0, avaibleTextureIndexes.Count);
            UVIndex.Add(avaibleTextureIndexes[randomUVIndex]);
            avaibleTextureIndexes.RemoveAt(randomUVIndex);
        }

        return UVIndex;
    }
}
