using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BlockCreater : MonoBehaviour
{
    public List<Texture> UVs;
    public Transform BlockParent;

    // Start is called before the first frame update
    void Start()
    {
        CreateBlocks();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateBlocks()
    {
        //sahnedeki blokc sayýsý 0dan büyük ve 144'ten küçük olmalý ayrýca 3'e tam bölünmeli
        if (BlockParent.childCount > 0 && BlockParent.childCount <= 144 && BlockParent.childCount % 3 == 0)
        {
            List<int> avaibleBlockIndexes = new List<int>();
            for (int i = 0; i < BlockParent.childCount; i++)
            {
                avaibleBlockIndexes.Add(i);
            }

            int randomUV = Random.Range(0, UVs.Count);
            for (int i = 0; i < BlockParent.childCount; i++)
            {
                //rastgele seçilen uvlerin her birinin 3 ve katlarý kadar atanmasýný saðlýyor
                if (i % 3 == 0)
                {
                    randomUV = Random.Range(0, UVs.Count);
                }

                int randomBlockIndex = Random.Range(0, avaibleBlockIndexes.Count);
                CreateMaterial(BlockParent.GetChild(avaibleBlockIndexes[randomBlockIndex]), UVs[randomUV]);

                //bloðun type'ýna uv'nin indexini atýyoruz.
                BlockParent.GetChild(avaibleBlockIndexes[randomBlockIndex]).GetComponent<BlockProperties>().blockType = randomUV;

                //rastgele seçilen bloklar tekrar seçilmesin diye listeden çýkarýyoruz.
                avaibleBlockIndexes.RemoveAt(randomBlockIndex);
            }
        }
        else
        {
            Debug.LogWarning("Sahnedeki blok sayýsý oyun için uygun deðil.");
        }
    }

    void CreateMaterial (Transform block,Texture uv)
    {
        Renderer rend = block.GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Mobile/VertexLitWithColor"));
        rend.material.mainTexture = uv;
        rend.material.color = UnityEngine.Color.white;
    }
}
