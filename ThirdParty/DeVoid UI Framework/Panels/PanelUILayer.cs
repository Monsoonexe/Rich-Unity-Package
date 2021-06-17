using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <seealso cref="APanelController"/>
public class PanelUILayer : AUILayer<IPanelController>
{
    [SerializeField]
    private PanelPriorityLayerList priorityLayers = 
        new PanelPriorityLayerList();

    private void ReparentToParaLayer(PanelPriorityENUM priority,
        Transform screenTransform)
    {
        Transform targetTransform;

        if (!priorityLayers.ParaLayerLookup.TryGetValue(priority, out targetTransform))
        {
            targetTransform = this.transform;
        }

        screenTransform.SetParent(targetTransform, false);
    }

    public override void ReparentScreen(IUIScreenController controller, 
        Transform screenTransform)
    {
        var panelController = controller as IPanelController;
        if (panelController != null)
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
        if(registeredScreens.TryGetValue(panelID, out panel))
        {
            return panel.IsVisible;
        }

        return false;
    }
}
