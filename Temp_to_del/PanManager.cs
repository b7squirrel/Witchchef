using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 들어오고 나가는 오브젝트의 위치값에만 관여한다
/// </summary>
public class PanManager : MonoBehaviour
{
    public static PanManager instance;
    PanSlot[] panSlots;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        panSlots = GetComponentsInChildren<PanSlot>();
    }

    public void AcquireRoll(Transform _prefab)
    {
        for (int i = 0; i < panSlots.Length; i++)
        {
            if (panSlots[i].IsEmpty())
            {
                panSlots[i].AddRoll(_prefab);
                return;
            }
        }
    }

    public void FlipRoll()
    {
        for (int i = panSlots.Length -1; i > -1; i--)
        {
            if (!panSlots[i].IsEmpty())
            {
                panSlots[i].Flip();
                return;
            }
        }
    }

    //public void SwitchRolls()
    //{
    //    for (int i = panSlots.Length - 1; i > 0; i--)
    //    {
    //        if (!panSlots[i].IsEmpty())
    //        {
    //            Transform _temp = transform;
    //            panSlots[i].GetRoll().transform.parent = _temp;
    //            panSlots[i].GetRoll().transform.position = panSlots[0].transform.position;
    //            panSlots[0].GetRoll().transform.position = panSlots[i].transform.position;
    //            panSlots[0].GetRoll().transform.parent = panSlots[i].transform;
    //            panSlots[i].GetRoll().transform.parent = panSlots[0].transform;

    //        }
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rolling"))
        {
            AcquireRoll(collision.transform);
        }
    }
}
