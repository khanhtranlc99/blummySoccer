using Sirenix.OdinInspector;
using System;


namespace XGame
{
    /// <summary>
    /// addressable variable 修改
    /// </summary>
    [Serializable]
    public struct AddressableVariableModify
    {
        [ValueDropdown("VariablesOption")] public string VariableName;
        public string Value;


        public AddressableVariableModify(string variableName, string value)
        {
            VariableName = variableName;
            Value = value;
        }

        private string[] VariablesOption
        {
            get
            {
                //如果有addressable模块
                if (AddressableReflection.HasModule() && AddressableReflection.HasDefaultSettings())
                {
                    return AddressableReflection.Settings?.profileSettings?.GetVariableNames().ToArray();
                }

                return new string[] { };
            }
        }
    }
}