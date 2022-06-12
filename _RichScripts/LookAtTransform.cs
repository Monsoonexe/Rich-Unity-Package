using UnityEngine;
using Sirenix.OdinInspector;
using NaughtyAttributes;

using HideIf = Sirenix.OdinInspector.HideIfAttribute;
using ShowIf = Sirenix.OdinInspector.ShowIfAttribute;

namespace RichPackage
{
    /// <seealso cref="LookAtCamera"/>
    public class LookAtTransform : RichMonoBehaviour
    {
        public bool dynamicallyAssign = false;

        [HideIf("@dynamicallyAssign")]
        public Transform target;

        [Tag, ShowIf("@dynamicallyAssign")]
        public string findByTag = GameObjectTags.Player;

        private void LateUpdate()
        {
            if (dynamicallyAssign && target == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag(findByTag);
                target = obj != null ? obj.GetComponent<Transform>() : null;
            }

            if (target != null)
                myTransform.LookAt(target);
        }
    }
}
