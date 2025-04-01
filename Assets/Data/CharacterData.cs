using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private GameObject m_CharacterData;
    public GameObject Model => m_CharacterData;
}
