using System.Collections;
using UnityEditor;
using UnityEngine;

public class SceneLoadingScreenBehavior : MonoBehaviour
{

    [SerializeField]
    private Animator linearInAnimation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ShaderUtil.anythingCompiling)
        {
            StartCoroutine(endLevelLoadingScreen());
        }
    }

    IEnumerator endLevelLoadingScreen()
    {
        yield return new WaitForSeconds(1.5f);
        linearInAnimation.SetBool("IsLoaded", true);
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

}
