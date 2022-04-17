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
        //for (int i = 0; i < _panSlots.Length; i++)
        //{
        //    if (_panSlots[i].isEmpty)
        //    {
        //        return;
        //    }
        //    _panSlots[i].Flip();
        //}
        if (_panSlots[0].IsEmpty())
            return;
        _panSlots[0].FlipRoll();
    }

    public void SwitchRolls()
    {
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

    public void ClearRoll()
    {
        int _numberOfRolls = CountRollNumber();
        GameObject _roll = _panSlots[0].GetRoll().gameObject;
        float _direction = PlayerController.instance.staticDirection;
        float _hSpeed = _roll.GetComponent<RollObject>().horizontalSpeed;
        float _vSpeed = _roll.GetComponent<RollObject>().verticalSpeed;
        Rigidbody2D _theRB = _roll.AddComponent<Rigidbody2D>();
        _theRB.gravityScale = _roll.GetComponent<RollObject>().gravity;
        _theRB.velocity = new Vector2(_direction * _hSpeed, _vSpeed);

        _panSlots[0].RemoveRoll();
        // 0번 슬롯의 롤을 비워주고 롤 갯수릉 하나 줄임
        for (int i = 0; i < _numberOfRolls - 1; i++)
        {
            _panSlots[i + 1].MoveRoll(_panSlots[i]);
        }
    }

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
        if (_boxCol == null)
            return;
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(_boxCol.bounds.center, _boxCol.bounds.size);
    }
}
