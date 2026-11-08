
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float laneDistance = 2f;
    public float jumpForce = 7f;

    [Header("Running Speed")]
    public float forwardSpeed = 8f;
    public float maxSpeed = 14f;
    public float speedIncreasePerCoin = 0.05f;

    [Header("Start UI")]
    public GameObject startInstruction;

    [Header("Game Over UI")]
    public MenuManager menuManager;

    private int currentLane = 1;
    private Rigidbody rb;
    private bool isRunning = false;
    private bool gameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gameOver) return;

        int coins = CoinManager.coinCount;

        forwardSpeed = Mathf.Min(
            8f + coins * speedIncreasePerCoin,
            maxSpeed
        );

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isRunning)
            {
                isRunning = true;

                if (startInstruction != null)
                    startInstruction.SetActive(false);
            }
            else if (IsGrounded())
            {
                rb.AddForce(
                    Vector3.up * jumpForce,
                    ForceMode.Impulse
                );
            }
        }

        if (isRunning)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) ||
                Input.GetKeyDown(KeyCode.A))
            {
                currentLane = Mathf.Max(0, currentLane - 1);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow) ||
                Input.GetKeyDown(KeyCode.D))
            {
                currentLane = Mathf.Min(2, currentLane + 1);
            }
        }
    }

    void FixedUpdate()
    {
        if (!isRunning || gameOver) return;

        Vector3 position = rb.position;
        position.z += forwardSpeed * Time.fixedDeltaTime;
        position.x = (currentLane - 1) * laneDistance;

        rb.MovePosition(position);
    }

    bool IsGrounded()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            1.1f
        );
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            isRunning = false;

            if (menuManager != null)
                menuManager.ShowGameOver();

            Debug.Log("Game Over!");
        }
    }

    public void ResetGame()
    {
        currentLane = 1;
        isRunning = false;
        gameOver = false;

        if (startInstruction != null)
            startInstruction.SetActive(true);
    }
}
