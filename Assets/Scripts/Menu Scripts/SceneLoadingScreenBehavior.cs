using System.Collections;
using UnityEditor;
using UnityEngine;

public class SceneLoadingScreenBehavior : MonoBehaviour
{

    [SerializeField]
    private Animator linearInAnimation;

    bool isDone = false;

    [SerializeField] 
    private Animator linearOutAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDone)
        {
           if (!ShaderUtil.anythingCompiling)
            {
                linearInAnimation.SetBool("IsLoaded", true);
                isDone = true;
            }
            
        }
    }

    IEnumerator endLevelLoadingScreen()
    {
        yield return new WaitForSeconds(1.5f);
        linearInAnimation.SetBool("IsLoaded", true);
        yield return new WaitForSeconds(3f);
    }

}
