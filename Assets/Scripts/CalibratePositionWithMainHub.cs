using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibratePositionWithMainHub : MonoBehaviour
{
    void Awake()
    {
        //This must remain in awake for several mesh controllers to update their origin transforms
        transform.SetPositionAndRotation(SceneCalibrationManager.Instance.ScenePivot.position, SceneCalibrationManager.Instance.ScenePivot.rotation);
    }
}
