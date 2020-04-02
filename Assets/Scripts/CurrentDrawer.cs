using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CurrentDrawer : MonoBehaviour
{
    [SerializeField]
    private float timeBetweenPoints = 0.1f;

    //[SerializeField]
    private CurrentTest lineRenderScript;
	private CurrentScript CurrentScript;

    private float timeAtLastPoint = 0;

    public GameObject currentPrefab;

    public void Start()
    {
        CreateNewCurrent();
    }
   
    public void CreateNewCurrent()
    {
        GameObject current = Instantiate(currentPrefab, new Vector3(0, 0, 0), Quaternion.identity);
		CurrentScript = current.GetComponent<CurrentScript>();
        lineRenderScript = current.GetComponent<CurrentTest>();
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
			if(CurrentScript != null)
			{
				CurrentScript.ConfirmList();
			}
			else
			{
				lineRenderScript.ConfirmList();
			}
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
			if (CurrentScript != null)
			{
				CurrentScript.AddPoint(pointerPos);
			}
			else
			{
				lineRenderScript.AddPoint(pointerPos);
			}
            timeAtLastPoint = Time.time;
        }
    }
}
