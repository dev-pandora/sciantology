using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;


[BurstCompile]
struct FollowerSteeringBehaviorJob : IJobParallelFor
{
    [ReadOnly] public Vector3 leaderPosition;
    [ReadOnly] public float evasionRadius;
    [ReadOnly] public float separationRadius;
    [ReadOnly] public NativeArray<Vector3> agentPositions;

    public NativeArray<Vector3> desiredDirections;

    public void Execute(int index)
    {
        Vector3 position = agentPositions[index];
        Vector3 directionToLeader = (leaderPosition - position).normalized;
        Vector3 positionOutsideOfRadius = leaderPosition + (-directionToLeader * evasionRadius);
        Vector3 directionToFinalPosition = (positionOutsideOfRadius - position).normalized;

        Vector3 steeringDirection = directionToFinalPosition;

        //if ( directionToLeader.sqrMagnitude >= (evasionRadius * evasionRadius) )
        //{

        //    steeringDirection = directionToLeader.normalized; // Go to the leader
        //}
        //else
        //{ 
        //    steeringDirection = -directionToLeader.normalized; // Evade the leader
        //}

        //steeringDirection = directionToFinalPosition;
        desiredDirections[index] = steeringDirection;
    }
}

public class Group : MonoBehaviour
{
    [SerializeField] private GameObject m_CharacterPrefab;
    private List<CharacterBehavior> m_Follower = new List<CharacterBehavior>();
    private CharacterBehavior m_Leader;

    [SerializeField, Range(0, 10f)] private float m_EvasionRadius;
    [SerializeField, Range(0, 10f)] private float m_SeparationRadius;

    public CharacterBehavior Leader
    {
        get { return m_Leader; }
        set { m_Leader = value; }
    }

    public float EvasionRadius
    {
        get { return m_EvasionRadius; }
        set { m_EvasionRadius = value; }
    }

    public CharacterBehavior CreateCharacter(bool leader, Vector3 spawnPosition)
    {
        GameObject newCharacter = Instantiate(m_CharacterPrefab, Vector3.zero, Quaternion.identity);
        CharacterBehavior characterBehavior = newCharacter.GetComponent<CharacterBehavior>();
        newCharacter.transform.SetParent(transform);
        newCharacter.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);

        if (!leader) AddFollower(characterBehavior);
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
        NativeArray<Vector3> agentPositions = new();
        NativeArray<Vector3> desiredDirections = new();

        if (m_Leader == null || m_Follower.Count == 0) return;

        int count = m_Follower.Count;

        // Allocate NativeArrays
        if (!agentPositions.IsCreated || agentPositions.Length != count)
        {
            if (agentPositions.IsCreated) agentPositions.Dispose();
            if (desiredDirections.IsCreated) desiredDirections.Dispose();
            agentPositions = new NativeArray<Vector3>(count, Allocator.TempJob);
            desiredDirections = new NativeArray<Vector3>(count, Allocator.TempJob);
        }

        // Fill agent positions
        for (int i = 0; i < count; i++)
        {
            agentPositions[i] = m_Follower[i].transform.position;
        }

        // Create and schedule the job
        FollowerSteeringBehaviorJob movementJob = new FollowerSteeringBehaviorJob
        {
            leaderPosition = m_Leader.transform.position,
            evasionRadius = m_EvasionRadius,
            separationRadius = m_SeparationRadius,
            agentPositions = agentPositions,
            desiredDirections = desiredDirections
        };

        JobHandle jobHandle = movementJob.Schedule(count, 64);
        jobHandle.Complete(); // Wait for the job to finish (or use dependency for async execution)

        // Apply movement results
        for (int i = 0; i < count; i++)
        {
            m_Follower[i].Mover.DesiredDirection = desiredDirections[i].normalized;
        }

        // Dispose NativeArrays
        agentPositions.Dispose();
        desiredDirections.Dispose();
    }
}
    /*

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
    */