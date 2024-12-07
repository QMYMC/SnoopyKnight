using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ExtensionMethod
{
    public const float dotThreshold = 0.5f;

    public static bool IsFacingTarget(this Transform transform,Transform target)
    {
        var vectorToTarget = target.position-transform.position;
        vectorToTarget.Normalize();

        float dot = Vector3.Dot(transform.forward, vectorToTarget);

        return dot >= dotThreshold; 
    }
}
