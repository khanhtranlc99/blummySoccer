
//渠道ID
let channelId = 1;

//字符串名
let MESSAGE_ID = "messageId";
let PRODUCT_ID = "productId";
let PRICE = "price";
let PRODUCT_DESC = "productDesc";
let PRODUCT_NAME = "productName";
let CODE = "code";
let SCENE_NAME = "sceneName";
let ORDER_ID = "orderId";


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
//DEBUG模式
let DEBUG_MODE = false;

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
      showBanner();
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

  }
}


//通知unity
function noticeUnity (method, msg) {
  window.unityInstance.SendMessage("WEB_TO_UNITY_CALLBACK", method, msg);
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
  log("initSdk","channelId",channelId);  
  let XGameGlobal=window.XGameGlobal;
  XGameGlobal["xgame.sdk.init"]((sdk) => {
    window["sdk"] = sdk;
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
    log("login result:",ret);
    let msg = "0";
    //登录成功
    if (ret == "ok") {
      msg = "1";
      //通知Unity
      noticeUnity("OnLoginResult", msg);
      //尝试补单
      tryHandleOrdersQueue();
    } else {
      qg.showModal({
        title: '提示',
        content: '登录失败，是否重试？',
        success (res) {
          if (res.confirm) {
            login();
          } else if (res.cancel) {
            noticeUnity("OnLoginResult", msg);
          }
        }
      })
    }

  });
}


//是否有插屏
function getIntersFlag () {
  log("getIntersFlag");
  let flag = getAd().getIntersFlag();
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
  let flag = getAd().getVideoFlag();
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


//显示banner
function showBanner () {
  log("showBanner");
  //构造回调
  var showResult = getAd().newResult();
  getAd().showBanner(showResult);
}

//隐藏banner
function hideBanner () {
  log("hideBanner");
  getAd().hideBanner();
}



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


//完成订单
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


function logError (msg) {
  console.error("## error xgame-unity " + msg);
}

function log (...args) {
  if (DEBUG_MODE) {
    console.log("## log xgame-unity ", ...args);
  }
}

window["MethodGet"] = mGet;
window["MethodCall"] = mCall;
console.log("## xgame-unity-oppo load done");