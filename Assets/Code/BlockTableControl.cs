using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

public class BlockTableControl : MonoBehaviour
{
    public Transform GeneralControllers;
    public bool BlocksRotating = false;

    Vector3 prePosition;
    Quaternion startRotation;

    //bool cameraMoveIsEnded = true;

    int cameraStatus;
    int cameraStatus_idle = 0;
    int cameraStatus_DragBegin = 1;
    int cameraStatus_Dragging = 2;
    int cameraStatus_DragEnd = 3;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
        cameraStatus = cameraStatus_idle;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (GlobalVariables.gameState == GlobalVariables.gameState_inGame)
        {
            BlockTableControlWithMouse();
        }
    }

    void BlockTableControlWithMouse ()
    {
        if (Input.GetMouseButtonDown(0) && GeneralControllers.GetComponent<BlockBoardController>().isSmoothMoveToSnapPointAnimationContinue == false)
        {
            RaycastHit raycastHit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                if (raycastHit.collider.gameObject.tag == GlobalVariables.TagBlock)
                {
                    if (cameraStatus != cameraStatus_DragEnd)
                    {
                        cameraStatus = cameraStatus_idle;
                        BlocksRotating = false;
                    }
                }
                else // dokunduðumuz þey blok deðilse
                {
                    cameraStatus = cameraStatus_DragBegin;
                    BlocksRotating = true;
                    prePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                }
            }
            else//hiçbirþeye dokunulmadýysa 
            {
                cameraStatus = cameraStatus_DragBegin;
                BlocksRotating = true;
                prePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            }
        }

        if (Input.GetMouseButton(0) && cameraStatus == cameraStatus_DragBegin || cameraStatus == cameraStatus_Dragging)
        {
            cameraStatus = cameraStatus_Dragging;
            Vector3 direction = prePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);
            transform.Rotate(new Vector3(0, 0, -1), -direction.x * 180, Space.World);
            transform.Rotate(new Vector3(-1, 0, 0), direction.y * 180);

            if (TransformUtils.GetInspectorRotation(transform).x >= 90)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(90, TransformUtils.GetInspectorRotation(transform).y, TransformUtils.GetInspectorRotation(transform).z));
            }
            else if (TransformUtils.GetInspectorRotation(transform).x <= -90)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(-90, TransformUtils.GetInspectorRotation(transform).y, TransformUtils.GetInspectorRotation(transform).z));
            }

            if (TransformUtils.GetInspectorRotation(transform).y >= 90)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(TransformUtils.GetInspectorRotation(transform).x, 90, TransformUtils.GetInspectorRotation(transform).z));
            }
            else if (TransformUtils.GetInspectorRotation(transform).y <= -90)
            {
                transform.localRotation = Quaternion.Euler(new Vector3(TransformUtils.GetInspectorRotation(transform).x, -90, TransformUtils.GetInspectorRotation(transform).z));
            }

            prePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0) && cameraStatus == cameraStatus_Dragging)
        {
            cameraStatus = cameraStatus_DragEnd;
        }

        if (cameraStatus == cameraStatus_DragEnd)
        {
            if (Quaternion.Angle(transform.rotation, startRotation) > 0.5f)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, Time.deltaTime * 5);
            }
            else
            {
                transform.rotation = startRotation;
                cameraStatus = cameraStatus_idle;
                BlocksRotating = false;
            }

        }
    }
}
