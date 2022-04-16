using UnityEngine;

namespace ScriptableObjectArchitecture
{
    /// <summary>
    /// Base class for SOArchitecture assets
    /// Implements developer descriptions
    /// </summary>
    public abstract class SOArchitectureBaseMonobehaviour : MonoBehaviour
    {
#pragma warning disable 0414
        [SerializeField]
        private DeveloperDescription DeveloperDescription = new DeveloperDescription();
#pragma warning restore

        private string _name;

        public string Name 
        {
            get => string.IsNullOrEmpty(_name) ? (_name = name) : _name;
            set => _name = value;
        }
    } 
}