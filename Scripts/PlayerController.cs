using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed;
    private float _initialMoveSpeed;
    public Rigidbody2D theRB;
    private Animator anim;
    private float currentDirection, previousDirection;
    [HideInInspector]
    public float staticDirection;

    private bool isGrounded;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    
    [Header("Jump")]
    public float jumpForce;
    private float _initialJumpForce;
    public float jumpRememberTime;
    private float jumpRemember;

    [Header("Roll Weight")]
    public float weight;
    public float weightRate;


    public float CurrentDirection
    {
        get { return currentDirection; }
    }

    private bool noDirectionChange;
    public bool NoDirectionChange
    {
        get { return noDirectionChange; }
        set { noDirectionChange = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        previousDirection = currentDirection;
        _initialJumpForce = jumpForce;
        _initialMoveSpeed = moveSpeed;
    }

    void Update()
    {
        DirectionCheck();
        Flip();

        GroundCheck();
        Gravity();
        Move();
        Jump();
        

        SetAnimationState();
    }

    //animation event
    void ResetNoDirectionChange()
    {
        noDirectionChange = false;
    }

    void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, .1f, whatIsGround);
    }

    void DirectionCheck()
    {
        currentDirection = Input.GetAxisRaw("Horizontal");

        // -1과 1만 있는 staticDirection
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            staticDirection = Input.GetAxisRaw("Horizontal");
        }
    }

    void Move()
    {
        theRB.velocity = new Vector2(currentDirection * moveSpeed, theRB.velocity.y);
    }

    void Flip()
    {
        if (currentDirection > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (currentDirection < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    void Jump()
    {
        jumpRemember -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.X))
        {
            jumpRemember = jumpRememberTime;
        }

        if(isGrounded && jumpRemember > 0f)
        {
            if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Player_PanThrowing"))
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
            }
        }
    }

    void Gravity()
    {
        if (theRB.velocity.y > 0)
        {
            theRB.gravityScale = 7f;
        }
        else if (theRB.velocity.y < 0)
        {
            theRB.gravityScale = 11f;
        }
    }

    public void WeightCalculation()
    {
        moveSpeed = moveSpeed - (weight * weightRate);
        jumpForce = jumpForce - (weight * weightRate);
    }

    public void DecreaseWeight()
    {
        weight--;
    }

    public void ResetWeight()
    {
        weight = 0f;
        jumpForce = _initialJumpForce;
        moveSpeed = _initialMoveSpeed;
    }

    void SetAnimationState()
    {
        if (Input.GetAxisRaw("Horizontal") == 0)  // idle
        {
            anim.SetBool("isWalking", false);
            anim.SetBool("isGrounded", true);

        }
        else if (Input.GetAxisRaw("Horizontal") != 0 && Mathf.Abs(theRB.velocity.y) <= .1f)  // walk
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isGrounded", true);
        }

        if (Mathf.Abs(theRB.velocity.y) < 0.1f && isGrounded)  // not falling
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
            anim.SetBool("isGrounded", true);
        }
        else if (theRB.velocity.y > 0.1f)  // jump
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isFalling", false);
            anim.SetBool("isGrounded", false);
        }
        else if (theRB.velocity.y < 0)  // falling

        {
            anim.SetBool("isFalling", true);
            anim.SetBool("isJumping", false);
            anim.SetBool("isGrounded", false);
        }
    }
}
