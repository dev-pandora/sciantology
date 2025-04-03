using UnityEngine;

public class ParticleEffectSpawner : MonoBehaviour
{
    public static ParticleEffectSpawner Instance;

    public GameObject enemyToPlayerEffect;
    public GameObject playerToEnemyEffect;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayEffect(Vector3 position, bool toPlayer)
    {
        GameObject effectPrefab = toPlayer ? enemyToPlayerEffect : playerToEnemyEffect;
        if (effectPrefab != null)
        {
            GameObject fx = Instantiate(effectPrefab, position, Quaternion.identity);
            Destroy(fx, 3f); // Destroy GameObject after 3 seconds
            Debug.Log("[ParticleEffectSpawner] Playing effect: " + (toPlayer ? "Smoke (Enemy to Player)" : "Fire (Player to Enemy)") + " at " + position);
        }
    }
}