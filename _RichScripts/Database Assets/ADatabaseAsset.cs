using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine;

namespace RichPackage.Databases
{
	/// <summary>
	/// An asset that holds references to <see cref="Object"/>s and can look them up by keys.
	/// Useful for propagating known entities through the network.
	/// </summary>
	/// <typeparam name="TData">The type of data that actually gets held in the database.</typeparam>
    public abstract class ADatabaseAsset<TData> : RichScriptableObject
		where TData : RichScriptableObject, IDatabaseMember
    {
		[Title("Database")]
		[Tooltip("Index all the replay objects here. Be sure their replay entity script has this index set in the inspector!")]
		[SerializeField, AssetsOnly, Required, ListDrawerSettings(NumberOfItemsPerPage = 40,
			ShowIndexLabels = true), LabelWidth(50)]
#if UNITY_EDITOR
		[InfoBox("$" + nameof(editorErrorMessage), InfoMessageType.Error, VisibleIf = nameof(EditorHasError))]
#endif
		protected TData[] items;

		public bool autoAssignKeys = false;

		protected virtual void Reset()
		{
			SetDevDescription($"A database for {typeof(TData).Name}s.");
		}

		public virtual TData Get(int index)
		{
			if (!items.IndexIsInRange(index))
				throw new System.IndexOutOfRangeException($"{index} | {items.Length}");

			return items[index];
		}

		public virtual int GetKey(TData value)
		{
			int index = items.IndexOf(value);

			// complain if couldn't be found
			if (index < 0)
			{
				Debug.LogError($"The item {value} could not be looked up in this database.", this);
				throw new System.Collections.Generic.KeyNotFoundException();
			}

			return items[index].Key;
		}

		#region Editor
#if UNITY_EDITOR

		protected string editorErrorMessage;

		protected bool EditorHasError { get => !editorErrorMessage.IsNullOrEmpty(); }

		/// <summary>
		/// Validate database and assign keys.
		/// </summary>
		protected virtual void OnValidate()
		{
			bool hasNull = false;
			editorErrorMessage = null;

			// assign indices
			foreach (var (Item, Index) in items.ForEachWithIndex())
			{
				if (Item == null)
				{
					Debug.LogError(editorErrorMessage = $"Entity {Index} is null, and it should not be. " +
						$"Please remove or assign the item.", this);
					hasNull = true;
				}
				else
                {
                    if (autoAssignKeys)
						Item.Key = Index;
				}
			}

			// can't continue if there are null entries
			if (hasNull)
				return;

			// detect duplicates
			var groups = items.GroupBy(x => x.name);
            foreach (var duplicate in groups.Where(g => g.Count() > 1))
                Debug.LogError(editorErrorMessage = $"There are duplicate <{duplicate.Key}> entities in the database. " +
                    $"Please remove them leaving only 1.", this);
        }

#endif
		#endregion Editor
	}
}
