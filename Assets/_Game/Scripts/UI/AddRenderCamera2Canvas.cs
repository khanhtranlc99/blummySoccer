using UnityEngine;
using UnityEngine.UI;

public class AddRenderCamera2Canvas : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Canvas>().worldCamera = Camera.main;
		CanvasScaler component = GetComponent<CanvasScaler>();
		if (component != null && (float)Screen.height / (float)Screen.width < 1.6f)
		{
			component.matchWidthOrHeight = 1f;
		}
	}
}
