using System.Collections.Generic;
using UnityEditor;
using GirooLib.Math;
using UnityEngine;

namespace Giroo.Utility
{
    public class GirooDebug
    {
        public class GirooDebugHelper : MonoBehaviour
        {
            public class TextInfo
            {
                public string text;
                public int fontSize;
                public Vector3 pos;
                public Color color;
            }
            public readonly List<TextInfo> textInfo = new List<TextInfo>(32);

            readonly GUIStyle style = new GUIStyle();

            void OnDrawGizmos()
            {
#if UNITY_EDITOR
                foreach (var text in textInfo)
                {
                    style.normal.textColor = text.color;
                    style.fontSize = text.fontSize;
                    style.fontStyle = FontStyle.Bold;
                    UnityEditor.Handles.Label(text.pos, text.text, style);
                }
#endif
                textInfo.Clear();
            }
        }

        static GirooDebugHelper helper;

        static GirooDebugHelper Helper
        {
            get
            {
                if (helper == null)
                {
                    helper = new GameObject("Giroo Debug Helper").AddComponent<GirooDebugHelper>();
                    GameObject.DontDestroyOnLoad(helper.gameObject);
                }

                return helper;
            }
        }


        public static void DrawText(string text, Vector3 pos, Color color, int fontSize = 16)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
            Helper.textInfo.Add(new GirooDebugHelper.TextInfo
            {
                text = text,
                color = color,
                pos = pos,
                fontSize = fontSize
            });
#endif

        }
        
        public static void Log(params object[] args)
        {
            Debug.Log(string.Join(", ", args));
        }

        public static void DrawCross(Vector3 pos, float radius, Color color,float duration = 0)
        {
            Vector3 p1 = pos + Vector3.left * radius;
            Vector3 p2 = pos + Vector3.right * radius;
            Vector3 p3 = pos + Vector3.up * radius;
            Vector3 p4 = pos + Vector3.down * radius;
            Vector3 p5 = pos + Vector3.forward * radius;
            Vector3 p6 = pos + Vector3.back * radius;
            
            Debug.DrawLine(p1,p2,color,duration);
            Debug.DrawLine(p3,p4,color,duration);
            Debug.DrawLine(p5,p6,color,duration);
        }
        
        public static void DrawFieldOfView(Vector3 pos, Vector3 forwardNormalized, float visionHalfAngleDeg, float offset, float range, Color color, float duration = 0, int nSteps = 10)
        {
            Vector3 circleCenter = pos - forwardNormalized * offset;
            float stepLength = (visionHalfAngleDeg * 2.0f) / nSteps;
            for (int i = 0; i <= nSteps; i++)
            {
                float angle = i * stepLength;
                Quaternion rot = Quaternion.Euler(0,angle - visionHalfAngleDeg,0);
                Vector3 radius = (rot * forwardNormalized).normalized;
            
                Vector3 p1 = circleCenter + radius * offset;
                Vector3 p2 = circleCenter + radius * (offset + range);
                Debug.DrawLine(p1,p2,color,duration);
            }
        }
        
        public static void DrawArch(Vector3 pos, Vector3 forwardNormalized, float halfAngle, float angleOffset, float range, Color color, float duration = 0, int nSteps = 10)
        {
            Vector3 circleCenter = pos;
            float stepLength = (halfAngle * 2.0f) / nSteps;
            for (int i = 0; i <= nSteps; i++)
            {
                float angle = angleOffset + i * stepLength;
                Quaternion rot = Quaternion.Euler(0,angle - halfAngle,0);
                Vector3 radius = (rot * forwardNormalized).normalized;

                Vector3 p = circleCenter + radius * range;
                Debug.DrawLine(circleCenter,p,color,duration);
            }
        }

        static readonly Vector3[] CubeLocalPoints = new[]
        {
            new Vector3(0.5f,0.5f,0.5f), 
            new Vector3(-0.5f,0.5f,0.5f), 
            new Vector3(-0.5f,0.5f,-0.5f), 
            new Vector3(0.5f,0.5f,-0.5f), 
            
            new Vector3(0.5f,-0.5f,0.5f), 
            new Vector3(-0.5f,-0.5f,0.5f), 
            new Vector3(-0.5f,-0.5f,-0.5f), 
            new Vector3(0.5f,-0.5f,-0.5f),  
        };

        static readonly Vector3[] Vec3Buffer = new Vector3[100];
        
        
        public static void DrawCube(Vector3 center, Vector3 size, Quaternion rot, Color color, float duration = 0)
        {
            Matrix4x4 tMat = Matrix4x4.TRS(center, rot, size);

            int n = 0;
            Vec3Buffer[n++] = tMat.MultiplyPoint(CubeLocalPoints[0]);
            Vec3Buffer[n++] = tMat.MultiplyPoint(CubeLocalPoints[1]);
            Vec3Buffer[n++] = tMat.MultiplyPoint(CubeLocalPoints[2]);
            Vec3Buffer[n++] = tMat.MultiplyPoint(CubeLocalPoints[3]);

            Vec3Buffer[n++] = tMat.MultiplyPoint(CubeLocalPoints[4]);
            Vec3Buffer[n++] = tMat.MultiplyPoint(CubeLocalPoints[5]);
            Vec3Buffer[n++] = tMat.MultiplyPoint(CubeLocalPoints[6]);
            Vec3Buffer[n++] = tMat.MultiplyPoint(CubeLocalPoints[7]);
            
            Debug.DrawLine(Vec3Buffer[0],Vec3Buffer[1],color,duration);
            Debug.DrawLine(Vec3Buffer[1],Vec3Buffer[2],color,duration);
            Debug.DrawLine(Vec3Buffer[2],Vec3Buffer[3],color,duration);
            Debug.DrawLine(Vec3Buffer[3],Vec3Buffer[0],color,duration);
            
            Debug.DrawLine(Vec3Buffer[4],Vec3Buffer[5],color,duration);
            Debug.DrawLine(Vec3Buffer[5],Vec3Buffer[6],color,duration);
            Debug.DrawLine(Vec3Buffer[6],Vec3Buffer[7],color,duration);
            Debug.DrawLine(Vec3Buffer[7],Vec3Buffer[4],color,duration);

            Debug.DrawLine(Vec3Buffer[0],Vec3Buffer[4],color,duration);
            Debug.DrawLine(Vec3Buffer[1],Vec3Buffer[5],color,duration);
            Debug.DrawLine(Vec3Buffer[2],Vec3Buffer[6],color,duration);
            Debug.DrawLine(Vec3Buffer[3],Vec3Buffer[7],color,duration);
                
        }
        
        
        
        public static void DrawCircleOnPlane(Vector3 center, Vector3 planeNormal, float radius, Color color, float duration = 0,
            int nSteps = 30)
        {
            float stepAngle = 360.0f / nSteps;
            Vector3 radiusDir = Vec3Math.GetOrthogonal(planeNormal).normalized;
            
            for (int i = 0; i < nSteps; i++)
            {
                Quaternion rot1 = Quaternion.AngleAxis(stepAngle * i,planeNormal);
                Quaternion rot2 = Quaternion.AngleAxis(stepAngle * (i + 1),planeNormal);
	
                Vector3 pos = center + rot1 * radiusDir * radius;
                Vector3 pos2 = center + rot2 * radiusDir * radius;
				
                Debug.DrawLine(pos,pos2,color,duration);
            }
        }
            
        public static void DrawCircleOnXY(Vector3 center, float radius,Color color, float duration = 0,int nSteps = 30)
        {
            DrawCircleOnPlane(center,Vector3.forward,radius,color,duration,nSteps);
        }
        
        public static void DrawCircleOnXZ(Vector3 center, float radius, Color color, float duration = 0, int nSteps = 30)
        {
            DrawCircleOnPlane(center,Vector3.up,radius,color,duration,nSteps);
        }
        
        public static void DrawCircleOnYZ(Vector3 center, float radius, Color color, float duration = 0, int nSteps = 30)
        {
            DrawCircleOnPlane(center,Vector3.right,radius,color,duration,nSteps);
        }

        public static void DrawSphere(Vector3 center, float radius, Color color, float duration = 0, int nSteps = 15)
        {
            DrawCircleOnXY(center,radius,color,duration,nSteps);
            DrawCircleOnXZ(center,radius,color,duration,nSteps);
            DrawCircleOnYZ(center,radius,color,duration,nSteps);
        }
   
    }
}