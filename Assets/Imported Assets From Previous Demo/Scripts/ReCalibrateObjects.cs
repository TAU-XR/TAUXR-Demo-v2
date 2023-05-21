using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReCalibrateObjects : MonoBehaviour
{
    [SerializeField] bool bTest;
    MeshController[] meshControllers;

    void Start()
    {
        meshControllers = FindObjectsOfType<MeshController>();
        //Calibration.BasestationCalibrator.OnCalibrateOccured += RecalibrateMeshControllers;
    }

    public void RecalibrateMeshControllers()
    {
        foreach (var m in meshControllers) m.updateOriginTransform();
        Debug.Log("Meshes re-calibarted");
    }
}
