using UnityEngine;
//using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        MainMenu,
        InGame,
        GameOver
    }

    [SerializeField] private GameObject m_BattleCanvas;
    [SerializeField] private CinemachineCamera m_CinemachineCamera;
    [SerializeField] private GameObject m_GroupPrefab;

    [SerializeField] private CharacterData m_OwnerCharacter;
    [SerializeField] private CharacterData m_MinionCharacter;

    [SerializeField] private CharacterData m_EnemyOwnerCharacter;
    [SerializeField] private CharacterData m_EnemyMinionCharacter;

    [SerializeField] private Vector3 m_Origin;
    [SerializeField] private SpawnLocation[] m_Spawns;
    [SerializeField] private Animator m_FadeInAnimator;

    [SerializeField] private SpawnLocation m_SpawnPointPlayer;


    [SerializeField, Range(0, 30)] private float m_SpawnInterval;
    [SerializeField,Range(0,10000)] private float m_MaxTime;
    private float m_StartTime;
    public float StartTime => m_StartTime;
    public float MaxTime => m_MaxTime;

    private GameState m_GameState;

    public UnityEvent OnGameEndEvent;


    private Group m_PlayerGroup;
    public Group PlayerGroup => m_PlayerGroup;

    [SerializeField] private int m_AmountStartGroups;
    [SerializeField] private int m_MinGroupsPerWave;
    [SerializeField] private int m_MaxGroupsPerWave;
    [SerializeField] private int m_MaxTotalGroups;
    [SerializeField] private int m_TimeIncreaseNPC;

    List<Group> m_Groups = new List<Group>();
    private float m_LastSpawnTime;
    private bool m_GameEnded;


    [SerializeField] private BattleMinigameController m_BattleMinigameController;
    public BattleMinigameController BattleMinigameController => m_BattleMinigameController;

    private SpawnLocation GetAvailableSpawnLocation()
    {
        return m_Spawns[Random.Range(0, m_Spawns.Length)];
    }

    private void InitializeGame()
    {
        m_StartTime = Time.time;
        m_LastSpawnTime = Time.time;

        if (m_BattleCanvas != null)
        {
            m_BattleCanvas.SetActive(false);
        }
    }

    private void Start()
    {
        m_GameState = GameState.InGame;
        InitializeGame();
        StartGame();
    }

    private void Update()
    {
        switch (m_GameState)
        {
            case GameState.InGame:
                UpdateInGame();
                break;
            case GameState.GameOver:
                UpdateGameOver();
                break;
        }
    }

    private Group SpawnGroup(int amountMembers,Vector3 spawnPosition,float spawnRange,bool enemy)
    {
        GameObject newGroup = Instantiate(m_GroupPrefab, Vector3.zero, Quaternion.identity);
        newGroup.transform.SetParent(transform);

        //newGroup.transform.SetPositionAndRotation(groupPosition, Quaternion.identity);

        Group group = newGroup.GetComponent<Group>();

        for (int i = 0; i < amountMembers; i++) {
            Vector3 spawnPositionCharacter = spawnPosition + new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            bool isLeader = group.Leader == null; 

            CharacterBehavior character = group.CreateCharacter(isLeader, spawnPositionCharacter);
            character.gameObject.layer = LayerMask.NameToLayer("Enemy");

            //character.Mover.DesiredDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            if (isLeader)
            {
                Debug.Log("Leader added !");
                group.Leader = character; // Set the group's leader

                CharacterData characterData = enemy ? m_EnemyOwnerCharacter : m_OwnerCharacter;
                character.LoadCharacter(characterData); // Temporary to just load the character
            } else
            {
                CharacterData characterData = enemy ? m_EnemyMinionCharacter : m_MinionCharacter;
                character.LoadCharacter(characterData);
            }

            character.AssignedGroup = group;
        }
         
        m_Groups.Add(group);
        return group;
    }

    private void SpawnPlayerGroup()
    {
        Debug.Log("Player group added !");

        Group playerGroup = SpawnGroup(1,m_SpawnPointPlayer.GetSpawnLocation(),5,false);
        playerGroup.SetTag("Player");
        playerGroup.gameObject.name = "PlayerGroup";

        playerGroup.Leader.gameObject.layer = LayerMask.NameToLayer("Player");


        BattleTrigger battleTrigger = playerGroup.Leader.gameObject.AddComponent<BattleTrigger>();
        battleTrigger.OwnerGroup = playerGroup;
        battleTrigger.GameManager = this;

        m_PlayerGroup = playerGroup;
    }
    private void SpawnEnemyGroup(int amount){
        // Spawn a set of AI groups that are gonna roam
        int amountInGroup = amount;
        SpawnLocation groupSpawnLocation = GetAvailableSpawnLocation();


        Vector3 groupPosition = groupSpawnLocation.GetSpawnLocation(); //m_Origin + new Vector3(Random.Range(-range, range), 2, Random.Range(-range, range));

        Group spawnedGroup = SpawnGroup(amountInGroup, groupPosition,5,true); // Spawn an AI group
        spawnedGroup.SetTag("Enemy");
        spawnedGroup.EvasionRadius /= 2;

        // Create an enemy controller
        EnemyController enemyController = spawnedGroup.gameObject.AddComponent<EnemyController>();
        enemyController.Group = spawnedGroup;
        enemyController.Target = m_PlayerGroup.Leader.transform;
        enemyController.DetectionRange = 30;

        // Add battle trigger
    }

    private void StartGame() 
    {
        SpawnPlayerGroup(); // Spawns the player and by extension the player.

        for (int i = 0; i < m_AmountStartGroups; i++)
        {
            SpawnEnemyGroup(2);
        }

        m_CinemachineCamera.Follow = m_PlayerGroup.Leader.transform;
        Debug.Log("Start game");
    }

    private void UpdateInGame()
    {
        // Check for the time
        float elapsedTime = Time.time - m_StartTime;
        if (elapsedTime > m_MaxTime) { m_GameState = GameState.GameOver; return; }
        
        float multiplier = 1 + (elapsedTime / m_TimeIncreaseNPC) ;
        int amountNPCs = (int)(2 * multiplier);
        // Spawn enemies while we can
        if (m_Groups.Count < m_MaxTotalGroups) {
            float lastGroupSpawnedElapsedTime = Time.time - m_LastSpawnTime;
            if (lastGroupSpawnedElapsedTime > m_SpawnInterval)
            {
                int amountGroups = Random.Range(m_MinGroupsPerWave, m_MaxGroupsPerWave);

                SpawnEnemyGroup(amountNPCs);
                m_LastSpawnTime = Time.time;
                Debug.Log("Spawned group");
            }
        }

        
    }


    public void GoToEndScreen()
    {
        EndScreenBehavior.Score = m_PlayerGroup.GetSize();
        OnGameEndEvent.Invoke();
    }



    private void UpdateGameOver()
    {
        if (m_GameEnded == false)
        {
            m_GameEnded = true;
            
            GoToEndScreen();
            Debug.Log("Game Over");
        }
    }

    public void HandleBattleGroupContact(Group contactedEnemy)
    {
        if (m_BattleMinigameController.IsBattleActive)
        {
            m_BattleMinigameController.AddEnemyGroupToCombet(contactedEnemy);
        }
        else
        {
            List<Group> initialEnemies = new List<Group> { contactedEnemy };
            m_BattleMinigameController.StartBattle(BattleTypeEnum.Mash, m_PlayerGroup, initialEnemies);
        }
    }
}
