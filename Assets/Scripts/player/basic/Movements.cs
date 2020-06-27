using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements : MonoBehaviour
{
    private GameController _GameController;
    private Animator playerAnimator;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSr;
    public Transform groundCheck;

    public bool playerFlip;
    public bool grounded;
    public int idAnimation;
    public float speed;
    public float jumpForce;
    private bool isAtack;

    public Transform slideAtack;
    public GameObject hitBoxPrefab;
    public Color hitColor;
    public Color noHitColor;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSr = GetComponent<SpriteRenderer>();
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        _GameController.playerTransform = this.transform;
    }

    private void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
        playerAnimator.SetFloat("speedY", playerRb.velocity.y);
    }

    // colisão com objetos
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "coletavel")
        {
            _GameController.playSFX(_GameController.sfxCoin, 0.5f);
            _GameController.getCoin();
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "damage")
        {
            _GameController.getHit();
            if (_GameController.vida > 0)
            {
                StartCoroutine("damageController");
            }
        }
        else if (col.gameObject.tag == "abismo")
        {
            _GameController.playSFX(_GameController.sfxDamage, 0.05f);
            _GameController.vida = 0;
            _GameController.heartController();
            _GameController.painelGameOver.SetActive(true);
            _GameController.currentState = gameState.GAMEOVER;
            _GameController.trocarMusica(musicaFase.GAMEOVER);
        }
        else if (col.gameObject.tag == "flag")
        {
            _GameController.theEnd();
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerAnimator.SetBool("grounded", grounded);

        if (_GameController.currentState != gameState.GAMEPLAY)
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            //playerAnimator.SetInteger("h", 0);
            return;
        }

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

        else if (horizontal != 0)
        {
            idAnimation = 1;
        }
        else
        {
            idAnimation = 0;
        }

        if (Input.GetButtonDown("Fire3") && grounded && horizontal != 0)
        {
            _GameController.playSFX(_GameController.sfxSlide, 0.5f);
            isAtack = true;
            idAnimation = 3;
        }

        if (grounded)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                print(grounded);
                _GameController.playSFX(_GameController.sfxJump, 0.5f);
                playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                idAnimation = 2;

            }
        }

        playerRb.velocity = new Vector2(horizontal * speed, playerRb.velocity.y);

        //playerAnimator.SetBool("grounded", grounded);
        playerAnimator.SetInteger("idAnimation", idAnimation);
        playerAnimator.SetBool("isAtack", isAtack);
    }

    void Flip()
    {
        playerFlip = !playerFlip;
        float x = transform.lossyScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void footSteep()
    {
        _GameController.playSFX(_GameController.sfxSteep[Random.Range(0, _GameController.sfxSteep.Length)], 4f);
    }

    void OnEndAtack()
    {
        isAtack = false;
    }

    void hitBoxAtack()
    {
        GameObject hitBoxTemp = Instantiate(hitBoxPrefab, slideAtack.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.6f);
    }

    IEnumerator damageController()
    {
        _GameController.playSFX(_GameController.sfxDamage, 0.5f);

        this.gameObject.layer = LayerMask.NameToLayer("Invencible");

        playerSr.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        playerSr.color = noHitColor;

        for (int i = 0; i < 5; i++)
        {
            playerSr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerSr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }


        playerSr.color = Color.white;
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
