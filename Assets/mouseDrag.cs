using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class mouseDrag : MonoBehaviour
{

    public Camera cam;
   Vector2 mouseClickPos;
    Vector2 mouseCurrentPos;
 
    private void Update()
    {
        // When LMB clicked get mouse click position and set panning to true
        if (Input.GetKey(KeyCode.Mouse1))
        {
            //Debug.Log("kkkkkkkk");
            if (mouseClickPos == default)
            {
                mouseClickPos = cam.ScreenToWorldPoint(Input.mousePosition);
            }
 
            mouseCurrentPos = cam.ScreenToWorldPoint(Input.mousePosition);
            var distance = mouseCurrentPos - mouseClickPos;

            cam.transform.position += new Vector3(-distance.x, -distance.y, 0);

        }
 
        // If LMB is released, stop moving the camera
        if (Input.GetKeyUp(KeyCode.Mouse1))
            mouseClickPos = default;

        //Debug.Log("kkkkkkkk");
    }
}
