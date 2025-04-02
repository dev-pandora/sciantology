using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //private InputActionAsset inputActions;

    [SerializeField] private GameManager m_GameManager;

    private InputAction moveAction;
    private InputAction interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        Vector3 direction = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        m_GameManager.PlayerGroup.Leader.Mover.DesiredDirection = direction.normalized;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interacted");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
