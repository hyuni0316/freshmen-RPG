using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ClickableArea : MonoBehaviour
{
    void OnGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            GameObject.FindObjectOfType<DialogueManager>().OnScreenClick();
        }
    }
}