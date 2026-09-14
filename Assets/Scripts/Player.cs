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

    private int score;
    private Vector2 velocity;
    private bool isGrounded;
    private float xMax;
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool StarPower { get; private set; }

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        xMax = Camera.main.orthographicSize * Camera.main.aspect;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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

        if (transform.position.x < -xMax + 0.5f && inputAxis <= 0)
            velocity.x = 0;
        else
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
        score += count;
        scoreText.text = score.ToString();
    }

    private IEnumerator StarPowerAnimation(float duration)
    {
        StarPower = true;
        speed *= speedCoefficient;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (Time.frameCount % 4 == 0)
            {
                spriteRenderer.color = Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f);
            }
            yield return null;
            elapsed += Time.deltaTime;
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
