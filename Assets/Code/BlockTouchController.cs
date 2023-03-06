using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTouchController : MonoBehaviour
{
    //bool isSmoothMoveToSnapPointAnimationContinue = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            ControlWithMouse();
        }
        else
        {
            //ControlWithTouch();
        }
    }

    #region mouse ile edit�rde kontol etme.
    void ControlWithMouse()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                if (raycastHit.collider.gameObject.tag == GlobalVariables.TagBlock
                    && raycastHit.collider.gameObject.GetComponent<BlockProperties>().IsThereAnyBlockOnIt == false
                    && raycastHit.collider.gameObject.GetComponent<BlockProperties>().IsSnapped == false
                    )
                {

                    transform.GetComponent<BlockBoardController>().PlaceBlock(raycastHit.collider.transform);
                    //int avaibleSlotIndex = transform.GetComponent<BlockBoardController>().GetAvaibleBlockSlotIndex();
                    //if (avaibleSlotIndex != -1) //bloklar� �stakaya yerle�tir
                    //{
                    //    //se�ilen blok �skaya yerle�tirilir.
                    //    //StartCoroutine(SmoothMoveToSnapPoint(raycastHit.collider.transform, transform.GetComponent<BlockBoardController>().BlockSlots[avaibleSlotIndex], avaibleSlotIndex));
                    //}
                }
            }
        }
    }
    #endregion

    //IEnumerator SmoothMoveToSnapPoint(Transform block, Transform snapPoint, int avaibleSlotIndex)
    //{
    //    isSmoothMoveToSnapPointAnimationContinue = true;
    //    block.GetComponent<Rigidbody>().detectCollisions = false;

    //    while (Vector3.Distance(block.position, snapPoint.position) > 0.05f)
    //    {
    //        block.position = Vector3.MoveTowards(block.position, snapPoint.position, Time.deltaTime * 5);
    //        yield return null;
    //    }
    //    block.position = snapPoint.position;

    //    //�stakada yer alan slota blo�un kendisi setlernir.
    //    transform.GetComponent<BlockBoardController>().BlockSlots[avaibleSlotIndex].transform.GetComponent<BlockSlotProperties>().snappedBlock = block;
    //    block.GetComponent<BlockProperties>().IsSnapped = true;

    //    //�staka �zerine yerle�tirilen blocklar kontrol edilir/patlat�l�r vs...
    //    transform.GetComponent<BlockBoardController>().FillBlockBoardStatus();

    //    //�staka �zerinde kalan bo� slotlar aran�r
    //    avaibleSlotIndex = transform.GetComponent<BlockBoardController>().GetAvaibleBlockSlotIndex();
    //    if (avaibleSlotIndex == -1)//-1 ise slotlar dolmu�tur game over olur.
    //    {
    //        Debug.Log("Game Over");
    //    }
    //    isSmoothMoveToSnapPointAnimationContinue = false;
    //}
}
