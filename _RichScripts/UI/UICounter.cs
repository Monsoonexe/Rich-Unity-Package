﻿using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

/// <summary>
/// Shows up to MAX icons. If VALUE > MAX, show a single icon
/// and an additional UI element (e.g. X5 lives). 
/// Like the life counter in Mario.
/// Pairs well with LayoutGroups.
/// </summary>
/// <seealso cref="SpawnCounter"/>
[RequireComponent(typeof(GameObjectPool))]
public class UICounter : VariableUIElement<IntVariable>
{
	/// <summary>
	/// If 'value' is greater than this, only spawn 1 
	/// and enable UI element.
	/// </summary>
	[Title("Options")]
	[Min(1)]
	[Tooltip("If 'value' is greater than this, only spawn 1 and enable UI element.")]
	public int maxSpawns = 5;

	public string prefix = "X";
	public string suffix = " lives";

	[Title("Scene Refs")]
	[SerializeField, Required]
	private TextMeshProUGUI textReadout;

	//member Components
	[SerializeField, Required]
	private GameObjectPool objectPool;

	private void Reset()
	{
		SetDevDescription("Shows up to MAX icons. If VALUE > MAX, show a single icon. " +
			"and an additional UI element (e.g. X5 lives). " +
			"Like the life counter in Mario.");

		objectPool = GetComponent<GameObjectPool>();

		//configure objectPool
		objectPool.initOnAwake = false; //I will init you.
		objectPool.startingAmount = maxSpawns; //spawn all at once
		objectPool.maxAmount = maxSpawns;//

#if UNITY_EDITOR
		UnityEditor.EditorUtility.SetDirty(gameObject);
#endif
	}

	protected override void Awake()
	{
		base.Awake();

		//configure objectPool
		objectPool.InitPool(maxSpawns, maxSpawns);
	}

	public override void UpdateUI()
	{
		//which display method?
		if (targetData.Value > maxSpawns)
		{   
			//setup text element like: X5 lives
			textReadout.text = CommunityStringBuilder.Instance
				.Append(prefix)
				.Append(ConstStrings.GetCachedString(targetData))
				.Append(suffix)
				.ToString();

			//make change from AAAAAAAAA to A x5 lives
			if (objectPool.InUseCount != 1) //only need to do this once 
			{
				objectPool.ReturnAllToPool();
				//show a single item
				objectPool.Depool();
				textReadout.enabled = true;
			}
		}
		else //standard method
		{
			textReadout.enabled = false;
			//need to create more
			while (targetData.Value > objectPool.InUseCount)
			{
				var item = objectPool.Depool();//(spawn)
				if (item == null) break;//pool is exhausted
			}

			//have too many
			while (targetData.Value < objectPool.InUseCount)
			{
				//local function to avoid lambda
				bool IsGameObjectActive(GameObject obj)
					=> obj.activeSelf == true;

				//look for an active item
				var itemInUse = objectPool.Manifest.Find(IsGameObjectActive);
				if (itemInUse == null) break;//break if not found
				objectPool.Enpool(itemInUse);//return to pool (despawn)
			}
		}
	}
}