using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupRateUs : UIPopupBehavior
{
    [SerializeField] protected Button btnSend;
    [SerializeField] protected Button btnStar01, btnStar02, btnStar03, btnStar04, btnStar05;
    [SerializeField] protected List<GameObject> ListStarActiveObject = new List<GameObject>();

    private int StarVoted = 1;
    protected override void Start()
    {
        base.Start();
        btnStar01.onClick.AddListener(OnStar01);
        btnStar02.onClick.AddListener(OnStar02);
        btnStar03.onClick.AddListener(OnStar03);
        btnStar04.onClick.AddListener(OnStar04);
        btnStar05.onClick.AddListener(OnStar05);

        btnSend.onClick.AddListener(OnSend);
    }
    protected void OnSend()
    {
        ActiveNormalPopup(false);
        UIManager.Instance.pfb_PopupController.ActiveNormalPopup(false);
        if (StarVoted >= 3)
        {
            AppRating.Instance.RateAndReview();
        }
        Facade.Instance.PlayerPrefManager.IsRateUs = true;
    }
    protected void OnStar01()
    {
        this.ListStarActiveObject.ForEach(x => x.SetActive(false));
        this.ListStarActiveObject[0].SetActive(true);
        this.StarVoted = 1;
    }
    protected void OnStar02()
    {
        this.ListStarActiveObject.ForEach(x => x.SetActive(false));
        this.ListStarActiveObject[0].SetActive(true);
        this.ListStarActiveObject[1].SetActive(true);
        this.StarVoted = 2;
    }
    protected void OnStar03()
    {
        this.ListStarActiveObject.ForEach(x => x.SetActive(false));
        this.ListStarActiveObject[0].SetActive(true);
        this.ListStarActiveObject[1].SetActive(true);
        this.ListStarActiveObject[2].SetActive(true);
        this.StarVoted = 3;
    }
    protected void OnStar04()
    {
        this.ListStarActiveObject.ForEach(x => x.SetActive(false));
        this.ListStarActiveObject[0].SetActive(true);
        this.ListStarActiveObject[1].SetActive(true);
        this.ListStarActiveObject[2].SetActive(true);
        this.ListStarActiveObject[3].SetActive(true);
        this.StarVoted = 4;
    }
    protected void OnStar05()
    {
        this.ListStarActiveObject.ForEach(x => x.SetActive(false));
        this.ListStarActiveObject[0].SetActive(true);
        this.ListStarActiveObject[1].SetActive(true);
        this.ListStarActiveObject[2].SetActive(true);
        this.ListStarActiveObject[3].SetActive(true);
        this.ListStarActiveObject[4].SetActive(true);
        this.StarVoted = 5;
    }
}
