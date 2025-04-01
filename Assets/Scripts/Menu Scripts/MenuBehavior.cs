using System.Collections;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField]
    private TextMeshProUGUI loadingText;
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
        StartCoroutine(levelLoadCoroutine());
    }

    IEnumerator levelLoadCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        spinner.SetActive(true);
        AsyncOperation asyncLevelLoad = SceneManager.LoadSceneAsync(1);
        asyncLevelLoad.allowSceneActivation = false;
        while ((asyncLevelLoad.progress < 0.9f))
        {
            Debug.Log(asyncLevelLoad.progress);
            loadingText.text = (asyncLevelLoad.progress * 100) + "%";
           yield return null;
        }
        yield return new WaitForSeconds(3);
        asyncLevelLoad.allowSceneActivation = true;
    }

}
