using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIA : MonoBehaviour
{
    private GameController _GameController;
    private Rigidbody2D slimeRb;
    private Animator slimeAnimator;

    public float speed;
    public float timeToWalk;

    public GameObject hitBox;

    public bool enemyFlip;
    private int horizontal;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        slimeRb = GetComponent<Rigidbody2D>();
        slimeAnimator = GetComponent<Animator>();
        StartCoroutine("slimeWalk");
    }

    // Update is called once per frame
    void Update()
    {
        if (horizontal > 0 && enemyFlip)
        {
            Flip();
        }
        else if (horizontal < 0 && !enemyFlip)
        {
            Flip();
        }

        slimeRb.velocity = new Vector2(horizontal * speed, slimeRb.velocity.y);

        if (horizontal != 0)
        {
            slimeAnimator.SetBool("isWalk", true);
        }
        else
        {
            slimeAnimator.SetBool("isWalk", false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "hitBox")
        {
            horizontal = 0;
            StopCoroutine("slimeWalk");
            Destroy(hitBox);
            _GameController.playSFX(_GameController.sfxEnemyDead, 0.3f);
            slimeAnimator.SetTrigger("dead");
        }
    }

    IEnumerator slimeWalk()
    {
        int rand = Random.Range(0, 100);

        if (rand < 33)
        {
            horizontal = -1;
        }
        else if (rand < 66)
        {
            horizontal = 0;
        }
        else
        {
            horizontal = 1;
        }

        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("slimeWalk");
    }

    void Flip()
    {
        enemyFlip = !enemyFlip;
        float x = transform.lossyScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    void OnDead()
    {
        Destroy(this.gameObject);
    }

}
