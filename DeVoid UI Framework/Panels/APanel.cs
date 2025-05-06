namespace RichPackage.UI.Framework
{
    /// <summary>
    /// Base class for Panels -- a given chunk of UI that can coexist at the same time as 
    /// other pieces of UI. Eg: status bars, elements on your HUD. Encapsulate 1+ widgets.
    /// </summary>
    /// <seealso cref="AWindow"/>
    public abstract class APanel<TProps> : AUIScreen<TProps>, IPanel
        where TProps : IPanelProperties
    {
        #region IPanel

        public EPanelPriority Priority => Properties?.Priority ?? EPanelPriority.Default;
        PanelUILayer IPanel.Layer { get; set; }
        protected IPanel Panel => this; // fast cast

        #endregion IPanel

        public void Open() => Panel.Layer.ShowScreen(this);
        public void Close() => Close(true);
        public void Close(bool animate) => Panel.Layer.HideScreen(this, animate);

        protected sealed override void SetProperties(TProps payload)
        {
            // seal the default impl
            base.SetProperties(payload);
        }
    }

    /// <summary>
    /// Base class for Panels with no special Properties.
    /// </summary>
    public abstract class APanel : APanel<PanelProperties>
    {
        // exists
    }
}
