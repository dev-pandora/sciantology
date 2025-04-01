using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    [SerializeField] private GameObject characterModel;
    public GameObject CharacterModel => characterModel;
}
