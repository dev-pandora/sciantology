using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private RectTransform m_PlayerBar;
    [SerializeField] private RectTransform m_FollowerOverlay;
    [SerializeField] private Canvas m_BattleCanvas;
    [SerializeField] private float m_TotalBarWidth = 100f;

    private void Awake()
    {
        ShowCanvas(false);
    }

    public void SetProgress(float progressNormalized)
    {
        float clamped = Mathf.Clamp01(progressNormalized);
        float playerWidth = (1f - clamped) * m_TotalBarWidth;

        if (m_PlayerBar != null)
        {
            m_PlayerBar.sizeDelta = new Vector2(playerWidth, m_PlayerBar.sizeDelta.y);
        }

        if (m_FollowerOverlay != null)
        {
            //m_PlayerBar.transform.
            float overlayX = m_PlayerBar.anchoredPosition.x + playerWidth;
            m_FollowerOverlay.anchoredPosition = new Vector2(overlayX, m_FollowerOverlay.anchoredPosition.y);
        }
    }

    public void ShowCanvas(bool show)
    {
        if (m_BattleCanvas != null)
        {
            m_BattleCanvas.enabled = show;
        }

        if (m_BattleCanvas.enabled == true)
            Debug.Log("Battle canvas shown");
    }

    public void SetVictoryState(bool playerWon)
    {
        Debug.Log(playerWon ? "Player won the battle!" : "Enemy won the battle!");
        // TODO: Replace this with actual UI feedback (text, animation, effects)
    }

    public void SetBarColor(Color playerColor)
    {
        Image barImage = m_PlayerBar.GetComponent<Image>();
        if (barImage != null)
        {
            barImage.color = playerColor;
        }
    }
}
