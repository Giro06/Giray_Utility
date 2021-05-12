

using UnityEngine;

namespace Giroo.Utility
{
    public static class FreeThrow
    {
        public static ThrowData CalculateThrowData(Vector3 objectPosition,Vector3 targetPosition,float h,float gravity)
        {
            h = Mathf.Clamp(h, targetPosition.y, Mathf.Infinity);
            float displacementY = targetPosition.y - objectPosition.y;
            Vector3 displacementXZ = new Vector3(targetPosition.x - objectPosition.x, 0, targetPosition.z - objectPosition.z);
            float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h);
            Vector3 velocityXZ = displacementXZ / time;
            return new ThrowData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
        }

        public static void DrawThrowPath(ThrowData throwData,Vector3 objectPos)
        {
            Vector3 previousDrawPoint = objectPos;
            int resoulution = 30;
        
            for (int i = 0; i <= resoulution; i++)
            {
                float gravity = Physics.gravity.y;
                float simulationTime = i / (float) resoulution * throwData.timeToTarget;
                Vector3 displacement = throwData.initialVelocity * simulationTime + Physics.gravity * simulationTime * simulationTime / 2f;
                Vector3 drawPoint = objectPos + displacement;
                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green);
                previousDrawPoint = drawPoint;
            }
        }
    }
    
    public struct ThrowData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public ThrowData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }
}