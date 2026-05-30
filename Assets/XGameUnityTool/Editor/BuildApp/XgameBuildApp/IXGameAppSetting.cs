using System.Collections.Generic;

namespace XGame
{
    public interface IXGameAppSetting
    {
        /// <summary>
        /// 绑定渠道
        /// </summary>
        AppChannel GetChannel { get; }

        /// <summary>
        /// 开启Addressable设置
        /// </summary>
        bool GetEnableAddressableSetting { get; }


        /// <summary>
        /// addressable  Profile  
        /// </summary>
        string GetAddressableUseProfile { get; }


        /// <summary>
        /// addressable variables 修改
        /// </summary>
        List<AddressableVariableModify> GetAAVariablesModify { get; }
    }
}