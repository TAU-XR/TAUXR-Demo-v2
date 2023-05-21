using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public bool bChangeState;
    bool _isOn;
    [SerializeField] float turnOnDuration = 5f;
    [SerializeField] float turnOffDuration = 2f;
    [SerializeField] AnimationCurve turnOnCurve;
    [SerializeField] AnimationCurve turnOffCurve;

    Light _light;
    float _OnIntensity;
    Color _color;

    IEnumerator intensityCo;

    private void Awake()
    {
        _light = GetComponent<Light>();
        _OnIntensity = _light.intensity;
        _color = _light.color;
    }
    // Start is called before the first frame update
    void Start()
    {

        _isOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (bChangeState)
        {
            bChangeState = false;
            SetPower(!_isOn);
        }
    }

    public void SetPower(bool isOn)
    {
        if (intensityCo != null)
            StopCoroutine(intensityCo);

        if (isOn)
            intensityCo = changeIntensity(_OnIntensity, turnOnDuration, turnOnCurve);
        else
            intensityCo = changeIntensity(0, turnOffDuration, turnOffCurve);

        StartCoroutine(intensityCo);
        _isOn = isOn;
    }

    // get intensity as of 0-1 clamped value of this light max intensity.
    public void SetIntensity(float clampedIntensity, float duration, AnimationCurve curve)
    {
        float targetIntensity = clampedIntensity * _OnIntensity;
        if (intensityCo != null)
            StopCoroutine(intensityCo);

        intensityCo = changeIntensity(targetIntensity, duration, curve);
        StartCoroutine(intensityCo);
    }

    IEnumerator changeIntensity(float targetIntensity, float duration, AnimationCurve curve)
    {
        float lerpTime = 0;
        float curIntensity = _light.intensity;

        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;

            _light.intensity = Mathf.Lerp(curIntensity, targetIntensity, curve.Evaluate(t));

            yield return null;
        }

    }
}
