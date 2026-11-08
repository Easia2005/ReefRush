
using UnityEngine;

public class RoadLoop : MonoBehaviour
{
    public Transform player;
    public Transform road1;
    public Transform road2;
    public Transform road3;
    public Transform road4;

    public float roadLength = 30f;

    private float[] startingZ = new float[4];

    void Start()
    {
        SaveStartingPositions();
    }

    void Update()
    {
        if (player == null || road1 == null ||
            road2 == null || road3 == null || road4 == null)
            return;

        Transform[] roads = { road1, road2, road3, road4 };

        Transform firstRoad = roads[0];

        for (int i = 1; i < roads.Length; i++)
        {
            if (roads[i].position.z < firstRoad.position.z)
                firstRoad = roads[i];
        }

        if (player.position.z > firstRoad.position.z + roadLength)
        {
            float furthestZ = roads[0].position.z;

            for (int i = 1; i < roads.Length; i++)
            {
                if (roads[i].position.z > furthestZ)
                    furthestZ = roads[i].position.z;
            }

            firstRoad.position = new Vector3(
                firstRoad.position.x,
                firstRoad.position.y,
                furthestZ + roadLength
            );
        }
    }

    void SaveStartingPositions()
    {
        startingZ[0] = road1.position.z;
        startingZ[1] = road2.position.z;
        startingZ[2] = road3.position.z;
        startingZ[3] = road4.position.z;
    }

    public void ResetRoads()
    {
        road1.position = new Vector3(
            road1.position.x, road1.position.y, startingZ[0]);

        road2.position = new Vector3(
            road2.position.x, road2.position.y, startingZ[1]);

        road3.position = new Vector3(
            road3.position.x, road3.position.y, startingZ[2]);

        road4.position = new Vector3(
            road4.position.x, road4.position.y, startingZ[3]);
    }
}
