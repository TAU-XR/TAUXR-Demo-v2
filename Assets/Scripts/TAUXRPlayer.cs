using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum HandType { Left, Right, None, Any }
public class TAUXRPlayer : TAUXRSingleton<TAUXRPlayer>
{
    [SerializeField] private Transform ovrRig;
    [SerializeField] private Transform playerHead;
    [SerializeField] private Transform rightHandAnchor;
    [SerializeField] private Transform leftHandAnchor;

    public bool IsEyeTrackingEnabled;
    public bool IsFaceTrackingEnabled;

    [Header("Eye Tracking")]
    [SerializeField] private Transform rightEye;
    [SerializeField] private Transform leftEye;
    private float EYERAYMAXLENGTH = 100000;
    private float EYETRACKINGCONFIDENCETHRESHOLD = .5f;
    private Vector3 NOTTRACKINGVECTORVALUE = new Vector3(-1f, -1f, -1f);

    [Header("Face Tracking")]
    [SerializeField] private OVRFaceExpressions ovrFace;

    private Transform focusedObject;
    private Vector3 eyeGazeHitPosition;

    private OVREyeGaze ovrEye;
    private OVRHand ovrHandR, ovrHandL;
    private OVRSkeleton skeletonR, skeletonL;


    public Transform PlayerHead => playerHead;
    public Transform RightHand => rightHandAnchor;
    public Transform LeftHand => leftHandAnchor;

    public Transform RightEye => rightEye;
    public Transform LeftEye => leftEye;
    public Transform FocusedObject => focusedObject;
    public Vector3 EyeGazeHitPosition => eyeGazeHitPosition;

    public OVRFaceExpressions OVRFace => ovrFace;

    protected override void DoInAwake()
    {
        if (rightEye.TryGetComponent(out OVREyeGaze e))
        {
            ovrEye = e;
        }
        focusedObject = null;
        eyeGazeHitPosition = NOTTRACKINGVECTORVALUE;
    }

  
    void Update()
    {
        if (IsEyeTrackingEnabled)
        {
            CalculateEyeParameters();
        }
    }


    private void CalculateEyeParameters()
    {
        if (ovrEye == null) return;

        if (ovrEye.Confidence < EYETRACKINGCONFIDENCETHRESHOLD)
        {
            Debug.LogWarning("EyeTracking confidence value is low. Eyes are not tracked");
            focusedObject = null;
            eyeGazeHitPosition = NOTTRACKINGVECTORVALUE;

            return;
        }

        Vector3 rightHitPosition;
        Vector3 leftHitPosition;

        RaycastHit hit;
        if (Physics.Raycast(rightEye.position, rightEye.forward, out hit, EYERAYMAXLENGTH))
        {
            focusedObject = hit.transform;
            //eyeGazeHitPosition = hit.point;
            rightHitPosition = hit.point;
        }
        else
        {
            focusedObject = null;
            //eyeGazeHitPosition = NOTTRACKINGVECTORVALUE;
            rightHitPosition = NOTTRACKINGVECTORVALUE;
        }

        RaycastHit leftEyeHit;
        if (Physics.Raycast(leftEye.position, leftEye.forward, out leftEyeHit, EYERAYMAXLENGTH))
        {
            focusedObject = leftEyeHit.transform;
            //eyeGazeHitPosition = hit.point;
            leftHitPosition = leftEyeHit.point;
        }
        else
        {
            focusedObject = null;
            //eyeGazeHitPosition = NOTTRACKINGVECTORVALUE;
            leftHitPosition = NOTTRACKINGVECTORVALUE;
        }

        if (rightHitPosition != NOTTRACKINGVECTORVALUE && leftHitPosition != NOTTRACKINGVECTORVALUE)
        {
            //Vector3 dif = rightHitPosition - leftHitPosition;
            //eyeGazeHitPosition = leftHitPosition + dif / 2;
            eyeGazeHitPosition = (leftHitPosition + rightHitPosition) / 2f;
        }
        else
        {
            if (rightHitPosition != NOTTRACKINGVECTORVALUE)
                eyeGazeHitPosition = rightHitPosition;
            else
            {
                eyeGazeHitPosition = leftHitPosition;
            }
        }

    }


    // covers player's view with color. 
    async public UniTask SetOverlayColor(Color color, float duration)
    {
        // IMPLEMENT
        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;


            await UniTask.Yield();
        }
    }

    public bool IsHoldingTrigger(HandType handType)
    {
        // implement: if holding trigger / pinching  this frame
        return false;
    }

    async public UniTask WaitForTriggerHold(float requiredDuration)
    {
        float holdingDurtaion = 0;
        while (holdingDurtaion < requiredDuration)
        {
            if (IsHoldingTrigger(HandType.Any))
                holdingDurtaion += Time.deltaTime;
            else
                holdingDurtaion = 0;

            await UniTask.Yield();
        }
    }
}
