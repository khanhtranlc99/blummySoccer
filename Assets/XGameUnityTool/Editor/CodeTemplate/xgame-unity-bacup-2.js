
//渠道ID
let channelId = -1;
//友盟参数
let umaAppKey = "";
let openUma = false;
//DEBUG模式
let DEBUG_MODE = false;

//字符串
let U_STRINGS = {
  MESSAGE_ID: "messageId",
  PRODUCT_ID: "productId",
  PRICE: "price",
  PRODUCT_DESC: "productDesc",
  PRODUCT_NAME: "productName",
  CODE: "code",
  SCENE_NAME: "sceneName",
  ORDER_ID: "orderId",
  TYPE: "type",
  X: "x",
  Y: "y",
  IMAGE_URL: "imageUrl",
  TITLE: "title",
  UMA_EVENT_ID: "uma_event_id",
  UMA_EVENT_PARAM: "uma_event_param",
  HEADER: "header",
  URL: "url",
  CALL_BACK_ID: "call_back_id",
  SUCCESS: "success",
  DATA: "data",
  PIVOT_X: "pivot_x",
  PIVOT_Y: "pivot_y",
  NAME: "name",
  KEY: "key",
  VERSION: "version",
  CONTENT: "content",
  HAS_HEADER: "has_header",
  TIME_OUT: "time_out",
  EVENT_ID: "event_id",
  EVENT_DATA: "event_data",
  IMAGE_URL_ID: "imageUrlId",
  PLAN_ID: "planId",
  COUNT: "count",
}

//方法ID号
let U_METHOD_ID = {
  //初始化SDK
  INIT_SDK: 1000,
  //是否有插屏
  HAS_INTERS: 1001,
  //显示插屏
  SHOW_INTERS: 1002,
  //是否有视频广告
  HAS_VIDEO: 1003,
  //展示广告
  SHOW_VIDEO: 1004,
  //支付
  PAY: 1005,
  //兑换礼物
  GIFT: 1006,
  //登录
  LOGIN: 1007,
  //显示banner
  SHOW_BANNER: 1008,
  //隐藏banner
  HIDE_BANNER: 1009,
  //提交订单号
  COMMIT_ORDER: 1010,
  //显示自定义广告
  SHOW_CUSTOM_AD: 1011,
  //隐藏自定义广告
  HIDE_CUSTOM_AD: 1012,
  //获取设备类型
  GET_DEVICE_TYPE: 1013,
  //分享
  SHARE_APP: 1014,
  //友盟打点
  UMA_TRACK_EVENT: 1015,
  //获取sdk用户ID
  GET_SDK_USER_ID: 1016,
  //POST
  HTTP_POST: 1017,
  //GET
  HTTP_GET: 1018,
  //显示自定义广告（新版本）
  SHOW_CUSTOM_AD_POS: 1019,
  //隐藏自定义广告（新版本）
  HIDE_CUSTOM_AD_POS: 1020,
  //获取云存档keys
  GET_CLOUD_ARCHIVE_KEYS: 1021,
  //设置云存档数据
  SET_CLOUD_ARCHIVE_DATA: 1022,
  //同步云存档
  FORCE_SYNC_CLOUD_ARCHIVE: 1023,
  //获取云存档key数据
  GET_CLOUD_ARCHIVE_DATA: 1024,
  //事件上报
  REPORT_EVENT: 1025,
  //设置分享样式
  SET_SHARE_APP_STYLE: 1026,

}

let _Cache_Share_Trigger = false;

//customAd map
let _customAdMap = new Map();

let _bannerMap = new Map();

//新customAd map
let _customAdPosMap = new Map();

//xgame_archive,云存储拓展
let _Xgame_Archive = null;

//小游戏互推扩展
let _Xgame_MPush = null;

let _sysInfo = wx.getSystemInfoSync();

//分享APP样式
let _onShareAppStyle = {
  title: "",
  imageUrl: "",
  imageUrlId: ""
}

//默认分享样式
let _defaultShareAppStyle = null;

let XGameUnitySdk = {

  //Unity调用SDK方法 并返回
  MethodGet: function (msg) {
    this.log(" MethodGet" + msg);
    var data = JSON.parse(msg);
    var id = data[U_STRINGS.MESSAGE_ID];
    switch (id) {
      case U_METHOD_ID.HAS_INTERS://是否有插页广告
        return this.getIntersFlag();
      case U_METHOD_ID.HAS_VIDEO://是否有视频广告
        return this.getVideoFlag();
      case U_METHOD_ID.GET_DEVICE_TYPE://获取设备类型
        return this.getDeviceType();
      case U_METHOD_ID.GET_SDK_USER_ID://获取SDK用户ID
        return this.getSdkUserId();
    }
    return "";
  },

  //Unity调用SDK方法
  MethodCall: function (msg) {
    this.log("##CallMethod" + msg);
    var data = JSON.parse(msg);
    var id = data[U_STRINGS.MESSAGE_ID];
    switch (id) {
      case U_METHOD_ID.INIT_SDK://初始化SDK
        this.initSdk();
        break;
      case U_METHOD_ID.LOGIN://登录
        this.login();
        break;
      case U_METHOD_ID.SHOW_BANNER://展示banner
        this.showBanner(msg);
        break;
      case U_METHOD_ID.SHOW_INTERS://展示插页
        this.showInters(msg);
        break;
      case U_METHOD_ID.SHOW_VIDEO://显示视频广告
        this.showVideo(msg);
        break;
      case U_METHOD_ID.PAY://支付
        this.pay(msg);
        break;
      case U_METHOD_ID.COMMIT_ORDER:
        this.commitOrder(msg);
        break;
      case U_METHOD_ID.HIDE_BANNER:
        this.hideBanner();
        break;
      case U_METHOD_ID.SHOW_CUSTOM_AD:
        this.showCustomAd(msg);
        break;
      case U_METHOD_ID.HIDE_CUSTOM_AD:
        this.hideCustomAd(msg);
        break;
      case U_METHOD_ID.SHARE_APP:
        this.shareApp(msg);
        break;
      case U_METHOD_ID.HTTP_POST:
        this.xgu_Http_Post(msg);
        break
      case U_METHOD_ID.HTTP_GET:
        this.xgu_Http_Get(msg);
        break;
      case U_METHOD_ID.SHOW_CUSTOM_AD_POS:
        this.showCustomAdPos(msg);
        break;
      case U_METHOD_ID.HIDE_CUSTOM_AD_POS:
        this.hideCustomAdPos(msg);
        break;
      case U_METHOD_ID.UMA_TRACK_EVENT:
        this.umaTrackEvent(msg);
        break;
      case U_METHOD_ID.GET_CLOUD_ARCHIVE_KEYS:
        this.xgu_archive_get_keys(msg);
        break;
      case U_METHOD_ID.SET_CLOUD_ARCHIVE_DATA:
        this.xgu_archive_save64K(msg);
        break;
      case U_METHOD_ID.FORCE_SYNC_CLOUD_ARCHIVE:
        this.xgu_archive_forceSync();
        break;
      case U_METHOD_ID.GET_CLOUD_ARCHIVE_DATA:
        this.xgu_archive_req64k(msg);
        break;
      case U_METHOD_ID.REPORT_EVENT:
        this.reportEvent(msg);
        break;
      case U_METHOD_ID.SET_SHARE_APP_STYLE:
        this.setShareAppStyle(msg);
        break;

    }
  },


  //弹出toast
  xgu_showToast: function (content) {
    wx.showToast({
      title: content,
      icon: 'none'
    });
  },





  send_to_unity (method, session, data) {

    let fn = method;
    fn(data, (rsp) => {
      method.xxxx;
      data.da;
      self.noticeUnity("common", session, rsp);
    })

  }
}


//声明方法名
let METHODS = {
  //初始化SDK
  ID_INIT_SDK: 1000,

  //是否有插屏
  ID_HAS_INTERS: 1001,

  //显示插屏
  ID_SHOW_INTERS: 1002,

  //是否有视频广告
  ID_HAS_VIDEO: 1003,

  //展示广告
  ID_SHOW_VIDEO: 1004,

  //支付
  ID_PAY: 1005,

  //兑换礼物
  ID_GIFT: 1006,

  //登录
  ID_LOGIN: 1007,

  //显示banner
  ID_SHOW_BANNER: 1008,

  //隐藏banner
  ID_HIDE_BANNER: 1009,

  //提交订单号
  ID_COMMIT_ORDER: 1010,

  //显示自定义广告
  ID_SHOW_CUSTOM_AD: 1011,

  //隐藏自定义广告
  ID_HIDE_CUSTOM_AD: 1012,

  //获取设备类型
  ID_GET_DEVICE_TYPE: 1013,

  //分享
  ID_SHARE_APP: 1014,

  //友盟统计
  ID_UMA_TRACK_EVENT: 1015,

  //获取sdk用户ID
  ID_GET_SDK_USER_ID: 1016,

  //http post
  ID_HTTP_POST: 1017,

  //http get
  ID_HTTP_GET: 1018,

  //显示自定义广告（新版本）
  ID_SHOW_CUSTOM_AD_POS: 1019,

  //隐藏自定义广告（新版本）
  ID_HIDE_CUSTOM_AD_POS: 1020,

  //获取云存档keys
  ID_GET_CLOUD_ARCHIVE_KEYS: 1021,

  //设置云存档数据
  ID_SET_CLOUD_ARCHIVE_DATA: 1022,

  //同步云存档
  ID_FORCE_SYNC_CLOUD_ARCHIVE: 1023,

  //获取云存档key数据
  ID_GET_CLOUD_ARCHIVE_DATA: 1024,

  //事件上报
  ID_REPORT_EVENT: 1025,

  //设置分享样式
  ID_SET_SHARE_APP_STYLE: 1026,

  //请求内推广告
  MPush_ReqItems: 1027,
};

//声明方法
let methodCaller = [];
methodCaller[""] = (data, session) => {

};

//声明get方法
let methodGetter = [];
methodGetter[""]


//与unity关联的部分
window["MethodGet"] = (msg) => { return XGameUnitySdk.MethodGet(msg); };
window["MethodCall"] = (json) => {
  var msg = JSON.parse(json);
  const method = msg.method;
  const data = msg.data;
  const session = msg.session;
  methodCaller[method](data, session);
};


wx.onShow(() => { XGameUnitySdk.xgOnShow(); });
wx.onHide(() => { XGameUnitySdk.xgOnHide(); });
//分享样式设置
wx.onShareAppMessage(() => {
  var arg_title = _onShareAppStyle.title.length > 0 ? _onShareAppStyle.title : undefined;
  var arg_imageUrl = _onShareAppStyle.imageUrl.length > 0 ? _onShareAppStyle.imageUrl : undefined;
  var arg_imageUrlId = _onShareAppStyle.imageUrlId.length > 0 ? _onShareAppStyle.imageUrlId : undefined;
  return {
    title: arg_title,
    imageUrl: arg_imageUrl,
    imageUrlId: arg_imageUrlId,
  }
});
console.log("## xgame-unity load done");
XGameUnitySdk.tryInitUma();

