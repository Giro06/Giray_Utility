using System.Security.Cryptography;
using UnityEngine;

namespace Giroo.Utility
{
    public static class Bezier
    {
        public static Vector3 GetBezierPointV3(Vector3 p0, Vector3 m0, Vector3 p1, float normalizedTime)
        {
            Vector3 l0 = Vector3.Lerp(p0, m0, normalizedTime);
            Vector3 l1 = Vector3.Lerp(m0, p1, normalizedTime);
            Vector3 f0 = Vector3.Lerp(l0, l1, normalizedTime);
            return f0;
        }
        public static Vector3 GetBezierPointV3(Vector3 p0, Vector3 m0,Vector3 m1, Vector3 p1, float normalizedTime)
        {
            Vector3 l0 = Vector3.Lerp(p0, m0, normalizedTime);
            Vector3 l1 = Vector3.Lerp(m0, m1, normalizedTime);
            Vector3 l2 = Vector3.Lerp(m1, p1, normalizedTime);
            Vector3 l3 = Vector3.Lerp(l0, l1, normalizedTime);
            Vector3 l4 = Vector3.Lerp(l1, l2, normalizedTime);
            Vector3 f0 = Vector3.Lerp(l3, l4, normalizedTime);
            return f0;
        }
        public static Vector2 GetBezierPointV2(Vector2 p0, Vector2 m0, Vector2 p1, float normalizedTime)
        {
            Vector2 l0 = Vector2.Lerp(p0, m0, normalizedTime);
            Vector2 l1 = Vector2.Lerp(m0, p1, normalizedTime);
            Vector2 f0 = Vector2.Lerp(l0, l1, normalizedTime);
            return f0;
        }
        public static Vector2 GetBezierPointV2(Vector2 p0, Vector2 m0,Vector2 m1, Vector2 p1, float normalizedTime)
        {
            Vector2 l0 = Vector2.Lerp(p0, m0, normalizedTime);
            Vector2 l1 = Vector2.Lerp(m0,m1, normalizedTime);
            Vector2 l2 = Vector2.Lerp(m1,p1, normalizedTime);

            Vector2 l3 = Vector2.Lerp(l0, l1, normalizedTime);
            Vector2 l4 = Vector2.Lerp(l1,l2,normalizedTime);
            Vector2 f0 = Vector2.Lerp(l3, l4, normalizedTime);
            return f0;
        }
    }
}