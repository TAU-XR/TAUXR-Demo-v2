using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEyeSphere : MonoBehaviour
{
    public float lerpSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TAUXRPlayer.Instance.EyeGazeHitPosition, lerpSpeed * Time.deltaTime);
    }
}
