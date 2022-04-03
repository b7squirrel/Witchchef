using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBolt : MonoBehaviour
{
    public float moveSpeed;
    [HideInInspector]
    public bool isParried;
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    public Transform wallDetectingPoint;
    public float knockBackForce;
    public float knockBackTime;
    
    Rigidbody2D _theRB;
    Animator _anim;
    TakeDamage _takeDamage;
    bool _isGrounded;
    [SerializeField]
    int _direction;
    bool _detectingWall;
    float knockBackCounter;

    private void Start()
    {
        _theRB = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _takeDamage = GetComponentInChildren<TakeDamage>();
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, groundLayer);
        _detectingWall = Physics2D.OverlapCircle(wallDetectingPoint.position, .2f, groundLayer);

        Direction();

        if (_takeDamage.knockBack)
        {
            if (knockBackCounter < knockBackTime)
            {
                KnockBack();
                knockBackCounter += Time.deltaTime;
            }
            else
            {
                _takeDamage.knockBack = false;
                StartCoroutine(Stun());
            }
        }
        else
        {
            if (_detectingWall)
            {
                ChangeDirection();
            }
            Run();
        }
    }

    void ChangeDirection()
    {
        if (_direction == 1)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if(_direction == -1)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }

    void Direction()
    {
        if (transform.rotation.y == 0)
        {
            _direction = -1;
        }
        else
        {
            _direction = 1;
        }
    }
    void Run()
    {
        _theRB.velocity = new Vector2(_direction * moveSpeed, _theRB.velocity.y);
    }

    void KnockBack()
    {
        _theRB.velocity = new Vector2(-1f * _direction * knockBackForce, _theRB.velocity.y);
    }

    IEnumerator Stun()
    {
        yield return new WaitForSeconds(2f);
    }
}
