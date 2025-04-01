using UnityEngine;

public class MonoBehaviourScript : MonoBehaviour
{
    enum GameState
    {
        MainMenu,
        InGame,
        GameOver
    }

    private GameState m_GameState;

    void Start()
    {
        m_GameState = GameState.MainMenu;
    }
}
