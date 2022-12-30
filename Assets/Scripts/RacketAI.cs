using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketAI : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed;
    public Transform ball;
    public bool live = true;

    private void Update()
    {
        if(!live) Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if(!live || ball == null) Destroy(gameObject);
        if(live)
        {
            float distance = Mathf.Abs(ball.localPosition.y - transform.localPosition.y);
            if (distance > 2)
            {
                if (ball.localPosition.y > transform.localPosition.y)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1) * moveSpeed;
                }

                if (ball.localPosition.y < transform.localPosition.y)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(0, -1) * moveSpeed;
                }
            }
        }
    }
}
