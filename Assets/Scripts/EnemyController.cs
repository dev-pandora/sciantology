using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;



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
       //Get leader from game manager here
       //m_playerLeader=
    }

    void Update()
    {
        if (m_playerLeader != null || m_Group.Leader == null) return;

        Vector3 groupPosition = m_Group.transform.position;
        Vector3 direction;
        float distance = Vector3.Distance(groupPosition, m_playerLeader.transform.position);

        if (distance < m_detectionRange)
        {
            //if enemy flock is within detection range then seek
            direction = (m_playerLeader.position - groupPosition).normalized;
            //direction = Seek()
        }
        else
        {
            //if outside detection range then wander
            float currentRot = m_Group.Leader.transform.eulerAngles.y * Mathf.Deg2Rad;
            direction= WanderSteering(groupPosition, currentRot);
        }

        //moving towards player leader
        MovementBehaviour mover = m_Group.Leader.Mover;
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

    private Vector3 SeekSteering()
    {
        return Vector3.zero;
    }
}

