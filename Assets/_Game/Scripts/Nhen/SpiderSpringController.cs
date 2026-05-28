using UnityEngine;

public class SpiderSpringController : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform springStart;
    [SerializeField] private Transform springEnd;
    private void Start()
    {
        lineRenderer.positionCount = 2;
    }
    private void Update()
    {
        lineRenderer.SetPosition(0, springStart.position);
        if (springEnd.position != lineRenderer.GetPosition(1))
            lineRenderer.SetPosition(1, springEnd.position);
    }
}
