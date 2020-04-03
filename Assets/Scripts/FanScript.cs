using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanScript : MonoBehaviour
{
	public float degreesToOpen;
	public Animator gateAnimator;

	private float lastAngle;

	public float degreesTurned;
    // Start is called before the first frame update
    void Start()
    {
		lastAngle = transform.rotation.eulerAngles.z;
		degreesTurned = 0;
    }

    // Update is called once per frame
    void Update()
    {
		float facing = transform.rotation.eulerAngles.z;
		//Debug.Log(facing.eulerAngles.z);
		float angle = facing - lastAngle;
		if(angle <= -90)
		{
			angle += 360;
		}else if (angle >= 90)
		{
			angle -= 360;
		}
		/*if(Mathf.Abs(angle) >= 180)
		{
			angle = lastAngle + facing;
		}*/

		degreesTurned += angle;
		lastAngle = facing;

		gateAnimator.SetFloat("Open", degreesTurned / degreesToOpen);
	}
}
