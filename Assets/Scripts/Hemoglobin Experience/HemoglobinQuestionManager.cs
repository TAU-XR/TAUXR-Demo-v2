using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using Assets.Scripts;

public class HemoglobinQuestionManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text MarkAreaText, TryAgainText, CorrectAnswerText;
    [SerializeField]
    private GameObject HemoglobinModel;
    [SerializeField]
    private List<Collider> AnswerColliders;

    private bool questionFinishedInitializing = false, questionAnswered = false, pressCooldown = false;
    private float holdTimer = 0;
    private bool wasWrongOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        MarkAreaText.gameObject.SetActive(false);
        TryAgainText.gameObject.SetActive(false);
        CorrectAnswerText.gameObject.SetActive(false);
        HemoglobinModel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (questionFinishedInitializing && !questionAnswered)
        {
            if (Input.GetButton("XRI_Right_TriggerButton") && !pressCooldown)
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= HemoglobinExperienceManager.Instance.PressHoldDuration)
                {
                    pressCooldown = true;
                    MeshRenderer answerBallMeshRenderer = Instantiate(HemoglobinExperienceManager.Instance.answerSpherePrefab, ExperienceManager.Instance.RightHandControllerInteractionSphereCollider.transform.position, Quaternion.identity, transform).GetComponent<MeshRenderer>();
                    if (ConfirmAnswer(ExperienceManager.Instance.RightHandControllerInteractionSphereCollider))
                    {
                        answerBallMeshRenderer.material = HemoglobinExperienceManager.Instance.GreenMat;
                        MarkAreaText.gameObject.SetActive(false);
                        TryAgainText.gameObject.SetActive(false);
                        CorrectAnswerText.gameObject.SetActive(true);
                        questionAnswered = true;
                        HemoglobinExperienceManager.Instance.CorrectAnswerSound.Play();
                        HemoglobinExperienceManager.Instance.NextQuestion();
                    }
                    else
                    {
                        answerBallMeshRenderer.material = HemoglobinExperienceManager.Instance.RedMat;
                        HemoglobinExperienceManager.Instance.WrongAnswerSound.Play();
                        MarkAreaText.gameObject.SetActive(false);
                        if (!wasWrongOnce)
                        {
                            TryAgainText.gameObject.SetActive(true);
                            wasWrongOnce = true;
                        }
                        else
                        {
                            questionAnswered = true;
                            TryAgainText.gameObject.SetActive(false);
                            _ = RevealCorrectLocationsAndMoveOn();
                        }
                    }
                }
            }
            else
            {
                holdTimer = 0;
            }
            if (!Input.GetButton("XRI_Right_TriggerButton"))
            {
                pressCooldown = false;
            }
        }
    }

    public async void InitializeQuestion()
    {
        await UniTask.Delay(HemoglobinExperienceManager.Instance.ModelFadeInDelay * 1000);
        FadeInModel();
        await UniTask.Delay(HemoglobinExperienceManager.Instance.TextAppearDelay * 1000);
        MarkAreaText.gameObject.SetActive(true);
        await UniTask.Delay(HemoglobinExperienceManager.Instance.ControllerSpheresAppearDelay * 1000);
        ExperienceManager.Instance.RightHandControllerInteractionSphereCollider.gameObject.SetActive(true);
        //ExperienceManager.Instance.LeftHandControllerInteractionSphereCollider.gameObject.SetActive(true);
        questionFinishedInitializing = true;
    }

    private bool ConfirmAnswer(Collider triggeredCollider)
    {
        foreach (Collider collider in AnswerColliders)
        {
            if (collider.bounds.Intersects(triggeredCollider.bounds))
            {
                return true;
            }
        }
        return false;
    }

    public void FadeInModel()
    {
        //temporary
        HemoglobinModel.gameObject.SetActive(true);
    }

    public void HighlightAllCorrectLocations()
    {
        foreach (Collider collider in AnswerColliders)
        {
            MeshRenderer meshRenderer = collider.GetComponent<MeshRenderer>();
            meshRenderer.enabled = true;
            meshRenderer.material = HemoglobinExperienceManager.Instance.GreenMat;
        }
    }

    public async UniTask RevealCorrectLocationsAndMoveOn()
    {
        await UniTask.Delay(HemoglobinExperienceManager.Instance.DelayBetweenSecondMistakeAndReveal * 1000);
        HighlightAllCorrectLocations();
        HemoglobinExperienceManager.Instance.NextQuestion();
    }
}
