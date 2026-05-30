using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using xgame.Zip;
using CompressionLevel = xgame.Zlib.CompressionLevel;

namespace XGame
{
    public class ZipTool
    {
        /// <summary>
        /// 压缩成zip
        /// </summary>
        /// <param name="outputZipFile">xx.zip保存位置</param>
        /// <param name="manifest">压缩成zip的清单，key:源文件，value:zip中的相对路径（填null或""表示不处理）</param>
        public static void Zip(string outputZipFile, Dictionary<string, string> manifest)
        {
            using (ZipFile zip = new ZipFile(outputZipFile, Encoding.Default))
            {
                foreach (var kv in manifest)
                {
                    var from = kv.Key;
                    var inside = kv.Value ?? "";
                    zip.AddItem(from, inside);
                }
#if !UNITY_2021_1_OR_NEWER
                zip.CompressionLevel = CompressionLevel.BestCompression;
#endif
                zip.Save();
            }
        }

        private static object lockProcess = new object();
        private static float cacheZipProcess = 0f;
        private static float zipProcess = 0f;
        private static string zipFilePath;


        /// <summary>
        /// 压缩成zip
        /// </summary>
        /// <param name="outputZipFile">xx.zip保存位置</param>
        /// <param name="manifest">压缩成zip的清单，key:源文件，value:zip中的相对路径（填null或""表示不处理）</param>
        public static async void ZipWithProcessBar(string outputZipFile, Dictionary<string, string> manifest,
            Action complete)
        {
            cacheZipProcess = -1;
            //订阅进度
            EditorApplication.update += OnZipProcessChange;
            await Task.Run(() =>
            {
                lock (lockProcess)
                {
                    zipFilePath = outputZipFile;
                    using (ZipFile zip = new ZipFile(outputZipFile, Encoding.Default))
                    {
                        var total = manifest.Count;
                        var current = -1;
                        var stepLength = 1 / total; //每一步的进度
                        zip.SaveProgress += (a, e) =>
                        {
                            if (e.EventType == ZipProgressEventType.Saving_Started)
                            {
                                Debug.Log($"开启生成压缩文件：{outputZipFile}...");
                            }
                            else if (e.EventType == ZipProgressEventType.Saving_BeforeWriteEntry)
                            {
                                current += 1;
                                zipProcess = (float)current / total;
                            }
                            else if (e.EventType == ZipProgressEventType.Saving_EntryBytesRead)
                            {
                                var add = (double)e.BytesTransferred / e.TotalBytesToTransfer * stepLength;
                                zipProcess = (float)current / total + (float)add;
                            }
                            else if (e.EventType == ZipProgressEventType.Saving_Completed)
                            {
                                zipProcess = 1;
                                Debug.Log("ZIP文件保存完成。");
                            }
                        };

                        foreach (var kv in manifest)
                        {
                            var from = kv.Key;
                            var inside = kv.Value ?? "";
                            zip.AddItem(from, inside);
                        }
#if !UNITY_2021_1_OR_NEWER
                        zip.CompressionLevel = CompressionLevel.BestCompression;
#endif
                        zip.Save();
                    }
                }
            });
            EditorApplication.update -= OnZipProcessChange;
            EditorUtility.ClearProgressBar();
            complete?.Invoke();
        }

        private static void OnZipProcessChange()
        {
            if (cacheZipProcess == zipProcess)
            {
                return;
            }

            cacheZipProcess = zipProcess;
            EditorUtility.DisplayCancelableProgressBar($"压缩文件中...进度：{(zipProcess * 100).ToString("F1")}%",
                $"生成：{zipFilePath}...", zipProcess);
            Debug.Log($"压缩进度：{zipProcess * 100f}%");
        }

        /// <summary>
        /// 解压zip到指定文件夹
        /// </summary>
        /// <param name="zipFile">zip文件路径</param>
        /// <param name="output">解压zip内容到指定目录（不会补充zip文件名作为目录）</param>
        /// <param name="complete">完成回调</param>
        /// <param name="onProcess">解压进度</param>
        /// <exception cref="Exception"></exception>
        public static void UnZip(string zipFile, string output, Action complete, Action<float> onProcess = null)
        {
            if (!zipFile.EndsWith(".zip"))
            {
                throw new Exception($"只支持.zip文件");
            }

            if (!File.Exists(zipFile))
            {
                throw new Exception($"找不到文件：{zipFile}");
            }

            //解压文件到目录
            using (ZipFile zip = ZipFile.Read(Path.GetFullPath(zipFile)))
            {
                var totalFiles = zip.Count;
                var filesExtracted = 0;
                zip.ExtractProgress += (a, e) =>
                {
                    if (e.EventType != ZipProgressEventType.Extracting_BeforeExtractEntry)
                        return;
                    filesExtracted++;
                    var progress = (float)filesExtracted / totalFiles;
                    onProcess?.Invoke(progress);
                    if (filesExtracted >= totalFiles)
                    {
                        complete?.Invoke();
                    }
                };
                zip.ExtractAll(output, ExtractExistingFileAction.OverwriteSilently);
            }
        }
    }
}