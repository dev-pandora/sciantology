using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] 
    private InputActionAsset inputActions;

    [SerializeField] private GameManager m_GameManager;

    private InputAction moveAction;
    private InputAction interactAction;

    public UnityEvent OnInteractEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //if (inputActions != null)
        //{
        //    inputActions.Enable();
        //    moveAction = inputActions.FindActionMap("Player").FindAction("Move");
        //    interactAction = inputActions.FindActionMap("Player").FindAction("Interact");
        //}

        //if (moveAction != null)
        //{
        //    moveAction.performed += OnMoveAction;
        //}

        //if (interactAction != null)
        //{
        //    interactAction.performed += OnInteract;
        //} 

    }

    public void OnMoveAction(InputAction.CallbackContext context)
    {
        //this.gameObject.GetComponent<CharacterBehavior>().Mover.DesiredDirection = context.ReadValue<Vector2>();
        Vector3 direction = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        m_GameManager.PlayerGroup.Leader.Mover.DesiredDirection = direction.normalized;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        OnInteractEvent.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
