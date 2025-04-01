using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Group : MonoBehaviour
{

    private List<CharacterBehavior> m_Follower = new List<CharacterBehavior>();
    private CharacterBehavior m_Leader;

    private float evasionRadius = 10;
    private float sqrEvasionRadius;

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
        sqrEvasionRadius = evasionRadius * evasionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (CharacterBehavior follower in m_Follower)
        {
            follower.Mover.DesiredDirection = GetSteering(follower);
        }
    }


    Vector3 GetSteering(CharacterBehavior follower)
    {
        Vector3 steering = Vector3.zero;

        // Check first if the ant is in the evasion range, if not then seek to leader

        Vector3 distanceVec = m_Leader.transform.position - follower.transform.position;

        if (distanceVec.magnitude < sqrEvasionRadius)
        {
            float distance = distanceVec.magnitude;

            Vector3 targetVelocity = m_Leader.Mover.CharacterController.velocity;
            float targetVelocityMagnitude = targetVelocity.magnitude;

            float time = (distance / targetVelocityMagnitude);
            Vector3 PredictedPosition = m_Leader.transform.position + targetVelocity * time;

            steering = -PredictedPosition + follower.transform.position;
            steering.Normalize();
            steering *= follower.Mover.Speed;
            return steering;
        } else
        {
            steering = distanceVec;
            steering.Normalize();
            steering *= follower.Mover.Speed;

           

            return steering;
        }
    }

   //IEnumerator followLeaderCoroutine()
   // {
   //     while (true)
   //     {
   //         foreach (CharacterBehavior follower in m_Follower)
   //         {
   //            follower.Mover.DesiredDirection = GetSteering(follower);
   //         }
   //         yield return null;
   //     }
   // }
}
