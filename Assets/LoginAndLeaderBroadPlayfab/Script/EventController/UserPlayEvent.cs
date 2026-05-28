using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserPlayEvent : PlayerContainBase
{
    public List<DataItemAcc> dataItemAccs;
    public override void Init(InformationUI_Base informationUI)
    {

        criticalRate = 15;

        //switch (idCharector)
        //{
        //    case 0:
        //        dataItemAccs = RemoteFirebase.Instance.dataItemAccsBoy;
        //        break;
        //    case 1:
        //        dataItemAccs = RemoteFirebase.Instance.dataItemAccsGirl;
        //        break;
        //    case 2:
        //        dataItemAccs = RemoteFirebase.Instance.dataItemAccsBoy2;
        //        break;
        //    case 3:
        //        dataItemAccs = RemoteFirebase.Instance.dataItemAccsGirl2;
        //        break;
        //}
    }

    public override void SetUpBody()
    {

     

        
        






    }

    public override void SetUpData()
    {
      
    }
}
