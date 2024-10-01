using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public static class Extensions
{
    public static Camera Camera
    {
        get
        {
            if (_cam == null)
                _cam = Camera.main;
            return _cam;
        }
    }
    static Camera _cam; 
    public static Vector3 ToUnsignedEulerAngles(this Quaternion quaternion)
    {
        Vector3 eulerAngles = quaternion.eulerAngles;
        return new Vector3(
            NormalizeAngle(eulerAngles.x),
            NormalizeAngle(eulerAngles.y),
            NormalizeAngle(eulerAngles.z)
        );
    }
    private static float NormalizeAngle(float angle)
    {
        return (angle + 360) % 360;
    }
}