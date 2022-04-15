using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanSlot : MonoBehaviour
{
    public LayerMask rollsOnPan;
    bool isEmpty = true;

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

    public RollObject GetRoll()
    {
        RollObject _roll = GetComponentInChildren<RollObject>();
        return _roll;
    }

    public void RemoveRoll()
    {
        GetRoll().transform.parent = null;
        isEmpty = true;
    }

    public void Flip()
    {
        GetRoll().transform.localEulerAngles += new Vector3(0, 0, 45f);
    }
}
