using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CurrentLeaveVelocitySet : MonoBehaviour
{
	private Rigidbody2D rb;
	private void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	public void OnCurrrentLeave()
	{
		Vector2 vel = rb.velocity;
		vel.y = 0;
		rb.velocity = vel;
	}
}
