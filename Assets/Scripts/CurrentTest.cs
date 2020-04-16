using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EdgeCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class CurrentTest : MonoBehaviour
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

	public float tolerance = 0.1f;

	[SerializeField]
	private List<Vector2> points;

	/// <summary>
	/// the vertical buyancy force of the current line
	/// </summary>
	private Vector2 buyonacyLine;

	private Collider2D col;

	//private float length = 0;

	private float speed = 0;

	private float clock = 0;

	private int pushnum = 0;

	private bool pushing = true;

	private Collider2D[] collider2Ds;

	//this helps for testing
	private void Reset()
	{
		Start();
		points.AddRange(edgeCollider.points);
		lineRenderer.positionCount = points.Count;
		ConfirmList();
		//OldConfirmList();
	}

	private void Start()
	{
		edgeCollider = GetComponent<EdgeCollider2D>();
		lineRenderer = GetComponent<LineRenderer>();
		collider2Ds = new Collider2D[5];
		/*for(int i = 0; i< 5; i++)
		{
			collider2Ds[i] = null;
		}*/
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
			//Debug.Log((DestroyTime - clock) / DestroyTime);
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
	public void OldConfirmList()
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

		float length = 0;
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

	public void ConfirmList()
	{
		float length = 0;
		lineRenderer.Simplify(tolerance);
		Vector3[] positions = new Vector3[lineRenderer.positionCount];
		lineRenderer.GetPositions(positions);
		points.Clear();
		for(int i = 0; i < positions.Length; i++)
		{
			points.Add(new Vector2(positions[i].x, positions[i].y));
			if (i != 0)
			{
				length += (points[i - 1] - points[i]).magnitude;
			}
		}
		length = Mathf.Clamp(length, minLength, maxLength);
		speed = maxSpeed + (length - minLength) / (maxLength - minLength) * (minSpeed - maxSpeed);
		edgeCollider.points = points.ToArray();
		pushing = false;
	}

	private float DistanceToPoint(int index, Vector2 point)
	{
		/*float lsqrd = Vector2.SqrMagnitude(points[index] - points[index + 1]);
		//float t = Mathf.Max(0, Mathf.Min(1, Vector2.Dot(point - points[index + 1], points[index] - points[index + 1]) / lsqrd));
		float t = Vector2.Dot(point - points[index], points[index+1] - points[index]) / lsqrd;
		//float t = Mathf.Abs(Vector2.Dot(point, points[index] - points[index + 1]) / lsqrd);

		//Vector2 proj = points[index + 1] + t * (points[index] - points[index + 1]);
		Vector2 proj = points[index] + t * (points[index+1] - points[index]);
		proj -= point;
		return Vector2.SqrMagnitude(proj);*/
		float lsqrd = Vector2.SqrMagnitude(points[index+1] - points[index]);
		float t = Mathf.Clamp(Vector2.Dot(point - points[index], points[index + 1] - points[index]) / lsqrd, 0, 0.99f);
		Vector2 proj = points[index] + t * (points[index + 1] - points[index]);
		return Vector2.SqrMagnitude(point - proj);
	}

	public int TestCurrentPoint(Vector2 point)
	{
		float minimumDist = float.MaxValue;
		int closest = 0;
		for (int i = 0; i < points.Count - 1; i++)
		{
			float dist = DistanceToPoint(i, point);
			if (dist < minimumDist)
			{
				minimumDist = dist;
				closest = i;
			}
		}
		return closest;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{
			CurrentEffectableScript currentEffectable = collision.GetComponent<CurrentEffectableScript>();
			if (currentEffectable != null && currentEffectable.extendsTimer)
			{
				pushing = true;
				pushnum++;
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{
			Vector2 temp = new Vector2(collision.transform.position.x, collision.transform.position.y);
			int testtemp = TestCurrentPoint(temp);
			Vector2 currentspeed = points[testtemp + 1] - points[testtemp];
			Vector2 perp = new Vector2(-currentspeed.y, currentspeed.x);
			buyonacyLine = (Vector2.Dot(Physics2D.gravity * 0.1f, perp) / perp.sqrMagnitude) * perp * -1;
			CurrentEffectableScript currentEffectable = collision.GetComponent<CurrentEffectableScript>();
			if (currentEffectable)
			{
				currentEffectable.ApplyCurrentEffect(currentspeed.normalized, buyonacyLine, speed);
			}
			else
			{
				collision.GetComponent<Rigidbody2D>().AddForce((currentspeed.normalized * speed) + buyonacyLine / 2);
			}

			//for current test
			{
				CurrentDebugScript cds = collision.GetComponent<CurrentDebugScript>();
				if (cds != null)
				{
					/*float lsqrd = Vector2.SqrMagnitude(points[testtemp] - points[testtemp + 1]);
					float t = Vector2.Dot(temp - points[testtemp + 1], points[testtemp] - points[testtemp + 1]) / lsqrd;
					//Vector2 proj = points[testtemp + 1] + t * (points[testtemp] - points[testtemp + 1]);
					//float t = Vector2.Dot(temp, points[testtemp] - points[testtemp + 1]) / lsqrd;
					Vector2 proj = points[testtemp] + t * (points[testtemp] - points[testtemp + 1]);
					proj -= temp;
					cds.DebugPoint(proj,points[testtemp]);*/
					float lsqrd = Vector2.SqrMagnitude(points[testtemp] - points[testtemp + 1]);
					float t = Mathf.Clamp(Vector2.Dot(temp - points[testtemp], points[testtemp + 1] - points[testtemp]) / lsqrd, 0, 0.99f);
					Vector2 proj = points[testtemp] + t * (points[testtemp + 1] - points[testtemp]);
					cds.DebugPoint(temp - proj, points[testtemp]);
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.isTrigger)
		{
			CurrentEffectableScript currentEffectable = collision.GetComponent<CurrentEffectableScript>();
			if (currentEffectable != null && currentEffectable.extendsTimer)
			{
				pushnum--;
				if (pushnum <= 0)
				{
					pushing = false;
					pushnum = 0;
				}
			}
			CurrentLeaveVelocitySet currentLeave = collision.GetComponent<CurrentLeaveVelocitySet>();
			if(currentLeave != null)
			{
				currentLeave.OnCurrrentLeave();
			}
		}
	}

	public int GetCurrentCount()
	{
		return points.Count;
	}
}
