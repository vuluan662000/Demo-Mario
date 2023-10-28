using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;
    private SpriteRenderer sprite;

    [SerializeField] private LayerMask jumpableGround;

    private float driX=0;
    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private float jumpForce = 14f;

    [SerializeField] private AudioSource jumpSoundEffect;

    private enum MovementState { idle, running, jumping, falling }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        anim = rb.GetComponent<Animator>(); 
        sprite = rb.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        driX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(driX * moveSpeed, rb.velocity.y);
        if(Input.GetButtonDown("Jump")&& IsGrounded())
        {
            jumpSoundEffect.Play(); 
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        } 
        UpdateAnimatorUpdate();
    }

    private void UpdateAnimatorUpdate()
    {
        MovementState state;
        if (driX > 0)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (driX < 0)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if(rb.velocity.y > .1f)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            state = MovementState.falling;
        }

        anim.SetInteger("state",(int)state);
    }

    private bool IsGrounded()
    {
        bool t=   Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
        print("isGround " + t);
        return t;
    }   
}
