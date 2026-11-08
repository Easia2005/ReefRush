
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform playerCamera;
    public GameObject[] housePrefabs;

    [Header("House Layout")]
    public float houseWidth = 3f;
    public float houseDepth = 3f;
    public float houseHeight = 5f;
    public float roadEdge = 3f;
    public float decorationGap = 0.5f;
    public float gapBetweenHouses = 0.1f;

    [Header("Spawning")]
    public float spawnAhead = 90f;
    public float destroyBehind = 15f;

    private float nextZ;
    private float startZ;
    private float startingPlayerZ;

    private readonly List<GameObject> houses = new();

    void Start()
    {
        if (player == null || playerCamera == null)
        {
            Debug.LogError(
                "HouseSpawner: Assign Player and Camera."
            );
            enabled = false;
            return;
        }

        startingPlayerZ = player.position.z;
        startZ = startingPlayerZ - houseDepth / 2f;

        ResetSpawner();
    }

    void Update()
    {
        if (player == null)
            return;

        // Use the player's position, not the camera.
        // The camera may lag behind after a restart.
        float playerZ = player.position.z;

        for (int i = houses.Count - 1; i >= 0; i--)
        {
            GameObject house = houses[i];

            if (house == null)
            {
                houses.RemoveAt(i);
                continue;
            }

            if (house.transform.position.z + houseDepth / 2f
                < playerZ - destroyBehind)
            {
                Destroy(house);
                houses.RemoveAt(i);
            }
        }

        float targetZ = playerZ + spawnAhead;
        float spacing = houseDepth + gapBetweenHouses;

        while (nextZ < targetZ)
        {
            SpawnHouseAt(nextZ, -1);
            SpawnHouseAt(nextZ, 1);
            nextZ += spacing;
        }
    }

    public void ResetSpawner()
    {
        foreach (GameObject house in houses)
        {
            if (house != null)
                Destroy(house);
        }

        houses.Clear();

        if (player == null)
            return;

        float spacing = houseDepth + gapBetweenHouses;
        float endZ = startingPlayerZ + spawnAhead;

        for (float z = startZ; z < endZ; z += spacing)
        {
            SpawnHouseAt(z, -1);
            SpawnHouseAt(z, 1);
        }

        nextZ = startZ +
            Mathf.Ceil((endZ - startZ) / spacing) * spacing;
    }

    void SpawnHouseAt(float z, int side)
    {
        if (housePrefabs == null || housePrefabs.Length == 0)
            return;

        GameObject prefab = housePrefabs[
            Random.Range(0, housePrefabs.Length)
        ];

        if (prefab == null)
            return;

        float x = side * (
            roadEdge + decorationGap + houseWidth / 2f
        );

        Vector3 position = new Vector3(
            x, houseHeight / 2f, z
        );

        Quaternion rotation = Quaternion.Euler(
            0f, side == -1 ? 90f : -90f, 0f
        );

        GameObject house = Instantiate(
            prefab, position, rotation, transform
        );

        // Keep the original naming style.
        house.name = prefab.name;

        house.transform.localScale = new Vector3(
            houseWidth, houseHeight, houseDepth
        );

        houses.Add(house);
    }
}
