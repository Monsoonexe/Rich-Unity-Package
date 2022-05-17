using UnityEngine;

namespace RichPackage.ProjectileSystem
{
    public interface ILaunchable
    {
        void Launch(Transform hardPoint);
    }
}
