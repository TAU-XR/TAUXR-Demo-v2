using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class InteractionBallScriptForShader : MonoBehaviour
{
    // Start is called before the first frame update

    private Material material;
    private GameObject RightHand;
    private GameObject LeftHand;
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        RightHand = ExperienceManager.Instance.RightHandController;
        LeftHand = ExperienceManager.Instance.LeftHandController;
    }

    // Update is called once per frame
    void Update()
    {
        material.SetVector("RightHand", RightHand.transform.position);
        material.SetVector("LeftHand", LeftHand.transform.position);
    }
}
