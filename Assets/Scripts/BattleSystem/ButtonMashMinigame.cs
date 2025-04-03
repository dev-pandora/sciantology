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

        m_PlayerController = FindAnyObjectByType<PlayerController>(); //dont change this
        if (m_PlayerController != null)
        {
            m_PlayerController.OnInteractEvent.AddListener(OnMashInput);
        }

    }

    public void UpdateMinigame()
    {
        if (m_IsComplete) return;

        // Enemy push bar dominate (enemy wins)
        float delta = loseRate * m_TotalEnemyPower * Time.deltaTime;
        m_CurrentProgress -= delta;
        m_CurrentProgress = Mathf.Clamp01(m_CurrentProgress);

        if (m_BattleUI != null)
            m_BattleUI.SetProgress(m_CurrentProgress);

        Tick();

        Debug.Log("[ButtonMash] Minigame initialized. PlayerPower: " + m_TotalPlayerPower + ", EnemyPower: " + m_TotalEnemyPower);
        Debug.Log($"[Tick] EnemyPressure: -{delta:F4} | CurrentProgress: {m_CurrentProgress:F3}");
        CheckForEnd();
    }

    public void OnMashInput()
    {
        if (m_IsComplete) return;

        // Player push bar dominate (player wins)
        m_CurrentProgress += mashPower * m_TotalPlayerPower;
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

    private void SwapFollower(CharacterBehavior follower,Group newGroup)
    {
        follower.AssignedGroup.RemoveFollower(follower); // Remove followers
        newGroup.AddFollower(follower); // Add players 
        follower.AssignedGroup = newGroup; // Set the enemy group as the new assigned group
        Debug.Log("Swapping follower " + follower.name + " to group " + newGroup.name);
    }

    public void Tick()
    {
        if (m_IsComplete) return;

        // TODO: Reward/loss logic
        if (m_CurrentProgress <= 0.5) {
            // Player losing...

            if (m_PlayerGroup.Followers.Length > 0)
            {
                Debug.Log("I'm losing !");

                SwapFollower(m_PlayerGroup.Followers[0], m_EnemyGroups[0]); 
            }
        } else
        {
            // Player winning
            if (m_EnemyGroups[0].Followers.Length > 0) { 
                SwapFollower(m_EnemyGroups[0].Followers[0], m_PlayerGroup); // Swap the first enemy follower to the player group
            }

            Debug.Log("I'm winning !");
        }

        Debug.Log("[ButtonMash] Applying win/loss effects");

        //EndMinigame();
    }

    public void EndMinigame()
    {
        Debug.Log("[ButtonMash] CALLING ENDMINIGAME");
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

    public void ForceEnd()
    {
        Debug.Log("[ButtonMash] Combat forcibly ended");
        m_IsComplete = true;
        m_PlayerWon = false; // defaulting to loss on flee — change if needed
        EndMinigame();
    }
}
