using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour
{

    [SerializeField]
    private Animator linearInAnimation;
    [SerializeField]
    private GameObject button;
    [SerializeField]
    private GameObject spinner;
    private bool isPaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSceneLoad()
    {
        Debug.Log("Start Scene Load");
        linearInAnimation.SetBool("IsPaused", true);
        button.SetActive(false);
    }

    IEnumerator startSpinner()
    {
        yield return new WaitForSeconds(1.5f);
        
    }

}
