using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    MeshRenderer mr;
    bool doesHaveMR;

    Vector3 oPos;
    Quaternion oRot;
    Vector3 oScale;

    IEnumerator colorCoroutine;
    IEnumerator scaleCoroutine;
    IEnumerator positionCoroutine;

    private void Awake()
    {
        if (TryGetComponent<MeshRenderer>(out MeshRenderer meshR))
        {
            doesHaveMR = true;
            mr = meshR;
        }
        else
            doesHaveMR = false;

        oPos = transform.position;
        oRot = transform.rotation;
        oScale = transform.localScale;
    }

    // called from calibration manager after scene is calibrated.
    public void updateOriginTransform()
    {
        oPos = transform.position;
        oRot = transform.rotation;
        oScale = transform.localScale;
    }

    #region Appearance
    public void SetAlpha(float targetAlpha, float duration, AnimationCurve curve)
    {
        if (doesHaveMR)
        {
            Color targetColor = mr.material.color;
            targetColor.a = targetAlpha;
            SetColor(targetColor, duration, curve);
        }
        else
            Debug.Log("Object " + transform.name + " tried to set its alpha but have no Mesh Renderer");
    }

    public void SetColor(Color targetColor, float duration, AnimationCurve curve)
    {
        if (doesHaveMR)
        {
            if (colorCoroutine != null)
                StopCoroutine(colorCoroutine);
            colorCoroutine = changeColor(targetColor, duration, curve);

            StartCoroutine(colorCoroutine);
        }
        else
            Debug.Log("Object " + transform.name + " tried to set its color but have no Mesh Renderer");
    }

    IEnumerator changeColor(Color targetColor, float duration, AnimationCurve curve)
    {
        Color curColor = mr.material.color;

        float lerpTime = 0;
        if (duration == 0)
        {
            mr.material.color = targetColor;
            yield break;
        }

        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = curve.Evaluate(lerpTime / duration);

            mr.material.color = Color.Lerp(curColor, targetColor, t);
            yield return null;
        }
    }

    #endregion

    #region Scale
    public void SetScale(Vector3 targetScale) => transform.localScale = targetScale;
    public void SetClampedScale(Vector3 targetScale)
    {
        float targetX = targetScale.x * oScale.x;
        float targetY = targetScale.y * oScale.y;
        float targetZ = targetScale.z * oScale.z;

        transform.localScale = new Vector3(targetX, targetY, targetZ);
    }

    public void SetClampedScale(Vector3 targetScale, float duration, AnimationCurve curve)
    {
        float targetX = targetScale.x * oScale.x;
        float targetY = targetScale.y * oScale.y;
        float targetZ = targetScale.z * oScale.z;

        SetScale(new Vector3(targetX, targetY, targetZ), duration, curve);
    }

    public void BackToOriginalScale() => transform.localScale = oScale;
    public void BackToOriginalScale(float duration, AnimationCurve curve) => SetScale(oScale, duration, curve);

    public void SetScale(Vector3 targetScale, float duration, AnimationCurve curve)
    {
        if (scaleCoroutine != null)
            StopCoroutine(scaleCoroutine);
        scaleCoroutine = changeScale(targetScale, duration, curve);

        StartCoroutine(scaleCoroutine);
    }


    IEnumerator changeScale(Vector3 targetScale, float duration, AnimationCurve curve)
    {
        if (duration == 0)
        {
            transform.localScale = targetScale;
            yield break;
        }

        Vector3 curScale = transform.localScale;
        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = curve.Evaluate(lerpTime / duration);

            transform.localScale = Vector3.Lerp(curScale, targetScale, t);
            yield return null;
        }
    }
    #endregion

    #region Position
    public void SetPosition(Vector3 targetPos) => transform.position = targetPos;

    public void Move(Vector3 movementVec) => transform.localPosition += movementVec;
    public void Move(Vector3 movementVec, float duration, AnimationCurve curve) => StartCoroutine(changeLocalPosition(movementVec, duration, curve));

    public void BackToOriginalPos() => transform.position = oPos;
    public void BackToOriginalPos(float duration, AnimationCurve curve) => SetPosition(oPos, duration, curve);

    public void SetPosition(Vector3 targetPos, float duration, AnimationCurve curve)
    {
        if (positionCoroutine != null)
            StopCoroutine(positionCoroutine);
        positionCoroutine = changePosition(targetPos, duration, curve);

        StartCoroutine(positionCoroutine);
    }

    IEnumerator changePosition(Vector3 targetPos, float duration, AnimationCurve curve)
    {
        if (duration == 0)
        {
            transform.position = targetPos;
            yield break;
        }

        Vector3 curPos = transform.position;
        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = curve.Evaluate(lerpTime / duration);

            transform.position = Vector3.Lerp(curPos, targetPos, t);
            yield return null;
        }
    }

    IEnumerator changeLocalPosition(Vector3 movementVec, float duration, AnimationCurve curve)
    {
        if (duration == 0)
        {
            transform.localPosition = movementVec;
            yield break;
        }

        Vector3 curPos = transform.localPosition;
        Vector3 targetPos = curPos + movementVec;
        float lerpTime = 0;
        while (lerpTime < duration)
        {
            lerpTime += Time.deltaTime;
            float t = curve.Evaluate(lerpTime / duration);

            transform.localPosition = Vector3.Lerp(curPos, targetPos, t);
            yield return null;
        }
    }
    #endregion
}
