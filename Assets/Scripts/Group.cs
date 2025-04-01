using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Group : MonoBehaviour
{

    private List<Character> m_Follower = new List<Character>();
    private Character m_Leader;

    public Character Leader
    {
        get { return m_Leader; }
        set { m_Leader = value; }
    }

    public void AddFollower(Character follower)
    {
        m_Follower.Add(follower);
    }

    public void RemoveFollower(Character follower)
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
        
    }
}
