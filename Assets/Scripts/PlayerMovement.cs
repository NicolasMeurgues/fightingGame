using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    private BoxCollider2D bc;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject player;

    [SerializeField] private LayerMask stageLM;

    private Vector3 spawn;

    public float walkSpeed;
    public float runSpeed;
    public float jumpOne;
    public float jumpTwo;

    public bool isJumpingOne = false;
    public bool isJumpingTwo = false;
    private bool canDoubleJump;

    private bool isAttacking1;
    private bool isAttacking2;

    private bool deadFlag = false;

    private bool IsGrounded()
    {
        RaycastHit2D rh = Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, .1f, stageLM);
        return rh.collider != null;
    }


    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        bc = transform.GetComponent<BoxCollider2D>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal") * walkSpeed * Time.deltaTime;

        MovePlayer(horizontalMovement);

        Flip(rb.velocity.x);




        // animations 
        if (IsGrounded())
        {
            float characterVelocity = Mathf.Abs(rb.velocity.x);
            animator.SetFloat("Speed", characterVelocity);
            animator.SetBool("IsAttacking1", isAttacking1);
            animator.SetBool("IsAttacking2", isAttacking2);
            animator.SetBool("IsJumping", isJumpingOne);
        }
        else
        {
            animator.SetBool("IsJumping", true);
            animator.SetBool("IsAttacking1", isAttacking1);

        }



    }

    void MovePlayer(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .005f);

        if(isJumpingOne == true)
        {
            rb.velocity = Vector2.up * jumpOne;
            animator.SetBool("IsJumping", true);
            isJumpingOne = false;
        }

        if(isJumpingTwo == true)
        {
            rb.velocity = Vector2.up * jumpTwo;
            animator.SetBool("IsJumping", true);
            isJumpingTwo = false;
            canDoubleJump = false;
        }

        if(isAttacking1 == true)
        {
            isAttacking1 = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                isJumpingOne = true;
            }
            else
            {
                if (canDoubleJump)
                {
                    isJumpingTwo = true;
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            isAttacking1 = true;
            animator.SetBool("IsAttacking1", isAttacking1);
        }

    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }else if(_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            transform.position = new Vector3(0f, 3.5f, 0f);
        }
    }


}
