
using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform player;
    public SpawnCoordinator spawnCoordinator;

    public float spawnDistance = 40f;
    public float rowSpacing = 5f;
    public float cleanupDistance = 10f;
    public float safeDistance = 2f;

    [Header("Jump Coins")]
    public float normalCoinHeight = 1f;
    public float jumpCoinHeight = 2f;

    [Range(0f, 1f)]
    public float jumpCoinChance = 0.3f;

    private float nextSpawnZ = 15f;
    private List<GameObject> spawnedCoins = new List<GameObject>();
    private ObstacleSpawner obstacleSpawner;

    void Start()
    {
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
    }

    void Update()
    {
        if (player == null || coinPrefab == null)
            return;

        int coins = CoinManager.coinCount;

        if (coins >= 70)
            rowSpacing = 12f;
        else if (coins >= 60)
            rowSpacing = 9f;
        else if (coins >= 50)
            rowSpacing = 7f;
        else
            rowSpacing = 5f;

        while (nextSpawnZ < player.position.z + spawnDistance)
        {
            List<int> safeLanes = new List<int>();
            List<int> blockedLanes = new List<int>();

            for (int lane = 0; lane < 3; lane++)
            {
                bool blocked = obstacleSpawner != null &&
                    obstacleSpawner.IsLaneBlocked(
                        lane, nextSpawnZ, safeDistance
                    );

                if (blocked)
                    blockedLanes.Add(lane);
                else
                    safeLanes.Add(lane);
            }

            bool spawned = false;

            if (blockedLanes.Count > 0 &&
                Random.value < jumpCoinChance)
            {
                ShuffleLanes(blockedLanes);

                foreach (int lane in blockedLanes)
                {
                    if (TrySpawnCoin(lane, nextSpawnZ, jumpCoinHeight))
                    {
                        spawned = true;
                        break;
                    }
                }
            }

            if (!spawned && safeLanes.Count > 0)
            {
                ShuffleLanes(safeLanes);

                foreach (int lane in safeLanes)
                {
                    if (TrySpawnCoin(lane, nextSpawnZ, normalCoinHeight))
                    {
                        spawned = true;
                        break;
                    }
                }
            }

            nextSpawnZ += rowSpacing;
        }

        for (int i = spawnedCoins.Count - 1; i >= 0; i--)
        {
            GameObject coin = spawnedCoins[i];

            if (coin == null)
            {
                spawnedCoins.RemoveAt(i);
                continue;
            }

            if (coin.transform.position.z <
                player.position.z - cleanupDistance)
            {
                Destroy(coin);
                spawnedCoins.RemoveAt(i);
            }
        }
    }

    bool TrySpawnCoin(int lane, float z, float height)
    {
        float x = (lane - 1) * 2f;
        Vector3 position = new Vector3(x, height, z);

        if (spawnCoordinator != null &&
            spawnCoordinator.IsPositionBlocked(
                position,
                0.5f,
                0.5f,
                0.5f))
        {
            return false;
        }

        GameObject coin = Instantiate(
            coinPrefab,
            position,
            Quaternion.identity
        );

        spawnedCoins.Add(coin);
        return true;
    }

    void ShuffleLanes(List<int> lanes)
    {
        for (int i = lanes.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (lanes[i], lanes[j]) = (lanes[j], lanes[i]);
        }
    }

    public void ResetSpawner()
    {
        foreach (GameObject coin in spawnedCoins)
        {
            if (coin != null)
                Destroy(coin);
        }

        spawnedCoins.Clear();
        nextSpawnZ = 15f;
        rowSpacing = 5f;
    }
}
