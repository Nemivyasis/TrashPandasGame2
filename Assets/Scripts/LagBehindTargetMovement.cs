using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagBehindTargetMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    private Queue<Vector3> prevPos;

    // Start is called before the first frame update
    void Start()
    {
        prevPos = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        prevPos.Enqueue(target.transform.position);

        Vector3 temp = Vector3.zero;
        if (prevPos.Count >= 100)
        {
            temp = prevPos.Dequeue();
        }
        else
        {
            temp = prevPos.Peek();
        }
        transform.position = temp;

    }
}
