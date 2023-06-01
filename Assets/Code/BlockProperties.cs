using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockProperties : MonoBehaviour
{
    public bool IsThereAnyBlockOnIt;
    public bool IsSnapped;
    public int blockType = -1;
    public int stepOfPlacedBlockBoard = -1;
    public Vector3 positionOfBlockTable;
}
