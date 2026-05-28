using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLine : MonoBehaviour
{
    [SerializeField] Transform ballTrans;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform arrowTrans;

    private void Update()
    {
        Vector3 direction = arrowTrans.position - ballTrans.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowTrans.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        lineRenderer.SetPosition(1, arrowTrans.localPosition);
    }
}
