using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollOffset : MonoBehaviour
{
    PanSlot[] _slots;
    [SerializeField] float _offset_vertical;
    [SerializeField] float _offset_horizontal;

    public Transform anchorPoint;    // pan manager ²ø¾î³õ±â
    [Range(1, 2)]
    public float offsetRatio = 1.2f;
    public float lerpSpeed;

    private void Start()
    {
        _slots = GetComponentsInChildren<PanSlot>();
        _offset_vertical = 4;
        _offset_horizontal = 0;

        _slots[0].transform.localPosition = anchorPoint.localPosition + new Vector3(0f, _offset_vertical * .5f, 0f);
        for (int i = 1; i < _slots.Length; i++)
        {
            _slots[i].transform.localPosition = _slots[i - 1].transform.localPosition + new Vector3(0, _offset_vertical, 0f);
        }
    }

    private void Update()
    {
        _slots[0].transform.localPosition = anchorPoint.localPosition + new Vector3(_offset_horizontal, _offset_vertical * .5f, 0f);
        for (int i = 1; i < _slots.Length; i++)
        {
            Vector3 _targetPoint = _slots[i - 1].transform.localPosition +
                new Vector3(_offset_horizontal * offsetRatio * i, _offset_vertical, 0f);

            _slots[i].transform.localPosition = Vector3.Lerp(_slots[i].transform.localPosition, _targetPoint, lerpSpeed * Time.deltaTime);
        }
    }

    public void SetOffsetsHorizontal(float _horizontal)
    {
        _offset_horizontal = _horizontal;
        
    }
    public void SetOffsetsVertical(float _vertical)
    {
        _offset_vertical = _vertical;
    }

}
