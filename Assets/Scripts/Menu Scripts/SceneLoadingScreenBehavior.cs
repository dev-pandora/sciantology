using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingScreenBehavior : MonoBehaviour
{

    [SerializeField]
    private Animator linearInAnimation;

    bool isDone = false;

    [SerializeField] 
    private Animator linearOutAnimation;

    [SerializeField]
    private GameManager m_GameManager;

    [SerializeField]
    private GameObject controlscheme;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_GameManager.OnGameEndEvent.AddListener(EndGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDone)
        {
           
            StartCoroutine(endLevelLoadingScreen());
            isDone = true;
        }
    }

    IEnumerator endLevelLoadingScreen()
    {
        yield return new WaitForSeconds(3f);
        controlscheme.SetActive(false);
        linearInAnimation.SetBool("IsLoaded", true);
        yield return new WaitForSeconds(3f);
    }

    public void EndGame()
    {
        StartCoroutine(transitionToEnd());
    }

    IEnumerator transitionToEnd()
    {
        linearOutAnimation.SetBool("IsPaused", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(2);
    }

}
