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

	public float maxSpeed = 5.0f;

	public float minSpeed = 3.0f;

	public float minLength = 0.5f;

	public float maxLength = 8.0f;

	public float MinDist = 0.01f;

	public float DestroyTime = 7.5f;

	[SerializeField]
	private List<Vector2> points;

	/// <summary>
	/// the index of the current point we are between
	/// </summary>
	private int currentPoint = -1;

	/// <summary>
	/// the line we are currently on
	/// </summary>
	private Vector2 currentLine;

	/// <summary>
	/// the vertical buyancy force of the current line
	/// </summary>
	private Vector2 buyonacyLine;

	private Collider2D col;

	private float length = 0;

	private float speed = 0;

	private float clock = 0;

	private bool pushing = true;

	//this helps for testing
	private void Reset()
	{
		Start();
		points.AddRange(edgeCollider.points);
		lineRenderer.positionCount = points.Count;
		ConfirmList();
	}

	private void Start()
	{
		edgeCollider = GetComponent<EdgeCollider2D>();
		lineRenderer = GetComponent<LineRenderer>();
		if (points == null)
		{
			points = new List<Vector2>();
		}
	}

	private void Update()
	{
		if (!pushing)
		{
			clock += Time.deltaTime;
			float per = (DestroyTime - clock) / DestroyTime;
			Debug.Log((DestroyTime - clock) / DestroyTime);
			/*Color c = new Color(1,1,1, (DestroyTime - clock) / DestroyTime);
			lineRenderer.startColor = c;
			lineRenderer.endColor = c;*/
			//GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
			Gradient g = lineRenderer.colorGradient;

			/*alphaKey[0].alpha = (((DestroyTime - clock) / DestroyTime));
			alphaKey[0].time = 0;
			alphaKey[1].alpha = (((DestroyTime - clock) / DestroyTime));
			alphaKey[1].time = 1;*/
			//g.SetKeys(g.colorKeys, alphaKey);
			g.SetKeys(g.colorKeys, new GradientAlphaKey[] { new GradientAlphaKey(per, 0), new GradientAlphaKey(per, 1) });
			//g.alphaKeys[1] = new GradientAlphaKey(((DestroyTime - clock) / DestroyTime), 1);
			//lineRenderer.colorGradient = g;
			lineRenderer.colorGradient = g;
			if (clock >= DestroyTime)
			{
				Destroy(gameObject);
			}
		}
	}

	/// <summary>
	/// called to add points to the list of points, also adds the points to the line renderer
	/// </summary>
	/// <param name="point">point to add</param>
	public void AddPoint(Vector2 point)
	{
		if (!points.Contains(point))
		{
			points.Add(point);
			lineRenderer.positionCount = points.Count;
			lineRenderer.SetPosition(points.Count - 1, new Vector3(point.x, point.y));
		}
	}

	/// <summary>
	/// removes all unnecessary points from the list and sets the line renderer and edge renderer points
	/// </summary>
	public void ConfirmList()
	{
		float minDistSqr = MinDist * MinDist;
		//bool shortening = false;
		int index = 0;
		while (!(points.Count < 4 || index == points.Count - 1))
		{
			float dist = (points[index + 1] - points[index]).sqrMagnitude;
			if (dist < minDistSqr)
			{
				points.RemoveAt(index + 1);
			}
			else
			{
				index++;
			}
		}

		length = 0;
		Vector3[] positions = new Vector3[points.Count];
		for (int i = 0; i < points.Count; i++)
		{
			positions[i] = new Vector3(points[i].x, points[i].y, 0);
			if (i != 0)
			{
				length += (positions[i - 1] - positions[i]).magnitude;
			}
		}
		length = Mathf.Clamp(length, minLength, maxLength);
		speed = maxSpeed + (length - minLength) / (maxLength - minLength) * (minSpeed - maxSpeed);
		//Debug.Log(speed);
		lineRenderer.positionCount = points.Count;
		lineRenderer.SetPositions(positions);
		edgeCollider.points = points.ToArray();
		pushing = false;
	}

	/// <summary>
	/// gets the point that the player is between
	/// </summary>
	/// <returns>the index of the first point the player is between</returns>
	public int DetermineCurrentPoint()
	{
		for (int i = 0; i < points.Count - 1; i++)
		{
			if (LinecastAtPoint(i))
			{
				//Debug.Log(i);
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
		if (hit > 0 && (col == null || rays[0].collider == col))
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
		col = collision;
		pushing = true;
		clock = 0;
	}


	/// <summary>
	/// set the current point
	/// </summary>
	/// <param name="index">the index of the current point to set</param>
	private void SetCurrentPoint(int index)
	{
		if (index >= 0)
		{
			currentPoint = index;
			currentLine = points[currentPoint + 1] - points[currentPoint];

			//calculate the buyancy line
			Vector2 perp = new Vector2(-currentLine.y, currentLine.x);
			buyonacyLine = (Vector2.Dot(Physics2D.gravity * 0.1f, perp) / perp.sqrMagnitude) * perp * -1;
			//im goin to third the velocity against the perpendicular, this is to try and keep it in the line 
			if (col != null)
			{
				Vector2 cv = col.GetComponent<Rigidbody2D>().velocity;
				Vector2 force = (Vector2.Dot(cv, perp) / perp.sqrMagnitude) * perp * -1;
				//force /= 2;
				col.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (currentPoint != -1)
		{
			if (currentPoint < points.Count - 2)
			{
				Vector2 proj = (currentLine * (Vector2.Dot(new Vector2(collision.bounds.center.x, collision.bounds.center.y)-points[currentPoint], currentLine) / currentLine.sqrMagnitude));
				//Debug.Log(proj);
				//Debug.Log(currentLine);
				//float distance = (currentLine * (Vector2.Dot(new Vector2(collision.bounds.center.x, collision.bounds.center.y), currentLine) / currentLine.sqrMagnitude)).sqrMagnitude;
				//float distance = (new Vector2(collision.bounds.center.x, collision.bounds.center.y) - points[currentPoint]).sqrMagnitude;
				//float distance = (proj - points[currentPoint]).sqrMagnitude;
				float distance = proj.sqrMagnitude;
				if (distance > length)
				{
					//Debug.Log("test");
					SetCurrentPoint(DetermineCurrentPoint());
				}
				else if (distance > currentLine.sqrMagnitude)
				{
					SetCurrentPoint(currentPoint + 1);
				}
			}
			collision.GetComponent<Rigidbody2D>().AddForce((currentLine.normalized * speed)+buyonacyLine/2);
		}
		else
		{
			SetCurrentPoint(DetermineCurrentPoint());
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		pushing = false;
	}

	/*
	private void OnDrawGizmos()
	{
		if (currentPoint != -1 && points.Count > 3)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawLine(new Vector3(points[currentPoint].x, points[currentPoint].y), new Vector3(points[currentPoint + 1].x, points[currentPoint + 1].y));
		}
		if (col != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawLine(new Vector3(points[currentPoint].x, points[currentPoint].y), new Vector3(points[currentPoint + 1].x, points[currentPoint + 1].y));
			Vector2 proj = (currentLine * (Vector2.Dot(new Vector2(col.bounds.center.x, col.bounds.center.y), currentLine) / currentLine.sqrMagnitude));
			Gizmos.color = Color.yellow;
			//Gizmos.DrawLine()
			//Gizmos.DrawLine(new Vector3(points[currentPoint].x, points[currentPoint].y), new Vector3(points[currentPoint].x+proj.x, points[currentPoint].y+proj.y));
		}
	}*/

	public int GetCurrentCount()
	{
		return points.Count;
	}
}
