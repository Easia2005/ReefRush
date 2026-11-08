using UnityEngine;

public class LaneMarkingLoop : MonoBehaviour
{
    public Transform player;
    public Transform laneMarkings;

    public float roadLength = 30f;

    void Update()
    {
        if (player == null || laneMarkings == null)
            return;

        if (player.position.z > laneMarkings.position.z + roadLength / 2f)
        {
            laneMarkings.position = new Vector3(
                laneMarkings.position.x,
                laneMarkings.position.y,
                laneMarkings.position.z + roadLength
            );
        }
    }
}