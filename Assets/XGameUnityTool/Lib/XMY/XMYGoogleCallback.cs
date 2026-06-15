using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// xmy google 回调
    /// </summary>
    [Preserve]
    public class XMYGoogleCallback : MonoBehaviour
    {
        //登录回调
        private Action<XMYLoginResult> _loginResult;
        //Google登录回调
        private Action<XMYLoginResult> _loginGoogleResult;

        //支付回调
        private Action<XMYPayResult> _payResult;

        //广告回调
        private Action<XMYAdResult> _adResult;

        //快照回调
        private Action<XMYSnapshotResult> _snapshotResult;

        //云控回调
        private Action<string> _remoteConfigResult;
        
        private Action<string> _onInoutResult;

        //商品信息请求回调
        private Action<bool> _reqProductInfoResult;

        private Action<bool> _reqSubStateResult;
        
        private Action<string,string> _requestAssetDeliveryResult;


        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(Action<XMYLoginResult> loginResult, Action<XMYPayResult> payResult,
            Action<XMYAdResult> adResult, Action<XMYSnapshotResult> snapshotResult, Action<string> remoteConfigResult,
            Action<bool> reqProductInfoResult, Action<bool> reqSubStateResult,Action<string,string> reqAssetDeliveryResult, Action<string> onInoutResult)
        {
            _loginResult = loginResult;
            _payResult = payResult;
            _adResult = adResult;
            _snapshotResult = snapshotResult;
            _remoteConfigResult = remoteConfigResult;
            _reqProductInfoResult = reqProductInfoResult;
            _reqSubStateResult = reqSubStateResult;
            _requestAssetDeliveryResult = reqAssetDeliveryResult;
            _onInoutResult = onInoutResult;
        }

        /// <summary>
        /// 登录回调
        /// </summary>
        public void OnLoginSuc(string jsonData)
        {
            XGameSdk.Log($"OnLoginSuc {jsonData}");
            if (string.IsNullOrEmpty(jsonData))
            {
                _loginResult?.Invoke(null);
                return;
            }

            XMYLoginResult result = null;
            try
            {
                var map = XJson.FromJson<Dictionary<string, object>>(jsonData);
                result = new XMYLoginResult();
                result.uid = ParseToString(map, "userId");
                result.name = ParseToString(map, "username");
                result.token = ParseToString(map, "token");
                result.userPic = ParseToString(map, "userpic");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result = null;
            }

            _loginResult?.Invoke(result);

        }



        /// <summary>
        /// 支付回调
        /// </summary>
        public void OnPaySuc(string jsonData)
        {
            XGameSdk.Log($"OnPaySuc {jsonData}");
            try
            {
                var map = XJson.FromJson<Dictionary<string, object>>(jsonData);
                XMYPayResult payResult = new XMYPayResult();
                payResult.state = ParseToBool(map, "state");
                payResult.productId = ParseToString(map, "productId");
                payResult.cpOrderId = ParseToString(map, "cpOrderId");
                payResult.token = ParseToString(map, "token");
                payResult.code = ParseToString(map, "code");
                payResult.msg = ParseToString(map, "msg");
                _payResult?.Invoke(payResult);
            }
            catch (Exception e)
            {
                XGameSdk.Log($"OnPaySuc  error :{e.Message}");
            }
        }

        /// <summary>
        /// 广告事件回调
        /// </summary>
        public void OnAdResult(string jsonData)
        {
            XGameSdk.Log($"OnAdResult {jsonData}");
            try
            {
                var map = XJson.FromJson<Dictionary<string, object>>(jsonData);
                XMYAdResult result = new XMYAdResult();
                result.eventType = ParseToString(map, "eventType");
                result.adType = ParseToString(map, "adType");
                result.adId = ParseToString(map, "adId");
                _adResult?.Invoke(result);
            }
            catch (Exception e)
            {
                XGameSdk.Log($"OnAdResult  error :{e.Message}");
            }
        }

        /// <summary>
        /// 游戏快照存档回调
        /// </summary>
        public void OnSnapshotResult(string jsonData)
        {
            XGameSdk.Log($"OnSnapshotResult {jsonData}");
            try
            {
                var result = XJson.FromJson<XMYSnapshotResult>(jsonData);
                _snapshotResult?.Invoke(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                XGameSdk.Log($"OnSnapshotResult error: {e}");
            }
        }

        /// <summary>
        /// 云控配置返回
        /// </summary>
        public void OnRemoteResult(string json)
        {
            XGameSdk.Log($"OnRemoteResult {json}");
            _remoteConfigResult?.Invoke(json);
        }


        /// <summary>
        /// 订阅回调
        /// </summary>
        public void OnSubResult(string json)
        {
            XGameSdk.Log($"OnSubResult json: {json}");
            var result = json.ToLower() == "true";
            _reqSubStateResult?.Invoke(result);
        }


        /// <summary>
        /// 商品信息请求成功/失败回调
        /// </summary>
        public void OnProductResult(string json)
        {
            XGameSdk.Log($"OnProductResult json: {json}");
            var result = json.ToLower() == "true";
            _reqProductInfoResult?.Invoke(result);
        }


        public void OnPadResult(string json)
        {
            XGameSdk.Log($"OnPadResult json: {json}");
            try
            {
                var map = XJson.FromJson<Dictionary<string, object>>(json);
                var code = ParseToString(map, "code");
                var data = ParseToString(map, "data");
                _requestAssetDeliveryResult?.Invoke(code,data);
            }
            catch (Exception e)
            {
                XGameSdk.Log($"OnPadResult error: {e.Message}");
            }
        }

        public void OnInOutResult(string ext)
        {
            XGameSdk.Log($"OnInOutResult ext: {ext}");
            try
            {
                _onInoutResult?.Invoke(ext);
            }
            catch (Exception e)
            {
                XGameSdk.Log($"OnInOutResult error: {e.Message}");
            }
        }
        
        
        
        
        
        //解析成int
        private static int ParseToInt(Dictionary<string, object> map, string key)
        {
            if (map.TryGetValue(key, out var match))
            {
                return int.Parse(match.ToString());
            }

            return default;
        }

        //解析成string
        private static string ParseToString(Dictionary<string, object> map, string key)
        {
            if (map.TryGetValue(key, out var match))
            {
                return match.ToString();
            }

            return default;
        }

        //解析成bool
        private static bool ParseToBool(Dictionary<string, object> map, string key)
        {
            if (map.TryGetValue(key, out var match))
            {
                return match.ToString().ToLower() == "true";
            }

            return default;
        }
    }
}