using UnityEngine;
using UnityEditor;
using ND.Tools;
using NUnit.Framework;

namespace ND.Tools
{
    public class NDToolControlPanel : EditorWindow
    {
        [MenuItem("Tools/NDTool Control Panel ╰‿╯", priority = 0)]
        static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow(typeof(NDToolControlPanel));
        }
        NDToolOption[] options = new NDToolOption[]
        {
            new NDPlayerPrefEditor(),
            new NDTimeEditor(),
            new NDImageResizer(),
            new NDTextureCompress()
        };
        private void OnBecameVisible()
        {
            NDToolLogoDrawer.LoadResource();
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].InUse)
                {
                    options[i].Init(this);
                    options[i].OnBecameVisible();
                }
            }
        }

        private void OnGUI()
        {
            NDToolLogoDrawer.DrawLogo();
            for (int i = 0; i < options.Length; i++)
            {
                options[i].DrawInUse();
            }
            for(int i = 0; i < options.Length; ++i)
            {
                if (options[i].InUse)
                {
                    options[i].OnGUI();
                }
            }
        }

        private void OnBecameInvisible()
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].InUse)
                {
                    options[i].OnBecameInvisible();
                }
            }
        }

        private void OnEnable()
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].InUse)
                {
                    options[i].OnEnable();
                }
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].InUse)
                {
                    options[i].OnDisable();
                }
            }
        }
        private void OnDestroy()
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (options[i].InUse)
                {
                    options[i].OnDestroy();
                }
            }
        }
    }

}
