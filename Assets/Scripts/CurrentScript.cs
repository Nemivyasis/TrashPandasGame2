using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class CurrentScript : MonoBehaviour
{
	private EdgeCollider2D edgeCollider;
	private LineRenderer lineRenderer;

	/// <summary>
	/// the radius of the current
	/// </summary>
	public float castRadius = 0.75f;

	public float speed = 3.0f;

	[SerializeField]
	private List<Vector2> points;

	/// <summary>
	/// the index of the current point we are between
	/// </summary>
	private int currentPoint;

	/// <summary>
	/// the line we are currently on
	/// </summary>
	private Vector2 currentLine;

	private void Start()
	{
		edgeCollider = GetComponent<EdgeCollider2D>();
		lineRenderer = GetComponent<LineRenderer>();
		if(points == null)
		{
			points = new List<Vector2>();
		}
	}

	/// <summary>
	/// called to add points to the list of points, also adds the points to the line renderer
	/// </summary>
	/// <param name="point">point to add</param>
	public void AddPoint(Vector2 point)
	{
		points.Add(point);
		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPosition(points.Count - 1, new Vector3(point.x, point.y));
	}

	/// <summary>
	/// removes all unnecessary points from the list and sets the line renderer and edge renderer points
	/// </summary>
	public void ConfirmList()
	{
		///TODO: add function to remove unnecessary points

		Vector3[] positions = new Vector3[points.Count];
		for(int i = 0; i < points.Count; i++)
		{
			positions[i] = new Vector3(points[i].x, points[i].y, 0);
		}
		lineRenderer.SetPositions(positions);
		edgeCollider.points = points.ToArray();
	}

	/// <summary>
	/// gets the point that the player is between
	/// </summary>
	/// <returns>the index of the first point the player is between</returns>
	public int DetermineCurrentPoint()
	{
		for(int i = 0; i < points.Count -1; i++)
		{
			if (LinecastAtPoint(i))
			{
				return i;
			}
		}
		return -1;
	}

	/// <summary>
	/// check if the player is between a point and the next point on the list 
	/// </summary>
	/// <param name="index">the index of the first point in the line we are casting</param>
	/// <returns>whether or not the player intersects with the line between the two points</returns>
	private bool LinecastAtPoint(int index)
	{
		return LinecastAtPoint(index, null);
	}

	//note the reason we have two LinecastAtPoint is if we later want multiple objects to be able to be affected by the current

	/// <summary>
	/// check if the player is between a point and the next point on the list 
	/// </summary>
	/// <param name="index">the index of the first point in the line we are casting</param>
	/// <param name="col">the collider of the player</param>
	/// <returns>whether or not the player intersects with the line between the two points</returns>
	private bool LinecastAtPoint(int index, Collider2D col)
	{
		//first check the line itself
		RaycastHit2D[] rays = new RaycastHit2D[1];
		int hit = 0;
		int layerMask = (1 << 8);
		hit += Physics2D.LinecastNonAlloc(points[index], points[index + 1], rays, layerMask);
		if(hit > 0&&(col == null||rays[0].collider == col))
		{
			return true;
		}
		//if the object was not on immediate line check the lines that are the cast radius away
		Vector2 perp = (points[index] - points[index + 1]).normalized;
		float temp = perp.x;
		perp.x = -perp.y;
		perp.y = temp;
		perp *= castRadius;
		hit += Physics2D.LinecastNonAlloc(points[index] + perp, points[index + 1] + perp, rays, layerMask);
		if (hit > 0 && (col == null || rays[0].collider == col))
		{
			return true;
		}
		hit += Physics2D.LinecastNonAlloc(points[index] - perp, points[index + 1] - perp, rays, layerMask);
		return (hit > 0 && (col == null || rays[0].collider == col));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		SetCurrentPoint(DetermineCurrentPoint());
	}

	private void SetCurrentPoint(int index)
	{
		if (index >= 0)
		{
			currentPoint = index;
			currentLine = points[currentPoint + 1] - points[currentPoint];
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (currentPoint < points.Count - 2)
		{
			float distance = (new Vector2(collision.bounds.center.x, collision.bounds.center.y) - points[currentPoint]).sqrMagnitude;
			if (distance > currentLine.sqrMagnitude)
			{
				SetCurrentPoint(currentPoint + 1);
			}
		}
		collision.GetComponent<Rigidbody2D>().AddForce(currentLine.normalized * 3);
	}
}
