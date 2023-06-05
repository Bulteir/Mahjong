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

    [Tooltip("Bölümde yer alan bloklara yüzdesel olarak kaç farklý blok tipi atamasý yapýlsýn.")]
    [Range(0, 100)]
    public float complexityPercentage;

    // Start is called before the first frame update
    void Start()
    {
        CreateBlocks();
    }

    void CreateBlocks()
    {
        //sahnedeki block sayýsý 0'dan büyük ve 144'ten küçük olmalý ayrýca 3'e tam bölünmeli
        if (BlockParent.childCount > 0 && BlockParent.childCount <= 144 && BlockParent.childCount % 3 == 0)
        {

            List<int> avaibleBlockIndexes = new List<int>();
            for (int i = 0; i < BlockParent.childCount; i++)
            {
                avaibleBlockIndexes.Add(i);
            }

            //bölümde kaç farklý uv kullanýlacaðý verilen yüzdelik oranla hesaplanýr.
            int usableUVCount = Convert.ToInt32(((BlockParent.childCount / 3) * complexityPercentage / 100f));
            if (usableUVCount > 0)
            {
                //birden fazla list yapýsý kullanarak verilen oranda seçilen blok tiplerinin random fonksiyonunda üst üste seçilmesini engelleyerek verilen oraný yakalamak için kullanýyoruz.
                List<int> avaibleUVIndexes = GetRandomUVIndexes(usableUVCount);
                List<int> avaibleUVIndexesForCorrection = new List<int>(avaibleUVIndexes);

                int randomUV = -1;
                for (int i = 0; i < BlockParent.childCount; i++)
                {
                    if (avaibleUVIndexesForCorrection.Count == 0)
                    {
                        avaibleUVIndexesForCorrection = new List<int>(avaibleUVIndexes);
                    }

                    //rastgele seçilen uvlerin her birinin 3 ve katlarý kadar atanmasýný saðlýyor
                    if (i % 3 == 0)
                    {
                        randomUV = avaibleUVIndexesForCorrection[Random.Range(0, avaibleUVIndexesForCorrection.Count)];
                        avaibleUVIndexesForCorrection.Remove(randomUV);
                    }

                    int randomBlockIndex = Random.Range(0, avaibleBlockIndexes.Count);
                    CreateMaterial(BlockParent.GetChild(avaibleBlockIndexes[randomBlockIndex]), UVs[randomUV]);

                    //bloðun type'ýna uv'nin indexini atýyoruz.
                    BlockParent.GetChild(avaibleBlockIndexes[randomBlockIndex]).GetComponent<BlockProperties>().blockType = randomUV;

                    //rastgele seçilen bloklar tekrar seçilmesin diye listeden çýkarýyoruz.
                    avaibleBlockIndexes.RemoveAt(randomBlockIndex);
                }

                StarController.GetComponent<InGame_StarController>().startBlockCount = BlockParent.childCount;
                //maximum taþ sayýsý 144. Her blok tipinden en az 3 tane taþ olacak. En zor bölüm 144 taþ ve 144/3=48 farklý blok tipi yani %100 karmaþýklýk oraný ile oluþur.
                float levelDifficultyRate = (((BlockParent.childCount / 3) * complexityPercentage) / (float)((144 / 3) * 100)) * 100;
                Debug.Log("Bölüm " + BlockParent.childCount + " tane blok ve " + usableUVCount + " farklý blok tipi kullanýlarak oluþturuldu. \nBölümün toplam zorluk oraný: %" + levelDifficultyRate.ToString("#.00"));
            }
            else
            {
                Debug.LogWarning("Blok tipi oraný yeterli deðil.");
            }
        }
        else
        {
            Debug.LogWarning("Bölümdeki blok sayýsý oyun için uygun deðil.");
        }
    }

    void CreateMaterial (Transform block,Texture uv)
    {
        Renderer rend = block.GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Mobile/VertexLitWithColor"));
        rend.material.mainTexture = uv;
        rend.material.color = UnityEngine.Color.white;
    }

    //bölüm içerisinde yer alan bloklara arayüzden veridðimiz karþýklýk yüzdesine göre kaç farklý blok tipi olmasý gerektiðinin atamasý
    List<int> GetRandomUVIndexes(int usableUVCount)
    {
        List<int> UVIndex = new List<int>();
        //geçici olarak tüm uvlerin indexini alýyoruz.
        List<int> avaibleTextureIndexes = new List<int>();
        for (int i = 0; i < UVs.Count; i++)
        {
            avaibleTextureIndexes.Add(i);
        }

        //kaç farklý uv seçilmeli ise burada random þekilde seçtiriyoruz. Seçilenin tekrar seçilmemesini garanti ediyoruz.
        for (int i = 0; i < usableUVCount; i++)
        {
            int randomUVIndex = Random.Range(0, avaibleTextureIndexes.Count);
            UVIndex.Add(avaibleTextureIndexes[randomUVIndex]);
            avaibleTextureIndexes.RemoveAt(randomUVIndex);
        }

        return UVIndex;
    }
}
