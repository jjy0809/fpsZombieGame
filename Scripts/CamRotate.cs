using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotate : MonoBehaviour
{

    public float rotSpeed = 200f;

    float mx = 0;
    float my = 0;

    int Mode = 0;

    int click = 0;


    void Start()
    {
        
    }

    
    void Update()
    {
        
        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }
        /*
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Mode = 0;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Mode = 1;
        }

        if (Mode == 0)
        {
            rotSpeed = 200f;
            click = 0;
        }

        else if (Mode == 1)
        {
            if (Input.GetMouseButtonUp(1))
            {
                click = 0;
            }

            else if (Input.GetMouseButtonDown(1))
            {
                click = 1;
            }

        }

        if (click == 1)
        {
            rotSpeed = 1f;
        }

        else
        {
            rotSpeed = 200f;
        }
        */
        float mouse_X = Input.GetAxis("Mouse X");
        float mouse_Y = Input.GetAxis("Mouse Y");

        mx += mouse_X * rotSpeed * Time.deltaTime;
        my += mouse_Y * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, -90f, 90f);
        //mx = Mathf.Clamp(mx, -180f, 180f);

        

        // if (Input.GetMouseButton(1)
        // {
        // Camera.orthographicSize;
        //  Camera.FieldOfViewAxis.;
        // }

        if (Input.GetKey(KeyCode.E))
        {
            transform.eulerAngles = new Vector3(-my, mx + 180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(-my, mx, 0);
        }
    }
}
