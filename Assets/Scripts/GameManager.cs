using UnityEngine;
//using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    enum GameState
    {
        MainMenu,
        InGame,
        GameOver
    }

    [SerializeField] private GameObject m_GroupPrefab;
    [SerializeField] private CharacterData m_OwnerCharacter;


    [SerializeField,Range(0,150)] private float m_MaxTime;
    private float m_StartTime;

    private GameState m_GameState;
    
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

    private Group SpawnGroup(int amountMembers)
    {
        Vector3 groupPosition = new Vector3(Random.Range(-45f, 45f), 2, Random.Range(-45f, 45f));

        GameObject newGroup = Instantiate(m_GroupPrefab, Vector3.zero, Quaternion.identity);
        newGroup.transform.SetParent(transform);
        //newGroup.transform.SetPositionAndRotation(groupPosition, Quaternion.identity);

        Group group = newGroup.GetComponent<Group>();

        for (int i = 0; i < amountMembers; i++) {
            bool isLeader = group.Leader == null;
            Vector3 spawnPositionCharacter = groupPosition + new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));

            CharacterBehavior character = group.CreateCharacter(spawnPositionCharacter);
            character.LoadCharacter(m_OwnerCharacter); // Temporary to just load the character
            Debug.Log(character.transform.position);

            if (isLeader)
            {
                group.Leader = character; // Set the group's leader
            }

            group.AddFollower(character); // Add the character to the group
        }

        m_Groups.Add(group);
        return group;
    }

    private void SpawnPlayerGroup()
    {
        Debug.Log("Player group added !");
        SpawnGroup(1);
    }
    private void SpawnEnemyGroup(){
        // Spawn a set of AI groups that are gonna roam
        for (int groupIndex = 0; groupIndex < m_AmountGroups; ++groupIndex)
        {
            SpawnGroup(5); // Spawn an AI group
        }
    }
    private void StartGame()
    {
        SpawnPlayerGroup(); // Spawns the player and by extension the player.
        SpawnEnemyGroup(); // Spawns the enemy groups
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
