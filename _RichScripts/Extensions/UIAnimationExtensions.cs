using UnityEngine;
using DG.Tweening;

/// <summary>
/// 
/// </summary>
public static class UIAnimationExtensions
{
    /// <summary>
    /// Assumes scale is currently where it will be when open, shrinks, then scales back up.
    /// </summary>
    /// <param name="_transform"></param>
    /// <returns>The tween for your own animation use.</returns>
    public static Tween Animate_CenterZoomIn(this Transform _transform,
        float duration = 0.5f)
    {
        var scale = _transform.localScale;
        _transform.localScale = new Vector3(0, 0, scale.z);//preserve z for UI stuff
        return _transform.DOScale(scale, duration);
    }

    /// <summary>
    /// Assumes scale is currently where it will be when open, shrinks, then scales back up.
    /// </summary>
    /// <param name="_transform"></param>
    /// <returns>The tween for your own animation use.</returns>
    public static Tween Animate_CenterZoomOut(this Transform _transform,
        float duration = 0.5f)
    {
        var preservedScale = _transform.localScale;
        var targetScale = new Vector3(0, 0, preservedScale.z);//preserve z for UI stuff

        void ResetScale() => _transform.localScale = preservedScale;//local function for callack

        var tween = _transform.DOScale(targetScale, duration);
        tween.onComplete += ResetScale;
        return tween;
    }

    /// <summary>
    /// Assumes scale is currently where it will be when open, shrinks, then scales back up.
    /// </summary>
    /// <param name="_transform"></param>
    /// <returns>The tween for your own animation use.</returns>
    public static Tween Animate_CenterExpandVert(this Transform _transform,
        float duration = 0.5f)
    {
        var scale = _transform.localScale;
        _transform.localScale = scale.With(y: 0);//shrink
        return _transform.DOScaleY(scale.y, duration);//scale y value
    }

    /// <summary>
    /// Assumes scale is currently where it will be when open, shrinks, then scales back up.
    /// </summary>
    /// <param name="_transform"></param>
    /// <returns>The tween for your own animation use.</returns>
    public static Tween Animate_CenterCollapseVert(this Transform _transform,
        float duration = 0.5f)
    {
        var preservedScale = _transform.localScale;

        void ResetScale() => _transform.localScale = preservedScale;//local function for callack

        var tween = _transform.DOScaleY(0, duration);//scale y value
        tween.onComplete += ResetScale;//fix scale on complete
        return tween;
    }

    public static Tween Animate_FadeIn(this CanvasGroup cg, float duration)
        => cg.DOFade(1, duration);

    public static Tween Animate_FadeOut(this CanvasGroup cg, float duration)
        => cg.DOFade(0, duration);

    /// <summary>
    /// Rotate this Transform by this euler rotation over this duration.
    /// </summary>
    /// <param name="tran"></param>
    /// <param name="rot"></param>
    /// <param name="dur"></param>
    /// <returns></returns>
    public static Tween DOLocalRotateBy(this Transform tran, Vector3 rot, float dur)
    {
        var targetRotation = tran.localEulerAngles + rot;
        return tran.DOLocalRotate(targetRotation, dur);
    }
}
