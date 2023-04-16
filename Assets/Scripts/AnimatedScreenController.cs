using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedScreenController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;
    private Animator animator;
    float startPositionX;
    bool isMovingRight = false;
    public float moveRange = 1.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        startPositionX = this.transform.position.x;
    }

    private void Update()
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

    void MoveRight()
    {
        this.transform.position = new Vector3(this.transform.position.x + Time.deltaTime * moveSpeed,
            this.transform.position.y, this.transform.position.z);
    }

    void MoveLeft()
    {
        this.transform.position = new Vector3(this.transform.position.x + Time.deltaTime * -moveSpeed,
            this.transform.position.y, this.transform.position.z);
    }
}
