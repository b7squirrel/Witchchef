using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 인스펙터에서 편하게 드래그해서 넣으려고 Roll prefab 폴더에 스크립트가 있음
/// </summary>
public class EnemyRolling : MonoBehaviour
{
    private enum rollingState { shooting, flying, dropped };
    private rollingState currentState;

    public float followingPanSpeed;
    private Rigidbody2D theRB;

    public bool beingHit;  // player pan attack에서 참조
    
    private float direction;
    public float initForce_x, initForce_y;

    public GameObject hitEffect;
    public Transform hitEffectPoint;

    [Header("Passed by Player")]
    public int numberOfRolls;
    public bool isFlavored;

    //public GameObject explosionFlavor;
    public FlavorSo theFlavorSo;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        currentState = rollingState.shooting;
        if (isFlavored)
        {
            this.tag = "RollFlavored";
        }
        else
        {
            this.tag = "Rolling";
        }
    }

    void Update()
    {
        SetDirection();

        if(PlayerHealthController.instance.isDead)
        {
            currentState = rollingState.dropped;
        }

        switch (currentState)
        {
            case rollingState.shooting:

                if (theRB.gravityScale != 1)
                {
                    theRB.gravityScale = 3;
                }
                theRB.velocity = new Vector2(direction * initForce_x, initForce_y);
                currentState = rollingState.flying;
                break;

            case rollingState.flying:

                theRB.gravityScale += .02f;

                break;

            case rollingState.dropped:
                DestroyPrefab();
                //transform.parent = null;
                //theRB.gravityScale = 5;
                break;
        }
    }

    void SetDirection()
    {
        direction = PlayerController.instance.staticDirection;
        if(direction > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
    // ground나 enemy에 충돌하면 explosion을 생성하고 사이즈값을 넘겨준 뒤 자신을 destroy시킨다
    // 만약 flavor가 있다면 폭발을 생성한다
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Enemy"))
        {
            if(isFlavored)
            {
                GameObject _action = Instantiate(theFlavorSo.actionPrefab, transform.position, Quaternion.identity); // 액션프리펩 생성
                _action.GetComponent<ExplosionFlavor>().numberOfRolls = numberOfRolls;
            }

            Destroy(gameObject);
        }
    }
    public void BeingHit()
    {
        currentState = rollingState.shooting;
        beingHit = false;
        GameObject clone = Instantiate(hitEffect, hitEffectPoint.position, hitEffectPoint.rotation);
        //시간 멈추고 카메라쉐이크
        GameManager.instance.StartCameraShake(8, .8f);
        GameManager.instance.TimeStop(.1f);
    }
    public void DestroyPrefab()
    {
        Destroy(gameObject);
    }
}
