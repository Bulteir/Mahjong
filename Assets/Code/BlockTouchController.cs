using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTouchController : MonoBehaviour
{
    public Transform BlockTable;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            if (Application.isEditor)
            {
                ControlWithMouse();
            }
            else
            {
                ControlWithTouch();
            }
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
                    && BlockTable.GetComponent<BlockTableControl>().BlocksRotating == false
                    )
                {
                    transform.GetComponent<BlockBoardController>().PlaceBlock(raycastHit.collider.transform);
                }
            }
        }
    }
    #endregion

    #region dokunma ile kontrol etme
    void ControlWithTouch()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {

                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
                {
                    if (raycastHit.collider.gameObject.tag == GlobalVariables.TagBlock
                    && raycastHit.collider.gameObject.GetComponent<BlockProperties>().IsThereAnyBlockOnIt == false
                    && raycastHit.collider.gameObject.GetComponent<BlockProperties>().IsSnapped == false
                    && BlockTable.GetComponent<BlockTableControl>().BlocksRotating == false)
                    {
                        transform.GetComponent<BlockBoardController>().PlaceBlock(raycastHit.collider.transform);
                    }
                }
            }
        }
    }
    #endregion
}
