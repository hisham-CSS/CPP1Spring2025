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

    //Private properties
    private bool isGrounded;

    //Private Components
    private LayerMask isGroundLayer;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private new Collider2D collider;

    private Vector2 groundCheckPos => new Vector2(collider.bounds.min.x + collider.bounds.extents.x, collider.bounds.min.y);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();

        isGroundLayer = LayerMask.GetMask("Ground");

        Debug.Log($"Ground Check Pos from collider: {groundCheckPos}");
        //Debug.Log($"Ground Check Pos from transform: {groundCheckTransform.position}");
    }

    // Update is called once per frame
    void Update()
    {
        //Update our ground check
        isGrounded = CheckIsGrounded();
        Debug.Log(isGrounded);

        //check for inputs
        float hInput = Input.GetAxis("Horizontal");

        //apply physics and mechanics
        rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }

        //apply changes to look
        SpriteFlip(hInput);

        //apply animations
        anim.SetFloat("hInput", Mathf.Abs(hInput));
        anim.SetBool("isGrounded", isGrounded);
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
    bool CheckIsGrounded()
    {
        if (!isGrounded && rb.linearVelocityY <= 0)
        {
            return Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, isGroundLayer);
        }
        else if (isGrounded) return Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, isGroundLayer);

        return false;
    }
}
