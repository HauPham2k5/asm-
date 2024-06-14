using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float moveDistance = 3f; // Khoảng cách di chuyển
    private Vector2 startingPosition; // Vị trí bắt đầu của quái vật
    private Rigidbody2D myRigidbody;
    private bool movingRight = true;
    void OnTriggerEnter2D(Collider2D other)
    {
        // Đổi hướng khi quái vật chạm phải vật cản
        if (!other.CompareTag("Player")) // Nếu không phải là người chơi
        {
            FlipDirection();
        }
    }
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position; // Lưu lại vị trí bắt đầu
    }

    void Update()
    {
        MoveBackAndForth();
    }

    void MoveBackAndForth()
    {
        if (movingRight)
        {
            myRigidbody.velocity = new Vector2(moveSpeed, myRigidbody.velocity.y);
            if (Vector2.Distance(startingPosition, transform.position) >= moveDistance)
            {
                movingRight = false;
                FlipDirection();
            }
        }
        else
        {
            myRigidbody.velocity = new Vector2(-moveSpeed, myRigidbody.velocity.y);
            if (Vector2.Distance(startingPosition, transform.position) <= 0)
            {
                movingRight = true;
                FlipDirection();
            }
        }
    }

    void FlipDirection()
    {
        // Đổi hướng hiển thị của sprite
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
