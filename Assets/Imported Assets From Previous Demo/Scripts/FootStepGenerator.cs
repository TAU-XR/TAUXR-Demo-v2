using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class FootStepGenerator : MonoBehaviour
    {
        [SerializeField]
        private bool isTracking;
        [SerializeField]
        private float stepSize = 0.5f;
        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private GameObject stepPrefab;
        [SerializeField]
        private float stepWidthOffset = 0.1f;
        [SerializeField]
        private Transform StepHeightTransform;

        public UnityEvent FinishedDrawing;

        private bool isLeftStep = true;
        private float trackingTimer;
        private List<Vector3> stepPositions;
        private List<Quaternion> stepRotations;
        private List<float> stepTimings;
        private List<GameObject> steps;

        private float DistanceFromLastStep => Mathf.Abs(
            (new Vector2(playerCamera.transform.position.x, playerCamera.transform.position.z) -  new Vector2(stepPositions[stepPositions.Count - 1].x, stepPositions[stepPositions.Count - 1].z))
            .magnitude);

        private IEnumerator OnDrawSteps()
        {
            isLeftStep = true;
            DrawStep(0); // draw two feet in initial spot
            for (int i = 0; i < stepTimings.Count; i++)
            {
                yield return new WaitForSeconds(stepTimings[i]);
                DrawStep(i);
            }
            yield return new WaitForSeconds(0.45f);
            DrawStep(stepTimings.Count-1); // draw two feet in the final position
            FinishedDrawing?.Invoke();
        }

        void Start()
        {
            //Todo initialize FinishedDrawing?
            //ResetSteps();
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }
        }

        void Update()
        {
            if (isTracking)
            {
                trackingTimer += Time.deltaTime;
                if (DistanceFromLastStep > stepSize)
                {
                    RecordStep();
                    trackingTimer = 0f;
                }
            }
        }

        public void SetTrackingState(bool isOn)
        {
            isTracking = isOn;
            if (isTracking) ResetSteps();
        }

        private void ResetSteps()
        {
            stepPositions = new List<Vector3>();
            stepRotations = new List<Quaternion>();
            stepTimings = new List<float>();
            DeleteSteps();
            steps = new List<GameObject>();
            trackingTimer = 0f;
            RecordStep(); // Start with the right foot ;)
        }

        private void RecordStep()
        {
            stepTimings.Add(trackingTimer);
            Vector3 stepPosition = playerCamera.transform.position;
            stepPosition.y = StepHeightTransform.position.y;
            stepPositions.Add(stepPosition);
            var stepRotation = playerCamera.transform.rotation.eulerAngles;
            stepRotation.x = 90; //Sets the sprite to an horizontal plane
            stepRotations.Add(Quaternion.Euler(stepRotation));
        }

        public void DrawSteps()
        {
            //Todo make sure coroutine is not running.
            isTracking = false;
            StartCoroutine(OnDrawSteps());
        }

        private void DrawStep(int index)
        {
            var step = Instantiate(stepPrefab, stepPositions[index], stepRotations[index], transform);
            //var offsetVec = new Vector3 (stepWidthOffset, 0 , 0);
            //step.transform.localPosition = isLeftStep ? step.transform.localPosition + offsetVec : step.transform.localPosition - offsetVec;
            step.transform.localPosition = isLeftStep ? step.transform.localPosition + step.transform.right * -stepWidthOffset : step.transform.localPosition + step.transform.right * stepWidthOffset;
            var spriteRenderer = step.GetComponent<SpriteRenderer>();
            spriteRenderer.flipX = isLeftStep;
            isLeftStep = !isLeftStep;
            steps.Add(step);
        }

        public void DeleteSteps()
        {
            if (steps == null || steps.Count == 0) return;
            foreach (GameObject step in steps)
            {
                Destroy(step);
            }
        }

    }
}