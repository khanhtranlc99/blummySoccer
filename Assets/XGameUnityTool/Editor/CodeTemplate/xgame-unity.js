//渠道ID
let channelId = -1;
//友盟参数
let umaAppKey = "";
let openUma = false;

//$__pluginGeConfig__start
//引力引擎实例
let __pluginGeConfig = {
  turnOn: false,//是否开启
  accessToken: "",
  appid: "",
  version: 0,
};
//$__pluginGeConfig__end

let __pluginGe = window["xgame-plugin-ge-wx"];

//DEBUG模式
let DEBUG_MODE = true;

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

//广告模块
let _Xgame_Ad = null;

//用户模块
let _Xgame_User = null;


let _sysInfo = wx.getSystemInfoSync();

//互推广告字典
let _mpushContentMap = new Map();


//分享APP样式
let _onShareAppStyle = {
  title: "",
  imageUrl: "",
  imageUrlId: ""
}


//声明方法名
let METHODS = {
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

  //友盟统计
  UMA_TRACK_EVENT: 1015,

  //获取sdk用户ID
  GET_SDK_USER_ID: 1016,

  //http post
  HTTP_POST: 1017,

  //http get
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

  //请求内推广告
  MPUSH_REQ_ITEMS: 1027,

  //点击内推广告
  MPUSH_CLICK_ITEM: 1028,

  //显示toast
  SHOW_TOAST: 1029,
};

//声明方法
let __caller = [];
//初始化sdk
__caller[METHODS.INIT_SDK] = (data, cb) => {
  log("initSdk", "channelId", channelId);
  let XGameGlobal = window["XGameGlobal"];
  XGameGlobal["xgame.sdk.init"]((sdk) => {
    _Xgame_Archive = new XGameGlobal["extension/archive"]();
    _Xgame_MPush = new XGameGlobal["extension/mpush"]();
    window["archive"] = _Xgame_Archive;
    window["mpush"] = _Xgame_MPush;
    window["sdk"] = sdk;
    sdk.regExtension(_Xgame_Archive);
    sdk.regExtension(_Xgame_MPush);
    sdk.init(channelId, (ret) => {
      let Ad = sdk.Ad();
      let User = sdk.User();
      _Xgame_Ad = Ad;
      _Xgame_User = User;
      User.setOnPayed((a, b) => OnPayResult(a, b));
      cb({ ret: ret });
    });
  });


}

//登录
__caller[METHODS.LOGIN] = (data, cb) => {
  log("login");
  getUser().login((ret, res) => {
    //登录成功
    if (ret == "ok") {
      //设置同步开始和结束监听
      getArchive().setOnSync(() => {
        xgu_archive_onSyncBegin();
      }, () => {
        xgu_archive_onSyncEnd();
      });
      const userId = res.data.user.user_id.toString();
      const openId = res.data.account.open_id;
      const unionId = res.data.account.data.unionid;
      //注册引力引擎
      __pluginGeRegister(userId, openId, unionId);
      //通知Unity
      cb({ ret: ret })
      //尝试补单
      tryHandleOrdersQueue();
    } else {
      xgu_showToast(`error：${ret}`);
      cb({ ret: ret })
    }
  });
}

//显示插页广告
__caller[METHODS.SHOW_INTERS] = (data, cb) => {
  log("showInters");
  const scene = data.sceneName;
  //构造回调
  var showResult = getAd().newResult();
  showResult.onShow = () => {
    log("reportEvent inter", scene);
  };
  getAd().showInters(showResult);
}

//显示视频广告
__caller[METHODS.SHOW_VIDEO] = (data, cb) => {
  log("showVideo");
  var scene = data.sceneName;
  var showResult = getAd().newResult();
  showResult.onShow = () => {
    log("reportEvent video", scene);
  };

  showResult.onResult = (e) => {
    log("video on result", e);
    var reward = false;
    if (e.isReward) {
      reward = true;
    }
    cb(reward);
  }
  getAd().showVideo(showResult);

}

//隐藏banner
__caller[METHODS.HIDE_BANNER] = (data, cb) => {
  log("hideBanner");
  getAd().hideBanner();
  log("bannerMap::", _bannerMap);
  for (let [k, v] of _bannerMap) {
    log("hideBanner::", v);
    v.hide();
  }
}
//显示banner
__caller[METHODS.SHOW_BANNER] = (data, cb) => {
  log("showBanner", data);
  var scene = data.sceneName;
  //构造回调
  var showResult = getAd().newResult();
  //隐藏不匹配的广告  
  for (let [k, v] of _bannerMap) {
    if (k != scene) {
      v.hide();
    }
  }

  if (scene == "") {
    getAd().showBanner(showResult);
  } else {
    //隐藏默认banner
    getAd().hideBanner();
    if (_bannerMap.has(scene)) {
      _bannerMap.get(scene).show(showResult);
    } else {
      //创建新广告
      var pos = getAd().createBannerPos(scene);
      if (pos != null) {
        pos.create();
        _bannerMap.set(scene, pos);
        pos.show(showResult);
      }
    }

  }
}

//显示自定义广告,老接口
__caller[METHODS.SHOW_CUSTOM_AD] = (data, cb) => {
  log("showCustomAd", data);
  var type = data.type;
  var x = data.x;
  var y = data.y;
  if (!_customAdMap.has(type)) {
    var ad = getAd().createCustomAd(type);
    if (ad) {
      // x = x * _sysInfo.screenWidth;
      // y = y * _sysInfo.screenHeight;
      ad.setPosition(x, y);
      ad.setAnchor(0, 0);
      ad.create();
    } else {
      console.log("warning 未配置custom ad id " + type);
    }
    _customAdMap.set(type, ad);
  }
  var target = _customAdMap.get(type);
  if (target) {
    target.show();
  }

}

//隐藏自定义广告
__caller[METHODS.HIDE_CUSTOM_AD] = (data, cb) => {
  log("hideCustomAd", data);
  var type = data.type;
  if (_customAdMap.has(type)) {
    var target = _customAdMap.get(type);
    if (target) {
      target.hide();
    }
  }
}

//显示自定义广告（Pos）
__caller[METHODS.SHOW_CUSTOM_AD_POS] = (data, cb) => {
  log("showCustomAdPos", data);
  const arg_name = data.name;
  const arg_pivot_x = data.pivotX;
  const arg_pivot_y = data.pivotY;
  const arg_x = data.x;
  const arg_y = data.y;
  //创建广告
  if (!_customAdPosMap.has(arg_name)) {
    var ad = getAd().createCustomPos(arg_name);
    if (ad) {
      var x = arg_x * _sysInfo.screenWidth;
      var y = arg_y * _sysInfo.screenHeight;
      ad.setPosition(x, y);
      ad.setAnchor(arg_pivot_x, arg_pivot_y);
      ad.create();
    } else {
      console.log("warning 未配置custom ad name " + arg_name);
    }
    _customAdPosMap.set(arg_name, ad);
  }
  var target = _customAdPosMap.get(arg_name);
  if (target) {
    target.show();
  }
}

//隐藏自定义广告（Pos）
__caller[METHODS.HIDE_CUSTOM_AD_POS] = (data, cb) => {
  log("hideCustomAdPos", data);
  const arg_name = data.name;
  if (_customAdPosMap.has(arg_name)) {
    var target = _customAdPosMap.get(arg_name);
    if (target) {
      target.hide();
    }
  }
}

//支付
__caller[METHODS.PAY] = (data, cb) => {
  log("pay " + data);
  var id = data.productId;
  getUser().pay(id, (ret) => {
    if (ret == "ok") {
      //不处理，会触发回调    
    } else {
      //支付失败    
      var result_productId = id;
      var result_orderId = "";
      var payResult = {
        success: false,
        productId: result_productId,
        orderId: result_orderId
      };
      //触发unity支付回调
      var msg = JSON.stringify(payResult);
      noticeUnity("OnPayResult", msg);
    }
  });
}

//推送订单
__caller[METHODS.COMMIT_ORDER] = (data, cb) => {
  var orderId = data.orderId;
  log("commitOrder " + data);
  getUser().commitOrder(orderId);
}

//分享app
__caller[METHODS.SHARE_APP] = (data, cb) => {
  var arg_imageUrl = data.imageUrl;
  var arg_title = data.title;
  var arg_imageUrlId = data.imageUrlId;
  //标记分享开启
  _Cache_Share_Trigger = true;
  wx.shareAppMessage({
    title: arg_title == "" ? undefined : arg_title,
    imageUrl: arg_imageUrl == "" ? undefined : arg_imageUrl,
    imageUrlId: arg_imageUrlId == "" ? undefined : arg_imageUrlId,
  });
}

//友盟统计
__caller[METHODS.UMA_TRACK_EVENT] = (data, cb) => {
  log("umaTrackEvent");
  if (!openUma) {
    return;
  }
  var tem_event_id = data.uma_event_id;
  var tem_uma_event_param = data.uma_event_param;
  if (tem_uma_event_param == "") {
    log("trackEvent", tem_event_id);
    //一般上报
    wx.uma.trackEvent(tem_event_id);
  } else {
    //带属性的上报
    var param_object = JSON.parse(tem_uma_event_param);
    log("trackEvent", tem_event_id, param_object);
    wx.uma.trackEvent(tem_event_id, param_object);

  }
}

//http post
__caller[METHODS.HTTP_POST] = (data, cb) => {
  log("xgu_Http_Post", data);
  var arg_header = data.header;
  var arg_post_data = data.data;
  var arg_url = data.url;
  var arg_time_Out = data.time_out * 1000;//转成毫秒
  wx.request({
    url: arg_url,
    method: "POST",
    data: arg_post_data,
    header: arg_header,
    dataType: "text",
    timeout: arg_time_Out,
    success (res) {
      log("xgu_Http_Post success", res);
      cb({ ret: "ok", succesResult: res });

    },
    fail (err) {
      log("xgu_Http_Post fail", err);
      cb({ ret: "fail", errResult: err });
    }
  });
}

//http get
__caller[METHODS.HTTP_GET] = (data, cb) => {
  log("xgu_Http_Get", data);
  var arg_url = data.url;
  var arg_header = data.header;
  wx.request({
    url: arg_url,
    method: "GET",
    header: arg_header ? arg_header : undefined,
    dataType: "text",
    success (res) {
      log("xgu_Http_Get success", res);
      cb({ success: true, successResult: res });
    },
    fail (err) {
      log("xgu_Http_Get fail", err);
      cb({ success: false, errResult: err });
    }
  });
}

//云存档，获取keys
__caller[METHODS.GET_CLOUD_ARCHIVE_KEYS] = (data, cb) => {
  log("xgu_archive_get_keys", data);
  if (getArchive() == null) {
    var err = "archive is null,please init sdk";
    xgu_showToast(err);
    cb({ success: false });
    return;
  }

  getArchive().reqAllKeys((ret, keys) => {
    log("xgu_archive_get_keys result:", ret, keys);
    if (ret == "ok") {
      cb({ success: true, keys });
    }
    else {
      cb({ success: false, err: ret });

    }
  });
}

//设置存档内容
__caller[METHODS.SET_CLOUD_ARCHIVE_DATA] = (data, cb) => {
  log("xgu_archive_save64K", data);
  var arg_key = data.key;
  var arg_version = data.version;
  var arg_content = data.content;
  log("xgu_archive_save64K", "key", arg_key, "version", arg_version, "content", arg_content);

  if (getArchive() == null) {
    xgu_showToast("archive is null,please init sdk");
    return;
  }

  getArchive().save64k(arg_key, arg_version, arg_content);
}

//主动同步存档数据
__caller[METHODS.FORCE_SYNC_CLOUD_ARCHIVE] = (data, cb) => {
  log("xgu_archive_forceSync");
  if (getArchive() == null) {
    xgu_showToast("archive is null,please init sdk");
    return;
  }
  //同步云存档
  getArchive().forceSync();
}

//请求单个key
__caller[METHODS.GET_CLOUD_ARCHIVE_DATA] = (data, cb) => {
  log("xgu_archive_forceSync", data);
  var arg_key = data.key;
  if (getArchive() == null) {
    const err = "archive is null,please init sdk";
    xgu_showToast(err);
    cb({ success: false, err: err });
    return;
  }

  // 请求获取单个key 的数据内容
  getArchive().req64k(arg_key, (ret, value) => {
    log("req64k result key result：", ret, value);
    if (ret == "ok") {
      cb({ success: true, data: value })
    } else {
      //返回给失败结果    
      cb({ success: false, err: ret })
    }
  });
}

//事件上报
__caller[METHODS.REPORT_EVENT] = (data, cb) => {
  log("reportEvent", data);
  const event_id = data.event_id;
  const event_data = data.event_data;
  wx.reportEvent(event_id, event_data);
}

//设置分享样式
__caller[METHODS.SET_SHARE_APP_STYLE] = (data, cb) => {
  log("setShareAppStyle", data);
  var title = data.title;
  var imageUrl = data.imageUrl;
  var imageUrlId = data.imageUrlId;
  _onShareAppStyle.title = title;
  _onShareAppStyle.imageUrl = imageUrl;
  _onShareAppStyle.imageUrlId = imageUrlId;
}

//请求互推广告
__caller[METHODS.MPUSH_REQ_ITEMS] = (data, cb) => {
  log("mpush_reqItems", data);
  if (_Xgame_MPush == null) {
    const err = "mpush is null,no login";
    xgu_showToast(err);
    cb({ success: false, err: err });
    return;
  }
  _Xgame_MPush.reqItems(Number(data.planId), data.count, (ret, res) => {
    if (ret == "ok") {
      for (const element of res.contents) {
        _mpushContentMap.set(element.content_id, element);
      }
      cb({ success: true, result: res });
    } else {
      cb({ success: false, err: ret });
    }
  });
}

//点击互推广告
__caller[METHODS.MPUSH_CLICK_ITEM] = (data, cb) => {
  log("mpush click item", data);
  const id = data.content_id;
  if (_Xgame_MPush == null) {
    const err = "mpush is null,no login";
    xgu_showToast(err);
    cb({ success: false, err: err });
    return;
  }
  if (_mpushContentMap.has(id)) {
    _Xgame_MPush.clickItem(_mpushContentMap.get(id), (ret) => {
      if (ret == "ok") {
        cb({ success: true });
      } else {
        xgu_showToast(ret);
        cb({ success: false, err: ret });
      }
    });
  } else {
    var err = "no mpush content";
    xgu_showToast(err);
    cb({ success: false, err: err });

  }
}

//显示toast
__caller[METHODS.SHOW_TOAST] = (data, cb) => {
  log("show toast", data);
  const message = data.message;
  wx.showToast({
    title: message,
    icon: "none",
  });
}


//声明get方法
let __getter = [];
//是否有插页广告
__getter[METHODS.HAS_INTERS] = (data) => {
  log("getIntersFlag");
  var flag = false;
  getAd().getIntersFlag((cb) => {
    flag = cb;
  });
  if (flag) {
    return JSON.stringify(true);
  }
  return JSON.stringify(false);
}

//是否有广告
__getter[METHODS.HAS_VIDEO] = (data) => {
  log("getVideoFlag");
  var flag = false;
  getAd().getVideoFlag((cb) => {
    flag = cb;
  });
  if (flag) {
    return JSON.stringify(true);
  }
  return JSON.stringify(false);
}

//获取设备类型
__getter[METHODS.GET_DEVICE_TYPE] = (data) => {
  log("getDeviceType");
  return _sysInfo.platform;
}

//获取玩家ID，需要登录成功后才可获取
__getter[METHODS.GET_SDK_USER_ID] = (data) => {
  log("getSdkUserId");
  var id = getUser().userId.toString();
  log("getSdkUserId result:", id);
  return id;
}





//打印日志f
function log (...args) {
  if (DEBUG_MODE) {
    console.log("## log xgame-unity ", ...args);
  }
}
//打赢错误
function logError (...args) {
  console.error("## error xgame-unity ", ...args);
}

//向unity发送消息
function noticeUnity (method, msg) {
  window.unityNamespace.Module.SendMessage("WEB_TO_UNITY_CALLBACK", method, msg);
}

//触发Unity session回调
function invokeUnitySessionCallBack (session, data) {
  noticeUnity("ReceiveSessionCode", session);
  noticeUnity("ExecuteSessionCallback", JSON.stringify(data));
}


//触发支付回调
function OnPayResult (ret, orders) {
  if (ret == "ok" && orders.length > 0) {
    for (let i in orders) {
      const element = orders[i];
      noticeUnity("OnPayResult", JSON.stringify({
        success: true,
        productId: element.product.id,
        orderId: element.orderId.toString(),
      }));
      //引力引擎打点
      __pluginGePayEvent(Math.floor(element.product.price / 10000), element.product.id, element.orderId.toString());
    }
  }
}


function getAd () {
  if (_Xgame_Ad == null) {
    logError("Ad is null");
  }
  return _Xgame_Ad;
}

function getUser () {
  if (_Xgame_User == null) {
    logError("User is null");
  }
  return _Xgame_User;
}

//获取云存档实例
function getArchive () {
  if (_Xgame_Archive == null) {
    log("error! _Xgame_Archive == null");
  }
  return _Xgame_Archive;
}


//弹出toast
function xgu_showToast (content) {
  wx.showToast({
    title: content,
    icon: 'none'
  });
}

//存档同步开始
function xgu_archive_onSyncBegin () {
  //发送通知
  noticeUnity("OnArchiveSyncBegin", "");
}

//存档结束同步
function xgu_archive_onSyncEnd () {
  //发送通知
  noticeUnity("OnArchiveSyncEnd", "");
}


//补单
function tryHandleOrdersQueue () {
  getUser().reqPayedOrders();
}


//尝试处理分享APP结果
function tryHandleShareAppResult () {
  if (_Cache_Share_Trigger) {
    _Cache_Share_Trigger = false;
    log("触发分享成功");
    //触发分享成功
    noticeUnity("OnShareApp", "");
  }
}

//初始化友盟
function tryInitUma () {
  if (openUma) {
    console.log("init uma");
    uma.init({
      appKey: umaAppKey,
      useOpenid: true,// default true
      autoGetOpenid: true,
      debug: true,
      uploadUserInfo: true// 上传用户信息，上传后可以看到有用户头像和昵称的分享信息
    });
  }
}


//是否为字符串
function xgu_isString (str) {
  return (typeof str == 'string') && str.constructor == String;
}

function xgOnShow () {
  log("xgOnShow");
  tryHandleShareAppResult();
}


function xgOnHide () {
  log("xgOnHide");
}

//引力引擎
function __pluginGeRegister (userId, openId, unionId) {
  if (!__pluginGeConfig.turnOn) {
    return;
  }
  //如果是debug模式
  if (DEBUG_MODE) {
    __pluginGe.debugOn();
  }
  const accessToken = __pluginGeConfig.accessToken;
  const version = __pluginGeConfig.version;
  __pluginGe.init(accessToken, openId, false);
  __pluginGe.register(userId, version, openId, unionId);


}


function __pluginGePayEvent (price, productName, orderId) {
  if (!__pluginGeConfig.turnOn) {
    return;
  }
  __pluginGe.payEvent(price, "CNY", orderId, productName, "微信");
}

//与unity关联的部分
window["MethodGet"] = (json) => {
  var msg = JSON.parse(json);
  const method = msg.method;
  const data = msg.data;
  var ret = __getter[method](data);
  return JSON.stringify(ret);
};

window["MethodCall"] = (json) => {
  var msg = JSON.parse(json);
  const method = msg.method;
  const data = msg.data;
  const session = msg.session;
  const cb = function (params) { invokeUnitySessionCallBack(session.toString(), params) };
  __caller[method](data, cb);
};

//工具
window.top["xgut"] = {
  debugModeOn () {
    console.log("开启调试模式");
    DEBUG_MODE = true;
  },
  debugModeOff () {
    console.log("关闭调试模式");
    DEBUG_MODE = false;
  }
}
wx.onShow(() => { xgOnShow(); });
wx.onHide(() => { xgOnHide(); });
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
//初始化uma
tryInitUma();

