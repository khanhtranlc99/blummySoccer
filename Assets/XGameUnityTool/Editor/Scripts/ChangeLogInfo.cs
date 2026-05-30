using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    [Serializable]
    [HideMonoScript]
    public class ChangeLogInfo : ScriptableObject
    {
        [LabelText("新增：（新增功能）")]
        //新增功能
        public List<string> Features = new List<string>();

        [LabelText("修复：（修复bug）")]

        //修复bug
        public List<string> Fixed = new List<string>();

        [LabelText("变更：（对于某些已存在的功能所发生的逻辑变化）")]

        //变更
        public List<string> Changed = new List<string>();

        [LabelText("优化：（性能或结构上的优化，并未带来实际逻辑变化）")]

        //性能或结构上的优化，并未带来实际逻辑变化
        public List<string> Refactored = new List<string>();

        [LabelText("删除：（已删除的功能）")]

        //删除
        public List<string> Removed = new List<string>();

        [LabelText("重大修复版本")] public bool IsMajorFixed = false;

        [ShowIf("IsMajorFixed")] [LabelText("重大修复提示")]
        public string MajorFixedContent = "";


        public static ChangeLogInfo Default
        {
            get
            {
                return XGameEditorUtil.LoadOrCreate<ChangeLogInfo>(
                    "Assets/XGameUnityTool/Editor/CHANG_LOG_DRAFT.asset");
            }
        }

        public void ClearAll(bool save = false)
        {
            Features.Clear();
            Fixed.Clear();
            Changed.Clear();
            Refactored.Clear();
            Removed.Clear();
            if (save)
            {
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
        }
    }
}