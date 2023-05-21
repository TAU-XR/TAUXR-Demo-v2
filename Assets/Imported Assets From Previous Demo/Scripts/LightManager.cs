using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    public bool bTurnOnLights;

    [SerializeField] LightController[] introLights;
    [SerializeField] LightController[] labLights;

    [SerializeField] float turnOnLabLightDuration;
    [SerializeField] float turnOffIntroLightDuration;

    [SerializeField] AnimationCurve curve;


    // Start is called before the first frame update
    void Start()
    {
        //SetStartLights();
        TurnOnLabLights(0);
    }

    // Update is called once per frame
    void Update() //TODO remove if obsolete
    {
        if (bTurnOnLights)
        {
            bTurnOnLights = false;
            TurnOnLabLights(5f);
        }
    }

    public void SetStartLights()
    {
        // turn on Intro Lights
        foreach (LightController light in introLights)
            light.SetIntensity(1, .1f, curve);

        // turn off lab lights
        foreach (LightController light in labLights)
            light.SetIntensity(0, .1f, curve);
    }

    public void TurnOnLabLights(float duration)
    {
        foreach (LightController light in introLights)
            light.SetIntensity(0, duration, curve);

        foreach (LightController light in labLights)
            light.SetIntensity(1, duration, curve);

    }


}
