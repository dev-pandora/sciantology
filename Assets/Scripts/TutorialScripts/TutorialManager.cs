using TMPro;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    [SerializeField]
    private GameObject TutorialFrame;
    [SerializeField]
    private TextMeshProUGUI TutorialText;
    [SerializeField]
    private PlayerController PlayerController;

    [SerializeField]
    private BoxCollider firstVictimTrigger;
    [SerializeField]
    private BoxCollider fireAntTrigger;

    bool tutorialIsShowing = false;

    private int currentTutorialIndex = 0;

    float lastInputTime = 0;
    float skipDialogueDebounceTime = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerController.OnInteractEvent.AddListener(OnInteract);
    }

    public void OnInteract()
    {
        if (Time.time - lastInputTime > skipDialogueDebounceTime)
        {
            lastInputTime = Time.time;
            if (tutorialIsShowing)
            {
                HideTutorial();
            }
        }
    }

    public void ShowTutorial()
    {
        tutorialIsShowing = true;
        TutorialFrame.SetActive(true);
        switch (currentTutorialIndex)
        {
            case 0:
                TutorialText.text = "Rise, Fun Gus, you are an ant serving me, the great mushroom god Mycenu. I, the great lord of mold, order you take over all the citizens of Ants-werp. To do this, you need to move. Use the joystick to move your body and A to interact.";
                break;
            case 1:
                TutorialText.text = "Ah, the first of many to begin serving me today. To begin the ritual of S'poré, walk into your target and have my divine powers do the work.";
                break;   
            case 2:
                TutorialText.text = "Excellent, Fun Gus. You have successfully converted your first citizen. However, therre is more at stake. The fire-ant nation has began exerting its influence on Ants-werp as well. You will have to fight them for influence over our new disciples.";
                break;
            case 3:
                TutorialText.text = "Look, Gus, fire ants. They are guarding the gate to Ants-werp. Collide into them to start the battle over influence. Mash the A key to use my power and free the citizens from the fire-ant grasp.";
                break;
            case 4:
                TutorialText.text = "Now that we've defeated the fire-ants, they have joined our cause. Now go forth and lead Ants-werp to salvation.";
                break;

        }
    }

    public void HideTutorial()
    {
        TutorialFrame.SetActive(false);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other == firstVictimTrigger)
        {
            currentTutorialIndex = 1;
            ShowTutorial();
        }
        else if (other == fireAntTrigger)
        {
            currentTutorialIndex = 3;
            ShowTutorial();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
