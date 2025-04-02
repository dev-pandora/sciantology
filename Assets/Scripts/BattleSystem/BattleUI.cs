using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Canvas m_BattleCanvas;
    [SerializeField] private Slider m_PlayerSlider;

    private void Awake()
    {
        ShowCanvas(false);
    }

    public void SetProgress(float progressNormalized)
    {
        if (m_PlayerSlider != null)
        {
            float clamped = Mathf.Clamp01(progressNormalized);
            m_PlayerSlider.value = clamped;
        }
    }
    public void ShowCanvas(bool show)
    {
        if (m_BattleCanvas != null)
        {
            m_BattleCanvas.gameObject.SetActive(show);
        }

        Debug.Log("[BattleUI] Canvas " + (show ? "enabled" : "disabled"));
    }

    public void SetVictoryState(bool playerWon)
    {
        Debug.Log(playerWon ? "Player won the battle!" : "Enemy won the battle!");
        // TODO: Replace this with actual UI feedback (text, animation, effects)
    }

    public void SetBarColor(Color color)
    {
        if (m_PlayerSlider != null)
        {
            Image fillImage = m_PlayerSlider.fillRect.GetComponent<Image>();
            if (fillImage != null)
            {
                fillImage.color = color;
            }
        }
    }
}
