using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Don't ask questions, just face the MainCamera while active.
    /// </summary>
    public class LookAtCamera : LookAtTransform
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Don't ask questions, just face the MainCamera while active.");
            dynamicallyAssign = false;
            findByTag = GameObjectTags.MainCamera;
            target = GameObject.FindGameObjectWithTag(findByTag)
                .GetComponent<Transform>();
        }
    }
}
