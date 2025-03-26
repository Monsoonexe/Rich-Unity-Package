using UnityEngine;
using Zenject;

namespace RichPackage.Coroutines
{
    /// <summary>
    /// Similar to <see cref="WaitForSeconds"/> except it can be re-used.
    /// </summary>
    public class RecyclableWaitForSeconds : CustomYieldInstruction
    {
        public float duration;
        private SimpleTimer timer;
        public override bool keepWaiting { get => timer < duration; }

        #if UNITY_2021_OR_NEWER
        public override void Reset() => Reset(duration);
        #endif

        public void Reset(float newDuration)
        {
            duration = newDuration;
            timer.Reset();
        }

        public class Pool : MemoryPool<float, RecyclableWaitForSeconds>
        {
            protected override void Reinitialize(float duration, RecyclableWaitForSeconds item)
            {
                item.Reset(duration);
            }
        }
    }
}
