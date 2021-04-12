using UnityEngine;

/// <summary>
/// Provides helpful functions to make spawning GameObjects easier.
/// </summary>
public class GameObjectSpawner : RichMonoBehaviour
{
    [Header("---Resources---")]
    public GameObject spawnable;

    [Tooltip("The Transform that should hold spawned items " +
        "[can be null].")]
    public Transform spawnableParent;

    public void SetSpawnablePrefab(GameObject newPrefab)
        => spawnable = newPrefab;

    public Transform SpawnNew() => SpawnNew(spawnable);

    public Transform SpawnNew(GameObject prefab)
    {
        var newInstance = Instantiate(prefab);
        var newITransform = newInstance.GetComponent<Transform>();
        newITransform.SetParent(spawnableParent);

        return newITransform;
    }

    public Transform SpawnNewHere()
        => SpawnAtPlace(myTransform);

    public Transform SpawnNewHere(GameObject prefab)
    {
        var place = myTransform;
        var newObjTrans = SpawnNew(prefab);
        newObjTrans.position = place.position;
        newObjTrans.rotation = place.rotation;
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
