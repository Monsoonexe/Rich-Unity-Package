using UnityEngine;

namespace ScriptableObjectArchitecture
{
    /// <summary>
    /// Base class for SOArchitecture assets
    /// Implements developer descriptions
    /// </summary>
    public abstract class SOArchitectureBaseObject : ScriptableObject
    {
        public DeveloperDescription DeveloperDescription = new DeveloperDescription();

        private string _name;

        public string Name 
        {
            get => string.IsNullOrEmpty(_name) ? (_name = name) : _name;
            set => _name = value;
        }
    } 
}