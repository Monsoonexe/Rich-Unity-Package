using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class with everything you need to save some persistent data.
/// </summary>
public abstract class ASaveableMono : RichMonoBehaviour
{//TODO - favor Composition over inheritance

    [SerializeField]
    [HideInInspector]
    [ContextMenuItem("GetNewUniqueID()", "GetNewUniqueID")]
    private string uniqueID = string.Empty;
    public string SaveID { get => uniqueID; }
    public static Dictionary<string, ASaveableMono> allUniqueIDs
        = new Dictionary<string, ASaveableMono>(30);

    protected override void Awake()
    {
        base.Awake();

        //events
        //SaveSystem.OnLoad += LoadState;
        //SaveSystem.OnSave += SaveState;
    }

    protected virtual void OnDestroy()
    {
        //events
        //SaveSystem.OnLoad -= LoadState;
        //SaveSystem.OnSave -= SaveState;
        allUniqueIDs.Remove(SaveID); //remove because a 'new' mono is created/destroy at every load/unload
    }

    protected virtual void OnValidate()
    {
        GetNewUniqueID();
    }

    [ContextMenu("GetNewUniqueID()")]
    public void GetNewUniqueID()
    {
        if (string.IsNullOrEmpty(uniqueID))
        {
            uniqueID = Guid.NewGuid().ToString();
        }

        while (allUniqueIDs.TryGetValue(uniqueID, out ASaveableMono val)
            && val != this)
        {
            if (val == null)
            {
                allUniqueIDs.Remove(uniqueID);
                break;
            }
            uniqueID = Guid.NewGuid().ToString();
        }

        if (!allUniqueIDs.ContainsKey(uniqueID))
        {
#if UNITY_EDITOR
            //don't save prefabs
            var isPrefabInProject = UnityEditor.PrefabUtility.
                IsPartOfPrefabAsset(gameObject);

            if (!isPrefabInProject)//only save Scene instances, not assets in Project
            {
                allUniqueIDs.Add(uniqueID, this);
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }
    }

    public abstract void SaveState(ES3File saveFile);

    public abstract void LoadState(ES3File saveFile);


#if UNITY_EDITOR
    [ContextMenu("DeleteAllKeys()")]
     public void Editor_DeleteAllSaveIDKeys()
    {
        allUniqueIDs.Clear();
    }
#endif 
}
