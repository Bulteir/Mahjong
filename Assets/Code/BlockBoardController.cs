using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class BlockBoardController : MonoBehaviour
{
    class BlockSlotStruct
    {
        public Transform slot;
        public int index;
        public Transform snappedBlock;
        public int snappedBlockType;
    }

    List<BlockSlotStruct> blockSlotsStruckList = new List<BlockSlotStruct>();
    public List<Transform> BlockSlots = new List<Transform>();
    public bool isSmoothMoveToSnapPointAnimationContinue = false;
    public Transform blockBoard;
    public Transform blockParent;
    public Transform blockCounter;

    public void PlaceBlock(Transform SelectedBlock)
    {
        if (isSmoothMoveToSnapPointAnimationContinue == false)
        {
            //�stakan�n durumunu al�yoruz.
            FillBlockBoardStatus();
            int avaibleSlotIndex = GetAvaibleBlockSlotIndex(SelectedBlock);
            if (avaibleSlotIndex != -1) //bloklar� �stakaya yerle�tir
            {
                ReplaceBlocks(SelectedBlock, avaibleSlotIndex);
            }
        }
    }

    #region se�ilen blo�un yerle�mesi gereken indexi d�nd�r�r
    //-1 d�nerse �stakada hi� yer yok demek. -1 d���nda gelen herhangi bir de�er se�ilen blo�un konulmas� gereken indextir.
    //Ancak bu indexte ba�ka blok olabilir. Varsa di�er bloklar�n kayr�lmas� gerekmektedir.
    int GetAvaibleBlockSlotIndex(Transform selectedBlock)
    {
        int index = -1;

        for (int i = 0; i < blockSlotsStruckList.Count; i++)
        {
            //Bir blok se�ildi�inde, varsa �stakada kendi cinsinden ilk blo�un indexini buluyoruz.
            if (blockSlotsStruckList[i].snappedBlock != null && blockSlotsStruckList[i].snappedBlockType == selectedBlock.GetComponent<BlockProperties>().blockType)
            {
                if ((i + 1) < blockSlotsStruckList.Count)
                {
                    //�stakada se�ilen blok tipinden bir tane daha blok var ve e�le�ti�i blokatan sonra ba�ka bir blok var.
                    if (blockSlotsStruckList[i + 1].snappedBlock != null)
                    {
                        //ilk e�lemeden sonraki elaman yine kendi tipindemi kontrol ediliyor. Yani se�ilen bloktan �nce �stakada kendi tipinden 2 elaman var m� kontrol�
                        if (blockSlotsStruckList[i + 1].snappedBlockType == selectedBlock.GetComponent<BlockProperties>().blockType)
                        {
                            if ((i + 2) < blockSlotsStruckList.Count)//kendisinden �nce 2 tane daha ayn� tip blok vard�r.
                            {
                                index = i + 2;
                                break;
                            }
                            else //�stakada yer yoktur.
                            {
                                index = -1;
                                break;
                            }
                        }
                        else //�stakada se�ilen blok tipinden bir tane daha blok var. Ancak e�le�en bloktan sonra ba�ka tipde bloka var.
                        {
                            index = i + 1;
                            break;
                        }
                    }
                    else//�stakada se�ilen blok tipinden bir tane daha blok var ve sonuncu s�rada ayr�ca �stakada yer var.
                    {
                        index = i + 1;
                        break;
                    }
                }
                else
                {
                    //�stakada yer yoktur. E�le�ti�i blok �stakan�n sonuncu eleman�d�r.
                    index = -1;
                    break;
                }
            }
            else if (blockSlotsStruckList[i].snappedBlock == null)//�stakada bir s�r� blok var ancak se�ilen blok tipinden hi� blok yok ya da �staka tamamen bo�.
            {
                index = i;
                break;
            }
        }

        return index;
    }
    #endregion

    //�staka �zerindeki son durumu toplar
    void FillBlockBoardStatus()
    {
        blockSlotsStruckList.Clear();
        for (int i = 0; i < BlockSlots.Count; i++)
        {
            BlockSlotStruct slot = new BlockSlotStruct()
            {
                slot = BlockSlots[i],
                index = i,
                snappedBlock = BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock ?? null,
                snappedBlockType = BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock == null ? -1 : BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock.GetComponent<BlockProperties>().blockType
            };
            blockSlotsStruckList.Add(slot);
        }
        
        BlockMatchControl();
    }

    //3 tane ayn� bloktan varsa patlat�r
    void BlockMatchControl()
    {
        for (int i = 0; i < blockSlotsStruckList.Count; i++)
        {
            if (blockSlotsStruckList[i].snappedBlock != null)
            {
                List<BlockSlotStruct> MatchedBlocks = blockSlotsStruckList.FindAll(x => x.snappedBlockType.Equals(blockSlotsStruckList[i].snappedBlockType));

                if (MatchedBlocks.Count == 3)
                {
                    foreach (BlockSlotStruct block in MatchedBlocks)
                    {
                        if (block.snappedBlock != null)
                        {
                            Destroy(block.snappedBlock.gameObject);
                        }

                        block.snappedBlock = null;
                        block.snappedBlockType = -1;
                    }

                    #region patlamas� gereken bloklar patlad�ktan sonra geriye kalan bloklar aralar�nda bo�luk kalmayacak �ekilde tekrar yerle�tirilir.

                    for (int x = 0; x < blockSlotsStruckList.Count; x++)
                    {
                        if (blockSlotsStruckList[x].snappedBlock == null)
                        {
                            for (int y = x; y < blockSlotsStruckList.Count; y++)
                            {
                                if (blockSlotsStruckList[y].snappedBlock != null)
                                {
                                    StartCoroutine(SmoothMoveToSnapPointSimple(blockSlotsStruckList[y].snappedBlock, blockSlotsStruckList[x].slot, blockSlotsStruckList[x].index));

                                    //bulunan dolu slotu bo�alan slota at�yoruz.
                                    blockSlotsStruckList[blockSlotsStruckList[x].index].snappedBlock = blockSlotsStruckList[y].snappedBlock;
                                    blockSlotsStruckList[blockSlotsStruckList[x].index].snappedBlockType = blockSlotsStruckList[y].snappedBlockType;

                                    blockSlotsStruckList[y].snappedBlock = null;
                                    BlockSlots[y].GetComponent<BlockSlotProperties>().snappedBlock = null;
                                    blockSlotsStruckList[y].snappedBlockType = -1;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                    break;
                }
            }
        }
    }

    //bir blo�u uygun yere smooth �ekilde yerle�mesini sa�lar
    IEnumerator SmoothMoveToSnapPoint(Transform block, Transform snapPoint, int avaibleSlotIndex)
    {
        isSmoothMoveToSnapPointAnimationContinue = true;
        block.GetComponent<Rigidbody>().detectCollisions = false;

        block.rotation = snapPoint.rotation;
        while (Vector3.Distance(block.position, snapPoint.position) > 0.05f)
        {
            block.position = Vector3.MoveTowards(block.position, snapPoint.position, Time.deltaTime * 5);
            yield return null;
        }
        block.position = snapPoint.position;

        //�stakada yer alan slota blo�un kendisi setlernir.
        BlockSlots[avaibleSlotIndex].transform.GetComponent<BlockSlotProperties>().snappedBlock = block;
        block.GetComponent<BlockProperties>().IsSnapped = true;
        block.SetParent(blockBoard);

        ////�staka �zerine yerle�tirilen blocklar kontrol edilir/patlat�l�r vs...
        FillBlockBoardStatus();
        yield return null;
        blockCounter.GetComponent<BlockCounterController>().RefreshBlockCount();

        //her yerle�tirilen bloktan sonra oyununn bitip bitmed�i kontrol edilir.
        if (IsGameOver())
        {
            GlobalVariables.gameState = GlobalVariables.gameState_gameOver;
            Debug.Log("Game Over");
        }
        else if (IsGameWon())
        {
            GlobalVariables.gameState = GlobalVariables.gameState_gameOver;
            Debug.Log("You Won");
        }
        
        isSmoothMoveToSnapPointAnimationContinue = false;
    }

    IEnumerator SmoothMoveToSnapPointSimple(Transform block, Transform snapPoint, int avaibleSlotIndex)
    {
        while (Vector3.Distance(block.position, snapPoint.position) > 0.05f)
        {
            block.position = Vector3.MoveTowards(block.position, snapPoint.position, Time.deltaTime * 5);
            yield return null;
        }
        block.position = snapPoint.position;

        //�stakada yer alan slota blo�un kendisi setlernir.
        BlockSlots[avaibleSlotIndex].transform.GetComponent<BlockSlotProperties>().snappedBlock = block;
        block.GetComponent<BlockProperties>().IsSnapped = true;
        block.SetParent(blockBoard);
    }

    //se�ilen blo�un yerle�mesi gerken index bulunduktan sonra o blo�un uygun indexe yerle�mesini sa�lar. Gerekiyorsa di�er bloklar� birer kayd�r�r.
    void ReplaceBlocks(Transform SelectedBlock, int avaibleSlotIndex)
    {
        if (blockSlotsStruckList[avaibleSlotIndex].snappedBlock != null) //se�ilen blo�un yerle�mesi gereken indexte ba�ka blok vard�r.
        {
            for (int i = avaibleSlotIndex; i < blockSlotsStruckList.Count; i++)
            {
                if (blockSlotsStruckList[i].snappedBlock != null)
                {
                    StartCoroutine(SmoothMoveToSnapPointSimple(blockSlotsStruckList[i].snappedBlock, blockSlotsStruckList[i + 1].slot, i + 1));
                }
            }
            StartCoroutine(SmoothMoveToSnapPoint(SelectedBlock, BlockSlots[avaibleSlotIndex], avaibleSlotIndex));
        }
        else // se�ilen blo�un yerle�mesi gereken index bo�tur.
        {
            StartCoroutine(SmoothMoveToSnapPoint(SelectedBlock, BlockSlots[avaibleSlotIndex], avaibleSlotIndex));
        }
    }

    bool IsGameOver()
    {
        bool result = true;

        foreach (BlockSlotStruct slot in blockSlotsStruckList)
        {
            if (slot.snappedBlock == null)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    bool IsGameWon()
    {
        bool result = false;

        if (blockParent.childCount == 0)
            result = true;

        return result;
    }
}
