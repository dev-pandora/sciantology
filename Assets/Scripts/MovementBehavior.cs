using UnityEngine;

public class MovementBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private CharacterController m_CharacterController;
    [SerializeField] private CharacterBehavior m_CharacterBehavior;

    public float Speed => m_Speed;

    private Vector3 m_DesiredDirection = Vector3.zero;
    private Vector3 m_DesiredOrientation = Vector3.zero;

    public Vector3 DesiredDirection { 
        get { return m_DesiredDirection; } 
        set { m_DesiredDirection = value; }
    }
    public Vector3 DesiredRotation
    {
        get { return m_DesiredOrientation; }
        set { m_DesiredOrientation = value; }
    }

    public CharacterController CharacterController => m_CharacterController;
    private void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_CharacterBehavior = GetComponent<CharacterBehavior>();
    }

    public void UpdateMovement()
    {
        if (m_CharacterController == null) return;

        Vector3 movementDirection = (DesiredDirection * m_Speed);
        m_CharacterController.SimpleMove(movementDirection);

        if (m_CharacterBehavior.CharacterModel) 
        {
            Vector3 flatLookAt = new Vector3(m_DesiredOrientation.x, 0, m_DesiredOrientation.z);
            if (flatLookAt != Vector3.zero)
            {
                m_CharacterBehavior.CharacterModel.transform.localRotation = Quaternion.LookRotation(flatLookAt);
            }
              
        }
    }



}
