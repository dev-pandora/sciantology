using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonMashMinigame : MonoBehaviour, IBattleMinigame
{
    [Header("Gameplay Settings")]
    [SerializeField] private float mashPower = 0.01f;
    [SerializeField] private float loseRate = 0.01f;

    [Header("UI Reference")]
    [SerializeField] private BattleUI m_BattleUI;

    private float m_CurrentProgress = 0.5f;
    private float m_TotalPlayerPower;
    private float m_TotalEnemyPower;

    private bool m_IsComplete = false;
    private bool m_PlayerWon = false;

    private Group m_PlayerGroup;
    private Group[] m_EnemyGroups;

    private PlayerController m_PlayerController;

    public bool IsMinigameComplete => m_IsComplete;
    public bool PlayerWinBattle => m_PlayerWon;

    public void SetBattleUI(BattleUI ui)
    {
        m_BattleUI = ui;
    }

    public void Init(Group playerGroup, Group[] enemyGroups)
    {
        m_PlayerGroup = playerGroup;
        m_EnemyGroups = enemyGroups;

        m_TotalPlayerPower = Mathf.Max(1f, playerGroup.GetSize());
        m_TotalEnemyPower = 0f;
        foreach (Group group in enemyGroups)
        {
            m_TotalEnemyPower += Mathf.Max(1f, group.GetSize());
        }

        m_CurrentProgress = 0.5f;
        m_IsComplete = false;
        m_PlayerWon = false;

        if (m_BattleUI != null)
        {
            m_BattleUI.ShowCanvas(true);
            m_BattleUI.SetProgress(m_CurrentProgress);
        }

        m_PlayerController = m_PlayerGroup.Leader.GetComponentInParent<PlayerController>();
        if (m_PlayerController != null)
        {
            m_PlayerController.OnInteractEvent.AddListener(OnMashInput);
        }

        Debug.Log("[ButtonMash] Minigame initialized. PlayerPower: " + m_TotalPlayerPower + ", EnemyPower: " + m_TotalEnemyPower);
    }

    public void UpdateMinigame()
    {
        if (m_IsComplete) return;

        // Enemy pulls bar toward 1 (enemy wins)
        m_CurrentProgress += loseRate * m_TotalEnemyPower * Time.deltaTime;
        m_CurrentProgress = Mathf.Clamp01(m_CurrentProgress);

        if (m_BattleUI != null)
            m_BattleUI.SetProgress(m_CurrentProgress);

        CheckForEnd();
    }

    public void OnMashInput()
    {
        if (m_IsComplete) return;

        // Player pulls bar toward 0 (player wins)
        m_CurrentProgress -= mashPower * m_TotalPlayerPower;
        m_CurrentProgress = Mathf.Clamp01(m_CurrentProgress);

        if (m_BattleUI != null)
            m_BattleUI.SetProgress(m_CurrentProgress);

        CheckForEnd();

        Debug.Log("[ButtonMash] Mash input received. New progress: " + m_CurrentProgress);
    }

    private void CheckForEnd()
    {
        if (m_CurrentProgress <= 0f || m_CurrentProgress >= 1f)
        {
            m_IsComplete = true;
            m_PlayerWon = (m_CurrentProgress <= 0f);
            Debug.Log("[ButtonMash] Battle complete. Player won? " + m_PlayerWon);
        }
    }

    public void Tick()
    {
        if (!m_IsComplete) return;

        // TODO: Reward/loss logic
        Debug.Log("[ButtonMash] Applying win/loss effects");

        EndMinigame();
    }

    public void EndMinigame()
    {
        if (m_BattleUI != null)
        {
            m_BattleUI.ShowCanvas(false);
            m_BattleUI.SetVictoryState(m_PlayerWon);
        }

        if (m_PlayerController != null)
        {
            m_PlayerController.OnInteractEvent.RemoveListener(OnMashInput);
        }

        Destroy(this);
    }
}
