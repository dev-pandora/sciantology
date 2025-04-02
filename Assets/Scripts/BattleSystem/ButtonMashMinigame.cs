using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonMashMinigame : MonoBehaviour, IBattleMinigame
{
    [SerializeField] private float mashPower = 0.01f;
    [SerializeField] private float loseRate = 0.01f;

    [SerializeField] private BattleUI m_BattleUI;

    private float m_MinBarScale = 0f;
    private float m_MaxBarScale = 1f;
    private float m_StartBarScalePercentage = 0.5f;
    private float m_CurrentBarPercentage = 0.5f;
    private float m_TotalPlayerPower;
    private float m_TotalEnemyPower;

    private bool isComplete = false;
    private bool playerWon = false;

    private Group m_PlayerGroup;
    private Group[] m_EnemyGroup;

    PlayerController playerController;

    public bool IsMinigameComplete => isComplete;
    public bool PlayerWinBattle => playerWon;


    public void Init(Group playerGroup, Group[] enemyGroups)
    {
        m_PlayerGroup = playerGroup;
        m_EnemyGroup = enemyGroups;

        //calc init powerr
        m_TotalPlayerPower = Mathf.Max(1f, playerGroup.GetSize());
        m_TotalEnemyPower = 0f;
        foreach (Group group in enemyGroups)
        {
            m_TotalEnemyPower += Mathf.Max(1f, group.GetSize());
        }

        //reset
        m_CurrentBarPercentage = m_StartBarScalePercentage;
        isComplete = false;
        playerWon = false;

        //show ui
        if (m_BattleUI != null)
        {
            m_BattleUI.gameObject.SetActive(true);
            m_BattleUI.ShowCanvas(true);
            m_BattleUI.SetProgress(m_CurrentBarPercentage);
        }

        //register on interact event
        playerController = m_PlayerGroup.Leader.GetComponentInParent<PlayerController>();
        if (playerController != null)
        {
            playerController.OnInteractEvent.AddListener(OnMashInput);
            
        }

        Debug.Log("Minigame Initialized");
        Debug.Log($"UI Assigned? {(m_BattleUI != null)}");
        Debug.Log($"Player Power: {m_TotalPlayerPower}, Enemy Power: {m_TotalEnemyPower}");
    }
    public void UpdateMinigame()
    {
        if (isComplete) return;

        //Enemy pull 
        m_CurrentBarPercentage -= loseRate * m_TotalEnemyPower * Time.deltaTime;
        m_CurrentBarPercentage = Mathf.Clamp(m_CurrentBarPercentage, m_MinBarScale, m_MaxBarScale);

        if (m_BattleUI != null)
            m_BattleUI.SetProgress(m_CurrentBarPercentage);

        // Check for win/lose condition
        CheckMinigameComplete();

        Debug.Log($"[Update] Progress: {m_CurrentBarPercentage:F2}");
    }

    private void CheckMinigameComplete()
    {
        if (m_CurrentBarPercentage <= 0f || m_CurrentBarPercentage >= 1f)
        {
            isComplete = true;
            playerWon = (m_CurrentBarPercentage <= 0f); // closer to 0 = player wins
        }
    }

    public void Tick()
    {
        if (!isComplete) return;

        //add and lose followers here
        if (playerWon)
        {
            Debug.Log("Gain followers");
        }

        EndMinigame();
    }

    public void EndMinigame()
    {
        if (m_BattleUI != null)
        {
            m_BattleUI.ShowCanvas(false);
            m_BattleUI.SetVictoryState(playerWon);
        }

        if (playerController != null)
        {
            playerController.OnInteractEvent.RemoveListener(OnMashInput);
        }

        Destroy(this);
    }

    public void OnMashInput()
    {
        if (isComplete) return;

        m_CurrentBarPercentage -= mashPower * m_TotalPlayerPower;
        m_CurrentBarPercentage = Mathf.Clamp01(m_CurrentBarPercentage);

        if (m_BattleUI != null)
            m_BattleUI.SetProgress(m_CurrentBarPercentage);

        CheckMinigameComplete();

        Debug.Log("MASH INPUT RECEIVED");
    }
}
