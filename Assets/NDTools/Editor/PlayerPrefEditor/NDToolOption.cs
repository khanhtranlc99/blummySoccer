using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ND.Tools
{
    public abstract class NDToolOption
    {
        protected EditorWindow window;
        protected bool inUse = false;
        public bool InUse
        {
            get
            {
                return inUse;
            }
            set
            {
                if (inUse != value)
                {
                    inUse = value;
                    if (inUse)
                    {
                        OnUse();
                    }
                    else
                    {
                        OnDontUse();
                    }
                }
            }
        }
        public virtual void DrawInUse()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(GetOptionName());
            InUse = EditorGUILayout.Toggle(InUse);
            EditorGUILayout.EndHorizontal();
        }
        public virtual void Init(EditorWindow windowParent)
        {
            this.window = windowParent;
        }
        protected virtual void OnUse() { }
        protected virtual void OnDontUse() { }
        protected virtual string GetOptionName() { return "NDTool Option"; }
        public virtual void OnBecameVisible() { }
        public virtual void OnBecameInvisible() { }
        public virtual void OnGUI() { }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void OnDestroy() { }
    }
}

