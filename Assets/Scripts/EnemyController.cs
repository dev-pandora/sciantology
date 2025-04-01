using UnityEngine;



public class EnemyController : MonoBehaviour
{

    [SerializeField] private Group m_Group;
    [SerializeField] private Transform m_playerLeader;
    [SerializeField] private float m_detectionRange;


    //Steering variables
    private float m_WanderRate = 0.6f; //how much displacement
    private float m_WanderOffset = 6f; //offset of the circle
    private float m_WanderRadius = 4f;
    private float m_WanderOrientation = 0f;


    void Start()
    {
        //Get leader transform YAH
        if (m_Group != null && m_Group.Leader != null)
        {
            m_playerLeader = m_Group.Leader.transform;
        }
    }

    void Update()
    {
        if (m_playerLeader != null || m_Group.Leader == null) return;

        var leader = m_Group.Leader;
        Vector3 groupPosition = leader.transform.position;
        Vector3 direction;
        float distance = Vector3.Distance(groupPosition, m_playerLeader.transform.position);

        if (distance < m_detectionRange)
        {
            //if enemy flock is within detection range then seek
            direction = (m_playerLeader.position - groupPosition).normalized;
            direction = SeekSteering(groupPosition, m_playerLeader.position, leader.Mover.Speed);

        }
        else
        {
            //if outside detection range then wander
            float currentRot = m_Group.Leader.transform.eulerAngles.y * Mathf.Deg2Rad;
            direction= WanderSteering(groupPosition, currentRot);
        }

        //moving towards player leader
        MovementBehaviour mover = leader.Mover;
        if (mover != null)
        {
            mover.DesiredDirection = direction;
        }
    }

    private Vector3 WanderSteering(Vector3 currentPosition, float currentRotation)
    {
        m_WanderOrientation += Random.Range(-1.0f, 1.0f) * m_WanderRate;

        float targetOrientation = m_WanderOrientation + currentRotation;

        // Convert orientation to forward vector
        Vector3 agentForward = new Vector3(Mathf.Sin(currentRotation), 0f, Mathf.Cos(currentRotation));

        // Circle center ahead of agent
        Vector3 circleCenter = currentPosition + agentForward * m_WanderOffset;

        // New wander target on the circle
        Vector3 wanderTarget = circleCenter + new Vector3(Mathf.Sin(targetOrientation), 0f, Mathf.Cos(targetOrientation)) * m_WanderRadius;

        // Final steering direction
        Vector3 steering = (wanderTarget - currentPosition).normalized;
        return steering;
    }

    private Vector3 SeekSteering(Vector3 currentPosition, Vector3 targetPosition, float speed)
    {
        Vector3 direction = (targetPosition - currentPosition).normalized;
        return direction * speed;
    }
}

