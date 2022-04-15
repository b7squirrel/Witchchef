using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanSlot : MonoBehaviour
{
    public bool isEmpty = true;

    public bool IsEmpty()
    {
        if (isEmpty)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void AddRoll(Transform _prefab)
    {
        _prefab.position = transform.position;
        _prefab.rotation = transform.rotation;
        _prefab.parent = transform;
        _prefab.tag = "RollsOnPan";
        isEmpty = false;
    }

    public Transform GetRoll()
    {
        return GetComponentInChildren<RollObject>().GetRollTransform();
    }

    public void MoveRoll(PanSlot _targetSlot)
    {
        if (!isEmpty)
        {
            GetRoll().position = _targetSlot.transform.position;
            GetRoll().parent = _targetSlot.transform;
            isEmpty = true;
        }
    }

    public void RemoveRoll()
    {
        GetRoll().parent = null;
        isEmpty = true;
    }

    public void Flip()
    {
        GetRoll().localEulerAngles += new Vector3(0, 0, 45f);
    }
}
