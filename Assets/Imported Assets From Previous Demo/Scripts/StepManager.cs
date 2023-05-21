using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class StepManager : MonoBehaviour
{
    [SerializeField] float turnOnLabLightsDuration = 5;
    [SerializeField] float buttonsAppearDelay = 10;
    [SerializeField] LightManager lightsManager;
    [SerializeField] DemoBehaviour firstWindow;

    public void TransitionTutorialToWindows()
    {
        StartCoroutine(tutorialToWindows());
    }

    public void TransitionTutorialToMaze()
    {
        StartCoroutine(tutorialToMaze());
    }

    IEnumerator tutorialToWindows()
    {
        // turn on lights
        lightsManager.TurnOnLabLights(turnOnLabLightsDuration);

        // wait until lights are on and then extra time before summoning buttons
        yield return new WaitForSecondsRealtime(buttonsAppearDelay + turnOnLabLightsDuration);

        firstWindow.EnableButtons();
    }

    IEnumerator tutorialToMaze()
    {
        yield return new WaitForSecondsRealtime(buttonsAppearDelay);
    }
}
