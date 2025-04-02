using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.UI.Framework
{
    /// <summary>
    /// Template for an UI. You can rig the prefab for the UI Frame itself and all the screens that should
    /// be instanced and registered upon instantiating a new UI Frame.
    /// </summary>
    public class UISettings : RichScriptableObject
    {
        [Tooltip("Prefab for the UI Frame structure itself")]
        [SerializeField, Required]
        private UIFrame templateUIPrefab = null;

        [Tooltip("Prefabs for all the screens (both Panels and Windows) that are to be instanced and registered when the UI is instantiated")]
        [SerializeField]
        private List<GameObject> screensToRegister = new List<GameObject>();

        [Title("Settings")]
        [Tooltip("In case a screen prefab is not deactivated, should the system automatically deactivate its GameObject upon instantiation? If false, the screen will be at a visible state upon instantiation.")]
        [SerializeField]
        private bool deactivateScreenGOs = true;

        [Title("Runtime")]
        [ShowInInspector, ReadOnly]
        public UIFrame Instance { get; private set; }

        private void OnValidate()
        {
            RemoveBadElements();
        }

        /// <summary>
        /// Creates an instance of the UI Frame Prefab. By default, also instantiates
        /// all the screens listed and registers them. If the deactivateScreenGOs flag is
        /// true, it will deactivate all Screen GameObjects in case they're active.
        /// </summary>
        public void DoCreateUIInstance()
            => CreateUIInstance(instanceAndRegisterScreens: true);

        /// <summary>
        /// Creates an instance of the UI Frame Prefab. By default, also instantiates
        /// all the screens listed and registers them. If the deactivateScreenGOs flag is
        /// true, it will deactivate all Screen GameObjects in case they're active.
        /// </summary>
        /// <param name="instanceAndRegisterScreens">Should the screens listed in the Settings file be instanced and registered?</param>
        /// <returns>A new UI Frame</returns>
        public UIFrame CreateUIInstance(bool instanceAndRegisterScreens = true)
        {
            Instance = Instantiate(templateUIPrefab);

            if (instanceAndRegisterScreens)
            {
                foreach (GameObject screenPrefab in screensToRegister)
                {
                    GameObject screenInstance = Instantiate(screenPrefab);

                    if (screenInstance.TryGetComponent(
                        out IUIScreen screenController))
                    {
                        bool usePrefabName = string.IsNullOrEmpty(screenController.ScreenID);
                        string screenID = usePrefabName ? screenPrefab.name : screenController.ScreenID;

                        Instance.RegisterScreen(screenID, screenController,
                            screenInstance.transform);

                        if (deactivateScreenGOs && screenInstance.activeSelf)
                            screenInstance.SetActive(false);
                    }
                    else
                    {
                        Debug.LogError($"[{nameof(UISettings)}] " +
                            $"Screen doesn't contain a {nameof(IUIScreen)}! " +
                            "Skipping " + screenPrefab.name, this);
                    }
                }
            }

            return Instance;
        }

        /// <summary>
        /// Remove elements that don't have an <see cref="IUIScreen"/> Component.
        /// </summary>
        private void RemoveBadElements()
        {
            var objectsToRemove = new List<GameObject>();
            for (int i = 0; i < screensToRegister.Count; i++)
            {
                IUIScreen screenCtl = screensToRegister[i].GetComponent<IUIScreen>();
                if (screenCtl is null)
                    objectsToRemove.Add(screensToRegister[i]);
            }

            if (objectsToRemove.Count > 0)
            {
                Debug.LogError("[" + name + "] Some GameObjects that were added to the "
                    + $"Screen Prefab List didn't have a {nameof(IUIScreen)} attached! "
                    + "Removing...", this);

                foreach (GameObject obj in objectsToRemove)
                {
                    screensToRegister.Remove(obj);
                    Debug.LogError($"[{nameof(UISettings)}] Removed {obj.name} from {name}"
                        + $" as it has no {nameof(IUIScreen)} component!", this);
                }
            }
        }
    }
}