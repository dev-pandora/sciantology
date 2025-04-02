using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBehavior : MonoBehaviour
{ 
    [SerializeField] private CharacterData m_CharacterData;
    private GameObject m_CharacterModel;
    private MovementBehaviour m_Movement;
    private SphereCollider m_Collider;
    public GameObject CharacterModel => m_CharacterModel;
    public MovementBehaviour Mover => m_Movement;
    public SphereCollider Collider => m_Collider;

    public Group AssignedGroup { get; set; }

    private void Awake()
    {
        m_Movement = GetComponent<MovementBehaviour>();
    }
    private void Start()
    {
        bool characterLoaded = LoadCharacter(m_CharacterData); // Load the default character
    }

    private void Update()
    {
        m_Movement.UpdateMovement(); // Update the movement
    }

    public void CreateCollider()
    {
        if (m_Collider == null)
        {
            m_Collider = gameObject.AddComponent<SphereCollider>(); // Add collider
            m_Collider.isTrigger = true;
        }
    }

    public bool LoadCharacter(CharacterData character)
    {
        if (m_CharacterModel != null && m_CharacterData == character) return false;// If character is the same, go back
        m_CharacterData = character;

        // Create a character model
        GameObject characterModel = Instantiate(m_CharacterData.Model); // Create game object based on the character data's model.
        characterModel.name = m_CharacterData.Name; // Set the name of the character model
        characterModel.transform.SetParent(transform);
        characterModel.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity); // Transform shenanigans

        // Set the character model
        m_CharacterModel = characterModel;

        return true;
    } 
}
