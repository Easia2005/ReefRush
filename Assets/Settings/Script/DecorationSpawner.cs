
using System.Collections.Generic;
using UnityEngine;

public class DecorationSpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Tall Decorations: 3 Prefabs")]
    public GameObject[] tallPrefabs;

    [Header("Short Decorations: 3 Prefabs")]
    public GameObject[] shortPrefabs;

    [Header("Placement")]
    public float roadEdge = 3f;
    public float decorationDistance = 6f;
    public float tallHeight = 1.5f;
    public float shortHeight = 0.5f;

    [Header("Random Spacing")]
    public float minSpacing = 7f;
    public float maxSpacing = 12f;
    public float spawnAhead = 90f;
    public float destroyBehind = 15f;

    private readonly List<GameObject> decorations = new List<GameObject>();
    private float nextZ;
    private float startingZ;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("DecorationSpawner: Assign the Player!");
            enabled = false;
            return;
        }

        startingZ = player.position.z;
        ResetSpawner();
    }

    void Update()
    {
        float playerZ = player.position.z;

        for (int i = decorations.Count - 1; i >= 0; i--)
        {
            GameObject decoration = decorations[i];

            if (decoration == null)
            {
                decorations.RemoveAt(i);
                continue;
            }

            if (decoration.transform.position.z < playerZ - destroyBehind)
            {
                Destroy(decoration);
                decorations.RemoveAt(i);
            }
        }

        float targetZ = playerZ + spawnAhead;

        while (nextZ < targetZ)
        {
            SpawnRow(nextZ);
            nextZ += Random.Range(minSpacing, maxSpacing);
        }
    }

    void SpawnRow(float z)
    {
        // Choose tall or short independently for each side.
        SpawnDecoration(z, -1);
        SpawnDecoration(z, 1);
    }

    void SpawnDecoration(float z, int side)
    {
        bool chooseTall = Random.value < 0.5f;

        GameObject[] choices = chooseTall ? tallPrefabs : shortPrefabs;
        float y = chooseTall ? tallHeight : shortHeight;

        if (choices == null || choices.Length == 0)
            return;

        GameObject prefab = choices[Random.Range(0, choices.Length)];

        if (prefab == null)
            return;

        Vector3 position = new Vector3(
            side * (roadEdge + decorationDistance),
            y,
            z
        );

        GameObject decoration = Instantiate(
            prefab,
            position,
            Quaternion.identity,
            transform
        );

        decoration.name = prefab.name;
        decorations.Add(decoration);
    }

    public void ResetSpawner()
    {
        foreach (GameObject decoration in decorations)
        {
            if (decoration != null)
                Destroy(decoration);
        }

        decorations.Clear();

        if (player == null)
            return;

        nextZ = startingZ;

        float endZ = startingZ + spawnAhead;

        while (nextZ < endZ)
        {
            SpawnRow(nextZ);
            nextZ += Random.Range(minSpacing, maxSpacing);
        }
    }
}
