using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script synchonizes the "Mirror" effect of the Mirror experience avatar with the player's camera after it's been rotated by the calibration,
/// in order to assure the mirror effect is retained at the right angle
/// </summary>
public class SynchonizeMirrorExperienceWithCamera : MonoBehaviour
{
    public Transform OVRCameraRig, MirroredObjects, Duplicate, MirrorPlane;
    public float curtainSpeed = 1f;

    private Transform mainCameraRigTransform;
    private Vector3 mainCameraRigOffsetRotation;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        mainCameraRigTransform = Camera.main.gameObject.GetComponentInParent<OVRCameraRig>().transform;
        mainCameraRigOffsetRotation = mainCameraRigTransform.parent.eulerAngles;
        this.transform.Rotate(mainCameraRigOffsetRotation);
        OVRCameraRig.transform.SetLocalPositionAndRotation(mainCameraRigTransform.localPosition, mainCameraRigTransform.localRotation);
        MirroredObjects.Rotate(-mainCameraRigOffsetRotation);
        Duplicate.Rotate(mainCameraRigOffsetRotation);
        Duplicate.localPosition = new Vector3(((MirrorPlane.position.x - mainCameraRigTransform.position.x) * 2), 0, 0);
        StartCoroutine(RaiseScreen());
    }

    private IEnumerator RaiseScreen()
    {
        yield return new WaitForSeconds(3);
        float timer = 0;
        while (timer < 5)
        {
            timer += Time.deltaTime;
            MirrorPlane.position += Vector3.up * Time.deltaTime * curtainSpeed;
            yield return new WaitForEndOfFrame();
        }
        MirrorPlane.gameObject.SetActive(false);
    }
}
