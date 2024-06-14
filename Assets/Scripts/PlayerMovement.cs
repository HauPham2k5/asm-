using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] int maxJumps = 2;
    [SerializeField] GameObject arrowPrefab; // Prefab đạn
    [SerializeField] Transform arrowSpawnPoint; // Điểm xuất hiện đạn
    [SerializeField] float arrowSpeed = 10f; // Tốc độ đạn

    Vector2 moveInput;
    Rigidbody2D myRi;
    Animator myA;
    bool isGrounded;
    int jumpCount; // Số lần nhảy

    void Start()
    {
        myRi = GetComponent<Rigidbody2D>();
        myA = GetComponent<Animator>();
    }

    void Update()
    {
        Run();
        FlipSprite();
        CheckGrounded();
    }

    // Xử lý sự kiện di chuyển từ Input System
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Xử lý sự kiện nhảy từ Input System
    void OnJump(InputValue value)
    {
        if (value.isPressed && (isGrounded || jumpCount < maxJumps))
        {
            Jump();
        }
    }
    void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            FireArrow();
        }
    }


    // Hàm điều khiển chạy của nhân vật
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRi.velocity.y);
        myRi.velocity = playerVelocity;
        bool hasHorizontalSpeed = Mathf.Abs(myRi.velocity.x) > Mathf.Epsilon;
        myA.SetBool("isRunning", hasHorizontalSpeed);
    }

    // Hàm đảo hướng sprite của nhân vật
    void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRi.velocity.x) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRi.velocity.x), 1f);
        }
    }

    // Hàm xử lý nhảy của nhân vật
    void Jump()
    {
        if (jumpCount == 0)
        {
            myA.SetBool("isDoubleJump", false);
        }
        else if (jumpCount == 1)
        {
            myA.SetBool("isDoubleJump", true);
        }

        myRi.velocity = new Vector2(myRi.velocity.x, jumpForce);
        jumpCount++;
    }

    // Hàm kiểm tra nhân vật có đang chạm đất không
    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        myA.SetBool("isGrounded", isGrounded);

        if (isGrounded)
        {
            jumpCount = 0;
            myA.SetBool("isDoubleJump", false);
        }
    }
    void FireArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        Rigidbody2D arrowRb = arrow.GetComponent<Rigidbody2D>();

        if (arrowRb != null)
        {
            float direction = transform.localScale.x; // Xác định hướng nhân vật đang đối diện
            arrowRb.velocity = new Vector2(direction * arrowSpeed, 0f);
        }
    }
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Poir"))
        {
            transform.position = arrowSpawnPoint.position; // Quay lại vị trí xuất phát
            myRi.velocity = Vector2.zero; // Reset vận tốc
            Debug.Log("Player hit a monster and returned to spawn point.");
        }
    }

}
