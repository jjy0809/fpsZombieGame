using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{

    public  float rotSpeed = 200f;

    float mx = 0;

    int Mode = 0;

    int click = 0;

    void Start()
    {
        
    }

    
    void Update()
    {
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

        if (GameManager.gm.gState != GameManager.GameState.Run)
        {
            return;
        }

        float mouse_X = Input.GetAxis("Mouse X");

        mx += mouse_X * rotSpeed * Time.deltaTime;

       // mx = Mathf.Clamp(mx, -180f, 180f);

        transform.eulerAngles = new Vector3(0, mx, 0);
    }
}
