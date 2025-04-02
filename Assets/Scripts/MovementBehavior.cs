using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private CharacterController m_CharacterController;

    public float Speed => m_Speed;

    private Vector3 m_DesiredDirection = Vector3.zero;
    public Vector3 DesiredDirection { 
        get { return m_DesiredDirection; } 
        set { m_DesiredDirection = value; }
    } 
    public CharacterController CharacterController => m_CharacterController;
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    public void UpdateMovement()
    {
        if (m_CharacterController == null) return;

        Vector3 movementDirection = (DesiredDirection * m_Speed);
        m_CharacterController.SimpleMove(movementDirection);
    }



}
