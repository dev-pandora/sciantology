using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    [SerializeField] private bool m_IsOccupied;
    [SerializeField] private float m_Radius;
    [SerializeField] private Vector3 m_Offset;

    public Vector3 GetSpawnLocation()
    {
        Vector3 randomOffset = new Vector3(Random.Range(-m_Radius, m_Radius), 0, Random.Range(-m_Radius, m_Radius));
        Vector3 position = (transform.position + m_Offset) + randomOffset;

        return position;
    }
}
