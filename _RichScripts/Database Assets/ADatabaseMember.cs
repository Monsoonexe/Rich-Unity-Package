using UnityEngine;

namespace RichPackage.Databases
{
	/// <summary>
	/// An asset that holds the data and lookup key for an item inside
	/// a <see cref="ADatabaseAsset{TData}"/>.
	/// </summary>
    public abstract class ADatabaseMember<TData> : RichScriptableObject<TData>,
		IDatabaseMember
	{
		[SerializeField, HideInInspector]
		private int key;

		/// <summary>
		/// The lookup key in <see cref="ADatabaseAsset{TData}"/>.
		/// </summary>
		public int Key { get => key; set => key = value; }
	}
}
