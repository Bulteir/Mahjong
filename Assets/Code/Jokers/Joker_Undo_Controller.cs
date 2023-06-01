using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Joker_Undo_Controller : MonoBehaviour
{
    public GameObject generalControllers;
    public Transform BlockParent;
    public Transform BlockBoard;
    public TMP_Text QuantityText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void OnClick()
    {
        SaveDataFormat saveFile = generalControllers.GetComponent<LocalSaveLoadController>().LoadGame();
        if (saveFile.saveTime != null)//Kay�tl� save dosyas� varsa
        {
            if (saveFile.undoJokerQuantity > 0 && generalControllers.GetComponent<BlockBoardController>().isSmoothMoveToSnapPointAnimationContinue == false)
            {
                UndoBlock();

                saveFile.undoJokerQuantity--;
                QuantityText.text = saveFile.undoJokerQuantity.ToString();
                generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
            }
            //else//test ama�l�
            //{
            //    saveFile.undoJokerQuantity = GlobalVariables.FirstUndoJokerQuantity;
            //    QuantityText.text = saveFile.undoJokerQuantity.ToString();
            //    generalControllers.GetComponent<LocalSaveLoadController>().SaveGame(saveFile);
            //}
        }
    }

    void UndoBlock()
    {
        int lastPlacedBlockStep = -1;
        int slotIndex = -1;

        List<Transform> blockSlots = generalControllers.GetComponent<BlockBoardController>().BlockSlots;

        #region Istakada yer alan bloklar i�erisinde en son konulan blo�un hangi ad�mda konuldu�unu buluyoruz.
        for (int i = 0; i < blockSlots.Count; i++)
        {
            Transform snappedBlock = blockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock;
            if (snappedBlock != null)
            {
                if (snappedBlock.GetComponent<BlockProperties>().stepOfPlacedBlockBoard > lastPlacedBlockStep)
                {
                    lastPlacedBlockStep = snappedBlock.GetComponent<BlockProperties>().stepOfPlacedBlockBoard;
                    slotIndex = i;
                }
            }
        }
        #endregion

        #region son konulan blok oyuna geri yerle�tirilir
        if (lastPlacedBlockStep > -1)
        {
            Transform block = blockSlots[slotIndex].GetComponent<BlockSlotProperties>().snappedBlock;

            block.SetParent(BlockParent);
            if (BlockParent.childCount > 0) block.rotation = BlockParent.GetChild(0).rotation;
            StartCoroutine(SmoothMoveToSnapPointSimple(block, block.GetComponent<BlockProperties>().positionOfBlockTable));
            block.GetComponent<BlockProperties>().IsSnapped = false;

            //blo�un ��km�� oldu�u blockboard slotunu null yap�yoruz
            generalControllers.GetComponent<BlockBoardController>().BlockSlots[slotIndex].GetComponent<BlockSlotProperties>().snappedBlock = null;
        }
        #endregion

        //�stakadaki bloklar� aralar�nda bo�luk kalmayacak �ekilde terkar yerle�tiriyoruz.
        FixBlockBoardSlotStatus();
    }

    void FixBlockBoardSlotStatus()
    {
        for (int i = 0; i < generalControllers.GetComponent<BlockBoardController>().BlockSlots.Count; i++)
        {
            if (i + 1 < generalControllers.GetComponent<BlockBoardController>().BlockSlots.Count)
            {
                if (generalControllers.GetComponent<BlockBoardController>().BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock == null && generalControllers.GetComponent<BlockBoardController>().BlockSlots[i + 1].GetComponent<BlockSlotProperties>().snappedBlock != null)
                {
                    generalControllers.GetComponent<BlockBoardController>().BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock = generalControllers.GetComponent<BlockBoardController>().BlockSlots[i + 1].GetComponent<BlockSlotProperties>().snappedBlock;
                    StartCoroutine(SmoothMoveToSnapPointSimple(generalControllers.GetComponent<BlockBoardController>().BlockSlots[i + 1].GetComponent<BlockSlotProperties>().snappedBlock, generalControllers.GetComponent<BlockBoardController>().BlockSlots[i].position));
                    generalControllers.GetComponent<BlockBoardController>().BlockSlots[i + 1].GetComponent<BlockSlotProperties>().snappedBlock = null;
                }
            }
        }
    }


    IEnumerator SmoothMoveToSnapPointSimple(Transform block, Vector3 snapPoint)
    {
        generalControllers.GetComponent<BlockBoardController>().isSmoothMoveToSnapPointAnimationContinue = true;
        while (Vector3.Distance(block.position, snapPoint) > 0.05f)
        {
            block.position = Vector3.MoveTowards(block.position, snapPoint, Time.deltaTime * 5);
            yield return null;
        }
        block.position = snapPoint;
        block.GetComponent<Rigidbody>().detectCollisions = true;
        generalControllers.GetComponent<BlockBoardController>().isSmoothMoveToSnapPointAnimationContinue = false;
    }
}
