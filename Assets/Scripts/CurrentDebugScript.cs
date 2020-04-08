using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentDebugScript : MonoBehaviour
{
	public void DebugPoint(Vector2 point, Vector2 point2)
	{
		Vector3 p = new Vector3(point.x, point.y, 0);
		Vector3 p2 = new Vector3(point2.x, point2.y, 0);
		Debug.DrawLine(p - Vector3.left - Vector3.up+Vector3.forward*2, p + Vector3.left + Vector3.up, Color.white,0,false);
		Debug.DrawLine(p - Vector3.right - Vector3.up, p + Vector3.right + Vector3.up, Color.white, 0, false);
		Debug.DrawLine(p2 - Vector3.left - Vector3.up + Vector3.forward * 2, p2 + Vector3.left + Vector3.up, Color.red, 0, false);
		Debug.DrawLine(p2 - Vector3.right - Vector3.up, p2 + Vector3.right + Vector3.up, Color.red, 0, false);
	}
}
