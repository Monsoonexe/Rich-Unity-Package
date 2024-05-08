using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Spawning
{
    /// <summary>
    /// Provides helpful functions to make spawning GameObjects easier.
    /// </summary>
    public sealed class GameObjectSpawner : RichMonoBehaviour
    {
        [Title("Resources")]
		[Required, PreviewField(Alignment = ObjectFieldAlignment.Center, Height = 150f)]
        public GameObject spawnable;

		[Title("Settings")]
        [Tooltip("The Transform that should hold spawned items " +
            "[can be null]."), SceneObjectsOnly]
        public Transform spawnableParent;

		[Tooltip("A name to give spawned object. Null or empty will ignore this behavior.")]
        public string SpawneeNameOverride = null;

		#region Unity Messages

		protected override void Reset()
		{
            SetDevDescription("Provides helpful functions to make spawning GameObjects easier.");
		}

		#endregion Unity Messages

		public void SetSpawnablePrefab(GameObject newPrefab)
            => spawnable = newPrefab;

        /// <summary>
        /// Spawn a new instance at World Origin.
        /// </summary>
        public Transform SpawnNew() => SpawnNew(spawnable);

        /// <summary>
        /// Spawn a new instance at World Origin.
        /// </summary>
        public Transform SpawnNew(GameObject prefab)
        {
            var newInstance = Instantiate(prefab);
            var newITransform = newInstance.GetComponent<Transform>();

            // set parent
            if (spawnableParent != null)
                newITransform.SetParent(spawnableParent);

            // set name
            if (!string.IsNullOrWhiteSpace(SpawneeNameOverride))
                newInstance.name = SpawneeNameOverride;

            return newITransform;
        }

        /// <summary>
        /// Spawn a new instance at World Origin.
        /// </summary>
        public Transform SpawnNew(GameObject prefab, Vector3 position)
        {
            var newInstance = Instantiate(prefab, position, Quaternion.identity);
            var newITransform = newInstance.GetComponent<Transform>();

            // set parent
            if (spawnableParent != null)
                newITransform.SetParent(spawnableParent);

            // set name
            if (!string.IsNullOrWhiteSpace(SpawneeNameOverride))
                newInstance.name = SpawneeNameOverride;

            return newITransform;
        }

        /// <summary>
        /// Spawn a new instance at World Origin.
        /// </summary>
        public Transform SpawnNew(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            var newInstance = Instantiate(prefab, position, rotation);
            var newITransform = newInstance.GetComponent<Transform>();

            // set parent
            if (spawnableParent != null)
                newITransform.SetParent(spawnableParent);

            // set name
            if (!string.IsNullOrWhiteSpace(SpawneeNameOverride))
                newInstance.name = SpawneeNameOverride;

            return newITransform;
        }

        /// <summary>
        /// Spawn at this Spawner's location.
        /// </summary>
        /// <returns></returns>
        public Transform SpawnNewHere()
            => SpawnAtPlace(myTransform);

        public Transform SpawnNewHere(GameObject prefab)
        {
            var newObjTrans = SpawnNew(prefab, myTransform.position, myTransform.rotation);
            return newObjTrans;
        }

        /// <summary>
        /// Spawn a new thing at given rot and pos.
        /// </summary>
        /// <param name="place"></param>
        /// <returns>Handle to newly created obj.</returns>
        public Transform SpawnAtPlace(Transform place)
        {
            var newObjTrans = SpawnNew(spawnable);
            newObjTrans.position = place.position;
            newObjTrans.rotation = place.rotation;
            
            return newObjTrans;
        }

        /// <summary>
        /// Spawn a new instance at point with Origin rotation.
        /// </summary>
        public Transform SpawnAtPoint(Vector3 point)
        {
            var newObjTrans = SpawnNew(spawnable);
            newObjTrans.position = point;

            return newObjTrans;
        }

        #region Event / Delegate Wrappers

        /* These functions wrap the preceding functions so they
        * can be used as event/delegate callbacks 
        * (void return types).
        */

        public void DoSpawnNew() 
            => SpawnNew(spawnable);

        public void DoSpawnNew(GameObject prefab)
            => SpawnNew();

        [Button, DisableInEditorMode]
        public void DoSpawnNewHere()
            => SpawnNewHere();

        public void DoSpawnNewHere(GameObject prefab)
            => SpawnNewHere(prefab);

        public void DoSpawnAtPlace(Transform place)
            => SpawnAtPlace(place);

        public void DoSpawnAtPoint(Vector3 point)
            => SpawnAtPoint(point);

        #endregion
    }
}
