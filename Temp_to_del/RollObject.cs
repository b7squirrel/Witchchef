using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollObject : MonoBehaviour
{
    public void GetInSlot(Transform _slotTransform)
    {
        transform.position = _slotTransform.GetComponent<Transform>().position;
        transform.parent = _slotTransform.GetComponent<Transform>().transform;
        transform.tag = "RollsOnPan";
    }
}
