using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            offset.x,
            player.position.y + offset.y,
            player.position.z + offset.z
        );
    }
}