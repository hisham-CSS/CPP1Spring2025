using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    //Public modifiable properties
    [Range(3, 10)]
    public float speed = 6.0f;
    [Range(0.01f, 0.2f)]
    public float groundCheckRadius = 0.02f;

    //Private Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    GroundCheck groundCheck;
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        groundCheck = new GroundCheck(LayerMask.GetMask("Ground"), GetComponent<Collider2D>(), rb, ref groundCheckRadius);
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);
        //Update our ground check
        groundCheck.CheckIsGrounded();

        //check for inputs
        float hInput = Input.GetAxis("Horizontal");

        if (curPlayingClips.Length > 0)
        {
            if (!(curPlayingClips[0].clip.name == "Fire"))
            {
                //apply physics and mechanics
                rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);

                if (Input.GetButtonDown("Fire1") && groundCheck.IsGrounded) anim.SetTrigger("Fire");
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
        

        if (Input.GetButtonDown("Jump") && groundCheck.IsGrounded)
        {
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }

        //apply changes to look
        SpriteFlip(hInput);

        //apply animations
        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", groundCheck.IsGrounded);
    }

    void SpriteFlip(float hInput) 
    {
        //if no input - we flip based on if input is less than zero - there is no real performance cost to setting sr.flipX every frame, however doing it in the follow two ways means that sr.flipX is set every frame there is an input
        if (hInput != 0) sr.flipX = (hInput < 0);
        //if (hInput > 0) sr.flipX = false;
        //else if (hInput < 0) sr.flipX = true;
        
        //this is good as the sr.flipX is only changed when it needs too
        //if ((hInput > 0 && sr.flipX) || (hInput < 0 && !sr.flipX)) sr.flipX = !sr.flipX;
        
    }
}
