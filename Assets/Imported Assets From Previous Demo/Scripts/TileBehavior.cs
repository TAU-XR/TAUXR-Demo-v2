using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehavior : MonoBehaviour
{
    Rigidbody rb;
    BoxCollider boxCollider;

    public bool bPop;
    public bool bReset;

    [SerializeField] float pushPower = 100;
    [SerializeField] Vector3 pushDirection = new Vector3(0, 1, 0);
    [SerializeField] Vector3 pushTorque = new Vector3(0, 0, 1);
    [SerializeField] float pushTorquePower = 50;

    Vector3 oPos;
    Quaternion oRot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        rb.isKinematic = true;

        oPos = transform.position;
        oRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (bPop)
        {
            bPop = false;
            Pop();
        }

        if (bReset)
        {
            bReset = false;
            Reset();
        }
    }

    IEnumerator popWithDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Pop();
    }

    public void Pop()
    {
        rb.isKinematic = false;
        boxCollider.enabled = false;
        rb.AddForce(pushDirection * pushPower);
        rb.AddTorque(pushTorque * pushTorquePower);
    }

    public void Pop(float delay)
    {
        StartCoroutine(popWithDelay(delay));
    }

    public void Reset()
    {
        boxCollider.enabled = true;
        rb.isKinematic = true;
        transform.position = oPos;
        transform.rotation = oRot;
    }
}
