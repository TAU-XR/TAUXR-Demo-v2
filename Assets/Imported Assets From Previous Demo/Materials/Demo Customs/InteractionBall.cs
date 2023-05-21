using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Experience
{
    /// <summary>
    /// Controls the interaction ball which starts the experience
    /// </summary>
    public class InteractionBall : MonoBehaviour
    {
        private const string ShowProperty = "Show";
        public GameObject RightHand;
        public GameObject LeftHand;

        public float Duration;

        private Material material;

        private int colliderCount;
        private float progress;

        public event System.Action OnComplete;

        private bool finished;

        private void Start()
        {
            Renderer ren = GetComponent<Renderer>();
            material = ren.sharedMaterial = new Material(ren.sharedMaterial);
            colliderCount = 0;
        }

        private void OnTriggerEnter(Collider other)
        {
            colliderCount++;
        }

        private void OnTriggerExit(Collider other)
        {
            colliderCount--;
        }

        private void Update()
        {
            material.SetVector("RightHand", RightHand.transform.position);
            material.SetVector("LeftHand", LeftHand.transform.position);
            if (finished)
                return;

            if (0 < colliderCount)
                progress += Time.deltaTime;
            else progress = Mathf.MoveTowards(progress, 0, Time.deltaTime);
            float factor = progress / Duration;
            material.SetFloat(ShowProperty, factor * 0.7f + 0.3f);
            if (1.2f < factor)
            {
                //gameObject.SetActive(false);
                StartCoroutine(Disappear());
                finished = true;
            }
        }

        private IEnumerator Disappear()
        {
            float startTime = Time.time, endTime = Time.time + 0.2f;
            while (Time.time < endTime)
            {
                yield return null;
                float progress = Mathf.InverseLerp(startTime, endTime, Time.time);
                if (Mathf.Approximately(1, progress) || 1 < progress)
                {
                    OnComplete?.Invoke();
                    Destroy(gameObject);
                    yield break;
                }
                material.SetFloat(ShowProperty, 1 - progress);
            }
        }
    }
}
