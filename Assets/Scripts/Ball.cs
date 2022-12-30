using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Racket;
    public Rigidbody2D ballRb;
    public float moveSpeed;
    public float maxSpeed = 50;

    void Start()
    {
        ballRb.velocity = new Vector2(1, 1) * moveSpeed;
    }

    private void Update()
    {
        if(Racket == null) Destroy(gameObject);
        if (Racket.GetComponent<Transform>().localPosition.x - 0.5 > transform.localPosition.x
            || transform.localPosition.x > 12
            || transform.localPosition.y > 6
            || transform.localPosition.y < -6)
        {
            Racket.GetComponent<RacketAI>().live = false;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Racket)
        {
            wayBall(collision, 1);
        }
    }

    private void wayBall(Collision2D collision, int x)
    {
        if(ballRb.velocity.y < maxSpeed)
        {
            float a = transform.position.y - collision.gameObject.transform.position.y;
            float b = collision.collider.bounds.size.y;
            float y = a / b;
            ballRb.velocity = new Vector2(x, y) * moveSpeed;
        }
    }
}
