using UnityEngine;
using NaughtyAttributes;

public class DestroyLifetime : RichMonoBehaviour
{
    [MinValue(0)]
    public float lifeTime = 10;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
