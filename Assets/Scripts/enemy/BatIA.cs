using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatIA : MonoBehaviour
{
    private GameController _GameController;

    private bool isFolow;

    public float speed;

    public bool enemyFlip;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFolow == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _GameController.playerTransform.position, speed * Time.deltaTime);
        }

        if (transform.position.x < _GameController.playerTransform.position.x && enemyFlip == true)
        {
            Flip();
        }
        else if (transform.position.x > _GameController.playerTransform.position.x && enemyFlip == false)
        {
            Flip();
        }
    }

    private void OnBecameVisible()
    {
        isFolow = true;
    }

    private void OnBecameInvisible()
    {
        isFolow = false;
    }

    void Flip()
    {
        enemyFlip = !enemyFlip;
        float x = transform.lossyScale.x;
        x *= -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
