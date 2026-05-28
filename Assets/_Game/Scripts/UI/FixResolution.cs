using UnityEngine;
using UnityEngine.UI;

public class FixResolution : MonoBehaviour
{
    private void Start()
	{
		CanvasScaler component = GetComponent<CanvasScaler>();
		if (component != null && (float)Screen.height / (float)Screen.width > 2.1f)
		{
			component.matchWidthOrHeight = 0f;
		}
	}
}
