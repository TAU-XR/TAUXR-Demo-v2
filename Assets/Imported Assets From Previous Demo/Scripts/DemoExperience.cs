using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class DemoExperience : MonoBehaviour
    {
        [SerializeField]
        private bool resetOnEnable;
        [SerializeField]
        private DemoMode experienceMode;

        [SerializeField]
        private UnityEvent resetEvent;
        [SerializeField]
        private UnityEvent disableEvent;

        void Start()
        {
            if (resetOnEnable) ResetExperience();
        }

        internal void EnableExperience()
        {
            if (resetOnEnable) ResetExperience();
            //Debug.Log(name + " EnableExperience");
        }

        internal void DisableExperience(DemoMode mode)
        {
            if (mode != experienceMode) return;
            disableEvent?.Invoke();
            //Debug.Log(name + " DisableExperience");
        }

        internal void ResetExperience()
        {
            resetEvent?.Invoke();
        }


    }
}
