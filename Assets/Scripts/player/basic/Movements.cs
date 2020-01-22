using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements : MonoBehaviour
{
    private Animator playerAnimator;
    private Rigidbody2D playerRb;
    public Transform groundCheck;

    public bool playerFlip;
    public bool grounded;
    public int idAnimation;
    public float speed;
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0 && playerFlip)
        {
            Flip();
        }
        else if (horizontal < 0 && !playerFlip)
        {
            Flip();
        }

        if (horizontal != 0)
        {
            idAnimation = 1;
        } else
        {
            idAnimation = 0;
        }

        if (Input.GetButtonDown("Fire1") && grounded)
        {
            idAnimation = 3;
        }

        if (Input.GetButtonDown("Fire3") && grounded)
        {
            playerRb.AddForce(new Vector2(0, jumpForce));
            idAnimation = 2;
        }

        playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);

        playerAnimator.SetBool("grounded", grounded);
        playerAnimator.SetInteger("idAnimation", idAnimation);
    }

    void Flip()
    {
        playerFlip = !playerFlip;
        float x = transform.lossyScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
