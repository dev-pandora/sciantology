using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;

    private void Start()
    {
        if (m_AudioSource != null && !m_AudioSource.isPlaying)
        {
            m_AudioSource.Play();
        }
    }
}
