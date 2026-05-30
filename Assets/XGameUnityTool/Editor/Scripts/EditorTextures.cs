using UnityEngine;

namespace XGame
{
    public class EditorTextures
    {
        private static Texture __icon_android = null;
        public static Texture icon_android => XGameEditorUtil.LoadEditorTexture("icon_android", ref __icon_android);
        private static Texture __icon_douyin = null;
        public static Texture icon_douyin => XGameEditorUtil.LoadEditorTexture("icon_douyin", ref __icon_douyin);
        private static Texture __icon_googleplay = null;

        public static Texture icon_googleplay =>
            XGameEditorUtil.LoadEditorTexture("icon_googleplay", ref __icon_googleplay);

        private static Texture __icon_ios = null;
        public static Texture icon_ios => XGameEditorUtil.LoadEditorTexture("icon_ios", ref __icon_ios);
        private static Texture __icon_oppo = null;
        public static Texture icon_oppo => XGameEditorUtil.LoadEditorTexture("icon_oppo", ref __icon_oppo);
        private static Texture __icon_vivo = null;
        public static Texture icon_vivo => XGameEditorUtil.LoadEditorTexture("icon_vivo", ref __icon_vivo);
        private static Texture __icon_wechat = null;
        public static Texture icon_wechat => XGameEditorUtil.LoadEditorTexture("icon_wechat", ref __icon_wechat);
        
        private static Texture __icon_kuaishou = null;
        public static Texture icon_kuaishou => XGameEditorUtil.LoadEditorTexture("icon_kuaishou", ref __icon_kuaishou);
        
        private static Texture __icon_huawei = null;
        public static Texture icon_huawei => XGameEditorUtil.LoadEditorTexture("icon_huawei", ref __icon_huawei);
        private static Texture __icon_bilibili = null;
        public static Texture icon_bilibili => XGameEditorUtil.LoadEditorTexture("icon_bilibili", ref __icon_bilibili);

        
    }
}