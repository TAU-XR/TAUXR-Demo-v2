using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.Events;

public class SceneCalibrationManager : SingletonMonoBehaviour<SceneCalibrationManager>
{
    public Transform rightHandCalibrationPoint, rightHandInteractionPoint;
    public Transform OVRCameraRigOffset, OVRCameraRig;
    public Transform ScenePivot, InstantiatedObjects;
    public GameObject CalibrationConfirmationObjects, CalibratePositionTitle, CalibrateRotationTitle;
    public GameObject CalibrationSphereRed, CalibrationSphereBlue;
    public LineRenderer DashedLinePrefab;
    public float CalibrationHoldTime = 1f;
    public float ManualPositionAdjustmentSpeedMultiplier = 0.1f;

    private Vector3 lockedPositionCalibrationPoint, lockedRotationCalibrationPoint;
    private Quaternion calibratedRoomRotation;
    private bool positionCalibrated = false, rotationCalibrated = false, calibrationEnabled = false;
    private float holdTimer = 0f;
    //Manual Modification Variables
    private Vector3 controllerPositionOnTriggerPress, currentControllerPosition, endPosition, OVRCameraRigOffsetPositionBeforeAdjustment;
    private bool currentlyAdjusting = false, currentlyMoving = false;
    private Transform RightControllerBody;
    private LineRenderer currentDashedLine;
    private Material redMaterial, blueMaterial;
    private MeshRenderer rightHandCalibrationSphereMeshRenderer;

    public UnityAction CalibrationSuccessful;

    // Start is called before the first frame update
    void Start()
    {
        currentDashedLine = null;
        RightControllerBody = rightHandCalibrationPoint.parent;
        redMaterial = CalibrationSphereRed.GetComponent<MeshRenderer>().sharedMaterial;
        blueMaterial = CalibrationSphereBlue.GetComponent<MeshRenderer>().sharedMaterial;
        rightHandCalibrationSphereMeshRenderer = rightHandCalibrationPoint.GetComponentInChildren<MeshRenderer>();

        StartCalibration();
    }

    // Update is called once per frame
    void Update()
    {
        //Calibrate Position Phase
        if (!positionCalibrated)
        {
            if (Input.GetButton("XRI_Right_TriggerButton"))
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= CalibrationHoldTime)
                {
                    lockedPositionCalibrationPoint = rightHandCalibrationPoint.position;
                    positionCalibrated = true;
                    CalibratePositionTitle.SetActive(false);
                    CalibrateRotationTitle.SetActive(true);
                    Instantiate(CalibrationSphereRed, lockedPositionCalibrationPoint, Quaternion.identity, InstantiatedObjects);
                    currentDashedLine = Instantiate(DashedLinePrefab, InstantiatedObjects);
                    currentDashedLine.SetPosition(0, lockedPositionCalibrationPoint);
                    rightHandCalibrationSphereMeshRenderer.sharedMaterial = blueMaterial;
                }
            }
            else if (Input.GetButtonUp("XRI_Right_TriggerButton"))
            {
                holdTimer = 0;
            }
        }
        //Calibrate Rotation Phase
        else if (!rotationCalibrated)
        {
            if (currentDashedLine != null)
            {
                currentDashedLine.SetPosition(1, rightHandCalibrationPoint.position);
            }
            if (Input.GetButton("XRI_Right_TriggerButton") && holdTimer < CalibrationHoldTime)
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= CalibrationHoldTime)
                {
                    //calculating the desired rotation relative to the room
                    lockedRotationCalibrationPoint = rightHandCalibrationPoint.position;
                    Instantiate(CalibrationSphereBlue, lockedRotationCalibrationPoint, Quaternion.identity, InstantiatedObjects);
                    Vector3 roomDirection = Vector3.Normalize(lockedPositionCalibrationPoint - lockedRotationCalibrationPoint); //this is only if the right hand position is to the left of the left hand position, otherwise reverse it
                    calibratedRoomRotation = Quaternion.LookRotation(roomDirection, Vector3.up);
                    calibratedRoomRotation.eulerAngles = new Vector3(0, -(calibratedRoomRotation.eulerAngles.y - 90), 0);

                    //positioning and rotation the camera rig
                    OVRCameraRigOffset.rotation = calibratedRoomRotation;
                    OVRCameraRig.localPosition = -lockedPositionCalibrationPoint;

                    rotationCalibrated = true;
                    PassthroughManager.Instance.TogglePassthrough(false);
                    CalibrationConfirmationObjects.SetActive(true);
                    CalibrateRotationTitle.SetActive(false);
                    rightHandCalibrationPoint.gameObject.SetActive(false);
                    rightHandInteractionPoint.gameObject.SetActive(true);
                    Destroy(currentDashedLine.gameObject);
                    currentDashedLine = null;
                    holdTimer = 0;
                }
            }
            else if (Input.GetButtonUp("XRI_Right_TriggerButton"))
            {
                holdTimer = 0;
            }
        }
        //Manual Modification Phase
        else if (positionCalibrated && rotationCalibrated && calibrationEnabled)
        {
            if (Input.GetButtonDown("XRI_Right_TriggerButton") && !currentlyMoving)
            {
                currentlyAdjusting = true;
                currentlyMoving = false;
                controllerPositionOnTriggerPress = rightHandInteractionPoint.position;
                OVRCameraRigOffsetPositionBeforeAdjustment = OVRCameraRigOffset.position;
                currentDashedLine = Instantiate(DashedLinePrefab, InstantiatedObjects);
                currentDashedLine.SetPosition(0, controllerPositionOnTriggerPress);
            }
            else if (Input.GetButtonUp("XRI_Right_TriggerButton") && currentlyAdjusting)
            {
                currentlyAdjusting = false;
                currentlyMoving = true;
                Vector3 controllerEndPosition = rightHandInteractionPoint.position;
                controllerPositionOnTriggerPress.y = controllerEndPosition.y = 0;
                Vector3 direction = controllerPositionOnTriggerPress - controllerEndPosition;
                endPosition = OVRCameraRigOffsetPositionBeforeAdjustment += direction;
                if (null != currentDashedLine)
                {
                    Destroy(currentDashedLine.gameObject);
                    currentDashedLine = null;
                }
            }
        }
        if (currentlyAdjusting)
        {
            currentDashedLine.SetPosition(1, rightHandInteractionPoint.position);
        }
        else if (currentlyMoving)
        {
            float step = ManualPositionAdjustmentSpeedMultiplier * Time.deltaTime;
            OVRCameraRigOffset.position = Vector3.MoveTowards(OVRCameraRigOffset.position, endPosition, step);
            if (OVRCameraRigOffset.position == endPosition)
            {
                currentlyMoving = false;
            }
        }
    }

    public void StartCalibration()
    {
        PassthroughManager.Instance.TogglePassthrough(true);

        calibrationEnabled = true;

        CalibrationConfirmationObjects.SetActive(false);
        CalibratePositionTitle.SetActive(true);
        CalibrateRotationTitle.SetActive(false);
        rightHandCalibrationPoint.gameObject.SetActive(true);
        rightHandCalibrationSphereMeshRenderer.sharedMaterial = redMaterial;
        rightHandInteractionPoint.gameObject.SetActive(false);

    }

    public void RedoCalibration()
    {
        positionCalibrated = false;
        rotationCalibrated = false;
        OVRCameraRigOffset.rotation = new Quaternion(0, 0, 0, 0);
        OVRCameraRig.localPosition = Vector3.zero;
        ClearInstantiatedObjects();
        StartCalibration();
    }

    public void ConfirmCalibration()
    {
        calibrationEnabled = false;
        ClearInstantiatedObjects();
        CalibrationConfirmationObjects.SetActive(false);
        rightHandCalibrationPoint.gameObject.SetActive(false);
        rightHandInteractionPoint.gameObject.SetActive(false);
        CalibrationSuccessful.Invoke();
    }

    private void ClearInstantiatedObjects()
    {
        foreach (Transform transform in InstantiatedObjects)
        {
            Destroy(transform.gameObject);
        }
    }
}
