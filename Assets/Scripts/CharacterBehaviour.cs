using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterBehavior : MonoBehaviour
{ 
    [SerializeField] private CharacterData m_CharacterData;
    private GameObject m_CharacterModel;
    private MovementBehaviour m_Movement;
    public GameObject CharacterModel => m_CharacterModel;
    public MovementBehaviour Mover => m_Movement;

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
