using UnityEngine;

namespace RichPackage.UI
{
	/// <summary>
	/// Quick and dirty script to automatically link the Main camera to a World Canvas's
	/// world camera (event camera) property. Replace if there's a better controller on this
	/// object that can do this.
	/// </summary>
	public class EventCameraLocator : RichMonoBehaviour
	{
		[Tooltip("[Optional] Camera to assign to canvas.worldCamera.")]
		public Camera worldEventCamera;

		[Tooltip("[Optional] Canvas which needs an event camera.")]
		public Canvas canvas;

		protected override void Reset()
		{
			base.Reset();
			SetDevDescription("Quick and dirty script to automatically " +
				"link the Main camera to a World Canvas's " + 
				"world camera(event camera) property. " +
				"Replace if there's a better controller on this " + 
				"object that can do this.");

			//set defaults
			if (canvas == null)
				canvas = GetComponent<Canvas>();

			if(worldEventCamera == null)
				worldEventCamera = GameObject.FindGameObjectWithTag(
					"MainCamera").GetComponent<Camera>();
			
			//assign if you can
			if (canvas != null && canvas.worldCamera == null)
				canvas.worldCamera = worldEventCamera;
		}

		protected void Start()
		{
			if(canvas == null)
				canvas = GetComponent<Canvas>();

			if (worldEventCamera == null)
				worldEventCamera = GameObject.FindGameObjectWithTag(
					"MainCamera").GetComponent<Camera>();

			canvas.worldCamera = worldEventCamera;

			Destroy(this);//my job is done!
		}
	}
}
