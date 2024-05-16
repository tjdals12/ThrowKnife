using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static Vector3 GetPositionFromAngle(float angle, float radius) {
        Vector3 position = Vector3.zero;
        position.x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        position.y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        return position;
    }
}
