using DG.Tweening;
public class pfb_TutorialPanel : UIBehavior
{
    public TutorialFocusController TutorialFocusController;
    public TutorialDragController TutorialDragController;
    public TutorialFocusOverlay TutorialFocusOverlay;
    public TutorialPanelMessage TutorialPanelMessage;
    public override void ActiveNormalPopup(bool active = true)
    {
        if (active)
        {
            gameObject.SetActive(true);
            CanvasComponent.alpha = 1;
        }
        else
        {
            gameObject.SetActive(false);
            CanvasComponent.alpha = 0;
        }
        CanvasComponent.blocksRaycasts = active;
        CanvasComponent.interactable = active;
        this.TutorialDragController.gameObject.SetActive(false);
        this.TutorialFocusController.gameObject.SetActive(false);
        this.TutorialFocusOverlay.gameObject.SetActive(false);
        this.TutorialPanelMessage.gameObject.SetActive(false);
    }
}
