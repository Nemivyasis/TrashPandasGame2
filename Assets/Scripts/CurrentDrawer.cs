using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentDrawer : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenPoints = 0.1f;

    //[SerializeField]
    private CurrentScript lineRenderScript;

    private float timeAtLastPoint = 0;

    public GameObject currentPrefab;

    public void Start()
    {
        CreateNewCurrent();
    }
   
    public void CreateNewCurrent()
    {
        GameObject current = Instantiate(currentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        lineRenderScript = current.GetComponent<CurrentScript>();
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
        EndCurrent();
    }

    public void EndCurrent()
    {
        if (lineRenderScript.GetCurrentCount() != 0)
        {
            lineRenderScript.ConfirmList();
            CreateNewCurrent();
        }
    }

    private void AddPoint(PointerEventData eventData)
    {
        if(Time.time - timeAtLastPoint > timeBetweenPoints)
        {
            Vector3 pointerPos = eventData.pointerCurrentRaycast.worldPosition;

            if (pointerPos == Vector3.zero || Physics2D.Raycast(pointerPos, new Vector2(0, 1), 0.001f, 1 << 9))
            {
                EndCurrent();
                return;
            }

            pointerPos.z = 0;
            lineRenderScript.AddPoint(pointerPos);
            timeAtLastPoint = Time.time;
        }
    }
}
