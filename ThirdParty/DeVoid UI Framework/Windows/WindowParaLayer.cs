using System.Collections.Generic;
using UnityEngine;
using RichPackage;

/// <summary>
/// 
/// </summary>
/// <seealso cref="WindowUILayer"/>
public class WindowParaLayer : RichMonoBehaviour
{
    [SerializeField]
    private Transform darkenBackgroundObject = null;

    private readonly List<GameObject> containedScreens = new List<GameObject>();

    public void AddScreen(Transform screenRect)
    {
        screenRect.SetParent(transform, false);
        containedScreens.Add(screenRect.gameObject);
    }

    public void RefreshDarken()
    {
        for (var i = 0; i < containedScreens.Count; ++i)
        {
            var screen = containedScreens[i];

            if (screen)
            {
                if (screen.activeSelf)
                {
                    DarkenBackground();
                    return;
                }
            }
        }

        darkenBackgroundObject.gameObject.SetActive(false);
    }

    public void DarkenBackground()
    {
        darkenBackgroundObject.gameObject.SetActive(true);
        darkenBackgroundObject.SetAsLastSibling();
    }
    
}
