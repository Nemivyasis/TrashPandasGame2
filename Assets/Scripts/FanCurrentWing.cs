using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanCurrentWing : CurrentEffectableScript
{
	private Rigidbody2D ParentBody;
	public override void ApplyCurrentEffect(Vector2 direction, Vector2 buyonacy, float speed)
	{
		ParentBody.AddForceAtPosition(direction * speed, new Vector2(transform.position.x, transform.position.y));
	}

	// Start is called before the first frame update
	void Start()
    {
		ParentBody = GetComponentInParent<Rigidbody2D>();
    }
}
