using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Group : MonoBehaviour
{
    [SerializeField] private GameObject m_CharacterPrefab;
    private List<CharacterBehavior> m_Follower = new List<CharacterBehavior>();
    private CharacterBehavior m_Leader;

    [SerializeField,Range(0,10f)] private float evasionRadius = 0.85f;
    [SerializeField,Range(0,10f)] private float m_NeighborhoodRadius = 1f;
    [SerializeField, Range(0, 10f)] private float m_SeparationRadius = 1f;

    public CharacterBehavior Leader
    {
        get { return m_Leader; }
        set { m_Leader = value; }
    }

    public float EvasionRadius
    {
        get { return evasionRadius; }
        set { evasionRadius = value; }
    }

    public CharacterBehavior CreateCharacter(Vector3 spawnPosition)
    {
        GameObject newCharacter = Instantiate(m_CharacterPrefab, Vector3.zero, Quaternion.identity);
        CharacterBehavior characterBehavior = newCharacter.GetComponent<CharacterBehavior>();
        newCharacter.transform.SetParent(transform);
        newCharacter.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);

        AddFollower(characterBehavior);
        return characterBehavior;
    }
    public void AddFollower(CharacterBehavior follower)
    {
        m_Follower.Add(follower);
    }

    public void RemoveFollower(CharacterBehavior follower)
    {
        m_Follower.Remove(follower);
    }

    public int GetSize()
    {
        return m_Follower.Count;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        foreach (CharacterBehavior follower in m_Follower)
        {
            if (m_Leader == null) return;
            if (follower == m_Leader) continue;

            Vector3 position = follower.transform.position;
            Vector3 leaderPosition = m_Leader.transform.position;
            Vector3 directionToLeader = leaderPosition - position;

            Vector3 steeringDirection = Vector3.zero;
            if (directionToLeader.magnitude >= evasionRadius)
            {
                steeringDirection = CalculateSeek(follower, 1.0f) + CalculateSeparate(follower,1f);
            }
            else
            {
                steeringDirection = CalculateEvade(follower, 1f);
            }

            follower.Mover.DesiredDirection = steeringDirection.normalized;
        }
    } 

    Vector3 CalculateSeek(CharacterBehavior follower,float weight)
    {
        float speed = follower.Mover.Speed;
        Vector3 position = follower.transform.position;

        Vector3 desiredDirection = (m_Leader.transform.position - position).normalized * m_Leader.Mover.Speed;
        return desiredDirection * weight;
    }

    Vector3 CalculateEvade(CharacterBehavior follower,float weight)
    {
        float speed = follower.Mover.Speed;
        Vector3 position = follower.transform.position;
        Vector3 anticipatedPosition = m_Leader.transform.position + m_Leader.Mover.DesiredDirection * m_Leader.Mover.Speed;

        Vector3 desiredDirection = (position - anticipatedPosition).normalized * m_Leader.Mover.Speed;
        return desiredDirection * weight;
    }

    Vector3 CalculateSeparate(CharacterBehavior follower,float weight)
    {
        Vector3 steeringDirection = Vector3.zero;
        int count = 0;

        foreach (CharacterBehavior otherFollower in m_Follower)
        {
            if (otherFollower == follower) continue;
            Vector3 position = follower.transform.position;
            Vector3 otherPosition = otherFollower.transform.position;
            Vector3 direction = position - otherPosition;
            float distance = direction.magnitude;

            if (distance > m_NeighborhoodRadius) continue;
            if (distance < m_SeparationRadius)
            {
                steeringDirection += direction.normalized;
                count++;
            }
        }

        if (count > 0)
        {
            return (steeringDirection / count) * follower.Mover.Speed * weight;
        }

        return Vector3.zero;
    }
}
