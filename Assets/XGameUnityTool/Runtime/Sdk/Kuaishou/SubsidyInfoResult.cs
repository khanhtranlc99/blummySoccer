using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace XGame
{
    [Preserve]
    [Serializable]
    public class SubsidyInfoResult
    {
        [Preserve]
        public bool hasSubsidy; //是否有补贴
        [Preserve]
        public List<SubsidyLevelItem> subsidyItems; //补贴信息
        
        [Preserve]
        [Serializable]
        public class SubsidyLevelItem
        {

            [Preserve]
            public int buyQuantity; //原购买数量
            [Preserve]
            public int money; //原价，单位：分
            [Preserve]
            public int subsidyMoney; //优惠金额，单位：分
            [Preserve]
            public int subsidyAfterMoney; //优惠后金额，单位：分
        }
    }
}