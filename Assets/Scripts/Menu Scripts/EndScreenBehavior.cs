using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreenBehavior : MonoBehaviour
{

    [SerializeField]
    private List<ParticleSystem> particleSystems;
    [SerializeField]
    private Animator fadeOutAnimator;
    [SerializeField]
    private Animator fadeInAnimator;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    public static int Score = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreText.text = "You converted " + Score + " subjects!";

        StartCoroutine(endScreenCoroutine());
    }

    IEnumerator endScreenCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        fadeOutAnimator.SetBool("IsLoaded", true);
        yield return new WaitForSeconds(1.5f);

        float amountOfCandlesLit = (int)(particleSystems.Count*(Score / 100f));
        Debug.Log(amountOfCandlesLit);


        for (int i = 0; i < amountOfCandlesLit; i++)
        {
            particleSystems[i].Play();
            yield return new WaitForSeconds(1f);
        }
    }

    public void GoToMenu()
    {
        StartCoroutine(GotToMenuCoroutine());
    }

    IEnumerator GotToMenuCoroutine()
    {
     
        fadeInAnimator.SetBool("IsLoaded", true);
        yield return new WaitForSeconds(3f);
        Score = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Retry()
    {
        StartCoroutine(RetryCoroutine());
    }

    IEnumerator RetryCoroutine()
    {
        fadeInAnimator.SetBool("IsLoaded", true);
        yield return new WaitForSeconds(3f);
        Score = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
