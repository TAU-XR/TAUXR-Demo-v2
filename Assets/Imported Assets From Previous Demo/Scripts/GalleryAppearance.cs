using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryAppearance : MonoBehaviour
{
    public bool bAppear;
    public bool bFold;

    [SerializeField] GazeTrackingController gazeTrackingController;
    [SerializeField] MeshController gallery;
    [SerializeField] Vector3 moveToFoldPosition;

    [SerializeField] float apperanceDuration;
    [SerializeField] AnimationCurve appearanceCurve;

    [SerializeField] float foldDuration;
    [SerializeField] AnimationCurve foldCurve;

    [SerializeField] GameObject galleryLight;
    // Start is called before the first frame update
    void Start()
    {
        //Calibration.BasestationCalibrator.OnCalibrateOccured += HideGallery;
    }

    public void HideGallery()
    {
        // update origin transform after scene was moved in calibration.
        foreach (MeshController mc in GetComponentsInChildren<MeshController>()) mc.updateOriginTransform();

        gallery.Move(moveToFoldPosition);
        //galleryLight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (bAppear)
        {
            bAppear = false;
            SummonGallery();
        }

        if (bFold)
        {
            bFold = false;
            FoldGallery();
        }
    }

    public void SummonGallery()
    {
        gallery.BackToOriginalPos(apperanceDuration, appearanceCurve);
        //galleryLight.SetActive(true);
    }
    public void FoldGallery()
    {
        gallery.Move(moveToFoldPosition, foldDuration, foldCurve);
        gazeTrackingController.MakeIndicatorsDisappear();
        //galleryLight.SetActive(false);
    }
}
