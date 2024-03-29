﻿using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <seealso cref="WindowUILayer"/>
/// <seealso cref="APanelController"/>
public class PanelUILayer : AUILayer<IPanelController>
{
    [SerializeField]
    private PanelPriorityLayerList priorityLayers =
        new PanelPriorityLayerList();

    private void ReparentToParaLayer(EPanelPriority priority,
        Transform screenTransform)
    {
        Transform targetTransform;

        if (!priorityLayers.ParaLayerLookup.TryGetValue(priority, out targetTransform))
        {
            targetTransform = transform;
        }

        screenTransform.SetParent(targetTransform, false);
    }

    public override void ReparentScreen(IUIScreenController controller,
        Transform screenTransform)
    {
        if (controller is IPanelController panelController)
        {
            ReparentToParaLayer(panelController.Priority, screenTransform);
        }
        else
        {
            base.ReparentScreen(controller, screenTransform);
        }
    }

    public override void HideScreen(IPanelController screen, bool animate = true)
    {
        screen.Hide(animate);
    }

    public override void ShowScreen(IPanelController screen)
    {
        screen.Show();
    }

    public override void ShowScreen<TProps>(IPanelController screen,
        TProps properties)
    {
        screen.Show(properties);
    }

    public bool IsPanelVisible(string panelID)
    {
        IPanelController panel;
        return registeredScreens.TryGetValue(panelID, out panel) && panel.IsVisible;
    }
}
