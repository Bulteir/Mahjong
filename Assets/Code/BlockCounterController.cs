using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlockCounterController : MonoBehaviour
{
    public GameObject GeneralControllers;
    public TMP_Text Counter;
    public GameObject BlockParent;
    public GameObject BlockBoard;

    // Start is called before the first frame update
    void Start()
    {
        RefreshBlockCount();
    }

    public void RefreshBlockCount()
    {
        Counter.text = (BlockParent.transform.childCount + (BlockBoard.transform.childCount - GeneralControllers.GetComponent<BlockBoardController>().BlockSlots.Count)).ToString();
    }
}
