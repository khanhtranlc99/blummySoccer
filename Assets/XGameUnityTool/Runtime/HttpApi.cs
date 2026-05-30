using System;
using System.Collections.Generic;

namespace XGame
{
    public class HttpApi
    {
        public const string MINI_GAME_SERVER_TIME_URL = "https://sdk.mini.stargame.group/api/game_content/server_info";
        public const string APP_SERVER_TIME_URL = "https://api.server.xgame.xplaymobile.com/inner/user/server_info";
        public const string OLD_SERVER_TIME_URL = "https://api.xplaymobile.com/info";

        //是否有缓存的ip地址
        public static bool hasCacheIp = false;
        //ip地址
        public static string ip;


        [Serializable]
        public class MiniServerInfoResult
        {
            public bool succeed;
            public int code;
            public string codeMsg;
            public ServerInfo data;
        }

        [Serializable]
        public class AppServerInfoResult
        {
            public int code;
            public string msg;
            public string detail;
            public ServerInfo data;
        }

        [Serializable]
        public class ServerInfo
        {
            public string clientIp;
            public long serverTimeMillis;
            public long serverTimeSeconds;
        }

        public static void GetNTP(Action<long> success, Action<string> fail)
        {
            GetServerInfo(info => { success?.Invoke(info.serverTimeSeconds); }, fail);
        }

        public static void GetIp(Action<string> success, Action<string> fail)
        {
            if (hasCacheIp)
            {
                success?.Invoke(ip);
                return;
            }

            if (CommonTool.IsSeaApp())
            {
                success?.Invoke("");
                return;
            }

            GetServerInfo(info => { success?.Invoke(info.clientIp); }, fail);
        }


        private static void GetServerInfo(Action<ServerInfo> success, Action<string> fail)
        {
            XGameSdk.Log("[HttpApi] GetServerInfo..");
            //是否为海外渠道
            var sea = CommonTool.IsSeaApp();
            var urls = new Queue<string>();
#if UNITY_EDITOR
            sea = false; //编辑器模式下，不为海外模式
#endif
            if (sea)
            {
                urls.Enqueue("https://microsoft.com");
                urls.Enqueue("https://google.com");
                urls.Enqueue("https://amazon.com");
                urls.Enqueue("https://facebook.com");
            }
            else
            {
                urls.Enqueue(MINI_GAME_SERVER_TIME_URL);
                urls.Enqueue(APP_SERVER_TIME_URL);
                urls.Enqueue(OLD_SERVER_TIME_URL);
            }

            TryGetServerInfo(sea, success, fail, urls, "");
        }

        private static void TryGetServerInfo(bool isSea, Action<ServerInfo> success, Action<string> fail,
            Queue<string> urls, string error)
        {
            //没有下一个地址
            if (urls.Count <= 0)
            {
                //失败
                fail?.Invoke(error);
                return;
            }

            var url = urls.Dequeue();
            XGameSdk.Instance.HttpGet(url, (res) =>
            {
                XGameSdk.Log($"TryGetServerInfo code={res.StatusCode}, data={res.Data}");
                var header = res.Header;
                if (!isSea)
                {
                    if (url == MINI_GAME_SERVER_TIME_URL && !string.IsNullOrEmpty(res.Data))
                    {
                        var info = XJson.FromJson<MiniServerInfoResult>(res.Data);
                        if (null != info.data)
                        {
                            //缓存ip
                            if (!string.IsNullOrEmpty(info.data.clientIp))
                            {
                                hasCacheIp = true;
                                ip = info.data.clientIp;
                            }

                            success?.Invoke(info.data);
                        }
                        else
                        {
                            var nextErr = "MiniServerInfoResult.data为空";
                            //解析失败,继续解析下一个url
                            TryGetServerInfo(isSea, success, fail, urls, nextErr);
                        }
                    }
                    else if (url == APP_SERVER_TIME_URL && !string.IsNullOrEmpty(res.Data))
                    {
                        var info = XJson.FromJson<AppServerInfoResult>(res.Data);
                        if (null != info.data)
                        {
                            //缓存ip
                            if (!string.IsNullOrEmpty(info.data.clientIp))
                            {
                                hasCacheIp = true;
                                ip = info.data.clientIp;
                            }

                            success?.Invoke(info.data);
                        }
                        else
                        {
                            var nextErr = "AppServerInfoResult.data为空";
                            //解析失败,继续解析下一个url
                            TryGetServerInfo(isSea, success, fail, urls, nextErr);
                        }
                    }
                    else if (url == OLD_SERVER_TIME_URL && !string.IsNullOrEmpty(res.Data))
                    {
                        //如果国内地区，直接解析数据data
                        var data = res.Data;
                        var strs = data.Split('|');
                        var aip = strs[0];
                        var d = double.Parse(strs[1]);
                        var timeStamp = (long)(d);
                        var timeStampMs = (long)(d * 1000L);

                        //缓存ip
                        if (!string.IsNullOrEmpty(aip))
                        {
                            hasCacheIp = true;
                            ip = aip;
                        }

                        var sInfo = new ServerInfo();
                        sInfo.clientIp = aip;
                        sInfo.serverTimeSeconds = timeStamp;
                        sInfo.serverTimeMillis = timeStampMs;
                        success?.Invoke(sInfo);
                    }
                    else
                    {
                        var nextErr = url + " 域名出错了";
                        //解析失败,继续解析下一个url
                        TryGetServerInfo(isSea, success, fail, urls, nextErr);
                    }
                }
                else
                {
                    //海外地区，解析Date
                    if (header.TryGetValue("Date", out var dateString))
                    {
                        var dateTime = DateTime.Parse(dateString);
                        //计算本地时间偏差量
                        var timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime();
                        var timeStamp = (long)timeSpan.TotalSeconds;
                        var sInfo = new ServerInfo();
                        sInfo.clientIp = "";
                        sInfo.serverTimeSeconds = timeStamp;
                        sInfo.serverTimeMillis = timeStamp * 1000L;
                        success?.Invoke(sInfo);
                    }
                    else
                    {
                        var nextErr = "header no  key:Date";
                        //解析失败,继续解析下一个url
                        TryGetServerInfo(isSea, success, fail, urls, nextErr);
                    }
                }
            }, (err) =>
            {
                //解析失败,继续解析下一个url
                TryGetServerInfo(isSea, success, fail, urls, err);
            });
        }
    }
}