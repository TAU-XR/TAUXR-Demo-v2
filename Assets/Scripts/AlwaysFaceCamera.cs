using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    private Transform mainCam;
    public bool faceWithRedAxis = false;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!faceWithRedAxis)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position) * Quaternion.Euler(0, 90, 0);
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, eulerRotation.y, 0);
        }
    }
}
