using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class DemoBehaviour : MonoBehaviour
    {
     
        [SerializeField] 
        private List<DemoHoldButton> holdButtons;
        
        public UnityEvent Behaviour;

        private int buttonsClicked;

        void Start()
        {
            foreach (DemoHoldButton holdButton in holdButtons)
            {
                holdButton.clickEvent.AddListener(RegisterClick);
            }
        }

        public void ResetCount()
        {
            buttonsClicked = 0;
        }

        internal void EnableButtons(bool reset = true)
        {
            foreach (DemoHoldButton holdButton in holdButtons)
            {
                holdButton.EnableButton();
            }
        }

        internal void DisableButtons()
        {
            foreach (DemoHoldButton holdButton in holdButtons)
            {
                holdButton.DisableButton();
            }
        }

        private void RegisterClick()
        {
            buttonsClicked += 1;
            if (buttonsClicked == holdButtons.Count)
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


