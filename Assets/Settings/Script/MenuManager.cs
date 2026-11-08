
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject startInstruction;

    public Transform player;
    public PlayerMovement playerMovement;
    public CoinManager coinManager;

    public RoadLoop roadLoop;
    public ObstacleSpawner obstacleSpawner;
    public CoinSpawner coinSpawner;
    public HouseSpawner houseSpawner;
    public DecorationSpawner decorationSpawner;
    public SeabedLoop seabedLoop;

    private Vector3 startPosition;

    void Start()
    {
        if (player != null)
            startPosition = player.position;
    }

    public void StartGame()
    {
        if (titlePanel != null)
            titlePanel.SetActive(false);

        if (gamePanel != null)
            gamePanel.SetActive(true);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (startInstruction != null)
            startInstruction.SetActive(true);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    void ResetWorld()
    {
        if (roadLoop != null)
            roadLoop.ResetRoads();

        if (obstacleSpawner != null)
            obstacleSpawner.ResetSpawner();

        if (coinSpawner != null)
            coinSpawner.ResetSpawner();

        if (houseSpawner != null)
            houseSpawner.ResetSpawner();

        if (decorationSpawner != null)
            decorationSpawner.ResetSpawner();

        if (seabedLoop != null)
            seabedLoop.ResetSeabed();
    }

    void ResetPlayer()
    {
        if (player != null)
        {
            player.position = startPosition;

            Rigidbody rb = player.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (playerMovement != null)
            playerMovement.ResetGame();
    }

    public void TryAgain()
    {
        if (player == null || playerMovement == null)
        {
            Debug.LogError("Player or PlayerMovement is not connected!");
            return;
        }

        ResetPlayer();
        ResetWorld();

        if (coinManager != null)
            coinManager.ResetCoins();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (gamePanel != null)
            gamePanel.SetActive(true);

        if (titlePanel != null)
            titlePanel.SetActive(false);

        if (startInstruction != null)
            startInstruction.SetActive(true);
    }

    public void ExitToTitle()
    {
        ResetPlayer();
        ResetWorld();

        if (coinManager != null)
            coinManager.ResetCoins();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (gamePanel != null)
            gamePanel.SetActive(false);

        if (titlePanel != null)
            titlePanel.SetActive(true);
    }
}
