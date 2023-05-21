using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class HoldButton : MonoBehaviour //TODO define colliders
    {
        //public List<Collider> TargetColliders;

        [SerializeField]
        protected string handTag;
        [SerializeField]
        public bool isBehaviourEnabled = true; //TODO make private and expose with a property.
        [SerializeField]
        protected bool isReusable = false;
        [SerializeField]
        protected DemoShaderController shaderController;
        [SerializeField]
        protected float activationTime = 2f;
        [SerializeField]
        protected float decayRate = 3f;

        protected int numberOfTargetsWithinCollider;
        protected float stimulationTime;
        protected bool wasTriggerStayLastFixedUpdate = false;

        SphereCollider btnCollider;

        public float activationProgress => stimulationTime / activationTime;
        protected bool isButtonEngaged => numberOfTargetsWithinCollider != 0;
        protected bool shouldDecay => !isButtonEngaged && stimulationTime > 0 && decayRate > 0;

        private void Start()
        {
            if (TryGetComponent(out SphereCollider c))
            {
                btnCollider = c;
            }
            else
            {
                btnCollider = null;
            }
        }

        void Update()
        {
            if (isBehaviourEnabled) ButtonUpdate();
        }

        void FixedUpdate()
        {
            if (isButtonEngaged && isBehaviourEnabled)
            {
                stimulationTime += Time.fixedDeltaTime;
                //shaderController.UpdateValues(activationProgress); //old shader disabled
                if (stimulationTime > activationTime)
                {
                    ResetButton();
                    if (!isReusable) DisableButton();
                    Activate();
                }
            }
        }

        protected bool IsColliderAccepted(Collider target) => target.tag.Equals(handTag); //Todo use layers instead of tags for efficiency

        protected abstract void Activate();

        public virtual void EnableButton(bool reset = true)
        {
            ResetButton();
            isBehaviourEnabled = true;
            shaderController.meshRenderer.enabled = true;
            if (btnCollider != null)
                btnCollider.enabled = true;
        }

        public virtual void DisableButton()
        {
            isBehaviourEnabled = false;
            shaderController.meshRenderer.enabled = false;
            if (btnCollider != null)
                btnCollider.enabled = false;
        }

        public void ResetButton()
        {
            numberOfTargetsWithinCollider = 0;
            stimulationTime = 0f;
            shaderController.UpdateValues(0f);
        }

        protected virtual void ButtonUpdate()
        {
            if (shouldDecay)
            {
                stimulationTime -= Time.deltaTime * decayRate;
                shaderController.UpdateValues(activationProgress);
            }
        }

        protected virtual void OnTriggerEnter(Collider target) //TODO maintain a list of colliders to prevent multiplications?
        {
            if (isBehaviourEnabled && IsColliderAccepted(target))
            {
                //Debug.Log(gameObject.name + " trigger enter");
                numberOfTargetsWithinCollider += 1;
            }
        }

        protected virtual void OnTriggerStay(Collider target)
        {
            //if (isBehaviourEnabled && IsColliderAccepted(target))
            //{
            //    stimulationTime += Time.fixedDeltaTime;
            //    shaderController.UpdateValues(activationProgress);
            //    if (stimulationTime > activationTime)
            //    {
            //        if (!isReusable) DisableButton();
            //        Activate();
            //    }
            //}
        }

        protected virtual void OnTriggerExit(Collider target) //TODO maintain a list of colliders to prevent multiplications
        {
            if (isBehaviourEnabled && IsColliderAccepted(target))
            {
                //Debug.Log(gameObject.name + " trigger exit");
                if (numberOfTargetsWithinCollider > 0)
                {
                    numberOfTargetsWithinCollider -= 1;
                }
            }
        }
    }
}

