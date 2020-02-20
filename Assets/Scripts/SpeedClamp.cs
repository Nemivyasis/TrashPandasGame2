using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpeedClamp : MonoBehaviour
{

	public float MaxSpeed = 25.0f;

	private Rigidbody2D rigidbody;

    // Start is called before the first frame update
    void Start()
    {
		rigidbody = GetComponent<Rigidbody2D>();
    }

	// Update is called once per frame
	private void FixedUpdate()
	{
		if(rigidbody.velocity.sqrMagnitude > MaxSpeed * MaxSpeed)
		{
			rigidbody.velocity = rigidbody.velocity.normalized * MaxSpeed;
		}
	}
}
