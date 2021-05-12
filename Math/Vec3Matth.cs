using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GirooLib.Math
{
    public static class Vec3Math
    {
        public static T FindClosest<T>(Vector3 pos, IEnumerable list) where T : MonoBehaviour
        {
            float closestDistance = float.MaxValue;
            T closest = null;
            foreach (T element in list)
            {
                float distanceSqr = (element.transform.position - pos).sqrMagnitude;
                if (distanceSqr > closestDistance)
                {
                    closest = element;
                    closestDistance = distanceSqr;
                }
            }
            return closest;
        }
        
        public static Vector3 RayToPointAtY(Ray ray, float y)
        {
            return ray.origin + (((ray.origin.y - y) / -ray.direction.y) * ray.direction);
        }

        public static Vector3 RayToPointAtZ(Ray ray, float z)
        {
            return ray.origin + (((ray.origin.z - z) / -ray.direction.z) * ray.direction);
        }

        public static Vector3 RayToPointAtX(Ray ray, float x)
        {
            return  ray.origin +(((ray.origin.x -x) / -ray.direction.x) * ray.direction);
        }
        public static bool IsInsideFieldOfVision(Vector3 pos, Vector3 forwardNormalized,Vector3 targetPos, float visionHalfAngleDeg, float visionStartWidth)
        {
            Vector3 centerToTarget = (targetPos - pos).normalized;
            float frontDot = Vector3.Dot(forwardNormalized, centerToTarget);
            if(frontDot < 0)
            {
                return false;
            }

            Vector3 behindPos = pos - forwardNormalized * visionStartWidth;
            Vector3 behindToTarget = (targetPos - behindPos).normalized;
            float behindDot = Vector3.Dot(forwardNormalized, behindToTarget);
            return behindDot > Mathf.Cos(visionHalfAngleDeg * Mathf.Deg2Rad);
        }
        
        public static Vector3 CalculateBezier(Vector3 startPos, Vector3 startPosHandle,Vector3 endPos, Vector3 endPosHandle, float t)
        {
            return Mathf.Pow((1 - t), 3) * startPos 
                   + 3 * Mathf.Pow(1 - t, 2) * t * startPosHandle
                   + 3 * (1 - t) * t * t * endPosHandle 
                   + t * t * t * endPos;
        }
        
        public static Vector3 GetClosestPointOnLine(Vector3 point, Vector3 lineP1, Vector3 lineP2)
        {
            // P point
            // AB line

            // A + dot(AP, AB) / dot(AB, AB) * AB

            Vector3 p1p2 = lineP2 - lineP1;
            Vector3 lineP1Point = point - lineP1;

            return lineP1 + Vector3.Dot(lineP1Point, p1p2) / Vector3.Dot(p1p2, p1p2) * p1p2;
        }
	    
        public static Vector3 GetClosestPointOnLineSegment(Vector3 point, Vector3 lineP1, Vector3 lineP2)
        {
            Vector3 projection = GetClosestPointOnLine(point, lineP1, lineP2);
            bool between = Vector3.Dot(lineP1 - projection, lineP2 - projection) < 0;
            if (between)
            {
                return projection;
            }

            float disP1 = (point - lineP1).sqrMagnitude;
            float disP2 = (point - lineP2).sqrMagnitude;

            if (disP1 < disP2)
            {
                return lineP1;

            }
            else
            {
                return lineP2;
            }
        }

        public static Vector3 GetClosestPointOnPath(Vector3 point, IList<Vector3> path, out int closestPointIdx)
        {
            float closestDistance = float.MaxValue;
            Vector3 closestPoint = Vector3.zero;
            closestPointIdx = -1;
            
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 p1 = path[i];
                Vector3 p2 = path[i + 1];

                Vector3 p = GetClosestPointOnLineSegment(point, p1, p2);

                float distance = (p - point).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPointIdx = i;
                    closestPoint = p;
                }
            }

            return closestPoint;
        }

        public static Vector3 GetOrthogonal(Vector3 vec)
        {
            // return  c<a  ? (b,-a,0) : (0,-c,b) 

            if (vec.z < vec.x)
            {
                return new Vector3(vec.y,-vec.x,0);
            }
            else
            {
                return new Vector3(0,-vec.z,vec.y);
            }
        }
        
    }

}