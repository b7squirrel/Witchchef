using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollOffset : MonoBehaviour
{
    PanSlot[] _slots;
    public float threshold; // �Ѿ�� �ȵǴ� offset��

    public PanSlot anchorPoint;    // pan manager �������
    Vector2 currentAnchorPoint, pastAnchorPoint;
    Vector2 deltaAnchorDistnace;
    Vector2[] currentSlotPoint = new Vector2[3];
    Vector2[] pastSlotPoint = new Vector2[3];
    Vector2[] deltaSlotDistance = new Vector2[3];
    float[] verticalVelocity = new float[3];

    BoxCollider2D[] _boxCol;

    public LayerMask panLayer;

    [Range(1, 2)]
    public float offsetRatio = 1.2f;
    public float lerpSpeed;

    [Header("Gravity")]
    public float gravityScale;
    float _gravity = 9.8f;

    [Header("Debug")]
    public bool[] isGrounded = new bool[3];

    private void Start()
    {
        transform.parent = null;
        _slots = GetComponentsInChildren<PanSlot>();

        _slots[0].transform.position = anchorPoint.transform.position + new Vector3(0f, .5f);
        for (int i = 1; i < _slots.Length; i++)
        {
            _slots[i].transform.localPosition = _slots[i - 1].transform.localPosition + new Vector3(0, 1f);
        }
        pastAnchorPoint = anchorPoint.transform.position;
        currentAnchorPoint = pastAnchorPoint;

        for (int i = 0; i < _slots.Length; i++)
        {
            pastSlotPoint[i] = _slots[i].transform.position;
            currentSlotPoint[i] = pastSlotPoint[i];
        }
    }
    private void Update()
    {
        CalDeltaDistant();
        BaseSlotMovement();
        HorizontalMovement();
        Flip();
    }
    private void FixedUpdate()
    {
        VerticalMovement();

    }
    void CalDeltaDistant()
    {
        currentAnchorPoint = anchorPoint.transform.position;
        deltaAnchorDistnace = currentAnchorPoint - pastAnchorPoint;
        pastAnchorPoint = currentAnchorPoint;

        for (int i = 0; i < _slots.Length; i++)
        {
            currentSlotPoint[i] = _slots[i].transform.position;
            deltaSlotDistance[i] = currentSlotPoint[i] - pastSlotPoint[i];
            pastSlotPoint[i] = currentSlotPoint[i];
        }
    }
    void BaseSlotMovement()
    {
        _slots[0].transform.position = anchorPoint.transform.position + new Vector3(0f, .5f);
    }
    /// <summary>
    /// ���� �̵��ϰ� �Ʒ����� ������ threshold ������ �ռ� ������ �������� �Ʒ��� ���԰� ���� �Ÿ���ŭ �̵��Ѵ�
    /// offset�� �Ʒ��ܰ��� x�� �Ÿ� ����
    /// delta�� �Ʒ����� 1�����ӵ��� �̵� �Ÿ�
    /// </summary>
    void HorizontalMovement()
    {
        for (int i = 1; i < _slots.Length; i++)
        {
            Vector2 _offset = _slots[i - 1].transform.position - _slots[i].transform.position;

            if (Mathf.Abs(deltaSlotDistance[i-1].x) > 0)
            {
                if (Mathf.Abs(_offset.x) >= threshold)
                {
                    _slots[i].transform.position += new Vector3(deltaSlotDistance[i-1].x, 0f);
                }
            }
        }
    }
    /// <summary>
    /// _slot[0]�� ���߿� ���� �ʴ´�. �������� �׶��� �Ǿ����� ������ �Ʒ��� �������� �׶��� �Ǹ� offset��ŭ �Ÿ��� ������
    /// </summary>
    void VerticalMovement()
    {
        _slots[0].transform.position = new Vector3(_slots[0].transform.position.x, .5f + anchorPoint.transform.position.y);
        for (int i = 1; i < _slots.Length; i++)
        {
            bool _isGrounded =
                Physics2D.OverlapBox(_slots[i].transform.position + new Vector3(0, -.5f), new Vector2(1, .3f), panLayer);

            verticalVelocity[i] += -gravityScale * _gravity * Time.deltaTime;

            Vector3 m_position = _slots[i].transform.position;
            _slots[i].transform.Translate(new Vector3(0, verticalVelocity[i], 0) * Time.deltaTime);
            _slots[i].transform.position = new Vector3(m_position.x, _slots[i].transform.position.y);

            if (_isGrounded)
            {
                Vector3 _position = _slots[i].transform.position;
                Vector3 _positionParent = _slots[i - 1].transform.position;
                _slots[i].transform.position = new Vector3(_position.x, 1f + _positionParent.y);
            }

        }
    }
    void Flip()
    {
        foreach (var _slot in _slots)
        {
            _slot.FlipSprite();
        }
    }
}
