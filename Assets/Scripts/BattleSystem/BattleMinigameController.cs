using System.Collections.Generic;
using UnityEngine;

public class BattleMinigameController : MonoBehaviour
{
    [SerializeField] private CharacterData m_MinionPlayer;
    [SerializeField] private CharacterData m_MinionEnemy;
    [SerializeField] private float m_TickInterval = 1.0f;
    [SerializeField] private BattleUI m_BattleUI;

    private IBattleMinigame m_CurrentMinigame;
    private float m_TickTimer;
    private bool m_IsABattleActive;

    private Group m_playerGroup;
    private List<Group> enemyGroupsInCombat = new List<Group>(); //list of enemy groups in current battle

    public bool IsBattleActive => m_IsABattleActive; //expose if battle is active
    public bool PlayerWon => m_CurrentMinigame!=null && m_CurrentMinigame.PlayerWinBattle; //expose if player won battle

    public void StartBattle(BattleTypeEnum type, Group playerGroup, List<Group> startingEnemies)
    {
        if (m_IsABattleActive) return;

        m_playerGroup = playerGroup;
        enemyGroupsInCombat.Clear();
        enemyGroupsInCombat.AddRange(startingEnemies);

        ButtonMashMinigame minigame = gameObject.AddComponent<ButtonMashMinigame>();

        minigame.SetBattleUI(m_BattleUI);
        minigame.SetEnemyMinion(m_MinionEnemy);
        minigame.SetPlayerMinion(m_MinionPlayer);   

        // Init minigame
        minigame.Init(playerGroup, enemyGroupsInCombat.ToArray());

        // Store reference for update/tick
        m_CurrentMinigame = minigame;

        m_TickTimer = 0f;
        m_IsABattleActive = true;
        gameObject.SetActive(true);

        Debug.Log("Battle Started");
        Debug.Log($"BattleUI passed to minigame: {m_BattleUI != null}");
    }

    //call in game manager or smt to add new group into current combat
    public void AddEnemyGroupToCombet(Group group)
    {
        if(!enemyGroupsInCombat.Contains(group))
        {
            enemyGroupsInCombat.Add(group);
            group.InBattle = true; // Set the group as in battle
            Debug.Log("Enemy group joined combat: " + group.name);
        }
    }

    private void Update()
    {
        if (!m_IsABattleActive || m_CurrentMinigame == null) return;

        int totalEnemyPower = 0;
        foreach (var group in enemyGroupsInCombat)
        {
            totalEnemyPower += group.GetSize() + 1;
        }

        m_CurrentMinigame.UpdateMinigame();

        m_TickTimer += Time.deltaTime;
        if(m_TickTimer> m_TickInterval)
        {
            m_CurrentMinigame.Tick();
            m_TickTimer = 0f;
        }

        if(m_CurrentMinigame.IsMinigameComplete)
        {
            EndBattle();
        }
    }
    
    public void EndBattle()
    {
        if (!m_IsABattleActive) return;
        bool playerWon = m_CurrentMinigame.PlayerWinBattle;
        m_CurrentMinigame.EndMinigame();
        m_IsABattleActive = false;
    }

    public void NotifyEnemyGroupDisengaged(Group group)
    {
        if (!enemyGroupsInCombat.Contains(group)) return;

        group.InBattle = false; // Set the group as not in battle
        enemyGroupsInCombat.Remove(group);
        Debug.Log("(BattleController) ENEMY GROUP DISENGAGED: " + group.name);

        // If no more enemies group in combat, end it
        if (enemyGroupsInCombat.Count <= 0 && m_CurrentMinigame is ButtonMashMinigame mash)
        {
            Debug.Log("(BattleController) NO MORE ENEMIES WE END COMBAT");
            mash.ForceEnd();
            m_CurrentMinigame = null;
            m_IsABattleActive = false;
        }
    }

    private IBattleMinigame InstantiateMinigame(BattleTypeEnum type)
    {
        switch (type)
        {
            case BattleTypeEnum.Mash:
                return gameObject.AddComponent<ButtonMashMinigame>();
            case BattleTypeEnum.QTE:
                return null;
            case BattleTypeEnum.KeyStroke:
                return null;
            default:
                Debug.Log("no battle type");
                return null;
        }
    }
}