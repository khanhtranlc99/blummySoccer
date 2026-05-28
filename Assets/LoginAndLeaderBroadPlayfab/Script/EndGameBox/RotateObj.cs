using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObj : MonoBehaviour
{
    public int rotate = 10;
    void Update()
    {
        this.transform.localEulerAngles -= new Vector3(0,0, rotate)  ;
    }
}
