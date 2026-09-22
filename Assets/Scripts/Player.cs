using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private Text scoreText;
    [SerializeField] private float speedCoefficient;
    [SerializeField] private SceneTransitionDataScript sceneData;

    private int score;
    private Vector2 velocity;
    private bool isGrounded;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool StarPower { get; private set; }

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        scoreText.text = sceneData.playerScore.ToString();
        if (sceneData.wasTransition)
        {
            transform.position = sceneData.position;

            if (sceneData.bonusDuration > 0)
            {
                StarPowerActive(sceneData.bonusDuration);
            }
        }   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            Jump();
        }
    }

    private void FixedUpdate()
    {
        float inputAxis = Input.GetAxis("Horizontal");
        velocity = rigidbody2d.velocity;

        velocity.x = inputAxis * speed;       

        rigidbody2d.velocity = velocity; 

        if (inputAxis < 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (inputAxis > 0)
        {
            spriteRenderer.flipX = true;
        }

        if (isGrounded)
        {
            if (inputAxis != 0)
            {
                animator.SetInteger("State", 1);
            }
            else
            {
                animator.SetInteger("State", 0);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded && collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = collision.contacts.All(c => c.point.y < transform.position.y);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = !collision.contacts.All(c => c.point.y > transform.position.y);
    }

    private void Jump()
    {
        animator.SetInteger("State", 2);
        rigidbody2d.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    public void AddCoin(int count)
    {
        sceneData.playerScore += count;
        scoreText.text = sceneData.playerScore.ToString();
    }

    private IEnumerator StarPowerAnimation(float duration)
    {
        StarPower = true;
        speed *= speedCoefficient;

        while (duration > 0)
        {
            if (Time.frameCount % 4 == 0)
            {
                spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
            yield return null;
            duration -= Time.deltaTime;
            sceneData.bonusDuration = duration;
        }

        speed /= speedCoefficient;
        spriteRenderer.color = Color.white;
        StarPower = false;
    }

    public void StarPowerActive(float duration = 5f)
    {
        StartCoroutine(StarPowerAnimation(duration));
    }
}
