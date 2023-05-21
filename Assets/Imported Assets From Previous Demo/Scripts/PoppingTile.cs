using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoppingTile : MonoBehaviour
{
    Rigidbody rb;
    MeshController mc;

    [SerializeField] float disappearDuration = 1f;
    [SerializeField] AnimationCurve disappearCurve;

    float popPower = 10;
    float popTorque = 5;

    Vector3 oPos;
    Quaternion oRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mc = GetComponent<MeshController>();
        oPos = transform.position;
        oRot = transform.rotation;

        /*rb.isKinematic = false;
        rb.useGravity = true;*/
    }

    public void Pop()
    {
        mc.SetAlpha(0, disappearDuration, disappearCurve);

        /*
        // not looking good enough. Just disappearing for now.

        Vector3 forceDirection = Vector3.Lerp(transform.up, transform.forward, .3f);
        Vector3 torque = Vector3.Lerp(transform.up, transform.forward, .3f);
        rb.AddForce(forceDirection * popPower, ForceMode.VelocityChange);
        rb.AddTorque(torque,ForceMode.VelocityChange);*/
    }

    public void Reset()
    {
        rb.isKinematic = true;
        transform.position = oPos;
        transform.rotation = oRot;
        rb.isKinematic = false;
    }
}
