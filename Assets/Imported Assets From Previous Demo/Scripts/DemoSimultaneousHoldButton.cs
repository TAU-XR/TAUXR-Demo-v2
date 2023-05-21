using UnityEngine;
using System.Collections;

namespace Assets.Scripts
{
    public class DemoSimultaneousHoldButton : HoldButton
    {
        public delegate void ButtonClicked();
        public event ButtonClicked onButtonActivation;
        public event ButtonClicked onButtonDeactivation;

        private IEnumerator currentCoroutine;

        private IEnumerator Activated()
        {
            onButtonActivation?.Invoke();
            isBehaviourEnabled = false;
            stimulationTime = activationTime * 0.99f;
            while (numberOfTargetsWithinCollider > 0) yield return null;
            yield return new WaitForSeconds(activationTime * 0.66f);
            onButtonDeactivation?.Invoke();
            isBehaviourEnabled = true;
            currentCoroutine = null;
        }

        void Awake()
        {
            shaderController.meshRenderer.enabled = isBehaviourEnabled;
        }

        public override void DisableButton()
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            base.DisableButton();
        }

        protected override void Activate()
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = Activated();
            StartCoroutine(currentCoroutine);
        }
    }
}