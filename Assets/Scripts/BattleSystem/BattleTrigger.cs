using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    private Group m_OwnerGroup; // Reference to the player group that owns this trigger
    public GameManager m_GameManager;   // Reference to the GameManager (set in GameManager.cs)

    public Group OwnerGroup
    {
        get { return m_OwnerGroup; }
        set { m_OwnerGroup = value; }
    }

    public GameManager GameManager
    {
        get { return m_GameManager; }
        set { m_GameManager = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name); // DEBUG

        // Only respond to enemy group leaders (not the player)
        Debug.Log(other.tag);
        if (other.CompareTag("Enemy"))
        {
            CharacterBehavior enemy = other.gameObject.GetComponent<CharacterBehavior>();
            if (enemy)
                Debug.Log("Hit player");
            {
                Group otherGroup = enemy.AssignedGroup;

                Debug.Log(otherGroup);
                if (otherGroup)
                {
                    Debug.Log("Hit enemy group");
                    m_GameManager.HandleBattleGroupContact(otherGroup); 
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            CharacterBehavior enemy = other.GetComponent<CharacterBehavior>();
            if (enemy != null)
            {
                Group enemyGroup = enemy.AssignedGroup;
                if (enemyGroup != null)
                {
                    Debug.Log("(BattleTrigger) Player exited combat zone of " + m_OwnerGroup.name);

                    BattleMinigameController controller = m_GameManager.BattleMinigameController;
                    if (controller != null && controller.IsBattleActive)
                    {
                        Debug.Log("CALLING NotifyEnemyGroupDisengaged");
                        controller.NotifyEnemyGroupDisengaged(enemyGroup);
                    }
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (m_OwnerGroup != null && m_OwnerGroup.Leader != null)
        {
            Gizmos.color = Color.green;
            float radius = Mathf.Max(3f, m_OwnerGroup.GetSize() + 1);
            Gizmos.DrawWireSphere(m_OwnerGroup.Leader.transform.position, radius);
        }
    }
}
