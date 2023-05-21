using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts
{
    public class DemoModeChangeButton : HoldButton
    {
        public delegate void ButtonClicked(DemoMode mode);
        public event ButtonClicked onButtonClick;

        [SerializeField]
        private MeshRenderer titleMeshRenderer;
        [SerializeField]
        private Transform playerCamera;
        [SerializeField]
        private float minVisibleDistance = .9f;

        [Header("Experience Properties")]
        [SerializeField]
        private DemoMode mode;
        [SerializeField]
        private bool DisplayArenaModel = true;
        [SerializeField]
        private bool DisplayHands = true;
        [SerializeField]
        [Range(0.0f, 30.0f)]
        private float delayUntilSwitch = 0;
        [Header("Testing")]
        public bool editorActivateButton = false;

        public UnityEvent additionalClickEvent;

        private void Update()
        {
            if (editorActivateButton)
            {
                editorActivateButton = false;
                Activate();
            }
        }

        void Awake()
        {
            shaderController.meshRenderer.enabled = isBehaviourEnabled;
            titleMeshRenderer.enabled = isBehaviourEnabled;
            if (playerCamera == null)
            {
                playerCamera = Camera.main.transform;
            }
        }
        public void SwitchMode()
        {
            ExperienceManager.Instance.SwitchMode(mode, DisplayArenaModel, DisplayHands);
        }

        protected override void ButtonUpdate()
        {
            base.ButtonUpdate();
            var playerFromTitle = Mathf.Abs((playerCamera.position - transform.position).magnitude);        
            titleMeshRenderer.enabled =  playerFromTitle > minVisibleDistance ? true : false; // Appear/disappear based on distance.
            titleMeshRenderer.transform.LookAtIgnoreHeight(playerCamera.transform);
        }

        protected override async void Activate()
        {
            additionalClickEvent.Invoke();
            if (delayUntilSwitch > 0)
            {
                await UniTask.Delay((int)(delayUntilSwitch * 1000));
            }
            SwitchMode();
            //Debug.Log(gameObject.name + " button was activated");
            if (!isReusable) DisableButton();
        }

        public override void EnableButton(bool reset = true)
        {
            base.EnableButton();
            titleMeshRenderer.enabled = true;
        }

        public override void DisableButton()
        {
            base.DisableButton();
            titleMeshRenderer.enabled = false;
        }
    }
}