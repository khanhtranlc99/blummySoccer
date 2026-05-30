
//渠道ID
let channelId = 1;
//友盟参数
let umaAppKey = "";
let openUma = false;
//字符串名
let MESSAGE_ID = "messageId";
let PRODUCT_ID = "productId";
let PRICE = "price";
let PRODUCT_DESC = "productDesc";
let PRODUCT_NAME = "productName";
let CODE = "code";
let SCENE_NAME = "sceneName";
let ORDER_ID = "orderId";
let STRING_TYPE = "type";
let STRING_X = "x";
let STRING_Y = "y";
let IMAGE_URL = "imageUrl";
let TITLE = "title";
let UMA_EVENT_ID = "uma_event_id";
let UMA_EVENT_PARAM = "uma_event_param";
let STRING_HEADER = "header";
let STRING_URL = "url";
let STRING_CALL_BACK_ID = "call_back_id";
let STRING_SUCCESS = "success";
let STRING_DATA = "data";
let STRING_PIVOT_X = "pivot_x";
let STRING_PIVOT_Y = "pivot_y";
let STRING_NAME = "name";
let STRING_KEY = "key";
let STRING_VERSION = "version";
let STRING_CONTENT = "content";
let STRING_HAS_HEADER = "has_header";
let STRING_TIME_OUT = "time_out";

//初始化SDK
let ID_INIT_SDK = 1000;
//是否有插屏
let ID_HAS_INTERS = 1001;
//显示插屏
let ID_SHOW_INTERS = 1002;
//是否有视频广告
let ID_HAS_VIDEO = 1003;
//展示广告
let ID_SHOW_VIDEO = 1004;
//支付
let ID_PAY = 1005;
//兑换礼物
let ID_GIFT = 1006;
//登录
let ID_LOGIN = 1007;
//显示banner
let ID_SHOW_BANNER = 1008;
//隐藏banner
let ID_HIDE_BANNER = 1009;
//提交订单号
let ID_COMMIT_ORDER = 1010;
//显示自定义广告
let ID_SHOW_CUSTOM_AD = 1011;
//隐藏自定义广告
let ID_HIDE_CUSTOM_AD = 1012;
//获取设备类型
let ID_GET_DEVICE_TYPE = 1013;
//分享
let ID_SHARE_APP = 1014;
//友盟打点
let ID_UMA_TRACK_EVENT = 1015;
//获取sdk用户ID
let ID_GET_SDK_USER_ID = 1016;
//POST
let ID_HTTP_POST = 1017;
//GET
let ID_HTTP_GET = 1018;
//显示自定义广告（新版本）
let ID_SHOW_CUSTOM_AD_POS = 1019;
//隐藏自定义广告（新版本）
let ID_HIDE_CUSTOM_AD_POS = 1020;
//获取云存档keys
let ID_GET_CLOUD_ARCHIVE_KEYS = 1021;
//设置云存档数据
let ID_SET_CLOUD_ARCHIVE_DATA = 1022;
//同步云存档
let ID_FORCE_SYNC_CLOUD_ARCHIVE = 1023;
//获取云存档key数据
let ID_GET_CLOUD_ARCHIVE_DATA = 1024;



//DEBUG模式
let DEBUG_MODE = false;

let Cache_Share_Trigger = false;

//customAd map
let customAdMap = new Map();

let bannerMap = new Map();

//新customAd map
let customAdPosMap = new Map();

//xgame_archive,云存储拓展
let _Xgame_Archive = null;

let sysInfo = wx.getSystemInfoSync();

//Unity调用SDK方法 并返回
let mGet = function MethodGet (msg) {
  log(" MethodGet" + msg);
  var data = JSON.parse(msg);
  var id = data[MESSAGE_ID];
  switch (id) {
    case ID_HAS_INTERS://是否有插页广告
      return getIntersFlag();
    case ID_HAS_VIDEO://是否有视频广告
      return getVideoFlag();
    case ID_GET_DEVICE_TYPE://获取设备类型
      return getDeviceType();
    case ID_GET_SDK_USER_ID://获取SDK用户ID
      return getSdkUserId();
  }
  return "";
}

//Unity调用SDK方法
let mCall = function MethodCall (msg) {
  log("##CallMethod" + msg);
  var data = JSON.parse(msg);
  var id = data[MESSAGE_ID];
  switch (id) {
    case ID_INIT_SDK://初始化SDK
      initSdk();
      break;
    case ID_LOGIN://登录
      login();
      break;
    case ID_SHOW_BANNER://展示banner
      showBanner(msg);
      break;
    case ID_SHOW_INTERS://展示插页
      showInters();
      break;
    case ID_SHOW_VIDEO://显示视频广告
      showVideo();
      break;
    case ID_PAY://支付
      pay(msg);
      break;
    case ID_COMMIT_ORDER:
      commitOrder(msg);
      break;
    case ID_HIDE_BANNER:
      hideBanner();
      break;
    case ID_SHOW_CUSTOM_AD:
      showCustomAd(msg);
      break;
    case ID_HIDE_CUSTOM_AD:
      hideCustomAd(msg);
      break;
    case ID_SHARE_APP:
      shareApp(msg);
      break;
    case ID_HTTP_POST:
      xgu_Http_Post(msg);
      break
    case ID_HTTP_GET:
      xgu_Http_Get(msg);
      break;
    case ID_SHOW_CUSTOM_AD_POS:
      showCustomAdPos(msg);
      break;
    case ID_HIDE_CUSTOM_AD_POS:
      hideCustomAdPos(msg);
      break;
    case ID_UMA_TRACK_EVENT:
      umaTrackEvent(msg);
      break;
    case ID_GET_CLOUD_ARCHIVE_KEYS:
      xgu_archive_get_keys(msg);
      break;
    case ID_SET_CLOUD_ARCHIVE_DATA:
      xgu_archive_save64K(msg);
      break;
    case ID_FORCE_SYNC_CLOUD_ARCHIVE:
      xgu_archive_forceSync();
      break;
    case ID_GET_CLOUD_ARCHIVE_DATA:
      xgu_archive_req64k(msg);
      break;


  }
}


//通知unity
function noticeUnity (method, msg) {
  window.unityNamespace.Module.SendMessage("WEB_TO_UNITY_CALLBACK", method, msg);
}


function OnPayResult (ret, orders) {
  if (ret == "ok" && orders.length > 0) {
    for (let i in orders) {
      const element = orders[i];
      var result_success = "1";
      var result_productId = element.product.id;
      var result_orderId = element.orderId.toString();
      var payResult = {
        success: result_success,
        productId: result_productId,
        orderId: result_orderId
      };
      var msg = JSON.stringify(payResult);
      noticeUnity("OnPayResult", msg);
    }
  }
}


//初始化sdk
function initSdk () {
  log("initSdk");
  let XGameGlobal = window["XGameGlobal"];
  XGameGlobal["xgame.sdk.init"]((sdk) => {
    _Xgame_Archive = new XGameGlobal["extension/archive"]();
    window["archive"] = _Xgame_Archive;
    window["sdk"] = sdk;
    sdk.regExtension(_Xgame_Archive);
    sdk.init(channelId, (ret) => {
      let Ad = sdk.Ad();
      let User = sdk.User();
      window["Ad"] = Ad;
      window["User"] = User;
      User.setOnPayed(OnPayResult);
      let resultCode = "0";
      if (ret == "ok") {
        resultCode = "1";
      }
      //发送消息，触发完成回调
      noticeUnity("OnInitSdkResult", resultCode);
    });
  });
}

function getAd () {
  if (window.Ad) {
    return window.Ad;
  }
  logError("Ad is null");
}


function getUser () {
  if (window.User) {
    return window.User;
  }
  logError("User is null");
}


//登录
function login () {
  log("login");
  getUser().login((ret) => {
    let msg = "0";
    //登录成功
    if (ret == "ok") {
      //设置同步开始和结束监听
      getArchive().setOnSync(() => {
        xgu_archive_onSyncBegin();
      }, () => {
        xgu_archive_onSyncEnd();
      });
      msg = "1";
      //通知Unity
      noticeUnity("OnLoginResult", msg);
      //尝试补单
      tryHandleOrdersQueue();
    } else {
      xgu_showToast(`error：${ret}`);
      noticeUnity("OnLoginResult", msg);
    }

  });
}

//是否有插屏
function getIntersFlag () {
  log("getIntersFlag");
  let flag = false;
  getAd().getIntersFlag((cb) => {
    flag = cb;
  });
  if (flag) {
    return "1";
  }
  return "0";
}

//播放插页广告
function showInters () {
  log("showInters");
  //构造回调
  var showResult = getAd().newResult();
  getAd().showInters(showResult);
}

//是否有视频广告
function getVideoFlag () {
  log("getVideoFlag");
  let flag = false;
  getAd().getVideoFlag((cb) => {
    flag = cb;
  });
  if (flag) {
    return "1";
  }
  return "0";
}

//播放视频广告
function showVideo () {
  log("showVideo");
  var showResult = getAd().newResult();
  showResult.onResult = (e) => {
    log("video on result", e);
    var msg = "0";
    if (e.isReward) {
      msg = "1";
    }
    noticeUnity("OnVideoResult", msg)
  }
  getAd().showVideo(showResult);
}

// //显示banner
// function showBanner () {
//   log("showBanner");
//   //构造回调
//   var showResult = getAd().newResult();
//   getAd().showBanner(showResult);
// }

//隐藏banner
function hideBanner () {
  log("hideBanner");
  getAd().hideBanner();
  console.log("bannerMap::", bannerMap);
  for (let [k, v] of bannerMap) {
    console.log("hideBanner::", v);
    v.hide();
  }
  // for (let k in bannerMap) {
  //   const element = bannerMap[k];
  //   console.log("hideBanner::", element);
  //   element.hide();
  // }
}

//显示banner
function showBanner (json) {
  log("showBanner", json);
  var data = JSON.parse(json);
  var scene = data[SCENE_NAME];
  //构造回调
  var showResult = getAd().newResult();
  //隐藏不匹配的广告  

  for (let [k, v] of bannerMap) {
    if (k != scene) {
      v.hide();
    }
  }

  if (scene == "") {
    getAd().showBanner(showResult);
  } else {
    //隐藏默认banner
    getAd().hideBanner();
    if (bannerMap.has(scene)) {
      bannerMap.get(scene).show(showResult);
    } else {
      //创建新广告
      var pos = getAd().createBannerPos(scene);
      if (pos != null) {
        pos.create();
        bannerMap.set(scene, pos);
        pos.show(showResult);
      }
    }

  }
}

//显示自定义广告,老接口
function showCustomAd (json) {
  log("showCustomAd", json);
  var data = JSON.parse(json);
  var type = data[STRING_TYPE];
  var x = data[STRING_X];
  var y = data[STRING_Y];
  if (!customAdMap.has(type)) {
    var ad = getAd().createCustomAd(type);
    if (ad) {
      x = x * sysInfo.screenWidth;
      y = y * sysInfo.screenHeight;
      ad.setPosition(x, y);
      ad.setAnchor(0, 0);
      ad.create();
    } else {
      console.log("warning 未配置custom ad id " + type);
    }
    customAdMap.set(type, ad);
  }
  var target = customAdMap.get(type);
  if (target) {
    target.show();
  }

}

//隐藏自定义广告
function hideCustomAd (json) {
  log("hideCustomAd", json);
  var data = JSON.parse(json);
  var type = data[STRING_TYPE];
  if (customAdMap.has(type)) {
    var target = customAdMap.get(type);
    if (target) {
      target.hide();
    }
  }
}

//显示自定义广告（Pos）
function showCustomAdPos (json) {
  log("showCustomAdPos", json);
  var data = JSON.parse(json);
  var arg_name = data[STRING_NAME];
  var arg_pivot_x = data[STRING_PIVOT_X];
  var arg_pivot_y = data[STRING_PIVOT_Y];
  var arg_x = data[STRING_X];
  var arg_y = data[STRING_Y];
  //创建广告
  if (!customAdPosMap.has(arg_name)) {
    var ad = getAd().createCustomPos(arg_name);
    if (ad) {
      var x = arg_x * sysInfo.screenWidth;
      var y = arg_y * sysInfo.screenHeight;
      ad.setPosition(x, y);
      ad.setAnchor(arg_pivot_x, arg_pivot_y);
      ad.create();
    } else {
      console.log("warning 未配置custom ad name " + arg_name);
    }
    customAdPosMap.set(arg_name, ad);
  }
  var target = customAdPosMap.get(arg_name);
  if (target) {
    target.show();
  }
}

//隐藏自定义广告（Pos）
function hideCustomAdPos (json) {
  log("hideCustomAdPos", json);
  var data = JSON.parse(json);
  var arg_name = data[STRING_NAME];
  if (customAdPosMap.has(arg_name)) {
    var target = customAdPosMap.get(arg_name);
    if (target) {
      target.hide();
    }
  }

}

//支付
function pay (json) {
  log("pay " + json);
  //解析传入的值
  var data = JSON.parse(json);
  // console.log("## pay data",data);
  var id = data[PRODUCT_ID];
  var name = data[PRODUCT_NAME];
  var price = data[PRICE];
  var desc = data[PRODUCT_DESC];
  // var productInfo = {
  //   id: id,
  //   // name: name,
  //   // desc: desc,
  //   // price: price,
  // };
  // console.log("## unity",productInfo);
  getUser().pay(id, (ret) => {
    if (ret == "ok") {
      // log("支付成功", orders);
      // for (let index = 0; index < orders.length; index++) {
      //   const element = orders[index];
      //   //成功
      //   var result_success = "1";
      //   var result_productId = element.product.id;
      //   //统一转为string
      //   var result_orderId = element.orderId.toString();
      //   var payResult = {
      //     success: result_success,
      //     productId: result_productId,
      //     orderId: result_orderId
      //   };
      //   //触发unity支付回调
      //   var msg = JSON.stringify(payResult);
      //   noticeUnity("OnPayResult", msg);
      // }
    } else {
      //支付失败
      var result_success = "0";
      var result_productId = id;
      var result_orderId = "";
      var payResult = {
        success: result_success,
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
function commitOrder (json) {
  var data = JSON.parse(json);
  var orderId = data[ORDER_ID];
  log("commitOrder " + orderId);
  getUser().commitOrder(orderId);
}

//补单
function tryHandleOrdersQueue () {
  getUser().reqPayedOrders();
  // var orders = getUser().reqPayedOrders();
  // if (orders) {
  //   for (let index = 0; index < orders.length; index++) {
  //     const element = orders[index];
  //     var result_success = "1";
  //     var result_productId = element.product.id;
  //     var result_orderId = element.orderId.toString();
  //     var payResult = {
  //       success: result_success,
  //       productId: result_productId,
  //       orderId: result_orderId
  //     };
  //     var msg = JSON.stringify(payResult);
  //     noticeUnity("OnPayResult", msg);
  //   }
  // }

}

//分享app
function shareApp (json) {
  var data = JSON.parse(json);
  // console.log("## pay data",data);
  var arg_imageUrl = data[IMAGE_URL];
  var arg_title = data[TITLE];
  //标记分享开启
  Cache_Share_Trigger = true;
  wx.shareAppMessage({
    title: arg_title == "" ? undefined : arg_title,
    imageUrl: arg_imageUrl == "" ? undefined : arg_imageUrl
  });

}

//尝试处理分享APP结果
function tryHandleShareAppResult () {
  if (Cache_Share_Trigger) {
    Cache_Share_Trigger = false;
    log("触发分享成功");
    //触发分享成功
    noticeUnity("OnShareApp", "");
  }
}


function xgOnShow () {
  log("xgOnShow");
  tryHandleShareAppResult();
}

function xgOnHide () {
  log("xgOnHide");
}

//获取设备类型
function getDeviceType () {
  log("getDeviceType");
  return wx.getSystemInfoSync().platform;
}


//获取玩家ID，需要登录成功后才可获取
function getSdkUserId () {
  log("getSdkUserId");
  var id = getUser().userId.toString();
  log("getSdkUserId result:", id);
  return id;
}

//抛出异常日志
function logError (msg) {
  console.error("## error xgame-unity " + msg);
}

//日志
function log (...args) {
  if (DEBUG_MODE) {
    console.log("## log xgame-unity ", ...args);
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
//友盟统计
function umaTrackEvent (json) {
  log("umaTrackEvent");
  if (!openUma) {
    return;
  }
  var data = JSON.parse(json);
  var tem_event_id = data[UMA_EVENT_ID];
  var tem_uma_event_param = data[UMA_EVENT_PARAM];
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
function xgu_Http_Post (json) {
  log("xgu_Http_Post", json);
  var json_data = JSON.parse(json);
  var arg_header = json_data[STRING_HEADER];
  var arg_post_data = json_data[STRING_DATA];
  var arg_url = json_data[STRING_URL];
  var arg_call_back_id = json_data[STRING_CALL_BACK_ID];
  var arg_time_Out = json_data[STRING_TIME_OUT] * 1000;//转成毫秒
  wx.request({
    url: arg_url,
    method: "POST",
    data: arg_post_data,
    header: arg_header,
    dataType: "text",
    timeout: arg_time_Out,
    success (res) {
      log("xgu_Http_Post success", res);
      var result_data = {
        r_call_back_id: arg_call_back_id,
        r_success: true,
        r_data: res.data,
        r_header: res.header,
        r_statusCode: res.statusCode,
      };
      var result_data_string = JSON.stringify(result_data);
      noticeUnity("OnHttpPostCallBack", result_data_string);
    },
    fail (err) {
      log("xgu_Http_Post fail", err);
      var result_data = {
        r_call_back_id: arg_call_back_id,
        r_success: false,
        r_errMsg: err.errMsg,
        r_errno: err.errno,
      };
      var result_data_string = JSON.stringify(result_data);
      noticeUnity("OnHttpPostCallBack", result_data_string);
    }
  });
}

//http get
function xgu_Http_Get (json) {
  log("xgu_Http_Get", json);
  var json_data = JSON.parse(json);
  var arg_url = json_data[STRING_URL];
  var arg_call_back_id = json_data[STRING_CALL_BACK_ID];
  var arg_header = json_data[STRING_HEADER];
  wx.request({
    url: arg_url,
    method: "GET",
    header: arg_header ? arg_header : undefined,
    dataType: "text",
    success (res) {
      log("xgu_Http_Get success", res);
      var result_data = {
        r_call_back_id: arg_call_back_id,
        r_success: true,
        r_data: res.data,
        r_header: res.header,
        r_statusCode: res.statusCode,
      };
      var result_data_string = JSON.stringify(result_data);
      noticeUnity("OnHttpGetCallBack", result_data_string);
    },
    fail (err) {
      log("xgu_Http_Get fail", err);
      var result_data = {
        r_call_back_id: arg_call_back_id,
        r_success: false,
        r_errMsg: err.errMsg,
        r_errno: err.errno,
      };
      var result_data_string = JSON.stringify(result_data);
      noticeUnity("OnHttpGetCallBack", result_data_string);
    }
  });
}


//是否为字符串
function xgu_isString (str) {
  return (typeof str == 'string') && str.constructor == String;
}


//获取云存档实例
function getArchive () {
  if (_Xgame_Archive == null) {
    log("error! _Xgame_Archive == null");
  }
  return _Xgame_Archive;
}

//存档同步开始
function xgu_archive_onSyncBegin () {
  //发送通知
  noticeUnity("OnArchiveSyncBegin", "");
}

//存档同步结束
function xgu_archive_onSyncEnd () {
  //发送通知
  noticeUnity("OnArchiveSyncEnd", "");
}



//云存档，获取keys
function xgu_archive_get_keys (json) {
  log("xgu_archive_get_keys", json);
  var json_data = JSON.parse(json);
  var arg_call_back_id = json_data[STRING_CALL_BACK_ID];

  if (getArchive() == null) {
    var err = "archive is null,please init sdk";
    var result_data = {
      r_call_back_id: arg_call_back_id.toString(),
      r_success: "0",
      r_ret: err,
    };
    xgu_showToast(err);
    noticeUnity("OnGetArchiveKeysResult", JSON.stringify(result_data));
    return;
  }

  getArchive().reqAllKeys((ret, keys) => {
    log("xgu_archive_get_keys result:", ret, keys);
    if (ret == "ok") {
      var result_data = {
        r_call_back_id: arg_call_back_id.toString(),
        r_success: "1",
        r_data: JSON.stringify(keys),
        r_ret: ret,
      };
      noticeUnity("OnGetArchiveKeysResult", JSON.stringify(result_data));
    }
    else {
      var result_data = {
        r_call_back_id: arg_call_back_id.toString(),
        r_success: "0",
        r_ret: ret,
      };
      noticeUnity("OnGetArchiveKeysResult", JSON.stringify(result_data));
    }
  });

}

//保存key
function xgu_archive_save64K (json) {
  log("xgu_archive_save64K", json);
  var json_data = JSON.parse(json);
  var arg_key = json_data[STRING_KEY];
  var arg_version = json_data[STRING_VERSION];
  var arg_content = json_data[STRING_CONTENT];
  log("xgu_archive_save64K", "key", arg_key, "version", arg_version, "content", arg_content);

  if (getArchive() == null) {
    xgu_showToast("archive is null,please init sdk");
    return;
  }

  getArchive().save64k(arg_key, arg_version, arg_content);
}

//主动同步存档数据
function xgu_archive_forceSync () {
  log("xgu_archive_forceSync");
  if (getArchive() == null) {
    xgu_showToast("archive is null,please init sdk");
    return;
  }
  //同步云存档
  getArchive().forceSync();
}


//请求单个key
function xgu_archive_req64k (json) {
  log("xgu_archive_forceSync", json);
  var json_data = JSON.parse(json);
  var arg_key = json_data[STRING_KEY];
  var arg_call_back_id = json_data[STRING_CALL_BACK_ID];

  if (getArchive() == null) {
    var err = "archive is null,please init sdk";
    xgu_showToast(err);
    //返回给失败结果
    var result_data = {
      r_call_back_id: arg_call_back_id.toString(),
      r_success: "0",
      r_ret: err,
    };
    noticeUnity("OnGetArchiveDataResult", JSON.stringify(result_data));
    return;
  }

  // 请求获取单个key 的数据内容
  getArchive().req64k(arg_key, (ret, value) => {
    log("req64k result key result：", ret, value);
    if (ret == "ok") {
      //返回给成功结果
      var result_data = {
        r_call_back_id: arg_call_back_id.toString(),
        r_success: "1",
        r_ret: ret,
        r_data: JSON.stringify(value),
      };
      noticeUnity("OnGetArchiveDataResult", JSON.stringify(result_data));
    } else {
      //返回给失败结果
      var result_data = {
        r_call_back_id: arg_call_back_id.toString(),
        r_success: "0",
        r_ret: ret,
      };
      noticeUnity("OnGetArchiveDataResult", JSON.stringify(result_data));
    }
  });
}

//弹出toast
function xgu_showToast (content) {
  wx.showToast({
    title: content,
    icon: 'none'
  });
}

window["MethodGet"] = mGet;
window["MethodCall"] = mCall;
wx.onShow(xgOnShow);
wx.onHide(xgOnHide);
console.log("## xgame-unity load done");
tryInitUma();
