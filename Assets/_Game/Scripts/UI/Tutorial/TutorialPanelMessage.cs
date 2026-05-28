using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum AnchorPresetType
{
    BOTTOM_LEFT = 1,
    BOTTOM_CENTER = 2,
    BOTTOM_RIGHT = 3,
    MIDDLE_LEFT = 4,
    MIDDLE_CENTER = 5,
    MIDDLE_RIGHT = 6,
    TOP_LEFT = 7,
    TOP_CENTER = 8,
    TOP_RIGHT = 9,
    STRETCH_FILL = 10,
    STRETCH_HORIZONTAL = 11,
    STRETCH_VERTICAL = 12,
}
public class TutorialPanelMessage : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI txtMessage;
    public RectTransform Layout;


    public void Init(string message)
    {
        this.gameObject.SetActive(true);
        txtMessage.text = message;

        LayoutRebuilder.ForceRebuildLayoutImmediate(this.Layout);
    }

    public void SetAnchorPos(Vector3 anchorPos)
    {
        this.Layout.anchoredPosition = anchorPos;
    }

    public void SetAnchorPresets(AnchorPresetType _type)
    {
        switch(_type)
        {
            case AnchorPresetType.BOTTOM_LEFT:
                Layout.anchorMin = new Vector2(0, 0);
                Layout.anchorMax = new Vector2(0, 0);
                Layout.pivot = new Vector2(0, 0);
                break;
            case AnchorPresetType.BOTTOM_CENTER:
                Layout.anchorMin = new Vector2(0.5f, 0);
                Layout.anchorMax = new Vector2(0.5f, 0);
                Layout.pivot = new Vector2(0.5f, 0);
                break;
            case AnchorPresetType.BOTTOM_RIGHT:
                Layout.anchorMin = new Vector2(1, 0);
                Layout.anchorMax = new Vector2(1, 0);
                Layout.pivot = new Vector2(1, 0);
                break;
            case AnchorPresetType.MIDDLE_LEFT:
                Layout.anchorMin = new Vector2(0, 0.5f);
                Layout.anchorMax = new Vector2(0, 0.5f);
                Layout.pivot = new Vector2(0, 0.5f);
                break;
            case AnchorPresetType.MIDDLE_CENTER:
                Layout.anchorMin = new Vector2(0.5f, 0.5f);
                Layout.anchorMax = new Vector2(0.5f, 0.5f);
                Layout.pivot = new Vector2(0.5f, 0.5f);
                break;
            case AnchorPresetType.MIDDLE_RIGHT:
                Layout.anchorMin = new Vector2(1, 0.5f);
                Layout.anchorMax = new Vector2(1, 0.5f);
                Layout.pivot = new Vector2(1, 0.5f);
                break;
            case AnchorPresetType.TOP_LEFT:
                Layout.anchorMin = new Vector2(0, 1);
                Layout.anchorMax = new Vector2(0, 1);
                Layout.pivot = new Vector2(0, 1);
                break;
            case AnchorPresetType.TOP_CENTER:
                Layout.anchorMin = new Vector2(0.5f, 1);
                Layout.anchorMax = new Vector2(0.5f, 1);
                Layout.pivot = new Vector2(0.5f, 1);
                break;
            case AnchorPresetType.TOP_RIGHT:
                Layout.anchorMin = new Vector2(1, 1);
                Layout.anchorMax = new Vector2(1, 1);
                Layout.pivot = new Vector2(1, 1);
                break;
            case AnchorPresetType.STRETCH_FILL:
                Layout.anchorMin = new Vector2(0, 0);
                Layout.anchorMax = new Vector2(1, 1);
                Layout.pivot = new Vector2(0.5f, 0.5f);
                Layout.anchoredPosition = Vector2.zero;
                Layout.sizeDelta = Vector2.zero;
                break;
            case AnchorPresetType.STRETCH_HORIZONTAL:
                Layout.anchorMin = new Vector2(0, 0.5f);
                Layout.anchorMax = new Vector2(1, 0.5f);
                Layout.pivot = new Vector2(0.5f, 0.5f);
                Layout.anchoredPosition = new Vector2(0, 0);
                break;
            case AnchorPresetType.STRETCH_VERTICAL:
                Layout.anchorMin = new Vector2(0.5f, 0);
                Layout.anchorMax = new Vector2(0.5f, 1);
                Layout.pivot = new Vector2(0.5f, 0.5f);
                Layout.anchoredPosition = new Vector2(0, 0);
                break;
        }
    }
    public void SetPivot(AnchorPresetType _type)
    {
        switch (_type)
        {
            case AnchorPresetType.BOTTOM_LEFT:
                Layout.pivot = new Vector2(0, 0);
                break;
            case AnchorPresetType.BOTTOM_CENTER:
                Layout.pivot = new Vector2(0.5f, 0);
                break;
            case AnchorPresetType.BOTTOM_RIGHT:
                Layout.pivot = new Vector2(1, 0);
                break;
            case AnchorPresetType.MIDDLE_LEFT:
                Layout.pivot = new Vector2(0, 0.5f);
                break;
            case AnchorPresetType.MIDDLE_CENTER:
                Layout.pivot = new Vector2(0.5f, 0.5f);
                break;
            case AnchorPresetType.MIDDLE_RIGHT:
                Layout.pivot = new Vector2(1, 0.5f);
                break;
            case AnchorPresetType.TOP_LEFT:
                Layout.pivot = new Vector2(0, 1);
                break;
            case AnchorPresetType.TOP_CENTER:
                Layout.pivot = new Vector2(0.5f, 1);
                break;
            case AnchorPresetType.TOP_RIGHT:
                Layout.pivot = new Vector2(1, 1);
                break;

            default:
                SetPivot(AnchorPresetType.MIDDLE_CENTER);
                break;
        }
    }
}
