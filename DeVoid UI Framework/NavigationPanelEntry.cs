using System;
using UnityEngine;

namespace ProjectEmpiresEdge.UI { 
    /// <summary>
    /// Holds icon, text, and target screen data.
    /// </summary>
    [Serializable]
    public struct NavigationPanelEntry
    {
        //data is readonly set in Inspector
        [SerializeField] private Sprite sprite;
        [SerializeField] private string buttonText;
        [SerializeField] private string targetScreen;

        public Sprite Sprite { get => sprite; }
        public string ButtonText { get => buttonText; }
        public string TargetScreen { get => targetScreen; }
    }
}
