using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager instance;

    private List<Touch> touches;
    private bool touchListDirty = true;

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        touches = new List<Touch>();
    }

    // Update is called once per frame
    void Update()
    {
        touchListDirty = true;
    }

    public int TouchCount()
    {
        RefreshTouchList();
        return touches.Count;
    }

    public Touch GetTouch(int pos)
    {
        RefreshTouchList();
        if (pos < touches.Count)
            return touches[pos];
        return new Touch();
    }

    private void RefreshTouchList()
    {
        if (touchListDirty)
        {
            touches.Clear();
            touches.AddRange(Input.touches);

            //get click input too
            if (Input.GetMouseButton(0))
            {
                Touch fakeTouch = new Touch();
                fakeTouch.fingerId = -1;
                fakeTouch.position = Input.mousePosition;
                touches.Add(fakeTouch);
            }

            touchListDirty = false;
        }
    }
}
