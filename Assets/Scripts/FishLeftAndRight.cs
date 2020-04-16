using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishLeftAndRight : FishBase
{
    [SerializeField]
    private float timeBetweenChanges = 1;
    float timeSinceChange;
	protected bool start = false;
    protected bool moveLeft = false;
    protected override void Move()
    {
        if (!start)
        {
            start = true;
            timeSinceChange = 0.5f * timeBetweenChanges;
        }

        timeSinceChange += Time.deltaTime;

        if (timeSinceChange > timeBetweenChanges)
        {
            timeSinceChange -= timeBetweenChanges;
            moveLeft = !moveLeft;
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }


        if (rb)
        {
            int dir = moveLeft ? -1 : 1;
            rb.AddForce(new Vector2(dir * speed * Time.deltaTime, 0));
        }
    }
}
