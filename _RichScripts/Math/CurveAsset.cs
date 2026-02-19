using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;
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

        protected virtual void Reset()
        {
            SetDevDescription("A mathematical curve that can be scripted.");
        }

        [Button]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Evaluate(float t)
        {
            return curve.Evaluate(t);
        }

        [Button]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Evaluate(int t)
        {
            return (int)curve.Evaluate(t);
        }

        #region Factory Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CurveAsset CreateInstance() => CreateInstance<CurveAsset>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CurveAsset CreateInstance(AnimationCurve curve)
        {
            var instance = CreateInstance();
            instance.curve = curve;
            return instance;
        }

        #endregion Factory Methods
    }
}
