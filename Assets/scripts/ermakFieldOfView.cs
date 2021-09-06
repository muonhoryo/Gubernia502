using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ermakFieldOfView : MonoBehaviour
{
	public float generateAngle;
	public LayerMask obstacleMask;
	public MeshFilter viewMeshFilter;
	Mesh viewMesh;
	public float test;
	public Vector3 test2;
	void Start()
	{
		viewMesh = new Mesh();
		viewMeshFilter.mesh = viewMesh;
	}
	void LateUpdate()
	{
		generateFieldOfViewMesh();
	}
	public struct EdgeInfo//ребро меша
	{
		public Vector3 pointA;
		public Vector3 pointB;

		public EdgeInfo(Vector3 pointA, Vector3 pointB)
		{
			this.pointA = pointA;
			this.pointB = pointB;
		}
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
	ViewCastInfo ViewCast(float globalAngle,float rayCastRange)
	{
		Vector3 dir = Gubernia502.directionFromAngle(globalAngle);//направление для рейкаста
		RaycastHit hit;
		if (Physics.Raycast(new Vector3(viewMeshFilter.transform.position.x,
										Gubernia502.constData.fieldOfViewRayCastHeight,
										viewMeshFilter.transform.position.z), 
							dir, out hit, rayCastRange, obstacleMask,QueryTriggerInteraction.Ignore))
		{
			return new ViewCastInfo(true, new Vector3(hit.point.x, Gubernia502.constData.fieldOfViewRayCastHeight,hit.point.z),
									hit.distance, globalAngle);
		}
		else
		{
			return new ViewCastInfo(false, new Vector3(transform.position.x+ dir.x * rayCastRange, Gubernia502.constData.fieldOfViewRayCastHeight, transform.position.z+dir.z * rayCastRange),
									rayCastRange, globalAngle);
		}
	}
	void generateMeshCycle(float startAngle,int stepCount,float stepAngleSize,ref List<Vector3>viewPoints,ref ViewCastInfo oldViewCast,
							ref ViewCastInfo newViewCast,int startIteraction,in float rayCastRange)
	{
		for (; startIteraction <= stepCount; startIteraction++,startAngle+=stepAngleSize)//цикл рейкастов для генерации меша поля
		{
			newViewCast = ViewCast(startAngle, rayCastRange);
			if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit &&
				Mathf.Abs(oldViewCast.dst - newViewCast.dst) > Gubernia502.constData.fieldOfViewEdgeDstThreshold))
			{
				EdgeInfo edge = FindEdge(oldViewCast, newViewCast,rayCastRange);
				if (edge.pointA != oldViewCast.point)
				{
					viewPoints.Add(edge.pointA);
				}
				if (edge.pointB != newViewCast.point)
				{
					viewPoints.Add(edge.pointB);
				}
			}
			viewPoints.Add(newViewCast.point);
			oldViewCast = newViewCast;
		}
	}
	void generateFieldOfViewMesh()//генерирует меш поля
	{
		int stepCount = Mathf.RoundToInt(Gubernia502.constData.ermakFieldOfViewAngle * Gubernia502.constData.FieldOfviewMeshResolution);//количество рейкастов для построения поля
		float stepAngleSize = Gubernia502.constData.ermakFieldOfViewAngle / stepCount;//промежуток между рейкастами
		List<Vector3> viewPoints = new List<Vector3>();//точки попадания рейкастом
		ViewCastInfo oldViewCast;//предыдущий рейкаст
		ViewCastInfo newViewCast = ViewCast(generateAngle - Gubernia502.constData.ermakFieldOfViewAngle / 2,
			Gubernia502.constData.ermakFieldOfViewRange);
		viewPoints.Add(newViewCast.point);
		oldViewCast = newViewCast;
		generateMeshCycle(newViewCast.angle + stepAngleSize,stepCount, stepAngleSize,ref viewPoints, ref oldViewCast, ref newViewCast,1,
						Gubernia502.constData.ermakFieldOfViewRange);
		int secondStepCount = Mathf.RoundToInt((360 - Gubernia502.constData.ermakFieldOfViewAngle) * Gubernia502.constData.FieldOfviewMeshResolution)-1;
		generateMeshCycle(newViewCast.angle + stepAngleSize, secondStepCount, stepAngleSize, ref viewPoints, ref oldViewCast,
						ref newViewCast, 0, Gubernia502.constData.ermakSecondFieldOfViewRange);
		viewPoints.Add(viewPoints[0]);
		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = transform.InverseTransformPoint(new Vector3(transform.position.x, 0.01f, transform.position.z)) ;
		for (int i = 0; i < vertexCount - 1; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(new Vector3(viewPoints[i].x,0.01f,viewPoints[i].z));

			if (i < vertexCount - 2)
			{
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
			}
		}
		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}
	EdgeInfo FindEdge(ViewCastInfo leftViewCast, ViewCastInfo rightViewCast,in float rayCastRange)
	{
		float leftAngle = leftViewCast.angle;
		float rightAngle = rightViewCast.angle;
		ViewCastInfo newLeftViewCast = leftViewCast;
		ViewCastInfo newRightViewCast = rightViewCast;

		for (int i = 0; i < Gubernia502.constData.fieldOfViewEdgeResolveIterations; i++)
		{
			float angle = (leftAngle + rightAngle) / 2;
			ViewCastInfo newViewCast = ViewCast(angle, rayCastRange);
			if (newViewCast.hit == newLeftViewCast.hit &&
				!(Mathf.Abs(leftViewCast.dst - newViewCast.dst) > Gubernia502.constData.fieldOfViewEdgeDstThreshold))
			{
				leftAngle = angle;
				newLeftViewCast = newViewCast;
			}
			else
			{
				rightAngle = angle;
				newRightViewCast = newViewCast;
			}
		}
		return new EdgeInfo(newLeftViewCast.point, newRightViewCast.point);
	}
}
