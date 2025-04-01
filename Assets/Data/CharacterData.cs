using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private GameObject m_CharacterModel;
    [SerializeField] private string m_CharacterName;

    public string Name => m_CharacterName;
    public GameObject Model => m_CharacterModel;
} 
