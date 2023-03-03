using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDetector : MonoBehaviour
{
    public List<GameObject> touchedBlocks;
    private void Start()
    {
        touchedBlocks = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GlobalVariables.TagBlock && !touchedBlocks.Contains(other.gameObject))
        {
            touchedBlocks.Add(other.gameObject);
            IsThereAnyBlockOnItControl();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GlobalVariables.TagBlock)
        {
            touchedBlocks.Remove(other.gameObject);
            IsThereAnyBlockOnItControl();
        }
    }

    public bool IsThereAnyBlockOnItControl()
    {
        bool result = false;
        if (touchedBlocks.Count > 0)
        {
            result = true;
            transform.parent.GetComponent<Renderer>().material.color = Color.gray;
            transform.parent.GetComponent<BlockProperties>().IsThereAnyBlockOnIt = true;
        }
        else
        {
            transform.parent.GetComponent<Renderer>().material.color = Color.white;
            transform.parent.GetComponent<BlockProperties>().IsThereAnyBlockOnIt = false;
        }

        return result;
    }
}
