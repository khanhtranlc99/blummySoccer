

public class pfb_Result : UIBehavior
{
    public PopupWin PopupWin;
    public PopupLose PopupLose;

    public override void ActiveNormalPopup(bool active = true)
    {
        base.ActiveNormalPopup(active);
        this.PopupWin.ForceActiveNormalPopup(false);
        this.PopupLose.ForceActiveNormalPopup(false);
    }
}
