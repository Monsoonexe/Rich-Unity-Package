using UnityEngine;

public class DestroyLifetime : RichMonoBehaviour
{
    public float lifeTime = 10;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
