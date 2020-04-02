using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBase : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody2D rb;
    [SerializeField]
    protected float speed = 1;
    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
#if UNITY_EDITOR
        Debug.Log("Base Fish Move called");
#endif
    }
}
