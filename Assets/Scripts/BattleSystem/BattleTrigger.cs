using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    //when overlap then call start battle
    [SerializeField] private Group ownerGroup;                   
    [SerializeField] private GameManager gameManager;

    private float DetectionRadius => Mathf.Max(3f, ownerGroup.GetSize() + 1);

    private void OnTriggerEnter(Collider other)
    {
        // Attempt to get the Group component from the object that entered the trigger
        Group otherGroup = other.GetComponent<Group>();

        if (otherGroup == null || ownerGroup == null || gameManager == null)
            return;

        // Detect if the player group has entered this trigger
        if (gameManager.PlayerGroup == otherGroup)
        {
            // Ask GameManager to handle battle logic (either start or add to existing)
            //gameManager.HandleGroupContact(ownerGroup);
        }
        Debug.Log("TRIGGERINGGGGGGGGGGGGGGGGGGG");
    }

    private void OnDrawGizmosSelected()
    {
        // show detection radius
        if (ownerGroup != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(ownerGroup.Leader.transform.position, DetectionRadius);
        }
    }
}
