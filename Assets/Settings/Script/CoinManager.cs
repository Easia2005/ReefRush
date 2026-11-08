
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static int coinCount = 0;

    public TextMeshProUGUI coinText;

    public AudioSource coinAudioSource;
    public AudioClip coinCollectSound;

    void Start()
    {
        coinCount = 0;
        UpdateUI();
    }

    public static void AddCoin()
    {
        coinCount++;

        CoinManager manager = FindFirstObjectByType<CoinManager>();

        if (manager != null)
        {
            manager.UpdateUI();
            manager.PlayCoinSound();
        }
    }

    void PlayCoinSound()
    {
        if (coinAudioSource != null && coinCollectSound != null)
        {
            coinAudioSource.PlayOneShot(coinCollectSound);
        }
    }

    public void ResetCoins()
    {
        coinCount = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (coinText != null)
            coinText.text = " " + coinCount;
    }
}
