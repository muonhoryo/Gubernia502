using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ermakFieldOfView : MonoBehaviour
{
	protected class fieldCorrecter
    {
        public fieldCorrecter(ermakFieldOfView owner,ViewCastInfo leftViewCast,ViewCastInfo rightViewCast,float range)
        {
			this.owner = owner;
			this.leftViewCast = leftViewCast;
			this.rightViewCast = rightViewCast;
			this.range = range;
        }
		void correctCycle(float viewCastAngle,ViewCastInfo leftViewCast, ViewCastInfo rightViewCast,in int iteraction)
        {
			ViewCastInfo newViewCast = ViewCast(owner,viewCastAngle, range);
			bool leftCast = edgeLineCast(leftViewCast.point, newViewCast.point)||
				Mathf.Abs(leftViewCast.dst-newViewCast.dst)>Gubernia502.constData.fieldOfViewEdgeDstThreshold;
			bool rightCast =edgeLineCast(rightViewCast.point,newViewCast.point)||
				Mathf.Abs(rightViewCast.dst - newViewCast.dst) > Gubernia502.constData.fieldOfViewEdgeDstThreshold;
			if (leftCast)
			{
				if (iteraction < Gubernia502.constData.fieldOfViewEdgeResolveIterations)
				{
					correctCycle((newViewCast.angle + leftViewCast.angle) / 2, leftViewCast, newViewCast, iteraction + 1);
				}
				else
				{
					correctPoint.Add(leftViewCast.point);
					correctPoint.Add(newViewCast.point);
				}
			}
			if (rightCast)
			{
				if (iteraction < Gubernia502.constData.fieldOfViewEdgeResolveIterations)
				{
					correctCycle((newViewCast.angle + rightViewCast.angle) / 2, newViewCast, rightViewCast, iteraction + 1);
				}
				else
				{
					if (!leftCast)
					{
						correctPoint.Add(newViewCast.point);
					}
					correctPoint.Add(rightViewCast.point);
				}
			}
			if ((!leftCast) && (!rightCast))
			{
                if (iteraction > 1)
                {
					correctPoint.Add(leftViewCast.point);
					correctPoint.Add(newViewCast.point);
					correctPoint.Add(rightViewCast.point);
                }
                else
				{
					correctPoint.Add(newViewCast.point);
				}
			}
		}
		public List<Vector3> startCorrect()
        {
			correctPoint = new List<Vector3> { };
			correctCycle((rightViewCast.angle+leftViewCast.angle)/2,leftViewCast,rightViewCast,1);
			return correctPoint;
        }

        readonly ermakFieldOfView owner;
		List<Vector3> correctPoint;
		ViewCastInfo leftViewCast;
		ViewCastInfo rightViewCast;
		readonly float range;
    }
	[SerializeField]
	protected MeshFilter viewMeshFilter;
	protected Mesh viewMesh;
	void Start()
	{
		viewMesh = new Mesh();
		viewMeshFilter.mesh = viewMesh;
	}
	void LateUpdate()
	{
		generateFieldOfViewMesh();
	}
	public struct ViewCastInfo//информация о рейкасте для построения поля
	{
		public bool hit;
		public Vector3 point;
		public float dst;
		public float angle;

		public ViewCastInfo(bool hit, Vector3 point, float dst, float angle)
		{
			this.hit = hit;
			this.point = point;
			this.dst = dst;
			this.angle = angle;
		}
	}
	public static bool edgeLineCast(Vector3 start, Vector3 end)
	{
		return Physics.Linecast(new Vector3(start.x, Gubernia502.constData.fieldOfViewRayCastHeight, start.z),
			new Vector3(end.x, Gubernia502.constData.fieldOfViewRayCastHeight, end.z), 512, QueryTriggerInteraction.Ignore)|| 
			Physics.Linecast(new Vector3(end.x, Gubernia502.constData.fieldOfViewRayCastHeight, end.z),
			new Vector3(start.x, Gubernia502.constData.fieldOfViewRayCastHeight, start.z), 512, QueryTriggerInteraction.Ignore);
	}
	protected ViewCastInfo ViewCast(float globalAngle,float rayCastRange)
	{
		return ViewCast(this, globalAngle, rayCastRange);
	}
	protected static ViewCastInfo ViewCast(ermakFieldOfView fieldOfView,float globalAngle, float rayCastRange)
	{
		Vector3 dir = Gubernia502.directionFromAngle(globalAngle);//направление для рейкаста
		RaycastHit hit;
		if (Physics.Raycast(new Vector3(fieldOfView.viewMeshFilter.transform.position.x,
										Gubernia502.constData.fieldOfViewRayCastHeight,
										fieldOfView.viewMeshFilter.transform.position.z),
							dir, out hit, rayCastRange, 512, QueryTriggerInteraction.Ignore))
		{
			return new ViewCastInfo(true,new Vector3( hit.point.x,fieldOfView.transform.position.y,hit.point.z),
				hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, fieldOfView.transform.position + dir * rayCastRange, rayCastRange, globalAngle);
		}
	}
	protected void generateMeshCycle(float startAngle,int stepCount,float stepAngleSize,ref List<Vector3>viewPoints,ref ViewCastInfo oldViewCast,
							ref ViewCastInfo newViewCast,int startIteraction,in float rayCastRange)
	{
		for (; startIteraction < stepCount; startIteraction++,startAngle+=stepAngleSize)//цикл рейкастов для генерации меша поля
		{
			newViewCast = ViewCast(startAngle, rayCastRange);
			if (edgeLineCast(newViewCast.point,oldViewCast.point))
			{
				fieldCorrecter corrector = new fieldCorrecter(this, oldViewCast, newViewCast, rayCastRange);
				viewPoints.AddRange(corrector.startCorrect());
			}
			viewPoints.Add(newViewCast.point);
			oldViewCast = newViewCast;
		}
	}
	protected void setTriangle(ref int[]triangles,int triangleIndex,int firstPoint,int secondPoint,int thirdPoint)
    {
		int index = triangleIndex * 3;
		triangles[index] = firstPoint;
		triangles[index + 1] = secondPoint;
		triangles[index + 2] = thirdPoint;
    }
	protected abstract void generateFieldOfViewMesh();
}
