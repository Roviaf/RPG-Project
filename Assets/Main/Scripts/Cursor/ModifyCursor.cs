using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyCursor : MonoBehaviour
{
    void Start(){
        Cursor.visible=false;
    }

    void Update() {
        Vector2 cursorPos = Camera.main.WorldToScreenPoint(Input.mousePosition);
        transform.position = cursorPos;
    }
}