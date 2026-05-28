using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public float speed;

    public void LoadSpeed(float _speed){
        this.speed = _speed;
    }
    public virtual void Move(Vector3 direction)
    {
        this.transform.position += direction * Time.deltaTime * speed;
    }

    public virtual void Rotate(Vector3 direction){
        this.transform.localEulerAngles = direction;
    }
    public virtual void LookAt(Transform target){
        this.transform.up = target.transform.position - this.transform.position;
    }
}
