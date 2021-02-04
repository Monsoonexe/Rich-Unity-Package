using UnityEngine;

/// <summary>
/// Quick and dirty script to automatically link the Main camera to a World Canvas'
/// world camera (event camera) property. Replace if there's a better Controller on this
/// object that can do this.
/// </summary>
[RequireComponent(typeof(Canvas))]
public class EventCameraLocator : RichMonoBehaviour
{
    private void Start()
    {
        GetComponent<Canvas>().worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Destroy(this);//my job is done!
    }
}
