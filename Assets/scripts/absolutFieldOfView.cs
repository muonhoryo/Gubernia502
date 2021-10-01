using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class absolutFieldOfView : NPCFieldOfView//singltone
{
	static absolutFieldOfView singltone = null;
	protected override void generateFieldOfViewMesh()//генерирует меш поля
	{
		float stepAngleSize = 360 / Gubernia502.constData.absolutVisionQuality;
		List<Vector3> fieldPoints = new List<Vector3> { };
		ViewCastInfo newViewCast = ViewCast(0, Gubernia502.constData.absolutVisionDistance);
		fieldPoints.Add(newViewCast.point);
		ViewCastInfo oldViewCast = newViewCast;
		generateMeshCycle(stepAngleSize, Gubernia502.constData.absolutVisionQuality, stepAngleSize,
			ref fieldPoints, ref oldViewCast, ref newViewCast, 1, Gubernia502.constData.absolutVisionDistance);
		newViewCast = ViewCast(360-stepAngleSize, Gubernia502.constData.absolutVisionDistance);
		if (edgeLineCast(newViewCast.point, oldViewCast.point))
		{
			fieldCorrecter corrector = new fieldCorrecter(this, oldViewCast, newViewCast, Gubernia502.constData.absolutVisionDistance);
			fieldPoints.AddRange(corrector.startCorrect());
		}
		Vector3[] vertices = new Vector3[fieldPoints.Count + 1];
		int[] triangles = new int[fieldPoints.Count*3];
		vertices[0] = transform.InverseTransformPoint(new Vector3(transform.position.x, 0.01f, transform.position.z));
		for (int i = 0; i < fieldPoints.Count; i++)
		{
			vertices[i + 1] = transform.InverseTransformPoint(new Vector3(fieldPoints[i].x, 0.01f, fieldPoints[i].z));
		}
		for(int i = 0; i < fieldPoints.Count - 1; i++)
        {
			setTriangle(ref triangles, i, 0, i + 1, i + 2);
        }
		setTriangle(ref triangles, fieldPoints.Count - 1, 0, fieldPoints.Count - 1, 1);
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