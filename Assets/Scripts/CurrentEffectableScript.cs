using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CurrentEffectableScript : MonoBehaviour
{
	public bool extendsTimer;
	public float forceMultiplier = 1.0f;
	public abstract void ApplyCurrentEffect(Vector2 direction, Vector2 buyonacy, float speed);
}
