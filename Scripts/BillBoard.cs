using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{

    public Transform target;

    void Start()
    {
        
    }

    
    void Update()
    {
        transform.forward = target.forward;
    }
}
