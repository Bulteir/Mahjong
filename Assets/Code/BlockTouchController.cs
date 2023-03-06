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

    #region mouse ile editörde kontol etme.
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
                    //if (avaibleSlotIndex != -1) //bloklarý ýstakaya yerleþtir
                    //{
                    //    //seçilen blok ýskaya yerleþtirilir.
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

    //    //ýstakada yer alan slota bloðun kendisi setlernir.
    //    transform.GetComponent<BlockBoardController>().BlockSlots[avaibleSlotIndex].transform.GetComponent<BlockSlotProperties>().snappedBlock = block;
    //    block.GetComponent<BlockProperties>().IsSnapped = true;

    //    //ýstaka üzerine yerleþtirilen blocklar kontrol edilir/patlatýlýr vs...
    //    transform.GetComponent<BlockBoardController>().FillBlockBoardStatus();

    //    //ýstaka üzerinde kalan boþ slotlar aranýr
    //    avaibleSlotIndex = transform.GetComponent<BlockBoardController>().GetAvaibleBlockSlotIndex();
    //    if (avaibleSlotIndex == -1)//-1 ise slotlar dolmuþtur game over olur.
    //    {
    //        Debug.Log("Game Over");
    //    }
    //    isSmoothMoveToSnapPointAnimationContinue = false;
    //}
}
