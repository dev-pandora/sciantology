using UnityEngine;
//using System;
using System.Collections.Generic;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        MainMenu,
        InGame,
        GameOver
    }

    [SerializeField] private CinemachineCamera m_CinemachineCamera;
    [SerializeField] private GameObject m_GroupPrefab;
    [SerializeField] private CharacterData m_OwnerCharacter;
    [SerializeField] private Vector3 m_Origin;


    [SerializeField,Range(0,150)] private float m_MaxTime;
    private float m_StartTime;

    private GameState m_GameState;


    private Group m_PlayerGroup;
    public Group PlayerGroup => m_PlayerGroup;

    List<Group> m_Groups = new List<Group>();
    [SerializeField] private int m_AmountGroups;
    
    private void Awake()
    {
        m_GameState = GameState.MainMenu;
        m_StartTime = Time.time;
        
    }

    private void Update()
    {
        switch (m_GameState)
        {
            case GameState.MainMenu:
                UpdateMainMenu();
                break;
            case GameState.InGame:
                UpdateInGame();
                break;
            case GameState.GameOver:
                UpdateGameOver();
                break;
        }
    }

    private void UpdateMainMenu()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_GameState = GameState.InGame;
            StartGame();
        }
    }

    private Group SpawnGroup(int amountMembers,Vector3 spawnPosition,float spawnRange)
    {
        GameObject newGroup = Instantiate(m_GroupPrefab, Vector3.zero, Quaternion.identity);
        newGroup.transform.SetParent(transform);
        //newGroup.transform.SetPositionAndRotation(groupPosition, Quaternion.identity);

        Group group = newGroup.GetComponent<Group>();

        for (int i = 0; i < amountMembers; i++) {
            Vector3 spawnPositionCharacter = spawnPosition + new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
            bool isLeader = group.Leader == null;

            CharacterBehavior character = group.CreateCharacter(isLeader, spawnPositionCharacter);
            character.LoadCharacter(m_OwnerCharacter); // Temporary to just load the character
            //character.Mover.DesiredDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));

            if (isLeader)
            {
                Debug.Log("Leader added !");
                group.Leader = character; // Set the group's leader
            }
        }

        m_Groups.Add(group);
        return group;
    }

    private void SpawnPlayerGroup()
    {
        Debug.Log("Player group added !");
        Group playerGroup = SpawnGroup(5,m_Origin,30);
        m_PlayerGroup = playerGroup;
    }
    private void SpawnEnemyGroup(int amount){
        // Spawn a set of AI groups that are gonna roam
        int amountInGroup = amount;

        for (int groupIndex = 0; groupIndex < m_AmountGroups; ++groupIndex)
        {
            Vector3 groupPosition = m_Origin + new Vector3(Random.Range(-45f, 45f), 2, Random.Range(-45f, 45f));

            Group spawnedGroup = SpawnGroup(amountInGroup, groupPosition,20); // Spawn an AI group
            spawnedGroup.EvasionRadius /= 2;

            // Create an enemy controller
            EnemyController enemyController = spawnedGroup.gameObject.AddComponent<EnemyController>();
            enemyController.Group = m_PlayerGroup;
            enemyController.Target = m_PlayerGroup.Leader.transform;
            enemyController.DetectionRange = 30;
            //

        }
    }
    private void StartGame() 
    {
        SpawnPlayerGroup(); // Spawns the player and by extension the player.
        SpawnEnemyGroup(2); // Spawns the enemy groups
        m_CinemachineCamera.Follow = m_PlayerGroup.Leader.transform;
        Debug.Log("Start game");
    }

    private void UpdateInGame()
    {
        // Check for the time
        float elapsedTime = Time.time - m_StartTime;
        if (elapsedTime > m_MaxTime) m_GameState = GameState.GameOver;
    }

    private void UpdateGameOver()
    {
        Debug.Log("Game Over");
    }


}
