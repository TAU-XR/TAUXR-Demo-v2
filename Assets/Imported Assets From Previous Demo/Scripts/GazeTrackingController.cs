using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class GazeTrackingController : MonoBehaviour
{
    [SerializeField]
    private List<NaiveEyeTracker> trackers;
    [SerializeField]
    private Collider trigger;
    BoxCollider galleryCollider;
    [SerializeField]
    private Assets.Scripts.DemoModeChangeButton returnToHubButton;

    private bool hasExitedOnce = false, hasEnteredOnce = false;

    [SerializeField]
    MeshRenderer[] paintables;

    /*void Update() // TODO delete Update after setup
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            foreach (var tracker in trackers)
            {
                tracker.SetRecordingMode(false);
                tracker.PaintGazes();
            }
        }
    }*/

    private void Start()
    {
        galleryCollider = GetComponent<BoxCollider>();
        if (trigger == null)
        {
            trigger = Camera.main.GetComponent<SphereCollider>();
            if (trigger == null)
            {
                Debug.LogError("You must add a sphere collider to your main camera for the GazeTrackingController to work!");
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider != trigger || hasEnteredOnce) return;
        Debug.Log(name + "trigger enter");
        /*foreach (var tracker in trackers)
        {
            tracker.SetRecordingMode(true);
            hasEnteredOnce = true;
            
        }*/
        returnToHubButton.DisableButton();

        foreach (MeshRenderer mr in paintables)
        {
            mr.enabled = false;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider != trigger || hasExitedOnce) return;
        Debug.Log(name + "trigger exit");
        /*foreach (var tracker in trackers)
        {
            tracker.SetRecordingMode(false);
            tracker.PaintGazes(); //CAUSES LAG SPIKE
        }*/
        galleryCollider.enabled = false;
        foreach (MeshRenderer mr in paintables)
        {
            mr.enabled = true;
        }
        returnToHubButton.EnableButton();
        hasExitedOnce = true;
    }

    public void MakeIndicatorsDisappear()
    {
        foreach (var tracker in trackers)
        {
            tracker.RemoveIndicators();
        }
    }
}
