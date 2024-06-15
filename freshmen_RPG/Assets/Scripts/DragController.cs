using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isDragging = false;
    private float zCoord;

    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        isDragging = true;
        zCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Debug.Log("OnMouseDrag");
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            curPosition.y = transform.position.y; // y축 고정
            curPosition.z = transform.position.z; // z축 고정
            transform.position = curPosition;
        }
    }

    void OnMouseUp()
    {
        Debug.Log("OnMouseUp");
        isDragging = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
