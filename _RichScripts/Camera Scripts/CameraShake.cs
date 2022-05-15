//inspired by: Daniel Moore (Firedan1176) - Firedan1176.webs.com/
//Place on the PARENT of a camera.

using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using Signals;

namespace RichPackage.Cameras
{
    /// <summary>
    /// Can shake the camera. 
    /// </summary>
    /// <see cref="ShakeSignal"/>
    public class CameraShake : RichMonoBehaviour
    {
        /// <summary>
        /// Instructs any camera shakers to do so.
        /// (float Duration, float amount);
        /// </summary>
        public class ShakeSignal : ASignal<float, float> { }

        /// <summary>
        /// How many seconds the object should shake for.
        /// </summary>
        [Header("---Settings---")]
        [Min(0)]
        public float shakeDuration = 0.7f;

        /// <summary>
        /// Amplitude of the shake. A larger value shakes the camera harder.
        /// </summary>
        [Min(0)]
        public float shakeAmount = 15.0f;

        [Tooltip("If false, leave where left off. " +
            "if true, put back where you found it.")]
        public bool resetToOriginOnEnd = true;

        public bool smooth = true;

        [ShowIf("smooth")]
        [Min(0)]
        [Tooltip("Lower is more smooth, higher is choppier.")]
        public float smoothAmount = 5f;

        /// <summary>
        /// Transform of the camera to shake. Grabs the gameObject's transform
        /// if null.
        /// </summary>
        [Header("---Scene References---")]
        [Tooltip("[default = this] Thing to get shooken.")]
        public Transform shakeHandle;

        //runtime values
        private float startAmount;//The initial shake amount (to determine percentage), set when ShakeCamera is called.
        private float startDuration;//The initial shake duration, set when ShakeCamera is called.

        private Quaternion originalPos;
        private Coroutine shakeRoutine;

        protected override void Awake()
        {
            base.Awake();
            if (shakeHandle == null)
                shakeHandle = myTransform;
        }

        private void OnEnable()
        {
            GlobalSignals.Get<ShakeSignal>().AddListener(Shake);
        }

        private void OnDisable()
        {
            GlobalSignals.Get<ShakeSignal>().RemoveListener(Shake);
            if (shakeRoutine != null)
                StopCoroutine(shakeRoutine);
        }

        [Button(null, EButtonEnableMode.Playmode)]
        public void Shake()
        {
            if (shakeRoutine != null)//prevent duplicates
                StopCoroutine(shakeRoutine);
            shakeRoutine = StartCoroutine(DoShake());
        }

        public void Shake(float shakeDuration,
            float shakeAmount)
        {
            this.shakeDuration = shakeDuration;
            this.shakeAmount = shakeAmount;
            Shake();
        }

        public void Shake(Transform handle)
        {
            shakeHandle = handle;
            Shake();
        }

        private IEnumerator DoShake()
        {
            startAmount = shakeAmount;//Set default (start) values
            startDuration = shakeDuration;//Set default (start) values

            var endShakeTime = Time.time + shakeDuration;
            originalPos = shakeHandle.localRotation; //cache

            while (Time.time < endShakeTime)
            {
                var newPos = (Random.insideUnitSphere * shakeAmount)
                    .WithZ(0); //don't shake z

                var shakePercentage = shakeDuration / startDuration;//Used to set the amount of shake (% * startAmount).

                shakeAmount = startAmount * shakePercentage;//Set the amount of shake (% * startAmount).

                if (smooth)
                    transform.localRotation = Quaternion.Lerp(
                        transform.localRotation, 
                        Quaternion.Euler(newPos), 
                        Time.deltaTime * smoothAmount);
                else
                    transform.localRotation = Quaternion.Euler(newPos);//Set the local rotation the be the rotation amount.

                yield return null;//next frame
            }

            if(resetToOriginOnEnd)
                shakeHandle.localRotation = originalPos;
        }
    }
}
