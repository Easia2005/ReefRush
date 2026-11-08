using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    public float spinSpeed = 180f;
    public float bounceHeight = 0.2f;
    public float bounceSpeed = 2f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Spin the coin
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

        // Gently bounce up and down
        float newY = startPosition.y +
                     Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }
}