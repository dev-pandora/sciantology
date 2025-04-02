using UnityEngine;



public class EnemyController : MonoBehaviour
{

    private Group m_Group;
    private Transform m_Target;
    private float m_DetectionRange;

    public float DetectionRange
    {
        get { return m_DetectionRange; }
        set { m_DetectionRange = value; }
    }

    public Group Group
    {
        get { return m_Group; }
        set { m_Group = value; }
    }
    public Transform Target
    {
        get { return m_Target; }
        set { m_Target = value; }
    }

    //Steering variables
    private float m_WanderOffset = 2f; //offset of the circle
    private float m_WanderRadius = 4f;

    private float m_WanderMaxAngleChange = 360f;
    private float m_WanderAngle = 0f;
    private Vector3 m_WanderForward = new Vector3(0,0,1);


    void Start()
    {
        //Get leader transform YAH
        //if (m_Group != null && m_Group.Leader != null)
        //{
        //    m_PlayerLeader = m_Group.Leader.transform;
        //}
    }

    void Update()
    {
        //if (m_PlayerLeader != null || m_Group.Leader == null) return;

        CharacterBehavior leader = m_Group.Leader;
        Vector3 groupPosition = leader.transform.position;
        Vector3 direction = Vector3.zero;
        float distance = Vector3.Distance(groupPosition, m_Target.position);

        //if (distance < m_DetectionRange)
        //{
        //    //if enemy flock is within detection range then seek
        //    direction = (m_PlayerLeader.position - groupPosition).normalized;
        //    direction = SeekSteering(groupPosition, m_PlayerLeader.position, leader.Mover.Speed);

        //}
        //else
        //{
        //    //if outside detection range then wander
        //    float currentRot = m_Group.Leader.transform.eulerAngles.y * Mathf.Deg2Rad;
        //    direction= WanderSteering();
        //}

        //moving towards player leader
        leader.Mover.DesiredDirection = WanderSteering();
        
    }

    private Vector3 WanderSteering()
    {
        Vector3 currentPosition = m_Group.Leader.transform.position;

        Vector3 steering = Vector3.zero;
        float maxAngleChange = m_WanderMaxAngleChange * Mathf.Deg2Rad;
        float randomAngle = maxAngleChange * Random.Range(0, 1f);

        Vector3 angleVector = new Vector3(Mathf.Sin(m_WanderAngle), 0, Mathf.Cos(m_WanderAngle));
        Vector3 originCircle = currentPosition + (m_WanderForward * m_WanderOffset);
        Vector3 target = originCircle + (angleVector * m_WanderRadius);
        Vector3 direction = (target - currentPosition).normalized;

        steering = direction;
        m_WanderForward = direction;
        m_WanderAngle += randomAngle;

        //m_WanderOrientation += Random.Range(-1.0f, 1.0f) * m_WanderRate;

        //float targetOrientation = m_WanderOrientation + currentRotation;

        //// Convert orientation to forward vector
        //Vector3 agentForward = new Vector3(Mathf.Sin(currentRotation), 0f, Mathf.Cos(currentRotation));

        //// Circle center ahead of agent
        //Vector3 circleCenter = currentPosition + (agentForward * m_WanderOffset);

        //// New wander target on the circle
        //Vector3 wanderOffset = new Vector3(Mathf.Sin(targetOrientation), 0f, Mathf.Cos(targetOrientation)) * m_WanderRadius;
        //Vector3 wanderTarget = circleCenter + wanderOffset;

        //// Final steering direction
        //Vector3 steering = (wanderTarget - currentPosition).normalized;
        return steering;
    }

    private Vector3 SeekSteering(Vector3 currentPosition, Vector3 targetPosition, float speed)
    {
        Vector3 direction = (targetPosition - currentPosition).normalized;
        return direction * speed;
    }
}

