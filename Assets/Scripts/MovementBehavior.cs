using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    private CharacterController m_CharacterController;
    public Vector3 DesiredDirection { get; set; } 
    public CharacterController CharacterController => m_CharacterController;
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    public void UpdateMovement()
    {
        if (m_CharacterController == null) return;
        Vector3 movement = (DesiredDirection * m_Speed * Time.deltaTime);
        m_CharacterController.SimpleMove(movement);

    }
}
