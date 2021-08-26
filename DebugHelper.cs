using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace  Giroo.Utility
{
    public struct DebugCall
{
    public Action action;

    public DebugCall(Action action)
    {
        this.action = action;
    }
}

public class DebugHelper : MonoBehaviour
{
    public List<DebugCall> debugCalls;
    public static DebugHelper instance;

    public static void CreateInstance(DebugCall debugCall)
    {
        GameObject debugObject = new GameObject("DebugHelper");
        DebugHelper debugHelper = debugObject.AddComponent<DebugHelper>();
        instance = debugHelper;
        instance.debugCalls = new List<DebugCall>();
        instance.debugCalls.Add(debugCall);
    }

    public static void AddDebug(DebugCall debugCall)
    {
        if (DebugHelper.instance != null)
        {
            instance.debugCalls.Add(debugCall);
        }
        else
        {
            DebugHelper.CreateInstance(debugCall);
        }
    }

    public static void DrawWireSphere(Vector3 center, float radius)
    {
        DebugCall debugCall = new DebugCall(() => { Gizmos.DrawWireSphere(center, radius); });
        AddDebug(debugCall);
    }

    public static void DrawWireCube(Vector3 center, Vector3 size)
    {
        DebugCall debugCall = new DebugCall(() => { Gizmos.DrawWireCube(center, size); });
        AddDebug(debugCall);
    }

    public static void DrawCross(Vector3 center, float size)
    {
        DebugCall debugCall = new DebugCall(() =>
        {
            Gizmos.DrawLine(center + Vector3.forward * size / 2, center + Vector3.back * size / 2);
            Gizmos.DrawLine(center + Vector3.left * size / 2, center + Vector3.right * size / 2);
        });
        AddDebug(debugCall);
    }

    public static void DrawCircle(Vector3 center, float radius, int segmentCount = 20)
    {
        DebugCall debugCall = new DebugCall(() =>
        {
            float angle = 360 / segmentCount;

            for (int i = 0; i < (segmentCount + 1); i++)
            {
                float x = Mathf.Sin(Mathf.Deg2Rad * angle * i) * radius;
                float y = Mathf.Cos(Mathf.Deg2Rad * angle * i) * radius;

                float x2 = Mathf.Sin(Mathf.Deg2Rad * angle * (i + 1)) * radius;
                float y2 = Mathf.Cos(Mathf.Deg2Rad * angle * (i + 1)) * radius;


                Gizmos.DrawLine(new Vector3(x, 0, y) + center, new Vector3(x2, 0, y2) + center);
            }
        });
        AddDebug(debugCall);
    }

    private void OnDrawGizmos()
    {
        foreach (var debugCall in debugCalls)
        {
            debugCall.action?.Invoke();
        }

        debugCalls.Clear();
    }
}
}
