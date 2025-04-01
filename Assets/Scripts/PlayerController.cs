using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] 
    private InputActionAsset inputActions;

    private InputAction moveAction;
    private InputAction interactAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inputActions != null)
        {
            inputActions.Enable();
            moveAction = inputActions.FindActionMap("Player").FindAction("Move");
            interactAction = inputActions.FindActionMap("Player").FindAction("Interact");
        }

        if (moveAction != null)
        {
            moveAction.performed += OnMoveAction;
        }

        if (interactAction != null)
        {
            interactAction.performed += OnInteract;
        }

    }

    void OnMoveAction(InputAction.CallbackContext context)
    {
        this.gameObject.GetComponent<CharacterBehavior>().Mover.DesiredDirection = context.ReadValue<Vector2>();
    }

    void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Interacted");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
