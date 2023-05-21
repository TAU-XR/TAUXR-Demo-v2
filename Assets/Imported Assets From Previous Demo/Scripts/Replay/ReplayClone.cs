using System;
using System.Collections;
using UnityEngine;


public class ReplayClone : MonoBehaviour
{
    private MeshRenderer mr;

    private IEnumerator alphaCoroutine;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    // called from ReplayPlayer upon instantiation.
    public void Init(float cloneAlpha, float appearTime, Material cloneMaterial)
    {
        mr.material = cloneMaterial;
        SetAlpha(0, 0);
        SetAlpha(cloneAlpha, appearTime);
    }

    public void Init(float cloneAlpha, float appearTime, float appearenceDelayTime, Material cloneMaterial)
    {
        mr.material = cloneMaterial;
        SetAlpha(0, 0);
        StartCoroutine(delayedAppearence(cloneAlpha, appearTime, appearenceDelayTime));
    }

    IEnumerator delayedAppearence(float cloneAlpha, float appearTime, float appearenceDelayTime)
    {
        yield return new WaitForSecondsRealtime(appearenceDelayTime);
        SetAlpha(cloneAlpha, appearTime);
    }

    public void Destroy(float timeToDestroy)
    {
        StopAllCoroutines();
        StartCoroutine(destroyClone(timeToDestroy));
    }

    IEnumerator destroyClone(float timeToDestroy)
    {
        yield return setAlpha(0, timeToDestroy);
        Destroy(this.gameObject);
    }

    public void SetAlpha(float alpha, float duration)
    {
        if (duration == 0)
        {
            Color color = mr.material.color;
            color.a = alpha;
            mr.material.color = color;
        }
        else
            StartCoroutine(setAlpha(alpha, duration));
    }

    IEnumerator setAlpha(float targetAlpha, float duration)
    {
        Color curColor = mr.material.color;
        Color targetColor = curColor;
        targetColor.a = targetAlpha;

        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = lerpTime / duration;

            mr.material.color = Color.Lerp(curColor, targetColor, t);

            yield return null;
        }
    }
}