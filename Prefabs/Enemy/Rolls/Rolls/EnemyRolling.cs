using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 인스펙터에서 편하게 드래그해서 넣으려고 Prefab > Enemy > Rolls > Rolls 폴더에 스크립트가 있음
/// </summary>
public class EnemyRolling : MonoBehaviour
{
    private enum rollingState { shooting, captured, onPan };
    private rollingState currentState;

    /// <summary>
    /// projectile을 캡쳐하면 EnemyProjectile이 PanManager의 acquireFlavor실행
    /// 그 projectile의 flavorSO를 롤의 m_flavorSO에 전달
    /// 그러면 나중에 쳐내면 EnemyRolling는 RollType, FlavorType을 모두 가지고 있게 된다.
    /// </summary>
    public bool isFlavored;
    public FlavorSo m_flavorSO;  

    [Header("When Cleared")]
    public float horizontalSpeed;
    public float verticalSpeed;
    public float gravity;

    [Header("When Captured")]
    Transform _captureBoxPlayer;
    public float captureSpeed; // 캡쳐된 후 player capture box까지 다가가는데 걸리는 시간
    // 캡쳐된 후 팬위에 올라와서 pan manager에서 acquireRoll이 실행된 상황
    // Player Capture Box에서 충돌체크 후 전달

    [Header("Hit Effects")]
    public GameObject hitEffect;
    public Transform hitEffectPoint;

    void Start()
    {
        currentState = rollingState.captured;
        this.tag = "RollCaptured";
        _captureBoxPlayer = FindObjectOfType<PlayerCaptureBox>().transform;
    }

    void Update()
    {
        // Pan Manager의 ClearRoll에서 모든 롤의 tag를 Rolling으로 적용.
        if (this.tag == "Rolling")
        {
            currentState = rollingState.shooting;
        }
        switch (currentState)
        {
            case rollingState.shooting:
                break;

            case rollingState.captured:

                transform.position = Vector2.Lerp(transform.position, _captureBoxPlayer.position, captureSpeed * Time.deltaTime);
                if (Vector2.Distance(transform.position, _captureBoxPlayer.position) < 1f)
                {
                    currentState = rollingState.onPan;
                    PanManager.instance.AcquireRoll(transform);
                }
                break;

            case rollingState.onPan:
                this.tag = "RollsOnPan";
                // 아무것도 하지 않음. PanSlot에서 AddRoll로 슬롯에 페어런트를 해버리기 때문
                break;
        }
    }
    // ground나 enemy에 충돌하면 자신을 destroy시킨다
    // 만약 flavor가 있다면 폭발을 생성한다
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Enemy"))
        {
            if (m_flavorSO != null)
            {
                Instantiate(m_flavorSO.actionPrefab, transform.position, transform.rotation);
            }
            Debug.Log(collision.CompareTag("Ground"));
            DestroyPrefab();
        }
    }
    public void DestroyPrefab()
    {
        Destroy(gameObject);
    }
}
