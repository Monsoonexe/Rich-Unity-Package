using System;
using UnityEngine;

namespace ScriptableObjectArchitecture
{
    /// <summary>
    /// <see cref="SceneVariable"/> is a scriptable constant variable whose scene values are assigned at
    /// edit-time by assigning a <see cref="UnityEditor.SceneAsset"/> instance to it.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SceneVariable.asset",
        menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "Scene",
        order = 120)]
    public sealed class SceneVariable : BaseVariable<SceneInfo>
    {
        /// <summary>
        /// Returns the <see cref="SceneInfo"/> of this instance.
        /// </summary>
        public override SceneInfo Value
        {
            get { return _value; }
        }

        // A scene variable is essentially a constant for edit-time modification only; there is not
        // any kind of expectation for a user to be able to set this at runtime.
        public override bool ReadOnly => true;
    }

    [Serializable]
    [MultiLine]
    public sealed class SceneInfo : ISerializationCallbackReceiver
    {
        [SerializeField]
        private string _scenePath;
        /// <summary>
        /// Returns the fully-qualified name of the scene.
        /// </summary>
        public string ScenePath => _scenePath;

        [SerializeField]
        private int _sceneIndex;
        /// <summary>
        /// Returns the index of the scene in the build settings; if not present, -1 will be returned instead.
        /// </summary>
        public int SceneIndex
        {
            get => _sceneIndex; 
            internal set => _sceneIndex = value; 
        }

        [SerializeField]
        private bool _isSceneEnabled;
        /// <summary>
        /// Returns true if the scene is enabled in the build settings, otherwise false.
        /// </summary>
        public bool IsSceneEnabled
        {
            get => _isSceneEnabled;
            internal set => _isSceneEnabled = value;
        }

        //player-facing info
        [SerializeField]
        [Tooltip("Player-facing name of scene.")]
        private string _sceneName;
        /// <summary>
        /// Player-facing name of scene.
        /// </summary>
        public string SceneName => _sceneName;

		/// <summary>
		/// Player-facing description of level.
		/// </summary>
		[Tooltip("Player-facing description of level.")]
		[SerializeField]
		private string _sceneDescription = "A nice place to visit.";
		public string Description => _sceneDescription;

		[SerializeField]
		[Tooltip("Player-facing icon.")]
		private Sprite _icon;
		public Sprite Icon => _icon;

        /// <summary>
        /// Returns true if the scene is present in the build settings, otherwise false.
        /// </summary>
        public bool IsSceneInBuildSettings => _sceneIndex != -1;

        #if UNITY_EDITOR
        internal UnityEditor.SceneAsset Scene
        {
            get { return UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(_scenePath); }
        }
        #endif

        public SceneInfo()
        {
            _sceneIndex = -1;
        }

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
			#if UNITY_EDITOR
			if (Scene != null)
            {
                var sceneAssetPath = UnityEditor.AssetDatabase.GetAssetPath(Scene);
                var sceneAssetGUID = UnityEditor.AssetDatabase.AssetPathToGUID(sceneAssetPath);
                var scenes = UnityEditor.EditorBuildSettings.scenes;

                SceneIndex = -1;
				int enabledSceneIndex = 0;//scenes are only given a build index if enabled.
				for (var i = 0; i < scenes.Length; i++)
                {
					bool sceneIsEnabled = scenes[i].enabled;
                    if (scenes[i].guid.ToString() == sceneAssetGUID)
                    {
						if(sceneIsEnabled)
							SceneIndex = enabledSceneIndex++;
                        IsSceneEnabled = sceneIsEnabled;
                        break;
                    }
					else if (sceneIsEnabled)
					{
						++enabledSceneIndex;
					}
                }
            }
            #endif
        }

        public void OnAfterDeserialize(){} //required for interface

        #endregion
    }
}
