using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.Mathematics
{
    /// <summary>
    /// A mathematical curve that can be scripted.
    /// </summary>
    public partial class CurveAsset : RichScriptableObject
    {
        [SerializeField]
        protected AnimationCurve curve;

        [Button]
        public virtual float Evaluate(float t)
        {
            return curve.Evaluate(t);
        }

        [Button]
        public virtual int Evaluate(int t)
        {
            return (int)curve.Evaluate(t);
        }
    }
}
