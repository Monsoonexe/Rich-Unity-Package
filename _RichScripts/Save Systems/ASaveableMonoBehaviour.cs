using System;
using System.Diagnostics;
using UnityEngine;
using Sirenix.OdinInspector;
using Signals;
using RichPackage.SaveSystem.Signals;
using Debug = UnityEngine.Debug;

/*	//example derived class
 * 
 * public class PlotController : ASaveableMonoBehaviour<PlotController.PlotSaveData>
	{
		[Serializable]
		public class PlotSaveData : AState
		{
			public int buildIndex;
			public Plot plotData;

			#region Constructors
			public PlotSaveData() 
			{
				buildIndex = 0;
				plotData = null;
			}

			public PlotSaveData(int buildIndex)
			{
				this.buildIndex = buildIndex;
				this.plotData = null;
			}

			public PlotSaveData(int buildIndex, Plot plotData)
			{
				this.buildIndex = buildIndex;
				this.plotData = plotData;
			}
			#endregion
		}

		public override void LoadState(ES3File file)
		{
			base.LoadState(file);
			//update model to reflect data
		}

 * 
 */

namespace RichPackage.SaveSystem
{
	/// <summary>
	/// Base class with everything you need to save some persistent data.
	/// All inheritors should derive save data from <see cref="AState"/>.
	/// </summary>
	/// <seealso cref="SaveSystem"/>
	public abstract class ASaveableMonoBehaviour<TState> : ASaveableMonoBehaviour
		where TState : ASaveableMonoBehaviour.AState, new()
	{	
		[SerializeField]
		private TState saveData = new TState();

		public TState SaveData => saveData; //readonly property

		#region ISaveable Implementation

		/// <summary>
		/// A persistent, unique string identifier.
		/// </summary>
		[ShowInInspector]
		[PropertyOrder(-1)]
		[DelayedProperty]
		[Title("Saving")]
		[CustomContextMenu("Set to Name", "SetDefaultSaveID")]
		[CustomContextMenu("Set to Scene-Name", "SetSaveIDToScene_Name")]
		[CustomContextMenu("Set to GUID", "SetSaveIDToGUID")]
		[CustomContextMenu("Complain if not unique", "Editor_PrintIDIsNotUnique")]
		[ValidateInput("@IsSaveIDUnique(this)", "ID collision. Regenerate.", InfoMessageType.Warning)]
		public override string SaveID 
		{ 
			get => saveData.saveID; 
			protected set => saveData.saveID = value;
		}

		/// <summary>
		/// Saves <see cref="AState"/> to saveFile.
		/// </summary>
		public override void SaveState(ES3File saveFile)
		{    //recommended code
			if (saveData.IsDirty)
				saveFile.Save(SaveID, saveData);
			saveData.IsDirty = false;
		}

		/// <summary>
		/// Loads <see cref="AState"/> state from saveFile.
		/// </summary>
		public override void LoadState(ES3File saveFile)
		{    //recommended code
			if (saveFile.KeyExists(SaveID))
				saveFile.LoadInto(SaveID, SaveData);
			SaveData.IsDirty = false;
		}

		#endregion
	}

	/// <summary>
	/// Base class for all things with default saving behaviour that responds to events.
	/// </summary>
	public abstract class ASaveableMonoBehaviour : RichMonoBehaviour,
		ISaveable, IEquatable<ISaveable>
	{
		/// <summary>
		/// Contains all the data that needs to be saved
		/// </summary>
		[Serializable]
		public abstract class AState : IEquatable<AState>
		{
			public AState()
			{
				saveID = string.Empty;
				IsDirty = false;
			}

			[HideInInspector]
			[ES3NonSerializable] //don't save it to file
			[Tooltip("Must be unique to all other saveables!")]
			public string saveID = string.Empty;

			//more fields....

			/// <summary>
			/// Set this flag to true to indicate that this has new data to save.
			/// </summary>
			[ES3NonSerializable] //don't save it to file
			[ShowInInspector, ReadOnly]
			[PropertyTooltip("True when the data has changed and this needs to be saved.")]
			public virtual bool IsDirty { get; set; } = false;

			public bool Equals(AState other) => this.saveID == other.saveID;
		}

		protected virtual void Reset()
		{
			SetDefaultSaveID();
		}

		protected void OnValidate()
		{
			Editor_PrintIDIsNotUnique();
		}

		protected virtual void OnEnable()
		{
			//subscribe to save events
			GlobalSignals.Get<SaveStateToFile>().AddListener(SaveState);
			GlobalSignals.Get<LoadStateFromFile>().AddListener(LoadState);
		}

		protected virtual void OnDisable()
		{
			//ubsubscribe from save events
			GlobalSignals.Get<SaveStateToFile>().RemoveListener(SaveState);
			GlobalSignals.Get<LoadStateFromFile>().RemoveListener(LoadState);
		}

		#region ISaveable Implementation

		public abstract string SaveID { get; protected set; }

		/// <summary>
		/// Saves <see cref="AState"/> to saveFile.
		/// </summary>
		public abstract void SaveState(ES3File saveFile);

		/// <summary>
		/// Loads <see cref="AState"/> state from saveFile.
		/// </summary>
		public abstract void LoadState(ES3File saveFile);

		/// <summary>
		/// Erases save data from file.
		/// </summary>
		public void DeleteState(ES3File saveFile) => saveFile.DeleteKey(SaveID);

		/// <summary>
		/// Saveables are equal if their data will be saved to the same entry (has same key).
		/// </summary>
		/// <returns>True if saves to either objects will write to the same entry (key collision).</returns>
		public bool Equals(ISaveable other) => this.SaveID == other.SaveID;

		#endregion

		public static bool IsSaveIDUnique(ASaveableMonoBehaviour query)
			=> IsSaveIDUnique(query, out _);

		public static bool IsSaveIDUnique(ASaveableMonoBehaviour query, 
			out ASaveableMonoBehaviour other)
		{
			//TODO - cache this cuz it's terribly slow
			var allSaveables = FindObjectsOfType<ASaveableMonoBehaviour>();
			bool isUnique = true; //return value
			other = default;
			foreach (var saveable in allSaveables)
			{
				if (saveable.Equals(query) //matching keys
					&& saveable != query) //check is not self
				{
					isUnique = false;
					other = saveable;
					break;
				}
			}
			return isUnique;
		}

		[HorizontalGroup("SetGUID"), Button("Complain if ID taken")]
		[Conditional(ConstStrings.UNITY_EDITOR)]
		public void Editor_PrintIDIsNotUnique()
		{
			if (!IsSaveIDUnique(this, out var other))
			{
				Debug.LogWarning($"{nameof(SaveID)} name collision! The uniqueID <{SaveID}> " +
					$"is already taken by \"{other.name}\". " +
					$"{Environment.NewLine} This means you might encounter " +
					$"problems when attempting to save data with this key.", other);
			}
		}

		#region Editor: Set Save ID Helper Functions

		public virtual void SetDefaultSaveID()
		{
			SaveID = gameObject.name;
			Editor_PrintIDIsNotUnique();
		}

		public void SetSaveIDToGUID()
		{
			SaveID = Guid.NewGuid().ToString();
			Editor_PrintIDIsNotUnique();
		}

		public void SetSaveIDToScene_Name()
		{
			SaveID =  gameObject.GetNameWithScene();
			Editor_PrintIDIsNotUnique();
		}

		#endregion
	}
}
