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
    public Transform _flavorEffectRot;  //파티클이 이상하게 붙어서 x축으로 -90도 회전시킴
    BoxCollider2D _boxCol;

    public Transform hitRollPoint;  // HitRoll을 할 때 자꾸 롤이 그라운드 판정이 나면서 사라짐. 그래서 좀 띄워서 발사해봄

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _panSlots = GetComponentsInChildren<PanSlot>();
        _boxCol = GetComponent<BoxCollider2D>();
    }

    // 빈 슬롯이 없으면 캡쳐가 실행되지 않으므로 AcquireRoll이 호출되었을 때는 무조건 빈 슬롯이 있음
    public void AcquireRoll(Transform _prefab)
    {
        if(IsAvailableToCapture())
        {
            // 기존 롤들을 한 칸씩 올림
            for (int i = _panSlots.Length - 1; i > -1; i--)
            {
                if (_panSlots[i].isEmpty == false)
                {
                    _panSlots[i].MoveRoll(_panSlots[i + 1]);
                }
            }
            // 그리고 가장 아래칸에 캡쳐한 롤을 집어넣음
            _panSlots[0].AddRoll(_prefab);
        }
    }

    //슬롯을 돌면서 매개변수로 받은 flavorSO에서 해당 이펙트를 추출하고 각 슬롯을 따라가게 한다. 
    //슬롯의 롤들에게 isFlavored = true 값을 전달한다. 
    public void AcquireFlavor(FlavorSo _flavorSo)
    {
        for (int i = 0; i < _panSlots.Length; i++)
        {
            if (!_panSlots[i].IsEmpty())
            {
                var _clone = Instantiate(_flavorSo.flavorParticle, _panSlots[i].transform.position, _flavorEffectRot.rotation);
                _clone.GetComponent<ParticleController>().SetSlotToFollow(_panSlots[i]);

                _panSlots[i].GetRoll().GetComponent<EnemyRolling>().isFlavored = true;
                _panSlots[i].GetRoll().GetComponent<EnemyRolling>().m_flavorSO = _flavorSo;
            }
        }
    }

    public void FlipRoll()
    {
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

    public void ClearRoll(){
        int _numberOfRolls = CountRollNumber();
        GameObject _roll = _panSlots[0].GetRoll().gameObject;
        _roll.transform.position = hitRollPoint.position;

        if (_roll.GetComponent<EnemyRolling>().isFlavored){
            _roll.tag = "RollFlavored";
            _roll.GetComponent<EnemyRolling>().isFlavored = false;
        }
        else{
            _roll.tag = "Rolling";
        }

        float _direction = PlayerController.instance.staticDirection;
        float _hSpeed = _roll.GetComponent<EnemyRolling>().horizontalSpeed;
        float _vSpeed = _roll.GetComponent<EnemyRolling>().verticalSpeed;
        Rigidbody2D _theRB = _roll.AddComponent<Rigidbody2D>();
        BoxCollider2D _boxCol = _roll.AddComponent<BoxCollider2D>();
        _boxCol.isTrigger = true;
        _theRB.gravityScale = _roll.GetComponent<EnemyRolling>().gravity;
        _theRB.velocity = new Vector2(_direction * _hSpeed, _vSpeed);

        _panSlots[0].RemoveRoll();

        // 0번 슬롯의 롤을 비워주고 롤 갯수를 하나 줄임
        for (int i = 0; i < _numberOfRolls - 1; i++){
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

    public bool IsAvailableToCapture()
    {
        if (CountRollNumber() < _panSlots.Length)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        if (_boxCol == null)
            return;
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(_boxCol.bounds.center, _boxCol.bounds.size);
    }
}
