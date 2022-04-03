using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Transform target;
    public float moveSpeed;
    private Vector2 moveDirection;
    private Rigidbody2D theRB;

    private Vector2 initialPoint; // parry 되었을 때 다시 되돌아 오기 위한 위치값
    public Vector2 contactPoint; // parry 된 지점을 시작점으로 하기 위한 변수
    public bool isParried; // projectile이 발사되었는지 parry 되었는지 판단
    public float homingTime; // 반사되어서 타겟에 도달하기까지 걸리는 시간
    private bool isFlying; // parry 되어서 날아가는 상태. 아무것도 안함. 

    public bool isCaptured; // 캡쳐되었음을 전달 받고 이 스크립트에서 getRolled를 구현
    private bool isGettingIn; // 캡쳐되어서 후라이팬 안으로 들어가는 단계
    public float gettingInSpeed; // 팬 위로 올라가는 속도

    public FlavorSo flavorSo;

    public GameObject sparkEffect;
    public GameObject smokeRed;
    public GameObject debris;
    private GameObject _smoke;
    private GameObject _debris;

    private bool hit;
    public GameObject cube;
    // projectile이 목표물과 거리가 멀거나 가까워도 항상 목표물에 도달하도록 값을 조절해 주는 상수
    public float distantConstant;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        moveDirection = (PlayerController.instance.transform.position - transform.position).normalized * moveSpeed;
        initialPoint = new Vector2(transform.position.x, transform.position.y - 1f);
        isParried = false;
        isFlying = false;
        _smoke = Instantiate(smokeRed, transform.position, Quaternion.identity);
        _debris = Instantiate(debris, transform.position, Quaternion.identity);
    }

    void Update()
    {
        _smoke.transform.position = Vector2.MoveTowards(_smoke.transform.position,
                transform.position, 5f);
        _debris.transform.position = Vector2.MoveTowards(_debris.transform.position,
    transform.position, 5f);

        if (isCaptured)
        {
            if (isGettingIn)
            {
                GetIn();
            }
            else
            {
                if(Inventory.instance.isFlavored) // 포화상태이면 소멸, 아니라면 GetFlavored
                {
                    Saturated();
                }
                else
                {
                    GetFlavored();
                }
            }
        }
        else
        {
            if (!isFlying)
            {
                if (!isParried)
                {
                    theRB.velocity = new Vector2(moveDirection.x, moveDirection.y);
                    
                }
                else
                {
                    if (!hit)
                    {
                        hit = true;
                    }
                    isFlying = true;
                    theRB.gravityScale = 1f;
                    //Deflection();
                    Temp();

                    GameManager.instance.StartCameraShake(6, 1.3f);
                    GameManager.instance.TimeStop(.08f);
                }
            }
            else
            {
                // 패링이 된 상태에서 날아가는 중

                float _distance = Mathf.Abs(initialPoint.x - transform.position.x);


                theRB.gravityScale += .08f / (_distance * distantConstant);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HurtBoxPlayer"))
        {
            DestroyProjectile();
        }
        if (collision.CompareTag("Ground"))
        {
            DestroyProjectile();
        }
        if (collision.CompareTag("Enemy"))
        {
            if (this.gameObject.tag == "ProjectileDeflected")
            {
                DestroyProjectile();
            }
        }
    }
    Vector2 CalculateVelecity(Vector2 _target, Vector2 _origin, float time)
    {
        Vector2 distance = _target - _origin;

        float Vx = distance.x / time;
        float Vy = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * time;

        Vector2 result;
        result.x = Vx;
        result.y = Vy;

        return result;
    }

    void Deflection()
    {
        this.gameObject.tag = "ProjectileDeflected";
        
        Transform effectPoint = transform;
        effectPoint.position += new Vector3(2f, .7f, 0f);
        effectPoint.eulerAngles = new Vector3(transform.rotation.x, PlayerController.instance.transform.rotation.y, -10f);

        theRB.velocity = CalculateVelecity(initialPoint, (Vector2)contactPoint, homingTime);
    }

    void Temp()
    {
        this.gameObject.tag = "ProjectileDeflected";
        float tempDirection = transform.position.x - initialPoint.x;
        int direc = 0;

        if(tempDirection > 0)
        {
            direc = -1;
        }
        else if(tempDirection < 0)
        {
            direc = 1;
        }

        theRB.velocity = new Vector2(direc * moveSpeed, 20f);
    }

    void GetFlavored()
    {
        AudioManager.instance.Play("GetRolled_01");
        Inventory.instance.AcquireFlavor(flavorSo);
        CookingSystem.instance.CreateFlavorOutput();
        isGettingIn = true;
        gameObject.tag = "Player";
    }
    
    void GetIn()
    {
        if(Vector2.Distance(transform.position,
            PlayerPanAttack.instance.panPoint.position) <= .1f)
        {
            DestroyProjectile();
        }
        else
        {
            Vector2.MoveTowards(transform.position,
                PlayerPanAttack.instance.panPoint.position, gettingInSpeed);
        }
    }

    private void DestroyProjectile()
    {
        isGettingIn = false;
        Destroy(_smoke);
        Destroy(_debris);
        Destroy(gameObject);
    }

    private void Saturated()
    {
        // 포화상태여서 사그라드는 애니메이션이 필요
        DestroyProjectile();
    }
}
