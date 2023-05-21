using UnityEngine;
namespace Assets.Scripts
{
    public class DemoShaderController : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        private MaterialPropertyBlock propertyBlock;

        [SerializeField]
        private Color initialColor = new Color(253f / 255f, 1, 131f / 255f); 
        [SerializeField]
        private Color ultimateColor = new Color(119f / 255f, 1, 119f / 255f);        
        [SerializeField, Range(0f,1f)]
        private float initialHoloDistance = 0.3f;        
        [SerializeField, Range(0f, 0.1f)]
        private float initialGlitchStrength = 0.003f;        
        [SerializeField, Range(0f, 0.1f)]
        private float interactingGlitchStrength = 0.03f;
        [SerializeField, Range(0f, 0.1f)]
        private float glitchThreshold = 0.66f;

        private string colorPropertyName = "_Color";
        private string holoDistancePropertyName = "_HoloDistance";
        private string glitchStrengthPropertyName = "_GlitchStrength";


        void Start()
        {
            ResetProperties();
        }

        internal void ResetProperties()
        {
            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor(colorPropertyName, initialColor);
            propertyBlock.SetFloat(holoDistancePropertyName, initialHoloDistance);
            propertyBlock.SetFloat(glitchStrengthPropertyName, initialGlitchStrength);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }

        internal void UpdateValues(float progress) //TODO expose magic values as properties
        {
            var newColor = Color.Lerp(initialColor, ultimateColor, progress);
            propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor(colorPropertyName, newColor);
            propertyBlock.SetFloat(holoDistancePropertyName, initialHoloDistance + progress * (1 - initialHoloDistance));
            propertyBlock.SetFloat(glitchStrengthPropertyName, progress > glitchThreshold ? Mathf.Lerp(initialGlitchStrength, interactingGlitchStrength, (progress - glitchThreshold)/(1- glitchThreshold)) : initialGlitchStrength);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }

        internal void SetGlitchOn(bool isInteracting) //Todo delete if obsolete
        {
            propertyBlock.SetFloat(glitchStrengthPropertyName, isInteracting ? interactingGlitchStrength : initialGlitchStrength);
            meshRenderer.SetPropertyBlock(propertyBlock);
        }
    }
}
