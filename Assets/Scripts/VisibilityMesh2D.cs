using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VisibilityMesh2D : MonoBehaviour
{
	const float RECALCULATE_THRESHOLD = 0.05f;

	[System.Serializable]
	public struct Segment
	{
		public Vector2 Start;
		public Vector2 End;
	};

	[SerializeField] private Vector2 queryPoint = new Vector2(0.5f, 2f);
	[SerializeField] private List<Segment> worldSegments;
	[SerializeField] private List<Segment> exampleResultSegments;
	[Header("Debug")]
	[SerializeField] private bool drawExampleResults = true;
	[SerializeField] private bool drawExampleTriangles;

	private List<Segment> visibleSegments;

	private GameObject queryGameObject;
	private Vector2 doneQueryPoint;

	// Use this for initialization
	void OnEnable ()
	{
		//Vector2[] worldPoints = new Vector2[6];
		//worldPoints[0] = new Vector2(0f, 4f);
		//worldPoints[1] = new Vector2(0f, 0f);
		//worldPoints[2] = new Vector2(3f, 2f);
		//worldPoints[3] = new Vector2(4f, 0f);
		//worldPoints[4] = new Vector2(4f, 4f);
		//worldPoints[5] = new Vector2(1f, 2f);

		//worldSegments = new List<Segment>(worldPoints.Length);
		//worldSegments.Add(new Segment() { Start = worldPoints[0], End = worldPoints[1] });
		//worldSegments.Add(new Segment() { Start = worldPoints[1], End = worldPoints[2] });
		//worldSegments.Add(new Segment() { Start = worldPoints[2], End = worldPoints[3] });
		//worldSegments.Add(new Segment() { Start = worldPoints[3], End = worldPoints[4] });
		//worldSegments.Add(new Segment() { Start = worldPoints[4], End = worldPoints[5] });
		//worldSegments.Add(new Segment() { Start = worldPoints[5], End = worldPoints[0] });

		//Point_2 p1(0,4), p2(0, 0), p3(3, 2), p4(4, 0), p5(4, 4), p6(1, 2);
		//std::vector<Segment_2> segments;
		//segments.push_back(Segment_2(p1, p2));
		//segments.push_back(Segment_2(p2, p3));
		//segments.push_back(Segment_2(p3, p4));
		//segments.push_back(Segment_2(p4, p5));
		//segments.push_back(Segment_2(p5, p6));
		//segments.push_back(Segment_2(p6, p1));

		queryGameObject = new GameObject("QueryPoint");
		queryGameObject.transform.SetParent(transform);
		queryGameObject.transform.localPosition = new Vector3(queryPoint.x, 0.0f, queryPoint.y);

		CalculateVisibility(queryPoint, ref worldSegments, out visibleSegments);
	}

	private void OnDisable()
	{
		if(queryGameObject != null)
		{
			DestroyImmediate(queryGameObject);
		}
	}

	private void CalculateVisibility(Vector2 queryPoint, ref List<Segment> worldSegments, out List<Segment> resultSegments)
	{
		Debug.Log("Calculating visibility at point: " + queryPoint.ToString("F3"));
		resultSegments = new List<Segment>();

		//TODO: Calculations...
		resultSegments.Add(new Segment() { Start = new Vector2(0f, 4f), End = new Vector2(0f, 0f) });
		resultSegments.Add(new Segment() { Start = new Vector2(1f, 2f), End = new Vector2(0f, 4f) });
		resultSegments.Add(new Segment() { Start = new Vector2(3f, 2f), End = new Vector2(0f, 0f) });
		resultSegments.Add(new Segment() { Start = new Vector2(1f, 2f), End = new Vector2(3f, 2f) });

		doneQueryPoint = queryPoint;
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector2 qPoint = new Vector2(queryGameObject.transform.localPosition.x, queryGameObject.transform.localPosition.z);
		if((qPoint - doneQueryPoint).sqrMagnitude >= RECALCULATE_THRESHOLD * RECALCULATE_THRESHOLD)
		{
			CalculateVisibility(qPoint, ref worldSegments, out visibleSegments);
		}
	}

	private void OnDrawGizmos()
	{
		if (queryGameObject != null)
		{
			Vector3 query = queryGameObject.transform.position;
			Gizmos.DrawWireSphere(query, 0.1f);
			Gizmos.DrawLine(query, query + new Vector3(0f, 3f, 0f));
			GizmoDrawSegments(ref worldSegments, Color.blue);
			if (drawExampleResults)
			{
				GizmoDrawSegments(ref exampleResultSegments, Color.green);
			}
			GizmoDrawSegments(ref visibleSegments, Color.red);

			if(drawExampleTriangles)
			{
				Gizmos.color = Color.gray;
				for(int i = 0; i < exampleResultSegments.Count; i++)
				{
					Gizmos.DrawLine(query, new Vector3(exampleResultSegments[i].Start.x, 0.0f, exampleResultSegments[i].Start.y));
					Gizmos.DrawLine(query, new Vector3(exampleResultSegments[i].End.x, 0.0f, exampleResultSegments[i].End.y));
				}
			}
		}
	}

	private void GizmoDrawSegments(ref List<Segment> segments, Color color)
	{
		if (segments != null)
		{
			for (int i = 0; i < segments.Count; i++)
			{
				Gizmos.color = color;
				Gizmos.DrawLine(new Vector3(segments[i].Start.x, 0f, segments[i].Start.y), new Vector3(segments[i].End.x, 0f, segments[i].End.y));
			}
		}
	}
}
