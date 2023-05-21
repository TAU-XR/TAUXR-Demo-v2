using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(MeshCollider), typeof(MeshRenderer))]
    public class NaiveEyeTracker : MonoBehaviour
    {
        [SerializeField]
        private bool InstantiatePrefabsInsteadOfTexture = true;
        [SerializeField]
        private bool isTracking;
        [SerializeField]
        private float recordingInterval = 0.66f;
        [SerializeField]
        private Camera playerCamera;
        [SerializeField]
        private LayerMask layerToHit;
        [SerializeField]
        private Color defaultColor;
        [SerializeField]
        private GameObject gazeSpherePrefab;

        private float trackingTimer;
        private float maxDist = 10; //TODO max distance for raycast.
        private List<Vector2> trackedGazes; // TODO address possible capacity issues
        private List<Vector3> trackedGazes3D;
        private Stack<GameObject> InstantiatedGazeSpheres;
        private MeshRenderer meshRenderer;

        private Color[] gazeColors;

        void Start()
        {
            InstantiatedGazeSpheres = new Stack<GameObject>();
            if (playerCamera == null) playerCamera = Camera.main;

            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.EnableKeyword("_DETAIL_MULX2");

            if (InstantiatePrefabsInsteadOfTexture)
            {
                trackedGazes3D = new List<Vector3>();
            }
            else
                trackedGazes = new List<Vector2>();

            #region -= Initialize Gaze Marker =- 

            var red1 = defaultColor;
            red1.a = 0.1f;
            var red2 = defaultColor;
            red2.a = 0.3f;
            var red3 = defaultColor;
            red2.a = 0.5f;
            var red4 = defaultColor;
            red2.a = 0.8f;
            var red5 = defaultColor;
            red2.a = 1f;
            var outl = Color.white;
            outl.a = 0.1f;
            var corn = Color.Lerp(red4, outl, 0.8f);
            var tran = Color.white;
            
                
            gazeColors = new Color[]
            {
                tran, outl, outl, outl, outl, outl, tran,
                outl, corn, red4, red3, red4, corn, outl,
                outl, red4, red3, red2, red3, red4, outl,
                outl, red3, red2, red1, red2, red3, outl,
                outl, red4, red3, red2, red3, red4, outl,
                outl, corn, red4, red3, red4, corn, outl,
                tran, outl, outl, outl, outl, outl, tran,
            };

            #endregion

        }

        void Update()
        {
            if (isTracking)
            {
                trackingTimer += Time.deltaTime;
                if (trackingTimer > recordingInterval)
                {
                    RecordGaze();
                    trackingTimer = 0f;
                }
            }
        }

        public void SetRecordingMode(bool isOn)
        {
            if (isOn)
            {
                if (isTracking) return;
                ResetTracking();
                isTracking = true;
            }
            else
            {
                if (isTracking) isTracking = false;
            }
        }

        private void ResetTracking()
        {
            if (InstantiatePrefabsInsteadOfTexture)
            {
                trackedGazes3D = new List<Vector3>();
            }
            else
                trackedGazes = new List<Vector2>();
            trackingTimer = 0f;
        }

        private void RecordGaze()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hitInfo, maxDist))
            {
                if (hitInfo.collider.gameObject == this.gameObject)
                {
                    if (InstantiatePrefabsInsteadOfTexture)
                    {
                        trackedGazes3D.Add(hitInfo.point);
                    }
                    else
                        trackedGazes.Add(hitInfo.textureCoord);
                }
            }
        }

        public void PaintGazes()
        {
            if (InstantiatePrefabsInsteadOfTexture)
            {
                for (int i = 0; i < trackedGazes3D.Count; i++)
                { 
                    InstantiatedGazeSpheres.Push(Instantiate(gazeSpherePrefab, trackedGazes3D[i], Quaternion.identity));
                }
            }
            else
            {
                var tempHeight = meshRenderer.material.mainTexture.height;
                var tempWidth = meshRenderer.material.mainTexture.width;

                Texture2D tempTexture = new Texture2D(tempWidth, tempHeight, TextureFormat.RGBA32, false);
                //tempTexture = (Texture2D)meshRenderer.material.mainTexture;
                for (int i = 0; i < trackedGazes.Count; i++)
                { //TODO clamp values!
                    tempTexture.SetPixels((int)Mathf.Clamp((trackedGazes[i].x * tempWidth), 5, tempWidth - 3), (int)Mathf.Clamp((trackedGazes[i].y * tempHeight), 5, tempHeight - 3), 7, 7, gazeColors);
                }
                tempTexture.Apply();
                meshRenderer.material.SetTexture("_DetailAlbedoMap", tempTexture);

                //Debug.Log(name + "Painted");
            }
        }

        public void RemoveIndicators()
        {
            while (InstantiatedGazeSpheres.Count > 0)
            {
                InstantiatedGazeSpheres.Pop().SetActive(false);
            }
        }
    }
}