using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class DemoSimultaneousBehaviour : MonoBehaviour
    {
        [SerializeField] 
        private List<DemoSimultaneousHoldButton> holdButtons;
        
        public UnityEvent Behaviour;

        private int activatedButtons;

        void Start()
        {
            foreach (DemoSimultaneousHoldButton holdButton in holdButtons)
            {
                holdButton.onButtonActivation += RegisterActivation;
                holdButton.onButtonDeactivation += RegisterDeactivation;
            }
        }

        public void ResetCount()
        {
            activatedButtons = 0;
        }

        public void EnableButtons(bool reset = true)
        {
            foreach (DemoSimultaneousHoldButton holdButton in holdButtons)
            {
                holdButton.EnableButton();
            }
        }

        public void DisableButtons()
        {
            foreach (DemoSimultaneousHoldButton holdButton in holdButtons)
            {
                holdButton.DisableButton();
            }
        }

        private void RegisterActivation()
        {
            activatedButtons += 1;
            if (activatedButtons == holdButtons.Count)
            {
                RunBehaviour();
            }
        }

        private void RegisterDeactivation()
        {
            activatedButtons -= 1;
            if (activatedButtons == holdButtons.Count)
            {
                RunBehaviour();
            }
        }

        public void RunBehaviour()
        {
            //Debug.Log(gameObject.name + "Behaviour init");
            Behaviour.Invoke();
        }
    }
}


