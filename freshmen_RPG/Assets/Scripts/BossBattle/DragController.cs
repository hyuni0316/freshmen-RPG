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

    public float moveSpeed = 5.0f; // 개빨라서 카메라 이동 속도 조정함

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, screenPoint.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            curPosition.y = transform.position.y; // y축 고정
            curPosition.z = transform.position.z; // z축 고정

            // 고정된 속도로 이동
            transform.position = Vector3.MoveTowards(transform.position, curPosition, moveSpeed * Time.deltaTime);
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
