using UnityEngine;

public class pfb_Message : UIBehavior
{
    public void ShowPopupText(MESSAGE_TYPE _type, Vector3 Pos)
    {
        var popuptxt = CreateController.Instance.GetPoolObject(PoolEnum.PopupText);
        popuptxt.transform.SetParent(this.transform);
        popuptxt.transform.localScale = Vector3.one;
        popuptxt.transform.position = Pos;
        popuptxt.gameObject.SetActive(true);

        PopupText PopupText  = popuptxt.GetComponent<PopupText>();
        PopupText.Play(_type);
    }
}
public enum MESSAGE_TYPE
{
    txt_AdsNotReady = 1,
    txt_SaveProgressSuccessful = 2,
    txt_SaveProgressFailed = 3,
    txt_AuthenFirst = 4,
    txt_FailToLoadDataGame = 5,
    txt_SignInSuccess = 6,
    txt_SignInFail = 7
}