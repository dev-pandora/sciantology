using System.ComponentModel;
using TMPro;
using UnityEngine;

public class HUDBehavior : MonoBehaviour
{

    [SerializeField] 
    private GameManager m_GameManager;
    [SerializeField]
    private TextMeshProUGUI m_TimeText;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Update is called once per frame
    void Update()
    {
        float elapsedTime = (Time.time - m_GameManager.StartTime);
        float time = m_GameManager.MaxTime - elapsedTime;
        int minutes = Mathf.FloorToInt(time/60);
        int seconds = Mathf.FloorToInt(time % 60);

        if (time > 0)
        {
            m_TimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else
        {
            m_TimeText.gameObject.SetActive(false); 
        }

        //if (elapsedTime < 5) { m_TimeText.gameObject.SetActive(false); };

    }
}
