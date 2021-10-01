using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainFieldOfView : NPCFieldOfView//singltone
{
	static mainFieldOfView singltone = null;
	public float generateAngle;
	protected override void generateFieldOfViewMesh()//генерирует меш поля
	{
		float stepAngleSize = Gubernia502.constData.FieldOfViewAngle /
			(Gubernia502.constData.FieldOfViewQuality - 1);//промежуток между рейкастами
		List<Vector3> mainFieldPoints = new List<Vector3> { };
		List<Vector3> secondFieldPoints = new List<Vector3> { };//точки попадания рейкастом
		ViewCastInfo newViewCast = ViewCast(generateAngle - Gubernia502.constData.FieldOfViewAngle / 2,
			Gubernia502.constData.FieldOfViewRange);
		mainFieldPoints.Add(newViewCast.point);
		ViewCastInfo oldViewCast = newViewCast;
		generateMeshCycle(newViewCast.angle + stepAngleSize, Gubernia502.constData.FieldOfViewQuality,
			stepAngleSize, ref mainFieldPoints, ref oldViewCast, ref newViewCast, 1, Gubernia502.constData.FieldOfViewRange);
		int secondFieldOfViewQuality = Mathf.RoundToInt(360 /
			Gubernia502.constData.FieldOfViewAngle * Gubernia502.constData.FieldOfViewQuality);
		float secondStepAngleSize = (360 - Gubernia502.constData.FieldOfViewAngle) / (secondFieldOfViewQuality - 1);
		newViewCast = ViewCast(generateAngle + Gubernia502.constData.FieldOfViewAngle / 2,
			 Gubernia502.constData.SecondFieldOfViewRange);
		secondFieldPoints.Add(newViewCast.point);
		oldViewCast = newViewCast;
		generateMeshCycle(generateAngle + Gubernia502.constData.FieldOfViewAngle / 2 + secondStepAngleSize,
			secondFieldOfViewQuality, secondStepAngleSize, ref secondFieldPoints, ref oldViewCast, ref newViewCast, 1,
			Gubernia502.constData.SecondFieldOfViewRange);
		Vector3[] vertices = new Vector3[mainFieldPoints.Count + secondFieldPoints.Count + 1];
		int[] triangles = new int[(mainFieldPoints.Count + secondFieldPoints.Count) * 3];
		vertices[0] = transform.InverseTransformPoint(new Vector3(transform.position.x, 0.01f, transform.position.z));
		for (int i = 0; i < mainFieldPoints.Count; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(new Vector3(mainFieldPoints[i].x, 0.01f, mainFieldPoints[i].z));
		}
		for (int i = 0; i < secondFieldPoints.Count; i++)
		{
			vertices[i + mainFieldPoints.Count + 1] =
				transform.InverseTransformPoint(new Vector3(secondFieldPoints[i].x, 0.01f, secondFieldPoints[i].z));
		}
		int triangleIndex = 0;
		setTriangle(ref triangles, triangleIndex, 0, vertices.Length - 1, 2);
		triangleIndex++;
		setTriangle(ref triangles, triangleIndex, vertices.Length - 1, 1, 2);
		triangleIndex++;
		for (int i = 1; i < mainFieldPoints.Count - 2; i++)
		{
			setTriangle(ref triangles, triangleIndex++, 0, i + 1, i + 2);
		}
		setTriangle(ref triangles, triangleIndex, 0, mainFieldPoints.Count - 1, mainFieldPoints.Count + 1);
		triangleIndex++;
		setTriangle(ref triangles, triangleIndex, mainFieldPoints.Count + 1,
			mainFieldPoints.Count - 1, mainFieldPoints.Count);
		triangleIndex++;
		for (int i = 1; i < secondFieldPoints.Count; i++)
		{
			setTriangle(ref triangles, triangleIndex++, 0, mainFieldPoints.Count + i, mainFieldPoints.Count + i + 1);
		}
		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
	}
	private void Awake()
	{
		if (singltone == null)
		{
			singltone = this;
		}
		else
		{
			Destroy(this);
		}
	}
}