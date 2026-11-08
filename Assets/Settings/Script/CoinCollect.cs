using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinManager.AddCoin();
            Destroy(gameObject);
        }
    }
}