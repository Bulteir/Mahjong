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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetAvaibleBlockSlotIndex()
    {
        int index = -1;
        for (int i = 0; i < BlockSlots.Count; i++)
        {
            if (BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock == null)
            {
                index = i;
                break;
            }
        }

        return index;
    }

    public void FillBlockBoardStatus()
    {
        blockSlotsStruckList.Clear();
        for (int i = 0; i < BlockSlots.Count; i++)
        {
            BlockSlotStruct slot = new BlockSlotStruct()
            {
                slot = BlockSlots[i],
                index = i,
                snappedBlock = BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock ?? null,
                snappedBlockType = BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock == null ? -1: BlockSlots[i].GetComponent<BlockSlotProperties>().snappedBlock.GetComponent<BlockProperties>().blockType
            };
            blockSlotsStruckList.Add(slot);
        }

        BlockMatchControl();
    }

    void BlockMatchControl()
    {
        for (int i = 0; i < blockSlotsStruckList.Count; i++)
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
            }
        }
    }

    public void PlaceBlock()
    {

    }

}
