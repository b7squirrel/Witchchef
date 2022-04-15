using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 들어오고 나가는 오브젝트의 위치값에만 관여한다
/// </summary>
public class PanManager : MonoBehaviour
{
    public static PanManager instance;
    
    public PanSlot spareSlot;

    PanSlot[] _panSlots;
    BoxCollider2D _boxCol;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _panSlots = GetComponentsInChildren<PanSlot>();
        _boxCol = GetComponent<BoxCollider2D>();
    }

    public void AcquireRoll(Transform _prefab)
    {
        for (int i = 0; i < _panSlots.Length; i++)
        {
            if (_panSlots[i].IsEmpty())
            {
                _panSlots[i].AddRoll(_prefab);
                return;
            }
        }
    }

    public void FlipRoll()
    {
        for (int i = 0; i < _panSlots.Length; i++)
        {
            if (_panSlots[i].isEmpty)
            {
                return;
            }
            _panSlots[i].Flip();
        }
    }

    public void SwitchRolls()
    {
        Debug.Log(CountRollNumber());
        if (CountRollNumber() == 1)
        {
            return;
        }
        for (int i = 0; i < CountRollNumber() - 1; i++)
        {
            _panSlots[i].MoveRoll(spareSlot);
            _panSlots[i + 1].MoveRoll(_panSlots[i]);
            spareSlot.MoveRoll(_panSlots[i + 1]);
        }
    }

    //public void ClearRoll()
    //{
    //    _panSlots[0].GetRoll();
    //}

    public int CountRollNumber()
    {
        for (int i = 0; i < _panSlots.Length; i++)
        {
            if (_panSlots[i].IsEmpty())
            {
                return i;
            }
        }
        return _panSlots.Length;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rolling"))
        {
            AcquireRoll(collision.transform);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(_boxCol.bounds.center, _boxCol.bounds.size);
    }
}
