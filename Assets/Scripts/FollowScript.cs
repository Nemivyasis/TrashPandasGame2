using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    [SerializeField]
    private GameObject followTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followTarget != null)
            transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, transform.position.z);
    }
}
