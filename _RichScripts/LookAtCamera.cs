using UnityEngine;

/// <summary>
/// Don't ask questions, just face the MainCamera while active.
/// </summary>
public class LookAtCamera : RichMonoBehaviour
{
    private static Transform mainCamTransform;

    private void Start()
    {
        if (!mainCamTransform)
            mainCamTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }
    private void Update()
    {
        transform.LookAt(mainCamTransform);
    }
}
