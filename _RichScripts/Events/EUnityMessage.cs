using System;

namespace RichPackage.UnityMessages
{
	/// <summary>
	/// 
	/// </summary>
	[Flags]
	public enum EUnityMessage
	{
		None = 1 << 0,
		Start = 1 << 1,
		Awake = 1 << 2,
		OnEnable = 1 << 3,
		OnDisable = 1 << 4,
		OnDestroy = 1 << 5,
		OnApplicationQuit = 1 << 6,
		EarlyUpdate = 1 << 7,
		Update = 1 << 8,
		FixedUpdate = 1 << 9,
		LateUpdate = 1 << 10,
		OnApplicationPause = 1 << 11,
	}
}
