using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Sirenix.OdinInspector;
using RichPackage.TriggerVolumes;

//TODO - decouple from Poolable
//decouple from explosion prefab
//filter layers at runtime
//raise CollideEvent with collision info

namespace RichPackage.ProjectileSystem
{
    [RequireComponent(typeof(Collider)),
        RequireComponent(typeof(TriggerVolume))]
    public class Projectile : RichMonoBehaviour // : Poolable
    {
        [Title("Settings")]
		[Min(0)]
        public float forwardSpeed = 50;

		[Title("References")]
		[field: SerializeField, Required, LabelText(nameof(Collider))]
        public Collider Collider { get; private set; }

		[field: SerializeField, Required, LabelText(nameof(TriggerVolume))]
        public TriggerVolume TriggerVolume { get; private set; }

		#region Unity Messages

		protected override void Reset()
		{
            myTransform = GetComponent<Transform>();
            Collider = GetComponent<Collider>();
            TriggerVolume = GetComponent<TriggerVolume>();
		}

		// Update is called once per frame
		private void Update()
        {
            // TODO - move with rigidbody instead of translation
            transform.position += transform.forward 
                * (Time.deltaTime * forwardSpeed);
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
