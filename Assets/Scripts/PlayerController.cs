using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    //private InputActionAsset inputActions;

    [SerializeField] private GameManager m_GameManager;

    private InputAction moveAction;
    private InputAction interactAction;

    public UnityEvent OnInteractEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        Vector3 direction = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        m_GameManager.PlayerGroup.Leader.Mover.DesiredDirection = direction.normalized;
        m_GameManager.PlayerGroup.Leader.Mover.DesiredRotation = direction.normalized;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            OnInteractEvent.Invoke();
        }
        //Debug.Log("Interacting");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
