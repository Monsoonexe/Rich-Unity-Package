using UnityEngine;
using Sirenix.OdinInspector;
using NaughtyAttributes;

using HideIf = Sirenix.OdinInspector.HideIfAttribute;
using ShowIf = Sirenix.OdinInspector.ShowIfAttribute;

/// <seealso cref="LookAtCamera"/>
public class LookAtTransform : RichMonoBehaviour
{
    [SerializeField]
    private bool dynamicallyAssign = false;

    [SerializeField, HideIf("@dynamicallyAssign")]
    private Transform target;

    [SerializeField, Tag, ShowIf("@dynamicallyAssign")]
    private string findByTag = "Player";

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
