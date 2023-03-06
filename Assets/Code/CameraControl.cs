using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform target;
    public float CameraDistance;
    public bool CameraMoving = false;
    public Transform GeneralControllers;

    Vector3 prePosition;
    Vector3 startPosition;
    Quaternion startRotation;

    int cameraStatus;
    int cameraStatus_idle = 0;
    int cameraStatus_DragBegin = 1;
    int cameraStatus_Dragging = 2;
    int cameraStatus_DragEnd = 3;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        cameraStatus = cameraStatus_idle;
    }

    // Update is called once per frame
    void LateUpdate()
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
                        CameraMoving = false;
                    }
                }
                else // dokunduðumuz þey blok deðilse
                {
                    cameraStatus = cameraStatus_DragBegin;
                    prePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    CameraMoving = true;
                }
            }
            else//hiçbirþeye dokunulmadýysa 
            {
                cameraStatus = cameraStatus_DragBegin;
                prePosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                CameraMoving = true;
            }
        }

        if (Input.GetMouseButton(0) && cameraStatus == cameraStatus_DragBegin || cameraStatus == cameraStatus_Dragging)
        {
            cameraStatus = cameraStatus_Dragging;
            Vector3 direction = prePosition - Camera.main.ScreenToViewportPoint(Input.mousePosition);

            transform.position = target.position;
            transform.Rotate(new Vector3(1, 0, 0), direction.y * 90);
            transform.Rotate(new Vector3(0, 1, 0), -direction.x * 90, Space.World);
            transform.Translate(new Vector3(0, 0, CameraDistance));

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
                transform.position = target.position;
                transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, Time.deltaTime * 5);
                //aþaðýdaki z deðeri editörde kameranýn canvasa olan uzaklýðý. Tüm rotation iþlemi sýrasýnda ayný mesafeyi korumasý için setlenir.
                transform.Translate(new Vector3(0, 0, -4.01f));
            }
            else
            {
                transform.rotation = startRotation;
                transform.position = startPosition;
                cameraStatus = cameraStatus_idle;
                CameraMoving = false;
            }

        }
    }
}
