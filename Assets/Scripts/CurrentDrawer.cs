using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentDrawer : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenPoints = 0.1f;

    [SerializeField]
    private CurrentScript lineRenderScript;

    private float timeAtLastPoint = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drag(BaseEventData data)
    {
        PointerEventData eventData = (PointerEventData)data;
        AddPoint(eventData);
    }

    public void DragEnd(BaseEventData data)
    {
        PointerEventData eventData = (PointerEventData)data;
        AddPoint(eventData);
        lineRenderScript.ConfirmList();
    }

    private void AddPoint(PointerEventData eventData)
    {
        if(Time.time - timeAtLastPoint > timeBetweenPoints)
        {
            Vector3 pointerPos = eventData.pointerCurrentRaycast.worldPosition;

            if (pointerPos == Vector3.zero) 
                return;

            pointerPos.z = 0;
            lineRenderScript.AddPoint(pointerPos);
            timeAtLastPoint = Time.time;
        }
    }
}
