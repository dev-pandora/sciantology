using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonMashMinigame : MonoBehaviour, IBattleMinigame
{
    [Header("Gameplay Settings")]
    [SerializeField] private float mashPower = 0.01f;
    [SerializeField] private float loseRate = 0.01f;
    [SerializeField] private float m_intervalTime = .5f;

    [SerializeField] private CharacterData m_MinionPlayer;
    [SerializeField] private CharacterData m_MinionEnemy;

    [Header("UI Reference")]
    [SerializeField] private BattleUI m_BattleUI;

    private float m_CurrentProgress = 0.5f;
    private float m_TotalPlayerPower;
    private float m_TotalEnemyPower;

    private bool m_IsComplete = false;
    private bool m_PlayerWon = false;
    private float m_TickTimer;

    private Group m_PlayerGroup;
    private Group[] m_EnemyGroups;

    private PlayerController m_PlayerController;

    public bool IsMinigameComplete => m_IsComplete;
    public bool PlayerWinBattle => m_PlayerWon;

    public void SetEnemyMinion(CharacterData minion)
    {
        m_MinionEnemy = minion;
    }

    public void SetPlayerMinion(CharacterData minion)
    {
        m_MinionPlayer = minion;
    }

    public void SetBattleUI(BattleUI ui)
    {
        m_BattleUI = ui;
    }

    public void Init(Group playerGroup, Group[] enemyGroups)
    {
        m_PlayerGroup = playerGroup;
        m_EnemyGroups = enemyGroups;

        playerGroup.InBattle = true; // Set the player's group as in battle

        m_TotalPlayerPower = Mathf.Max(1f, playerGroup.GetSize());
        m_TotalEnemyPower = 0f;

        foreach (Group group in enemyGroups)
        {
            if (group != playerGroup) // Ensure the player's group is not included in the enemy power calculation
            {
                m_TotalEnemyPower += Mathf.Max(1f, group.GetSize());
                group.InBattle = true;
            }
        }

        m_TickTimer = Time.time;
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

    private void TakeAllFollowers(Group winnerGroup, Group[] losers,CharacterData character)
    {
        if (!winnerGroup) return;

        foreach (Group loser in losers)
        {
            if (!loser) continue;
            foreach (CharacterBehavior follower in loser.Followers)
            {
                if (follower == null) continue;
                Debug.Log("Taking follower " + follower.name + " from group " + loser.name);
                SwapFollower(follower, winnerGroup,character);
            }
        }
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

    private void SwapFollower(CharacterBehavior follower,Group newGroup,CharacterData character)
    {


        follower.AssignedGroup.RemoveFollower(follower); // Remove followers
        newGroup.AddFollower(follower); // Add players 
        //follower.AssignedGroup = newGroup; // Set the enemy group as the new assigned group

        follower.LoadCharacter(character); // Load the player character data

        Debug.Log("Swapping follower " + follower.name + " to group " + newGroup.name);
    }

    public void Tick()
    {
        float elapsedTime = Time.time - m_TickTimer;
        if (elapsedTime < m_intervalTime) return;
        if (m_IsComplete) return;

        // TODO: Reward/loss logic
        if (m_CurrentProgress <= 0.5) {
            // Player losing...
             
            if (m_PlayerGroup.Followers.Length > 0)
            {
                Debug.Log("I'm losing !");

                SwapFollower(m_PlayerGroup.Followers[0], m_EnemyGroups[0],m_MinionEnemy); 
            }
        } else
        {
            // Player winning
            if (m_EnemyGroups[0].Followers.Length > 0) { 
                SwapFollower(m_EnemyGroups[0].Followers[0], m_PlayerGroup, m_MinionPlayer); // Swap the first enemy follower to the player group
            }

            Debug.Log("I'm winning !");
        }

        Debug.Log("[ButtonMash] Applying win/loss effects");
        m_TickTimer = Time.time;
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


        // Check if absolute wins/losses
        if (m_CurrentProgress <= 0f)
        {
            // Player lost
            Debug.Log("[ButtonMash] Player lost the minigame");
            List<Group> loserGroups = new List<Group>();
            
            TakeAllFollowers(m_EnemyGroups[0], loserGroups.ToArray(),m_MinionEnemy);
        } 
        else if (m_CurrentProgress >= 1f)
        {
            // Player won
            Debug.Log("[ButtonMash] Player won the minigame");
            TakeAllFollowers(m_PlayerGroup, m_EnemyGroups, m_MinionPlayer);

            // Take the leaders
            foreach (Group group in m_EnemyGroups)
            {
                if (group != null)
                {
                    SwapFollower(group.Leader, m_PlayerGroup, m_MinionPlayer); // Swap the enemy leader to the player group
                    Destroy(group.Leader.Collider);
                }
            }

        }

        // Toggle off battle mode
        m_PlayerGroup.InBattle = false;
        foreach (Group group in m_EnemyGroups)
        {
            if (group != null)
            {
                group.InBattle = false;
            }
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
