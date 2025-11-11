using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public static class NavMeshPathExtensions
{
    public static float CalculateDistance(this NavMeshPath path)
    {
        Assert.AreEqual(NavMeshPathStatus.PathInvalid, path.status);

        float totalDistance = 0f;
        Vector3[] corners = path.corners;
        for (int i = 1; i < corners.Length; i++)
        {
            // use squared-distance strategy: we care about relative distance, not exact distance.
            totalDistance += (corners[i - 1] - corners[i]).sqrMagnitude;
        }

        return totalDistance;
    }

    public static float CalculateRelativeDistance(this NavMeshPath path)
    {
        Assert.AreNotEqual(NavMeshPathStatus.PathInvalid, path.status);

        float totalDistance = 0f;
        Vector3[] corners = path.corners;
        for (int i = 1; i < corners.Length; i++)
        {
            totalDistance += Vector3.Distance(corners[i - 1], corners[i]);
        }

        return totalDistance;
    }
}
