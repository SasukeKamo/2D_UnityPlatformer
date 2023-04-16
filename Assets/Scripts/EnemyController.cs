using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;
    private Animator animator;
    float startPositionX;
    bool isMovingRight = false;
    public float moveRange = 1.0f;
    public BoxCollider2D hitbox;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitbox = GetComponent<BoxCollider2D>();
        startPositionX = this.transform.position.x;
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            if (isMovingRight)
            {
                if (this.transform.position.x <= startPositionX + moveRange)
                {
                    transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
                    MoveRight();
                }
                else
                {
                    isMovingRight = false;
                    transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    MoveLeft();
                }
            }
            else
            {
                if (this.transform.position.x > startPositionX - moveRange)
                {
                    transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                    MoveLeft();
                }
                else
                {
                    isMovingRight = true;
                    transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
                    MoveRight();
                }
            }
        }
    }

    void MoveRight()
    {
        this.transform.position = new Vector3(this.transform.position.x + Time.deltaTime *moveSpeed,
            this.transform.position.y, this.transform.position.z);
    }

    void MoveLeft()
    {
        this.transform.position = new Vector3(this.transform.position.x + Time.deltaTime * -moveSpeed,
            this.transform.position.y, this.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.gameObject.transform.position.y>transform.position.y)
            {
                animator.SetBool("isDead", true);
                hitbox.enabled = false;
                StartCoroutine(KillOnAnimationEnd());
                //gameObject.SetActive(false);
            }
        }
    }

    IEnumerator KillOnAnimationEnd()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
