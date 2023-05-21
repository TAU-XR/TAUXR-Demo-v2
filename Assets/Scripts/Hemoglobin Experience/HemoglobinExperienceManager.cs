using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts;
using Cysharp.Threading.Tasks;

public class HemoglobinExperienceManager : SingletonMonoBehaviour<HemoglobinExperienceManager>
{
    [Header("Questions")]
    public List<HemoglobinQuestionManager> Questions;
    
    [Range(0, 10)]
    public int ModelFadeInDelay = 2, TextAppearDelay = 2, ControllerSpheresAppearDelay = 2, DelayBetweenSecondMistakeAndReveal = 3, DelayBetweenQuestions = 5;
    [Range(0, 10)]
    public int PressHoldDuration = 2;

    public GameObject answerSpherePrefab;
    public Material RedMat, GreenMat;
    public AudioSource CorrectAnswerSound, WrongAnswerSound;

    [SerializeField]
    private DemoModeChangeButton ReturnToHubButton;

    private HemoglobinQuestionManager currentQuestion;
    private int currentQuestionIndex;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Questions.Count; i++)
        {
            Questions[i].gameObject.SetActive(false);
        }
        currentQuestionIndex = 0;
        currentQuestion = Questions[0];
        currentQuestion.gameObject.SetActive(true);
        currentQuestion.InitializeQuestion();
    }

    public async void NextQuestion()
    {
        ExperienceManager.Instance.RightHandControllerInteractionSphereCollider.gameObject.SetActive(false);
        //ExperienceManager.Instance.LeftHandControllerInteractionSphereCollider.gameObject.SetActive(false);
        if (currentQuestionIndex + 1 < Questions.Count)
        {
            await UniTask.Delay(DelayBetweenQuestions * 1000);
            currentQuestion.gameObject.SetActive(false);
            currentQuestionIndex++;
            currentQuestion = Questions[currentQuestionIndex];
            currentQuestion.gameObject.SetActive(true);
            currentQuestion.InitializeQuestion();
        }
        else
        {
            ReturnToHubButton.EnableButton();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
