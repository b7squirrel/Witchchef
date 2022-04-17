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

    public void SetToOccupied()
    {
        isEmpty = false;
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
        return GetComponentInChildren<RollObject>().transform;
    }

    public void MoveRoll(PanSlot _targetSlot)
    {
        if (!isEmpty)
        {
            GetRoll().position = _targetSlot.transform.position;
            GetRoll().parent = _targetSlot.transform;
            isEmpty = true;
            _targetSlot.SetToOccupied();
        }
    }

    public void RemoveRoll()
    {
        GetRoll().parent = null;
        isEmpty = true;
    }

    public void FlipRoll()
    {
        GetRoll().localEulerAngles += new Vector3(0, 0, 45f);
    }

    public void FlipSprite()
    {
        if (!isEmpty)
        {
            if (PlayerController.instance.staticDirection > 0)
            {
                RollObject _roll = GetComponentInChildren<RollObject>();
                float _rotationZ = _roll.transform.eulerAngles.z;
                _roll.transform.eulerAngles = new Vector3(0, 0, _rotationZ);
            }
            else
            {
                RollObject _roll = GetComponentInChildren<RollObject>();
                float _rotationZ = _roll.transform.eulerAngles.z;
                _roll.transform.eulerAngles = new Vector3(0, 180f, _rotationZ);
            }
        }
    }
}
