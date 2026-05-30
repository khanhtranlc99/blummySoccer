//引力引擎sdk
import GravityEngine from "./xgame-plugins/gravityengine.mg.wx.min";

let __initializeFlag = false;
let __registerFlag = false;
let __logMode = false;
let __registerSuccess = false;
let __plugin = {};
//初始化
__plugin.init = function (accessToken, clientId, debugMode) {
  __log("init", { accessToken, clientId, debugMode });
  if (__initializeFlag) {
    this.log("已执行init，跳过！");
    return;
  }
  __initializeFlag = true;
  const config = {
    accessToken: accessToken, // 项目通行证，在：网站后台-->管理中心-->应用列表中找到Access Token列 复制（首次使用可能需要先新增应用）
    clientId: clientId, // 用户唯一标识，如微信小程序的openid
    autoTrack: {
      appLaunch: true, // 自动采集 $MPLaunch
      appShow: true, // 自动采集 $MPShow
      appHide: true, // 自动采集 $MPHide
    },
    name: "ge", // 全局变量名称, 默认为 gravityengine
    debugMode: debugMode ? "debugMode" : "none", // 是否开启测试模式，开启测试模式后，可以在 网站后台--管理中心--元数据--事件流中查看实时数据上报结果。（测试时使用，上线之后一定要关掉，改成none或者删除）
  };
  new GravityEngine(config);
  ge.init();
  __log('init 成功：', ge);

}

//注册
__plugin.register = function (name, version, wx_openid, wx_unionid, success, fail) {
  __log("resigter", { name, version, wx_openid, wx_unionid });
  if (__registerFlag) {
    this.log("已注册，跳过！");
    return;
  }
  __registerFlag = true;
  ge.register({
    name: name,
    version: version,
    wx_openid: wx_openid,
    wx_unionid: wx_unionid,
  }).then(() => {
    __registerSuccess = true,
      __logSuper("注册成功");
    if (success != null) {
      success();
    }
    //成功回调
  }).catch((e) => {
    //失败回调
    __logSuper("注册失败");
    if (fail != null) {
      fail(e);
    }
  });

}

//设置公共属性
__plugin.setSuperProperties = function (eventObj) {
  __log("setSuperProperties(设置公共属性)", eventObj);
  if (!__isRegisterSuccess()) {
    return;
  }
  ge.setSuperProperties(eventObj);
}

//清除单个公共属性
__plugin.unsetSuperProperty = function (propertyName) {
  __log("unsetSuperProperty 清除单个公共属性", propertyName);
  if (!__isRegisterSuccess()) {
    return;
  }
  ge.unsetSuperProperty(propertyName);
}

//删除公共属性名
__plugin.clearSuperProperties = function (propertyName) {
  __log("clearSuperProperties 删除公共属性名", propertyName);
  if (!__isRegisterSuccess()) {
    return;
  }
  ge.clearSuperProperties(propertyName);
}

//事件上报
__plugin.track = function (eventName, eventData) {
  __log("track 事件上报", { eventName, eventData });
  if (!__isRegisterSuccess()) {
    return;
  }
  if (!__logMode) {

    ge.track(eventName, eventData);
  } else {
    //日志模式    
    ge.track(eventName, eventData, new Date(), (res) => {
      __log("track", eventName, eventData, res);
    });
  }



}

//设置用户属性
__plugin.userSet = function (propertyName, propertyValue) {
  __log("userSet 设置用户属性", { propertyName, propertyValue });
  if (!__isRegisterSuccess()) {
    return;
  }
  ge.userSet(propertyName, propertyValue);
}

/**
 * 上报付费事件
 * @param payAmount     付费金额 单位为分
 * @param payType       货币类型 按照国际标准组织ISO 4217中规范的3位字母，例如CNY人民币、USD美金等
 * @param orderId       订单号
 * @param payReason     付费原因 例如：购买钻石、办理月卡
 * @param payMethod     付费方式 例如：支付宝、微信、银联等
 */
//付费上报
__plugin.payEvent = function (payAmount, payType, orderId, payReason, payMethod) {
  __log("payEvent 上报付费事件", { payAmount, payType, orderId, payReason, payMethod });
  if (!__isRegisterSuccess()) {
    return;
  }
  ge.payEvent(payAmount, payType, orderId, payReason, payMethod);
}

/**
* 上报广告展示事件 参数如下
* @param adType        广告类型 （取值为：reward、banner、native、interstitial、video_feed、video_begin，分别对应激励视频广告、Banner广告、原生模板广告、插屏广告、视频广告、视频贴片广告）
* @param adUnitId      广告位ID
*/
__plugin.adShowEvent = function (adType, adUnitId) {
  __log("adShowEvent 上报广告展示事件", adType, adUnitId);
  if (!__isRegisterSuccess()) {
    return;
  }
  ge.adShowEvent(adType, adUnitId);
}


__plugin.ping = function (...params) {
  __logSuper(params);
}

//开启debug模式
__plugin.debugOn = function () {
  __logMode = true;
}

//关闭debug模式
__plugin.debugOff = function () {
  __logMode = false;
}

function __log (...params) {
  if (!__logMode) {
    return;
  }
  console.log("##xgame-ge-plugins 日志", params);
}
function __logSuper (...params) {
  console.log("##xgame-ge-plugins 日志", params);
}

function __isRegisterSuccess () {

  if (__registerSuccess) {
    return true;
  }
  __logSuper('未注册，无法上报失败');
  return false;

}

//对外暴露
window["xgame-plugin-ge-wx"]=__plugin;
export default __plugin

