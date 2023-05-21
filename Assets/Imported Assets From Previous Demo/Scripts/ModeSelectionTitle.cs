using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ModeSelectionTitle : MonoBehaviour
{
    Transform player;
    // TextMeshPro titleMesh;
    MeshRenderer titleMesh;
    bool isVisible;

    [SerializeField] float minVisibleDistance = .8f;

    [SerializeField] float appearDuration = 1.5f;
    [SerializeField] AnimationCurve appearCurve;

    [SerializeField] float disappearDuration = .5f;
    [SerializeField] AnimationCurve disappearCurve;

    IEnumerator changeAlphaCoroutine;

    private void Awake()
    {
        // titleMesh = GetComponent<TextMeshPro>();
        titleMesh = GetComponent<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.transform;
        isVisible = true;
    }

    void Update()
    {
        float playerFromTitle = (player.position - transform.position).sqrMagnitude;

        // if too close- disappear
        if (playerFromTitle <= minVisibleDistance)
        {
            if (isVisible)
            {
                titleMesh.enabled = false;
                isVisible = false;
                //Debug.Log("Set False");
            }
        }
        else
        {
            // if player is far enough- appear
            if (!isVisible)
            {
                titleMesh.enabled = true;
                isVisible = true;
                //Debug.Log("Set true");
            }
        }
        //Debug.Log(playerFromTitle);
    }

    public void SetAppearence(bool bAppear)
    {
        // if (changeAlphaCoroutine != null)
        // StopCoroutine(changeAlphaCoroutine);

        if (bAppear)
        {
            titleMesh.enabled = false;
            // changeAlphaCoroutine = setTextAlpha(0, appearDuration, appearCurve);
            isVisible = false;
            //Debug.Log("Set False");
        }
        else
        {
            titleMesh.enabled = true;
            // changeAlphaCoroutine = setTextAlpha(1, disappearDuration, disappearCurve);
            isVisible = true;
            //Debug.Log("Set true");
        }
        // StartCoroutine(changeAlphaCoroutine);
    }


    /*
        IEnumerator setTextAlpha(float targetAlpha, float duration, AnimationCurve curve)
        {
            Color curColor = titleMesh.material.color;
            Color targetColor = new Color(curColor.r, curColor.g, curColor.b, targetAlpha);

            float lerpTime = 0;
            if (duration == 0)
            {
                titleMesh.material.color = targetColor;
                yield break;
            }

            while (lerpTime < duration)
            {
                lerpTime += Time.deltaTime;
                float t = curve.Evaluate(lerpTime / duration);

                titleMesh.material.color = Color.Lerp(curColor, targetColor, t);
                yield return null;
            }
        }*/

}
