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
        //sahnedeki blokc say�s� 0dan b�y�k ve 144'ten k���k olmal� ayr�ca 3'e tam b�l�nmeli
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
                //rastgele se�ilen uvlerin her birinin 3 ve katlar� kadar atanmas�n� sa�l�yor
                if (i % 3 == 0)
                {
                    randomUV = Random.Range(0, UVs.Count);
                }

                int randomBlockIndex = Random.Range(0, avaibleBlockIndexes.Count);
                CreateMaterial(BlockParent.GetChild(avaibleBlockIndexes[randomBlockIndex]), UVs[randomUV]);

                //blo�un type'�na uv'nin indexini at�yoruz.
                BlockParent.GetChild(avaibleBlockIndexes[randomBlockIndex]).GetComponent<BlockProperties>().blockType = randomUV;

                //rastgele se�ilen bloklar tekrar se�ilmesin diye listeden ��kar�yoruz.
                avaibleBlockIndexes.RemoveAt(randomBlockIndex);
            }
        }
        else
        {
            Debug.LogWarning("Sahnedeki blok say�s� oyun i�in uygun de�il.");
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
