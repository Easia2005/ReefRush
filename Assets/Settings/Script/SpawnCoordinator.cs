
using UnityEngine;
using System.Collections.Generic;

public class SpawnCoordinator : MonoBehaviour
{
    public float minimumCoinObstacleDistance = 3f;

    private readonly List<Vector3> obstaclePositions =
        new List<Vector3>();

    public void RegisterObstacle(Vector3 position)
    {
        obstaclePositions.Add(position);
    }

    public bool IsPositionBlocked(
        Vector3 coinPosition,
        float coinRadius,
        float obstacleHalfWidth,
        float obstacleHalfDepth)
    {
        foreach (Vector3 obstaclePosition in obstaclePositions)
        {
            bool sameLane =
                Mathf.Abs(coinPosition.x - obstaclePosition.x) < 0.5f;

            bool tooClose =
                Mathf.Abs(coinPosition.z - obstaclePosition.z)
                < minimumCoinObstacleDistance;

            bool overlapsX =
                Mathf.Abs(coinPosition.x - obstaclePosition.x)
                < coinRadius + obstacleHalfWidth;

            bool overlapsZ =
                Mathf.Abs(coinPosition.z - obstaclePosition.z)
                < coinRadius + obstacleHalfDepth;

            if (overlapsX && overlapsZ)
                return true;

            if (sameLane && tooClose)
                return true;
        }

        return false;
    }

    public void RemoveObstacle(Vector3 position)
    {
        obstaclePositions.Remove(position);
    }

    public void ClearObstacles()
    {
        obstaclePositions.Clear();
    }
}
