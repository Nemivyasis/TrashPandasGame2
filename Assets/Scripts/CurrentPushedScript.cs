using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CurrentPushedScript : CurrentEffectableScript
{
	private Rigidbody2D RigidBodyEfected;

	public bool useBuyonacy = true;

	// Start is called before the first frame update
	void Start()
    {
		RigidBodyEfected = GetComponent<Rigidbody2D>();
    }

	public override void ApplyCurrentEffect(Vector2 direction, Vector2 buyonacy, float speed)
	{
		Vector2 force = direction * speed *forceMultiplier;
		if (useBuyonacy)
		{
			force += buyonacy / 2;
		}
		RigidBodyEfected.AddForce(force);
	}

}
