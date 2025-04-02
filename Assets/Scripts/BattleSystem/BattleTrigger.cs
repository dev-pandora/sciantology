using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    public Group ownerGroup;                  // Reference to the player group that owns this trigger
    public GameManager gameManager;           // Reference to the GameManager (set in GameManager.cs)

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.name); // DEBUG

        // Only respond to enemy group leaders (not the player)
        if (!other.CompareTag("Player"))
        {

        }
    }

    private void OnDrawGizmosSelected()
    {
        if (ownerGroup != null && ownerGroup.Leader != null)
        {
            Gizmos.color = Color.green;
            float radius = Mathf.Max(3f, ownerGroup.GetSize() + 1);
            Gizmos.DrawWireSphere(ownerGroup.Leader.transform.position, radius);
        }
    }
}
