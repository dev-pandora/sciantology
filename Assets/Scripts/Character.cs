using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{ 
    [SerializeField] private CharacterData m_CharacterData;

    private GameObject m_CharacterModel;

    private void Start()
    {
        // Create a character
        GameObject characterModel = Instantiate(m_CharacterData.Model, Vector3.zero, Quaternion.identity); // Create game object based on the character data's model.

        // Transform shenanigans
        characterModel.transform.SetParent(transform);

        // Set the character model
        m_CharacterModel = characterModel;
    }
}
