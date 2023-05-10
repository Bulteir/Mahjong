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
            //ýstakanýn durumunu alýyoruz.
            FillBlockBoardStatus();
            int avaibleSlotIndex = GetAvaibleBlockSlotIndex(SelectedBlock);
            if (avaibleSlotIndex != -1) //bloklarý ýstakaya yerleþtir
            {
                ReplaceBlocks(SelectedBlock, avaibleSlotIndex);
            }
        }
    }

    #region seçilen bloðun yerleþmesi gereken indexi döndürür
    //-1 dönerse ýstakada hiç yer yok demek. -1 dýþýnda gelen herhangi bir deðer seçilen bloðun konulmasý gereken indextir.
    //Ancak bu indexte baþka blok olabilir. Varsa diðer bloklarýn kayrýlmasý gerekmektedir.
    int GetAvaibleBlockSlotIndex(Transform selectedBlock)
    {
        int index = -1;

        for (int i = 0; i < blockSlotsStruckList.Count; i++)
        {
            //Bir blok seçildiðinde, varsa ýstakada kendi cinsinden ilk bloðun indexini buluyoruz.
            if (blockSlotsStruckList[i].snappedBlock != null && blockSlotsStruckList[i].snappedBlockType == selectedBlock.GetComponent<BlockProperties>().blockType)
            {
                if ((i + 1) < blockSlotsStruckList.Count)
                {
                    //ýstakada seçilen blok tipinden bir tane daha blok var ve eþleþtiði blokatan sonra baþka bir blok var.
                    if (blockSlotsStruckList[i + 1].snappedBlock != null)
                    {
                        //ilk eþlemeden sonraki elaman yine kendi tipindemi kontrol ediliyor. Yani seçilen bloktan önce ýstakada kendi tipinden 2 elaman var mý kontrolü
                        if (blockSlotsStruckList[i + 1].snappedBlockType == selectedBlock.GetComponent<BlockProperties>().blockType)
                        {
                            if ((i + 2) < blockSlotsStruckList.Count)//kendisinden önce 2 tane daha ayný tip blok vardýr.
                            {
                                index = i + 2;
                                break;
                            }
                            else //ýstakada yer yoktur.
                            {
                                index = -1;
                                break;
                            }
                        }
                        else //ýstakada seçilen blok tipinden bir tane daha blok var. Ancak eþleþen bloktan sonra baþka tipde bloka var.
                        {
                            index = i + 1;
                            break;
                        }
                    }
                    else//ýstakada seçilen blok tipinden bir tane daha blok var ve sonuncu sýrada ayrýca ýstakada yer var.
                    {
                        index = i + 1;
                        break;
                    }
                }
                else
                {
                    //ýstakada yer yoktur. Eþleþtiði blok ýstakanýn sonuncu elemanýdýr.
                    index = -1;
                    break;
                }
            }
            else if (blockSlotsStruckList[i].snappedBlock == null)//ýstakada bir sürü blok var ancak seçilen blok tipinden hiç blok yok ya da ýstaka tamamen boþ.
            {
                index = i;
                break;
            }
        }

        return index;
    }
    #endregion

    //ýstaka üzerindeki son durumu toplar
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

    //3 tane ayný bloktan varsa patlatýr
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

                    #region patlamasý gereken bloklar patladýktan sonra geriye kalan bloklar aralarýnda boþluk kalmayacak þekilde tekrar yerleþtirilir.

                    for (int x = 0; x < blockSlotsStruckList.Count; x++)
                    {
                        if (blockSlotsStruckList[x].snappedBlock == null)
                        {
                            for (int y = x; y < blockSlotsStruckList.Count; y++)
                            {
                                if (blockSlotsStruckList[y].snappedBlock != null)
                                {
                                    StartCoroutine(SmoothMoveToSnapPointSimple(blockSlotsStruckList[y].snappedBlock, blockSlotsStruckList[x].slot, blockSlotsStruckList[x].index));

                                    //bulunan dolu slotu boþalan slota atýyoruz.
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

    //bir bloðu uygun yere smooth þekilde yerleþmesini saðlar
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

        //ýstakada yer alan slota bloðun kendisi setlernir.
        BlockSlots[avaibleSlotIndex].transform.GetComponent<BlockSlotProperties>().snappedBlock = block;
        block.GetComponent<BlockProperties>().IsSnapped = true;
        block.SetParent(blockBoard);

        ////ýstaka üzerine yerleþtirilen blocklar kontrol edilir/patlatýlýr vs...
        FillBlockBoardStatus();
        yield return null;
        blockCounter.GetComponent<BlockCounterController>().RefreshBlockCount();

        //her yerleþtirilen bloktan sonra oyununn bitip bitmedði kontrol edilir.
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

        //ýstakada yer alan slota bloðun kendisi setlernir.
        BlockSlots[avaibleSlotIndex].transform.GetComponent<BlockSlotProperties>().snappedBlock = block;
        block.GetComponent<BlockProperties>().IsSnapped = true;
        block.SetParent(blockBoard);
    }

    //seçilen bloðun yerleþmesi gerken index bulunduktan sonra o bloðun uygun indexe yerleþmesini saðlar. Gerekiyorsa diðer bloklarý birer kaydýrýr.
    void ReplaceBlocks(Transform SelectedBlock, int avaibleSlotIndex)
    {
        if (blockSlotsStruckList[avaibleSlotIndex].snappedBlock != null) //seçilen bloðun yerleþmesi gereken indexte baþka blok vardýr.
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
        else // seçilen bloðun yerleþmesi gereken index boþtur.
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
