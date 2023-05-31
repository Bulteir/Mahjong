using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Joker_Shuffle : MonoBehaviour
{
    public GameObject generalControllers;
    public Transform BlockParent;
    public TMP_Text QuantityText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnClick()
    {
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kayýtlý save dosyasý varsa
        {
            if (saveFile.shuffleJokerQuantity > 0)
            {
                List<int> blockTypes = new List<int>();

                foreach (Transform block in BlockParent)
                {
                    blockTypes.Add(block.GetComponent<BlockProperties>().blockType);
                }

                for (int i = 0; i < BlockParent.childCount; i++)
                {
                    int randomBlockTypeIndex = Random.Range(0, blockTypes.Count);
                    int randomBlockType = blockTypes[randomBlockTypeIndex];
                    blockTypes.RemoveAt(randomBlockTypeIndex);

                    CreateMaterial(BlockParent.GetChild(i), generalControllers.GetComponent<BlockCreater>().UVs[randomBlockType]);
                    BlockParent.GetChild(i).GetComponent<BlockProperties>().blockType = randomBlockType;
                }
                saveFile.shuffleJokerQuantity--;
                QuantityText.text = saveFile.shuffleJokerQuantity.ToString();
                generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
            }
        }
    }

    void CreateMaterial(Transform block, Texture uv)
    {
        Renderer rend = block.GetComponent<Renderer>();
        rend.material = new Material(Shader.Find("Mobile/VertexLitWithColor"));
        rend.material.mainTexture = uv;
        if (block.GetComponentInChildren<BlockDetector>().IsThereAnyBlockOnItControl())
        {
            rend.material.color = new Color(97f / 255f, 97f / 255f, 97f / 255f);
        }
        else
        {
            rend.material.color = UnityEngine.Color.white;
        }
    }
}
