using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class DemoHoldButton : HoldButton
    {
        public UnityEvent clickEvent;

        void Awake()
        {
            shaderController.meshRenderer.enabled = isBehaviourEnabled;
        }

        protected override void Activate()
        {
            clickEvent?.Invoke();
            //Debug.Log(gameObject.name + " button was activated");
        }
    }
}