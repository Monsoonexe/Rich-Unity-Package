using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using RichPackage.TriggerVolumes;
using Sirenix.OdinInspector;

namespace RichPackage.ProjectileSystem
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Projectile"/>
    [RequireComponent(typeof(Collider2D)),
        RequireComponent(typeof(TriggerVolume))]
	[Obsolete("Just use Projectile instead. it's exactly the same.")]
    public class Projectile2D : RichMonoBehaviour
    {
        [Title("Projectile"), Min(0)]
        public float speed = 5.0f;

        [Title("References")]
        [field: SerializeField, Required, LabelText(nameof(Collider))]
        public Collider2D Collider { get; private set; }

        [field: SerializeField, Required, LabelText(nameof(TriggerVolume))]
        public TriggerVolume TriggerVolume { get; private set; }

        #region Unity Messages

        protected override void Reset()
        {
            myTransform = GetComponent<Transform>();
            Collider = GetComponent<Collider2D>();
            TriggerVolume = GetComponent<TriggerVolume>();
        }

        // Update is called once per frame
        private void Update()
        {
            // TODO - move with rigidbody instead of translation
            myTransform.position += myTransform.forward
                * (Time.deltaTime * speed);
        }

        #endregion Unity Messages

        /// <summary>
        /// Returns a <see cref="UniTask"/> that completes when this impacts something,
        /// the <paramref name="lifetime"/> expires, or the <see cref="Component.gameObject"/> is destroyed.
        /// </summary>
        public UniTask GetImpactOrLifetimeOrDestroyAwaiter(float lifetime)
        {
            var cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());
            var onImpactHandler = TriggerVolume.OnEnterEvent.GetAsyncEventHandler(cts.Token);
            UniTask lifetimeTask = UniTask.Delay(TimeSpan.FromSeconds(lifetime), cancellationToken: cts.Token);
            UniTask awaiter = UniTask.WhenAny(onImpactHandler.OnInvokeAsync(), lifetimeTask); // return value
            return awaiter.ContinueWith(() =>
            {
                cts.Cancel();
                cts.Dispose();
                onImpactHandler.Dispose();
            });

            //return awaiter;
        }
    }
}
