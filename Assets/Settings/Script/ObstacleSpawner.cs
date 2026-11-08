
using UnityEngine;
using System.Collections.Generic;

[DefaultExecutionOrder(-100)]
public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public Transform player;
    public SpawnCoordinator spawnCoordinator;

    public float spawnDistance = 40f;
    public float rowSpacing = 15f;
    public float cleanupDistance = 10f;

    private float nextSpawnZ = 20f;
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    void Update()
    {
        if (player == null || obstaclePrefab == null)
            return;

        while (nextSpawnZ < player.position.z + spawnDistance)
        {
            SpawnObstacle();
            nextSpawnZ += rowSpacing;
        }

        for (int i = spawnedObstacles.Count - 1; i >= 0; i--)
        {
            GameObject obstacle = spawnedObstacles[i];

            if (obstacle == null)
            {
                spawnedObstacles.RemoveAt(i);
                continue;
            }

            if (obstacle.transform.position.z <
                player.position.z - cleanupDistance)
            {
                if (spawnCoordinator != null)
                    spawnCoordinator.RemoveObstacle(obstacle.transform.position);

                Destroy(obstacle);
                spawnedObstacles.RemoveAt(i);
            }
        }
    }

    void SpawnObstacle()
    {
        int coins = CoinManager.coinCount;

        float secondObstacleChance = 0.5f;

        if (coins >= 70)
            secondObstacleChance = 1f;
        else if (coins >= 50)
            secondObstacleChance = 0.8f;
        else if (coins >= 30)
            secondObstacleChance = 0.65f;

        int firstLane = Random.Range(0, 3);
        SpawnInLane(firstLane);

        if (Random.value < secondObstacleChance)
        {
            int secondLane = Random.Range(0, 2);

            if (secondLane >= firstLane)
                secondLane++;

            SpawnInLane(secondLane);
        }
    }

    void SpawnInLane(int lane)
    {
        float x = (lane - 1) * 2f;
        Vector3 position = new Vector3(x, 0.6f, nextSpawnZ);

        GameObject obstacle = Instantiate(
            obstaclePrefab,
            position,
            Quaternion.identity
        );

        spawnedObstacles.Add(obstacle);

        if (spawnCoordinator != null)
            spawnCoordinator.RegisterObstacle(position);
    }

    public bool IsLaneBlocked(int lane, float z, float safeDistance)
    {
        float laneX = (lane - 1) * 2f;

        foreach (GameObject obstacle in spawnedObstacles)
        {
            if (obstacle == null)
                continue;

            bool sameLane =
                Mathf.Abs(obstacle.transform.position.x - laneX) < 0.5f;

            bool closeEnough =
                Mathf.Abs(obstacle.transform.position.z - z) < safeDistance;

            if (sameLane && closeEnough)
                return true;
        }

        return false;
    }

    public void ResetSpawner()
    {
        foreach (GameObject obstacle in spawnedObstacles)
        {
            if (obstacle != null)
                Destroy(obstacle);
        }

        spawnedObstacles.Clear();
        nextSpawnZ = 20f;

        if (spawnCoordinator != null)
            spawnCoordinator.ClearObstacles();
    }
}
