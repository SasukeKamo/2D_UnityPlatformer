using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.1f;
    float startPositionX;
    bool isMovingRight = false;
    public float moveRange = 1.0f;

    private void Awake()
    {
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
                    MoveRight();
                }
                else
                {
                    isMovingRight = false;
                    MoveLeft();
                }
            }
            else
            {
                if (this.transform.position.x > startPositionX - moveRange)
                {
                    MoveLeft();
                }
                else
                {
                    isMovingRight = true;
                    MoveRight();
                }
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
