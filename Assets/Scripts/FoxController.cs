using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action { collect, kill };

public class FoxController : MonoBehaviour
{
    Rigidbody2D body;
    public const float rayLength = 0.2f;
    public LayerMask groundLayer;
    private float horizontalAxis;
    private float horizontalInput;
    private Animator animator;
    static public int keysFound = 0;
    public int keysNumber = 3;
    public int maxHP = 3;

    [SerializeField] public float jumpForce = 6.0f;
    [SerializeField] private float moveSpeed = 0.1f;
    //public GameObject levelUI;
    static public int lives = 3;
    Vector2 startPosition;
    [SerializeField] public AudioClip bSound;
    [SerializeField] public AudioClip healSound;
    [SerializeField] public AudioClip killSound;
    [SerializeField] public AudioClip deathSound;
    [SerializeField] public AudioClip fallSound;
    [SerializeField] public AudioClip jumpSound;
    [SerializeField] public AudioClip gemSound;
    private AudioSource[] sources;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sources = GetComponents<AudioSource>();
        startPosition = transform.position;
        keysFound = 0;
        lives = 3;
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameState.GS_GAME)
        {
            horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput > 0.01f)
            {
                transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            else if (horizontalInput < -0.01f)
            {
                transform.localScale = new Vector3(-0.7f, 0.7f, 0.7f);
            }
            horizontalAxis = horizontalInput * moveSpeed;
            horizontalAxis *= Time.deltaTime;
            transform.Translate(horizontalAxis, 0.0f, 0.0f, Space.World);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
            {
                Jump();
            }
            //Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.white, 1, false);
            animator.SetBool("isGrounded", isGrounded());
            animator.SetBool("isWalking", horizontalInput != 0);
        }
    }

    private void Jump()
    {
        sources[0].clip = jumpSound;
        sources[0].PlayOneShot(jumpSound, AudioListener.volume);
        body.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
    }

    bool isGrounded()
    {
        return Physics2D.Raycast(this.transform.position, Vector2.down, rayLength, groundLayer.value);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Win") && keysFound == keysNumber)
        {
            GameManager.instance.totalScore += 100 * lives;
            GameManager.instance.LevelCompleted();
        }
        else if (other.CompareTag("Win") && keysFound != keysNumber)
        {
            Debug.Log("not all keys collected");
        }
        else if (other.CompareTag("Health") && lives!=maxHP)
        {
            sources[0].clip = healSound;
            sources[0].PlayOneShot(healSound, AudioListener.volume);
            lives++;
            Debug.Log("EXTRA LIFE");
            other.gameObject.SetActive(false);
            GameManager.instance.AddHP();
        }
        if(other.CompareTag("Enemy") && transform.position.y <= other.gameObject.transform.position.y)
        {
            sources[0].clip = deathSound;
            sources[0].PlayOneShot(deathSound, AudioListener.volume);
            Death();
        }
        if(other.CompareTag("FallLevel"))
        {
            sources[0].clip = fallSound;
            sources[0].PlayOneShot(fallSound, AudioListener.volume);
            Death();
        }
        if (other.CompareTag("MovingPlatform"))
        {
            transform.SetParent(other.transform);
        }
        if (other.CompareTag("Bonus"))
        {
            sources[0].clip = bSound;
            sources[0].PlayOneShot(bSound, AudioListener.volume);
            other.gameObject.SetActive(false);
            GameManager.instance.AddPoints(10);
            GameManager.instance.AddTotalScore(Action.collect);
        }
        if (other.CompareTag("Key"))
        {
            sources[0].clip = gemSound;
            sources[0].PlayOneShot(gemSound, AudioListener.volume);
            GameManager.instance.AddKeys();
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Enemy") && transform.position.y > other.gameObject.transform.position.y)
        {
            sources[0].clip = killSound;
            sources[0].PlayOneShot(killSound, AudioListener.volume);
            GameManager.instance.AddKills();
            Debug.Log("Killed an enemy");
            GameManager.instance.AddTotalScore(Action.kill);
        }

    }

    void Death()
    {
        transform.position = startPosition;
        lives--;
        if (lives < 0)
        {
            Debug.Log("GAME OVER!");
            GameManager.instance.GameOver();
        }
        else
        {
            GameManager.instance.LostHP();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        transform.SetParent(null);
    }

}
