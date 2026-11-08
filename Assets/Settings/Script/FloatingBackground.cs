
using UnityEngine;

public class FloatingBackground : MonoBehaviour
{
    public float horizontalRange = 100f;
    public float verticalRange = 45f;

    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;

    private RectTransform rectTransform;
    private Vector2 startPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        float x = Mathf.Sin(Time.time * horizontalSpeed) * horizontalRange;
        float y = Mathf.Sin(Time.time * verticalSpeed) * verticalRange;

        rectTransform.anchoredPosition =
            startPosition + new Vector2(x, y);
    }
}
