//渠道ID
let channelId = 4;
//DEBUG模式
let DEBUG_MODE = false;
//xgame_archive,云存储拓展
let _Xgame_Archive = null;

//字符串名
let U_STRINGS = {
  MESSAGE_ID: "messageId",
  PRODUCT_ID: "productId",
  PRICE: "price",
  PRODUCT_DESC: "productDesc",
  PRODUCT_NAME: "productName",
  CODE: "code",
  SCENE_NAME: "sceneName",
  ORDER_ID: "orderId",
  KEY: "key",
  VERSION: "version",
  CONTENT: "content",
  HAS_HEADER: "has_header",
  TIME_OUT: "time_out",
  CALL_BACK_ID: "call_back_id",
}

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
  //用户id  
  GET_SDK_USER_ID: 1016,
  //获取云存档keys
  GET_CLOUD_ARCHIVE_KEYS: 1021,
  //设置云存档数据
  SET_CLOUD_ARCHIVE_DATA: 1022,
  //同步云存档
  FORCE_SYNC_CLOUD_ARCHIVE: 1023,
  //获取云存档key数据
  GET_CLOUD_ARCHIVE_DATA: 1024,

}

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
      case U_METHOD_ID.GET_SDK_USER_ID:
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
        this.showBanner();
        break;
      case U_METHOD_ID.SHOW_INTERS://展示插页
        this.showInters();
        break;
      case U_METHOD_ID.SHOW_VIDEO://显示视频广告
        this.showVideo();
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

    }
  },

  //通知unity
  noticeUnity: function (method, msg) {
    window.unityInstance.SendMessage("WEB_TO_UNITY_CALLBACK", method, msg);
  },

  //支付回调  
  OnPayResult: function (ret, orders) {
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
        this.noticeUnity("OnPayResult", msg);
      }
    }
  },
  //初始化sdk
  initSdk: function () {
    this.log("initSdk", "channelId", channelId);
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
        User.setOnPayed((a,b)=>this.OnPayResult(a,b));
        let resultCode = "0";
        if (ret == "ok") {
          resultCode = "1";
        }
        //发送消息，触发完成回调
        this.noticeUnity("OnInitSdkResult", resultCode);
      });
    });
  },

  getAd: function () {
    if (window.Ad) {
      return window.Ad;
    }
    this.logError("Ad is null");
  },

  getUser: function () {
    if (window.User) {
      return window.User;
    }
    this.logError("User is null");
  },


  //获取玩家ID，需要登录成功后才可获取
  getSdkUserId: function () {
    this.log("getSdkUserId");
    var id = this.getUser().userId.toString();
    this.log("getSdkUserId result:", id);
    return id;
  },

  //登录
  login: function () {
    this.log("login");
    this.getUser().login((ret) => {
      let msg = "0";
      //登录成功
      if (ret == "ok") {
        //设置同步开始和结束监听
        this.getArchive().setOnSync(() => {
          this.xgu_archive_onSyncBegin();
        }, () => {
          this.xgu_archive_onSyncEnd();
        });
        msg = "1";
        //通知Unity
        this.noticeUnity("OnLoginResult", msg);
        //尝试补单
        this.tryHandleOrdersQueue();
      } else {
        this.xgu_showToast(`error：${ret}`);
        this.noticeUnity("OnLoginResult", msg);
      }

    });
  },


  //是否有插屏
  getIntersFlag: function () {
    this.log("getIntersFlag");
    var flag = false;
    this.getAd().getIntersFlag((cb) => {
      flag = cb;
    });
    if (flag) {
      return "1";
    }
    return "0";
  },

  //播放插页广告
  showInters: function () {
    this.log("showInters");
    //构造回调
    var showResult = this.getAd().newResult();
    this.getAd().showInters(showResult);
  },

  //是否有视频广告
  getVideoFlag: function () {
    this.log("getVideoFlag");
    var flag = false;
    this.getAd().getVideoFlag((cb) => {
      flag = cb;
    });
    if (flag) {
      return "1";
    }
    return "0";
  },

  //播放视频广告
  showVideo: function () {
    this.log("showVideo");
    var showResult = this.getAd().newResult();
    showResult.onResult = (e) => {
      this.log("video on result", e);
      var msg = "0";
      if (e.isReward) {
        msg = "1";
      }
      this.noticeUnity("OnVideoResult", msg)
    }
    this.getAd().showVideo(showResult);
  },


  //显示banner
  showBanner: function () {
    this.log("showBanner");
    //构造回调
    var showResult = this.getAd().newResult();
    this.getAd().showBanner(showResult);
  },

  //隐藏banner
  hideBanner: function () {
    this.log("hideBanner");
    this.getAd().hideBanner();
  },

  //支付
  pay: function (json) {
    this.log("pay " + json);
    //解析传入的值
    var data = JSON.parse(json);
    // console.log("## pay data",data);
    var id = data[U_STRINGS.PRODUCT_ID];
    // var name = data[U_STRINGS.PRODUCT_NAME];
    // var price = data[U_STRINGS.PRICE];
    // var desc = data[U_STRINGS.PRODUCT_DESC];
    this.getUser().pay(id, (ret) => {
      if (ret == "ok") {
        //不处理，会触发回调    
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
        this.noticeUnity("OnPayResult", msg);
      }
    });
  },


  //推送订单
  commitOrder: function (json) {
    var data = JSON.parse(json);
    var orderId = data[U_STRINGS.ORDER_ID];
    this.log("commitOrder " + orderId);
    this.getUser().commitOrder(orderId);
  },


  //补单
  tryHandleOrdersQueue: function () {
    this.getUser().reqPayedOrders();

  },


  //抛出异常日志
  logError: function (msg) {
    console.error("## error xgame-unity " + msg);
  },

  //日志
  log: function (...args) {
    if (DEBUG_MODE) {
      console.log("## log xgame-unity ", ...args);
    }
  },

  //弹出toast
  xgu_showToast: function (content) {
    qg.showToast({
      title: content,
      icon: 'none'
    });
  },


  //是否为字符串
  xgu_isString: function (str) {
    return (typeof str == 'string') && str.constructor == String;
  },


  //获取云存档实例
  getArchive: function () {
    if (_Xgame_Archive == null) {
      this.log("error! _Xgame_Archive == null");
    }
    return _Xgame_Archive;
  },

  //存档同步开始
  xgu_archive_onSyncBegin: function () {
    //发送通知
    this.noticeUnity("OnArchiveSyncBegin", "");
  },

  //存档同步结束
  xgu_archive_onSyncEnd: function () {
    //发送通知
    this.noticeUnity("OnArchiveSyncEnd", "");
  },


  //云存档，获取keys
  xgu_archive_get_keys: function (json) {
    this.log("xgu_archive_get_keys", json);
    var json_data = JSON.parse(json);
    var arg_call_back_id = json_data[U_STRINGS.CALL_BACK_ID];

    if (this.getArchive() == null) {
      var err = "archive is null,please init sdk";
      var result_data = {
        r_call_back_id: arg_call_back_id.toString(),
        r_success: "0",
        r_ret: err,
      };
      this.xgu_showToast(err);
      this.noticeUnity("OnGetArchiveKeysResult", JSON.stringify(result_data));
      return;
    }

    this.getArchive().reqAllKeys((ret, keys) => {
      this.log("xgu_archive_get_keys result:", ret, keys);
      if (ret == "ok") {
        var result_data = {
          r_call_back_id: arg_call_back_id.toString(),
          r_success: "1",
          r_data: JSON.stringify(keys),
          r_ret: ret,
        };
        this.noticeUnity("OnGetArchiveKeysResult", JSON.stringify(result_data));
      }
      else {
        var result_data = {
          r_call_back_id: arg_call_back_id.toString(),
          r_success: "0",
          r_ret: ret,
        };
        this.noticeUnity("OnGetArchiveKeysResult", JSON.stringify(result_data));
      }
    });

  },

  //保存key
  xgu_archive_save64K: function (json) {
    this.log("xgu_archive_save64K", json);
    var json_data = JSON.parse(json);
    var arg_key = json_data[U_STRINGS.KEY];
    var arg_version = json_data[U_STRINGS.VERSION];
    var arg_content = json_data[U_STRINGS.CONTENT];
    this.log("xgu_archive_save64K", "key", arg_key, "version", arg_version, "content", arg_content);

    if (this.getArchive() == null) {
      this.xgu_showToast("archive is null,please init sdk");
      return;
    }

    this.getArchive().save64k(arg_key, arg_version, arg_content);
  },

  //主动同步存档数据
  xgu_archive_forceSync: function () {
    this.log("xgu_archive_forceSync");
    if (this.getArchive() == null) {
      this.xgu_showToast("archive is null,please init sdk");
      return;
    }
    //同步云存档
    this.getArchive().forceSync();
  },


  //请求单个key
  xgu_archive_req64k: function (json) {
    this.log("xgu_archive_forceSync", json);
    var json_data = JSON.parse(json);
    var arg_key = json_data[U_STRINGS.KEY];
    var arg_call_back_id = json_data[U_STRINGS.CALL_BACK_ID];

    if (this.getArchive() == null) {
      var err = "archive is null,please init sdk";
      this.xgu_showToast(err);
      //返回给失败结果
      var result_data = {
        r_call_back_id: arg_call_back_id.toString(),
        r_success: "0",
        r_ret: err,
      };
      this.noticeUnity("OnGetArchiveDataResult", JSON.stringify(result_data));
      return;
    }

    // 请求获取单个key 的数据内容
    this.getArchive().req64k(arg_key, (ret, value) => {
      this.log("req64k result key result：", ret, value);
      if (ret == "ok") {
        //返回给成功结果
        var result_data = {
          r_call_back_id: arg_call_back_id.toString(),
          r_success: "1",
          r_ret: ret,
          r_data: JSON.stringify(value),
        };
        this.noticeUnity("OnGetArchiveDataResult", JSON.stringify(result_data));
      } else {
        //返回给失败结果
        var result_data = {
          r_call_back_id: arg_call_back_id.toString(),
          r_success: "0",
          r_ret: ret,
        };
        this.noticeUnity("OnGetArchiveDataResult", JSON.stringify(result_data));
      }
    });
  },


}

window["MethodGet"] = (msg) => { return XGameUnitySdk.MethodGet(msg); };
window["MethodCall"] = (msg) => { XGameUnitySdk.MethodCall(msg); };
console.log("## xgame-unity-oppo load done");
