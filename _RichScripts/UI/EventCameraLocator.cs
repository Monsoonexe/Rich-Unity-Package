using UnityEngine;

/// <summary>
/// Quick and dirty script to automatically link the Main camera to a World Canvas'
/// world camera (event camera) property. Replace if there's a better Controller on this
/// object that can do this.
/// </summary>
public class EventCameraLocator : RichMonoBehaviour
{
	[Tooltip("[Optional] Camera to assign to canvas.worldCamera.")]
	public Camera worldEventCamera;

	[Tooltip("[Optional] Canvas which needs an event camera.")]
	public Canvas canvas;

    private void Awake()
    {
    	if(canvas == null)
    		canvas = GetComponent<Canvas>();

        canvas.worldCamera = worldEventCamera != null ? 
        	worldEventCamera : GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Destroy(this);//my job is done!
    }
}
