namespace XGame
{
    public class CommonTool
    {
        public static bool IsSeaApp()
        {
            var flag = false;
            switch (AppConfig.CHANNEL)
            {
                case AppChannel.Mar:
                    flag = XGameSdk.Instance.GetChannelName() == ChannelName.HuaWei_Sea;
                    break;
                case AppChannel.Android_Light:
                    flag = XGameSdk.Instance.GetChannelName() == ChannelName.HuaWei_Sea;
                    break;
                case AppChannel.Google:
                case AppChannel.XMYGoogle:
                case AppChannel.Google_Log_SDK:
                case AppChannel.IOSOverseas:
                case AppChannel.IOS_XGUG_Sea:
                    flag = true;
                    break;
            }

            XGameSdk.Log($"[CommonTool] IsSeaApp flag = {flag}");
            return flag;
        }
    }
}