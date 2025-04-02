using System.Collections.Generic;
using UnityEngine;

public class BattleMinigameController : MonoBehaviour
{
    [SerializeField] private float m_TickInterval = 1.0f;

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

        m_CurrentMinigame = InstantiateMinigame(type);
        m_CurrentMinigame.Init(playerGroup, enemyGroupsInCombat[0]);

        m_TickTimer = 0f;
        m_IsABattleActive = true;
        gameObject.SetActive(true);
    }

    //call in game manager or smt to add new group into current combat
    public void AddEnemyGroupToCombet(Group group)
    {
        if(!enemyGroupsInCombat.Contains(group))
        {
            enemyGroupsInCombat.Add(group);
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