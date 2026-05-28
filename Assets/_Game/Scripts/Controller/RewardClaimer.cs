using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardClaimer : MonoBehaviour
{
    //Do anim to modify data
    public void GetReward(RewardStruct RewardStruct, Vector3 startPos = default(Vector3))
    {
        ProcessUserDataReward(RewardStruct.REWARD_TYPE, RewardStruct.numberReward);
        switch (RewardStruct.REWARD_TYPE)
        {
            //case REWARD_TYPE.MONSTER_9_STAR: return;
            default:
                UIManager.Instance.pfb_VFXUI.OnGetReward(startPos, RewardStruct); break;
        }
    }
    protected void ProcessUserDataReward(REWARD_TYPE Reward_Type, int number)
    {
        if (number == 0)
            return;
        switch (Reward_Type)
        {
            // case REWARD_TYPE.GOLD: Facade.Instance.ConfigManager.UserData.UserPropertyData.Gold += number; return;
        }

        EventHandler.ExecuteEvent(EventID.SaveUserProperty);
    }
}
