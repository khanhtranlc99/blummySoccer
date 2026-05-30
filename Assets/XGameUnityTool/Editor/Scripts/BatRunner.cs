using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 批处理运行
    /// </summary>
    public class BatRunner : MonoBehaviour
    {
        /// <summary>
        /// 执行批处理
        /// </summary>
        public static void Run(string batPath, params string[] args)
        {
            //获取批处理
            if (!File.Exists(batPath))
            {
                throw new Exception($"丢失 {batPath}");
            }

            string inputArg = string.Empty;
            if (args != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var element in args)
                {
                    sb.Append(element);
                    sb.Append(',');
                }

                inputArg = sb.ToString();
                if (inputArg.EndsWith(","))
                {
                    inputArg.Remove(inputArg.Length - 1, 1);
                }
            }

            FileInfo info = new FileInfo(batPath);
            batPath = info.FullName;
            try
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = batPath;
                    proc.StartInfo.WorkingDirectory = info.Directory.FullName;
                    if (inputArg.Length > 0)
                    {
                        proc.StartInfo.Arguments = inputArg;
                    }

                    proc.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString());
            }
        }
    }
}