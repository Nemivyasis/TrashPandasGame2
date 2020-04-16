using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserScript : FishLeftAndRight
{
	private Collider2D target;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			target = collision;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision == target)
		{
			target = null;
			moveLeft = rb.velocity.x < 0;
			start = false;
		}
	}

	protected override void Move()
	{
		if(target != null)
		{
			Vector2 diff = (target.transform.position - transform.position);
			rb.AddForce(diff.normalized * speed);
			if(Mathf.Abs(diff.x) > 0.1)
			{
				spriteRenderer.flipX = diff.x < 0;
			}
		}
	}
}
