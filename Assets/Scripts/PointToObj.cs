using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToObj : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(targetObj.transform.position.y - transform.position.y, targetObj.transform.position.x - transform.position.x) + 90;
        angle *= Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
