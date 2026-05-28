// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class DragController : MonoBehaviour
// {
//     public LineRenderer lineRenderer;

//     public float DragLimit = 3f;
//     public float ForceToAdd = 10f;

//     protected Camera Cam;
//     protected bool isDragging = false;

//     Vector3 MousePos
//     {
//         get
//         {
//             var pos = Cam.ScreenToWorldPoint(Input.mousePosition);
//             pos.z = 0;
//             return pos;
//         }
//     }
//     private void Start()
//     {
//         this.Cam = Camera.main;
//         this.lineRenderer.positionCount = 2;
//         lineRenderer.SetPosition(0, Vector2.zero);
//         lineRenderer.SetPosition(1, Vector2.zero);
//         lineRenderer.enabled = false;
//     }

//     private void Update()
//     {
//         if (Input.GetMouseButtonDown(0) && !isDragging)
//         {
//             DragStart();
//         }

//         if (isDragging) Drag();
//     }
//     private void LateUpdate()
//     {
//         if(Input.GetMouseButtonUp(0) && isDragging)
//         {
//             DragEnd();
//         }
//     }

//     protected void DragStart()
//     {
//         lineRenderer.enabled = true;
//         isDragging = true;
//         lineRenderer.SetPosition(0, MousePos);
//     }

//     protected void Drag()
//     {
//         Vector3 startPos = lineRenderer.GetPosition(0);
//         Vector3 CurrentPos = MousePos;

//         Vector3 Distance = CurrentPos - startPos;
//         if(Distance.magnitude <= DragLimit)
//         {
//             lineRenderer.SetPosition(1, CurrentPos);
//         }
//         else
//         {
//             Vector3 limitVector = startPos + (Distance.normalized * DragLimit);
//             lineRenderer.SetPosition(0, limitVector);
//         }

//         lineRenderer.SetPosition(1, CurrentPos);
//     }
//     protected void DragEnd()
//     {
//         isDragging=false;
//         lineRenderer.enabled = false;

//         Vector3 startPos = lineRenderer.GetPosition(0);
//         Vector3 EndPos = lineRenderer.GetPosition(1);

//         Vector3 Direction = startPos - EndPos;
//         Vector3 totalForce = Direction * ForceToAdd;

//         //add force to ball
//         //BallController.Instance.PushBall(totalForce);
//         MapController.Instance.PlayerController.Fire(totalForce);
//     }
// }
