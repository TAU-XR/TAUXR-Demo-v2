using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfGalleryDetector : MonoBehaviour
{
    private Collider triggerCollider;
    [SerializeField] private DemoModeChangeButton returnToHubButton;
    [SerializeField] private MeshRenderer[] paintables;
    BoxCollider galleryCollider;

    void Start()
    {
        triggerCollider = Camera.main.GetComponent<SphereCollider>();
        returnToHubButton.DisableButton();
        galleryCollider = GetComponent<BoxCollider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != triggerCollider) return;
        galleryCollider.enabled = false;
        returnToHubButton.EnableButton();
        foreach (MeshRenderer mr in paintables)
        {
            mr.enabled = true;
        }
    }
}
