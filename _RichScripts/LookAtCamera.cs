using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Don't ask questions, just face the MainCamera while active.
    /// </summary>
    public class LookAtCamera : LookAtTransform
    {
        private void Reset()
        {
            SetDevDescription("Don't ask questions, just face the MainCamera while active.");
            dynamicallyAssign = false;
            findByTag = ConstStrings.TAG_MAIN_CAMERA;
            target = GameObject.FindGameObjectWithTag(findByTag)
                .GetComponent<Transform>();
        }
    }
}
