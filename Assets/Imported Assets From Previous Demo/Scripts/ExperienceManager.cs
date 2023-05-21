using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


namespace Assets.Scripts
{
    public enum DemoMode
    {
        MainMenu = 0,
        Intro = 1,
        Column = 2,
        Maze = 3,
        ArtGallery = 4,
        Hemoglobin = 5,
        Passthrough = 6,
        Mirror = 7,
        Height = 8,
        EyeTracking = 9,
    }

    /// <summary>
    /// 
    /// </summary>
    public class ExperienceManager : SingletonMonoBehaviour<ExperienceManager>
    {
        [System.Serializable]
        public struct SceneByMode
        {
            public DemoMode Mode;
            public DevLocker.Utils.SceneReference ExperienceScene;
        }

        [SerializeField]
        private List<SceneByMode> Experiences;
        [SerializeField]
        private List<DemoModeChangeButton> modeSelectionButtons;

        public GameObject RightHandController, LeftHandController;
        [SerializeField] private OVRMeshRenderer rightHandRenderer, leftHandRenderer;
        public SphereCollider RightHandControllerInteractionSphereCollider, LeftHandControllerInteractionSphereCollider;
        public GameObject ArenaModel, ModeSelectionButtonsGO;


        public bool StartWithIntro = true;
        public GameObject floor;
        private Scene currentModeScene { get => SceneManager.GetActiveScene(); }
        private Scene HubScene;
        private bool isArenaModelDisplayed = true;
        private Dictionary<DemoMode, DevLocker.Utils.SceneReference> experienceScenesByMode;

        void Start()
        {
            SceneCalibrationManager.Instance.CalibrationSuccessful += OnCalibrationSuccessful;
            HubScene = SceneManager.GetActiveScene();
            experienceScenesByMode = new Dictionary<DemoMode, DevLocker.Utils.SceneReference>();
            foreach (SceneByMode SbM in Experiences)
            {
                if (!experienceScenesByMode.ContainsKey(SbM.Mode))
                {
                    experienceScenesByMode.Add(SbM.Mode, SbM.ExperienceScene);
                }
            }
            SceneManager.sceneLoaded += updateActiveScene;

            DisableOpenWorldButtons();
            SceneCalibrationManager.Instance.StartCalibration();
        }

        private void updateActiveScene(Scene newScene, LoadSceneMode loadSceneMode)
        {
            SceneManager.SetActiveScene(newScene);
        }

        public async void SwitchMode(DemoMode nextMode, bool displayArena = true, bool displayHands = true)
        {
            if (isArenaModelDisplayed != displayArena)
            {
                DisplayArenaModel(displayArena);
            }

            DisplayHands(displayHands);

            await DisableCurrentExperience(currentModeScene);

            await EnableExperience(nextMode);
        }

        private void OnCalibrationSuccessful()
        {
            if (StartWithIntro)
            {
                SwitchMode(DemoMode.Intro);
            }
            else
            {
                EnableOpenWorldButtons();
            }
        }

        private void DisplayArenaModel(bool isOn)
        {
            ArenaModel.SetActive(isOn);
            isArenaModelDisplayed = isOn;
        }

        public void DisplayHands(bool isOn)
        {
            rightHandRenderer.ToggleHand(isOn);
            leftHandRenderer.ToggleHand(isOn);
        }

        private async UniTask EnableExperience(DemoMode nextMode)
        {
            if (nextMode != DemoMode.MainMenu && nextMode != DemoMode.Passthrough)
            {
                var progress = SceneManager.LoadSceneAsync(experienceScenesByMode[nextMode].ScenePath, LoadSceneMode.Additive);
                await progress;
            }
            else
            {
                if (nextMode == DemoMode.Passthrough)
                {
                    PassthroughManager.Instance.TogglePassthrough(true);
                }
                else
                    EnableOpenWorldButtons();
            }

            if (nextMode == DemoMode.Height)
                floor.SetActive(false);
            if(nextMode == DemoMode.MainMenu)
                floor.SetActive(true);

        }

        private async UniTask DisableCurrentExperience(Scene sceneToDisable)
        {
            if (currentModeScene == HubScene)
            {
                DisableOpenWorldButtons();
            }
            else
            {
                //unloads the previous active scene
                await SceneManager.UnloadSceneAsync(currentModeScene);
            }
        }

        public void EnableOpenWorldButtons()
        {
            foreach (DemoModeChangeButton button in modeSelectionButtons)
            {
                button.EnableButton();
            }
        }

        public void DisableOpenWorldButtons()
        {
            foreach (DemoModeChangeButton button in modeSelectionButtons)
            {
                if (button.isBehaviourEnabled) button.DisableButton();
            }
        }

        public void SetFloorVisibility(bool isVisible)
        {
            floor.SetActive(isVisible);
        }
    }
}