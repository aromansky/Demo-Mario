using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rigidbody2d;
    private Vector2 direction = Vector2.left;
    private float xMax;
    private Player player;
    
    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rigidbody2d.velocity = direction * speed;
        xMax = Camera.main.orthographicSize * Camera.main.aspect;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        if (rigidbody2d == null) return;

        if (transform.position.x < -xMax + 0.5f && direction.x <= 0 || 
            transform.position.x > xMax - 0.5f && direction.x > 0)
            ChangeDirection(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rigidbody2d == null) return;
        
        if (collision.gameObject.tag == "Player")
        {
            bool dead = collision.contacts.All(c => c.point.y > transform.position.y);
            if (dead || player.StarPower)
            {
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject);
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
        else
        {
            ChangeDirection(!collision.contacts.All(c => c.point.y < transform.position.y));
        }
    }

    private void ChangeDirection(bool makeChange)
    {
        if (rigidbody2d == null) return;

        if (makeChange)
        {
            direction = -direction;
            Vector2 velocity = rigidbody2d.velocity;
            velocity.x = direction.x * speed;
            rigidbody2d.velocity = velocity;
        }
    }
}
