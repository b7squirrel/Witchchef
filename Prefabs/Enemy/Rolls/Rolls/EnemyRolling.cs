using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 인스펙터에서 편하게 드래그해서 넣으려고 Prefab > Enemy > Rolls > Rolls 폴더에 스크립트가 있음
/// </summary>
public class EnemyRolling : MonoBehaviour
{
    private enum rollingState { shooting, flying, captured, onPan };
    private rollingState currentState;

    [Header("When Cleared")]
    public float horizontalSpeed;
    public float verticalSpeed;
    public float gravity;

    [Header("When Captured")]
    PlayerCaptureBox _playerCaptureBox;
    public float captureSpeed; // 캡쳐된 후 player capture box까지 다가가는데 걸리는 시간
    // 캡쳐된 후 팬위에 올라와서 pan manager에서 acquireRoll이 실행된 상황
    // Player Capture Box에서 충돌체크 후 전달
    public bool isOnPan;

    [Header("Hit Effects")]
    public GameObject hitEffect;
    public Transform hitEffectPoint;

    void Start()
    {
        _playerCaptureBox = FindObjectOfType<PlayerCaptureBox>();
        currentState = rollingState.captured;
        this.tag = "RollCaptured";
    }

    void Update()
    {
        if (isOnPan)
        {
            currentState = rollingState.onPan;
        }

        switch (currentState)
        {
            case rollingState.shooting:
                this.tag = "Rolling";
                currentState = rollingState.flying;
                break;

            case rollingState.flying:
                break;

            case rollingState.captured:
                Vector2.MoveTowards(transform.position, _playerCaptureBox.transform.position, captureSpeed * Time.deltaTime);
                break;

            case rollingState.onPan:
                this.tag = "RollsOnPan";
                // 아무것도 하지 않음. PanSlot에서 AddRoll로 슬롯에 페어런트를 해버리기 때문
                break;
        }
    }

    // ground나 enemy에 충돌하면 explosion을 생성하고 사이즈값을 넘겨준 뒤 자신을 destroy시킨다
    // 만약 flavor가 있다면 폭발을 생성한다
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RollsOnPan"))
        {
            return;
        }
        if (collision.CompareTag("Ground") || collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
    public void DestroyPrefab()
    {
        Destroy(gameObject);
    }
}
