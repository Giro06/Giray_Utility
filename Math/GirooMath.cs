using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GirooLib
{
	public static class GirooMath 
	{
		public static bool IsInsidePolygon(List<Vector2> polygon,Vector2 point)
		{
			// https://stackoverflow.com/questions/217578/how-can-i-determine-whether-a-2d-point-is-within-a-polygon
			float testY = point.y;
			float testX = point.x;
			int nPoints = polygon.Count;
            
			int i, j = 0;
			bool c = false;
			for (i = 0, j = nPoints-1; i < nPoints; j = i++)
			{
				if (((polygon[i].y > testY) != (polygon[j].y > testY)) &&
				    (testX < (polygon[j].x - polygon[i].x) * (testY - polygon[i].y) / (polygon[j].y - polygon[i].y) +
					    polygon[i].x))
				{
					c = !c;
				}
			}
			return c;
		}
			
	    

		public static Vector3 Project(Vector3 vec, Vector3 projectOnNormalized)
		{
			return Vector3.Dot(vec, projectOnNormalized) / 
			       Vector3.Dot(projectOnNormalized, projectOnNormalized) * projectOnNormalized;
		}
		
		public static Vector2 Project(Vector2 vec, Vector2 projectOnNormalized)
		{
			return Vector3.Dot(vec, projectOnNormalized) / 
			       Vector3.Dot(projectOnNormalized, projectOnNormalized) * projectOnNormalized;
		}



	    public static int RepeatIndex(int idx, int arrayLength)
	    {
		    idx = idx % arrayLength;
		    if (idx < 0)
		    {
			    idx += arrayLength;
		    }

		    return idx;
	    }
	    
	    
	    //TODO refactor
	    public static Vector3 Barycentric(Vector3 p, Vector3 triangleA, Vector3 triangleB, Vector3 triangleC)
	    {
			float m1 = (triangleA.x - triangleB.x) / (triangleA.y - triangleB.y);
			float m2 = (triangleC.x - triangleB.x) / (triangleC.y - triangleB.y);
			if(float.IsNaN(m1) || float.IsNaN(m2) || Mathf.Abs(m1 - m2) <= 0.001f)
			{
				return new Vector3(-1, -1, -1);
			}
	        //TODO move check inside rope mover
			

	        Vector3 v0 = triangleB - triangleA;
	        Vector3 v1 = triangleC - triangleA;
	        Vector3 v2 = p - triangleA;
	        float d00 = Vector3.Dot(v0, v0);
	        float d01 = Vector3.Dot(v0, v1);
	        float d11 = Vector3.Dot(v1, v1);
	        float d20 = Vector3.Dot(v2, v0);
	        float d21 = Vector3.Dot(v2, v1);
	        float denom = d00 * d11 - d01 * d01;

			if(Mathf.Abs(denom) <= float.Epsilon)
			{
			}

	        float v = (d11 * d20 - d01 * d21) / denom;
	        float w = (d00 * d21 - d01 * d20) / denom;
	        float u = 1.0f - v - w;
	        return new Vector3(v, w, u);
	    }

		public static bool DoesTriangleContains(Vector2 triangleA, Vector2 triangleB, Vector2 triangleC, Vector2 point)
	    {
			//if (AreColinear(triangleA, triangleB, triangleC)) return false;

			float s1 = Vector3.Cross(triangleB - triangleA, point - triangleA).z;
			float s2 = Vector3.Cross(triangleC - triangleB, point - triangleB).z;
			float s3 = Vector3.Cross(triangleA - triangleC, point - triangleC).z;

	        
			if(
				(s1 < 0 || Mathf.Abs(s1) < Mathf.Epsilon) && 
				(s2 < 0 || Mathf.Abs(s2) < Mathf.Epsilon) &&
	            (s3 < 0 || Mathf.Abs(s3) < Mathf.Epsilon)
			  )
			{
				return true;
			}

			if (
	            (s1 > 0 || Mathf.Abs(s1) < Mathf.Epsilon) &&
	            (s2 > 0 || Mathf.Abs(s2) < Mathf.Epsilon) &&
	            (s3 > 0 || Mathf.Abs(s3) < Mathf.Epsilon)
	          )
	        {
	            return true;
	        }

			return false;
	        







			//Vector2 center = (triangleA + triangleB + triangleC) / 2;
	        
	  //      Vector3 bary = Barycentric(point, triangleA, triangleB, triangleC);

			//const float min = 0;
			//const float max = 1;

			//bool inside = bary.x >= min && bary.x <= max && bary.y >= min && bary.y <= max;// && bary.z >= min && bary.z <= max;

			//Debug.Log(inside +"," + s1 + ","+ s2 + "," + s3);
			//return inside;
	    }

		public static bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
		{
			//To avoid floating point precision issues we can add a small value
			float epsilon = 0.00001f;

			bool isIntersecting = false;

			float denominator = (l2_p2.y - l2_p1.y) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.y - l1_p1.y);

			//Make sure the denominator is > 0, if not the lines are parallel
			if (denominator != 0f)
			{
				float u_a = ((l2_p2.x - l2_p1.x) * (l1_p1.y - l2_p1.y) - (l2_p2.y - l2_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;
				float u_b = ((l1_p2.x - l1_p1.x) * (l1_p1.y - l2_p1.y) - (l1_p2.y - l1_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;

				//Are the line segments intersecting if the end points are the same
				if (shouldIncludeEndPoints)
				{
					//Is intersecting if u_a and u_b are between 0 and 1 or exactly 0 or 1
					if (u_a >= 0f + epsilon && u_a <= 1f - epsilon && u_b >= 0f + epsilon && u_b <= 1f - epsilon)
					{
						isIntersecting = true;
					}
				}
				else
				{
					//Is intersecting if u_a and u_b are between 0 and 1
					if (u_a > 0f + epsilon && u_a < 1f - epsilon && u_b > 0f + epsilon && u_b < 1f - epsilon)
					{
						isIntersecting = true;
					}
				}
			}

			return isIntersecting;
		}

		// the lines p1 --> p2 and p3 --> p4.
		/* http://csharphelper.com/blog/2014/08/determine-where-two-lines-intersect-in-c/ */
		public static void FindLineIntersection(
			Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4,
			out bool lineIntersects, out bool segmentIntersects,
			out Vector2 intersection,
			out Vector2 closeP1, out Vector2 closeP2)
		{
			
			// Get the segments' parameters.
			float dx12 = p2.x - p1.x;
			float dy12 = p2.y - p1.y;
			float dx34 = p4.x - p3.x;
			float dy34 = p4.y - p3.y;

			// Solve for t1 and t2
			float denominator = (dy12 * dx34 - dx12 * dy34);

			float t1 =
				((p1.x - p3.x) * dy34 + (p3.y - p1.y) * dx34)
				/ denominator;
			if (float.IsInfinity(t1))
			{
				// The lines are parallel (or close enough to it).
				lineIntersects = false;
				segmentIntersects = false;
				intersection = new Vector2(float.NaN, float.NaN);
				closeP1 = new Vector2(float.NaN, float.NaN);
				closeP2 = new Vector2(float.NaN, float.NaN);
				return;
			}
			lineIntersects = true;

			float t2 =
				((p3.x - p1.x) * dy12 + (p1.y - p3.y) * dx12)
				/ -denominator;

			// Find the point of intersection.
			intersection = new Vector2(p1.x + dx12 * t1, p1.y + dy12 * t1);

			// The segments intersect if t1 and t2 are between 0 and 1.
			segmentIntersects =
				((t1 >= 0) && (t1 <= 1) &&
				 (t2 >= 0) && (t2 <= 1));

			// Find the closest points on the segments.
			if (t1 < 0)
			{
				t1 = 0;
			}
			else if (t1 > 1)
			{
				t1 = 1;
			}

			if (t2 < 0)
			{
				t2 = 0;
			}
			else if (t2 > 1)
			{
				t2 = 1;
			}

			closeP1 = new Vector2(p1.x + dx12 * t1, p1.y + dy12 * t1);
			closeP2 = new Vector2(p3.x + dx34 * t2, p3.y + dy34 * t2);
		}

		public static bool FindLineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
		{
			bool lineIntersects, segmentIntersects;
			Vector2 closeP1, closeP2;
			FindLineIntersection(p1, p2, p3, p4, out lineIntersects, out segmentIntersects, out intersection, out closeP1,
				out closeP2);
			return segmentIntersects;
		}
		
		public static bool FindLineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
		{
			bool lineIntersects, segmentIntersects;
			Vector2 closeP1, closeP2;
			FindLineIntersection(p1, p2, p3, p4, out lineIntersects, out segmentIntersects, out intersection, out closeP1,
				out closeP2);
			return lineIntersects;
		}
		
		public static bool InsideTriangle(Vector2 A, Vector2 B, Vector2 C, Vector2 P)
	    {
	        float ax, ay, bx, by, cx, cy, apx, apy, bpx, bpy, cpx, cpy;
	        float cCROSSap, bCROSScp, aCROSSbp;

	        ax = C.x - B.x; ay = C.y - B.y;
	        bx = A.x - C.x; by = A.y - C.y;
	        cx = B.x - A.x; cy = B.y - A.y;
	        apx = P.x - A.x; apy = P.y - A.y;
	        bpx = P.x - B.x; bpy = P.y - B.y;
	        cpx = P.x - C.x; cpy = P.y - C.y;

	        aCROSSbp = ax * bpy - ay * bpx;
	        cCROSSap = cx * apy - cy * apx;
	        bCROSScp = bx * cpy - by * cpx;

	        return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
	    }
	    
		public static Vector2 GetNormal(Vector2 lineP1, Vector2 lineP2, Vector2 sideP)
	    {
			Vector2 lineDir = lineP2 - lineP1;
			Vector2 sideDir = sideP - lineP1;
			float side = Mathf.Sign(Vector3.Cross(sideDir, lineDir).z);
	        Quaternion rot = Quaternion.AngleAxis(side * 90, Vector3.forward);

	        Vector2 normal = (rot * lineDir).normalized;
			return normal;
	    }

		public static bool AreColinear(Vector2 p1, Vector2 p2, Vector2 p3)
		{
			// (a,b) , (m,n), and (x,y)
			// (p1.x,p1.y), (p2.x, p2.y), p3.x,p3.y
			//determinant a(n−y) + m(y−b) + x(b−n)

			return Mathf.Abs(p1.x * (p2.y - p3.y) + p2.x * (p3.y - p1.y) + p3.x * (p1.y - p2.y)) < 0.0001f;
		}
	    

		public static float AngleBetweenLineAndPoint(Vector2 lineStart, Vector2 lineEnd, Vector2 point)
		{
			float angle = Vector2.Angle(lineEnd - lineStart, point - lineStart);
			return angle;
		}

		public static Vector2 FixNormal(Vector2 normal, Vector2 surfaceNormal)
	    {
	        if (Vector2.Dot(normal, surfaceNormal) < 0)
	        {
	            return -normal;
	        }
	#if UNITY_EDITOR
	        if (Vector2.Dot(normal, surfaceNormal) < 0)
	        {
	            Debug.LogError("Fix Normal Nor Working!");
	        }
	#endif
	        return normal;

	    }

		public static bool AreSignsSame(float a, float b)
		{
			return (a < 0 && b < 0) || (a > 0 && b > 0) || (a == 0 && b == 0);
		}

		public static float ClockwiseAngle(Vector2 from, Vector2 to)
		{
			float signedAngle = -Vector2.SignedAngle(from, to);
			if (signedAngle < 0)
			{
				// counter clockwise
				signedAngle += 360;
			}
			return signedAngle;
		}
		
		public static List<Vector2> SmoothLine(List<Vector2> points,float radius, float smoothingStepAngle, bool loop = false)
		{
			// https://stackoverflow.com/questions/24771828/algorithm-for-creating-rounded-corners-in-a-polygon
			List<Vector2> newPoints = new List<Vector2>();
			System.Action<Vector2> AddPoint = (newPoint) =>
			{
				if (newPoints.Count != 0)
				{
					if ((newPoints[newPoints.Count - 1] - newPoint).magnitude > float.Epsilon)
					{
						newPoints.Add(newPoint);
					}
				}
				else
				{
					newPoints.Add(newPoint);
				}
			};

			int endIdx = points.Count - 1;
			int startIdx = 0;
			if (!loop)
			{
				endIdx--;
				startIdx++;
				AddPoint(points[0]);
			}
	        for (int i = startIdx; i <= endIdx; i++)
	        {
	            int nIdx = (i + 1) % points.Count;
	            int bIdx = (i - 1);
	            if (bIdx < 0) bIdx = points.Count - 1;

	            Vector2 p1 = points[bIdx];
	            Vector2 p = points[i];
	            Vector2 p2 = points[nIdx];

	            Vector2 p1p = p - p1;
	            Vector2 p2p = p - p2;
	            float angle = Mathf.Atan2(p1p.x, p1p.y) - Mathf.Atan2(p2p.x, p2p.y);
	            float segmentLength,p1cLength,p2cLength;
	            segmentLength = p1cLength = p2cLength = radius / Mathf.Abs(Mathf.Tan(angle / 2));

	            float min = Mathf.Min(p1p.magnitude, p2p.magnitude) / 2;
	            if (segmentLength > min)
	            {
	                segmentLength = min;
	                radius = segmentLength * Mathf.Abs(Mathf.Tan(angle / 2));
	            }

	            float poLength = Mathf.Sqrt(segmentLength * segmentLength + radius * radius);
	            // C1X = PX - (PX - P1X) * PC1 / PP1
	            // C1Y = PY - (PY - P1Y) * PC1 / PP1
	            Vector2 c1 = p - (p - p1) * (p1cLength / p1p.magnitude);
	            Vector2 c2 = p - (p - p2) * (p2cLength / p2p.magnitude);

	            Vector2 c = c1 + c2 - p;
	            Vector2 cp = p - c;
	            float pcLength = (cp).magnitude;
	            Vector2 o = p - cp * (poLength / pcLength);
		        
		        float sweepAngle = Vector2.SignedAngle(c1 - o, c2 - o);
		        float sign = Mathf.Sign(sweepAngle);
		        sweepAngle = Mathf.Abs(sweepAngle);
	            
	            //One point for each degree. But in some cases it will be necessary 
	            // to use more points. Just change a degreeFactor.
		        int pointsCount = (int) (sweepAngle / smoothingStepAngle);
		        if (pointsCount > 1)
		        {
			        Vector2 rad = (c1 - o).normalized;
		        
			        for (int si = 0; si < pointsCount + 1; ++si)
			        {
				        float t = si / (float)pointsCount;
				        float a = sign * t * sweepAngle;
				        Quaternion rot = Quaternion.Euler(0,0,a);
				        Vector2 intPoint = (rot * rad) * radius;
				        intPoint += o;
				        AddPoint(intPoint);
			        } 
		        }
		        else
		        {
			        AddPoint(p);
		        }

	        }
	        if(!loop)
	        {
		        AddPoint(points[points.Count - 1]);
	        }

			return newPoints;

		}
		
		public static Vector2[] ShapeCircle(Vector2 center, float radius, int nPoints = 30)
		{
			Quaternion rot = Quaternion.Euler(0, 0, 360.0f / nPoints);
			Vector2 rad = Vector2.up * radius; 

			Vector2[] points = new Vector2[nPoints];
			for (int i = 0; i < nPoints; i++)
			{
				Vector2 pos = center + rad;
				rad = rot * rad;
				points[i] = pos;
			}
			return points;
		}
		
		public static Vector3[] ShapeCircle(Vector3 center, float radius, int nPoints = 30)
		{
			Quaternion rot = Quaternion.Euler(0, 0, 360.0f / nPoints);
			Vector3 rad = Vector2.up * radius; 

			Vector3[] points = new Vector3[nPoints];
			for (int i = 0; i < nPoints; i++)
			{
				Vector3 pos = center + rad;
				rad = rot * rad;
				points[i] = pos;
			}
			return points;
		}

		public static Vector2[] ShapeRect(Vector2 center, Vector2 size)
		{
			size *= 0.5f;
			return new[]
			{
				center + new Vector2(-size.x,  size.y),
				center + new Vector2( size.x,  size.y),
				center + new Vector2( size.x, -size.y),
				center + new Vector2(-size.x, -size.y)
			};
		}

		public static float GetLineLength(Vector2[] points, bool loop)
		{
			float length = 0;
			for (int i = 0; i < points.Length - 1; i++)
			{
				length += Vector2.Distance(points[i], points[i + 1]);
			}

			if (loop)
			{
				length += Vector2.Distance(points[0], points[points.Length - 1]);
			}

			return length;
		}

		
		public static void SamplePath(Vector2[] points, List<Vector2> result, float distanceBetweenPoints)
		{
			int currentIdx = 0;
			float currentDistance = 0;

			result.Add(points[0]);
			while (true)
			{
				NextPoint(points,ref currentIdx,ref currentDistance,distanceBetweenPoints);
				if (currentIdx == points.Length - 1)
				{
					if (Vector2.Distance(result[result.Count - 1], points[points.Length - 1]) > Mathf.Epsilon)
					{
						result.Add(points[points.Length - 1]);
					}
					break;
				}
				
				int nIdx = currentIdx + 1;
				Vector2 dir = (points[nIdx] - points[currentIdx]).normalized;
				result.Add(points[currentIdx] + dir * currentDistance);
			}
		}

		public static void NextPoint(Vector2[] points, ref int currentIdx, ref float currentDis, float movement)
		{
			movement += currentDis;
			while (true)
			{
				int nIdx = currentIdx + 1;
				if (nIdx >= points.Length) nIdx = 0;
				float dis = Vector2.Distance(points[currentIdx], points[nIdx]);
				if (movement > dis)
				{
					movement -= dis;
					currentIdx = nIdx;
				}
				else
				{
					currentDis = movement;
					break;
				}
			}
		}
		
		/*
			Tension: 1 is high, 0 normal, -1 is low
			Bias: 0 is even, positive is towards first segment, negative towards the other
			http://paulbourke.net/miscellaneous/interpolation/
		 */
		public static Vector2 HermiteInterpolate(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float t, float tension = 0, float bias = 0)
		{

			var mu2 = t * t;
			var mu3 = mu2 * t;
			var m0  = (p1-p0)*(1+bias)*(1-tension)/2;
			m0 += (p2-p1)*(1-bias)*(1-tension)/2;
			var m1  = (p2-p1)*(1+bias)*(1-tension)/2;
			m1 += (p3-p2)*(1-bias)*(1-tension)/2;
			var a0 =  2*mu3 - 3*mu2 + 1;
			var a1 =    mu3 - 2*mu2 + t;
			var a2 =    mu3 -   mu2;
			var a3 = -2*mu3 + 3*mu2;

			return(a0*p1+a1*m0+a2*m1+a3*p2);
			
		}


		public static bool IsPathClockwise(Vector2[] path)
		{
			/*
			 * https://stackoverflow.com/questions/1165647/how-to-determine-if-a-list-of-polygon-points-are-in-clockwise-order
			 */
			float sum = 0;
			for (int i = 0; i < path.Length; i++)
			{
				Vector2 s = path[(i + 1) % path.Length] - path[i];
				sum += s.x * s.y;
			}

			return sum > 0;

		}

		public static int IdxClockwiseDistance(int from, int to, int arrayLength)
		{
			int d = to - from;
			if (d < 0)
			{
				d += arrayLength;
			}

			return d;
		}

		public static float SmoothStep(float value, float min, float max)
		{
			if (value < min)
			{
				return 0;
			}

			if (value > max)
			{
				return 1;
			}

			return (value - min) / (max - min);
		}
		
		
		static bool IsClockwise(List<Vector2> path)
		{
			// Positive = Clockwise

			// https://stackoverflow.com/questions/1165647/how-to-determine-if-a-list-of-polygon-points-are-in-clockwise-order#:~:text=If%20the%20determinant%20is%20negative,q%20and%20r%20are%20collinear.
			float t = 0;

			for (int i = 0; i < path.Count; i++)
			{
				Vector2 p1 = path[i];
				Vector2 p2 = path[(i + 1) % path.Count];
				t += (p2.x - p1.x) * (p2.y + p1.y);
			}

			return t > 0;
		}
		
	}
}
