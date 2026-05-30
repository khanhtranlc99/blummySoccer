/* eslint-disable  quotes */
require("./qgame-adapter.js")
require("./store.js")
require("./unityAdapter.js")
require("./xgame-sdk-vivo-mini.js")
require("./xgame-extension-archive.js")
require("./xgame-unity-audio-vivo.js")
require("./xgame-unity-vivo.js")

const md5Utils = require('./md5.js');

//使用该功能时，需判断引擎是否支持
//创建CustomizeLoading组件
var loading;
if (qg.createCustomizeLoading) {
  loading = qg.createCustomizeLoading({
    background: 'image/background.png',
    text: '正在加载中...',
    textColor: '#ffffff',
    loadingColorTop: '#ffffff',
    loadingColorBottom: '#ffffff',
    loadingProgress: 0
  });
}

function updateLoading(progress) {
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }

  //根据实际场景进行更新进度、背景、文字以及文字颜色
  //更新CustomizeLoading样式
  loading.update({
    loadingProgress: progress * 100
  });
}

function updateLoadingError() {
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }

  loading.update({
    text: '加载失败，请重启游戏'
  });
}

function removeLoading() {
  if (!qg.createCustomizeLoading || !loading) {
    return;
  }

  //移除CustomizeLoading组件
  loading.remove();
}

function compileRateSimulate() {
  if (!qg.createCustomizeLoading || !loading) {
    clearInterval(loadingTask)
    return;
  }
  loadCurRate += 0.02
  if (loadCurRate >= 0.99) {
    clearInterval(loadingTask)
    return
  }
  updateLoading(loadCurRate);
}

var loadingTask = null
const down_take_rate = 0.5 // 下载所占比例
var loadCurRate = 0

function execUnity() {
  /* eslint-disable  quotes */
  preloadAssets();
  loadCurRate = down_take_rate
  loadingTask = setInterval(compileRateSimulate, 1000)
  window['unityInstance'] = window.UnityLoader.instantiate('canvas', '/buildUnity/webgl.json', {
    onProgress: function (_, i) {
      // 更新启动loading组件进度
      // unity自身进度逻辑：文件准备完成90%，编译完成99%，完成100%
      // 由于编译时间较久，此处仅使用100%的逻辑，其他使用模拟数据
      if (i === 1) {
        clearInterval(loadingTask)
        updateLoading(i);

        console.log('加载完成')
        //移除启动loading组件
        removeLoading();
      }
    }
  })

}

function downloadSource(sourceUrl) {

  var key = md5Utils.hex_md5(sourceUrl)
  var cache_key = window.qg.getStorageSync({
    key: 'mini_wasm_cache_url_md5',
    default: 'default'
  })
  if (cache_key === key &&
    'true' === window.qg.accessFile({
      uri: window.qg.env.USER_DATA_PATH + "/" + key + "/online_mini.data.unityweb"
    }) &&
    'true' === window.qg.accessFile({
      uri: window.qg.env.USER_DATA_PATH + "/" + key + "/online_mini.wasm.code.unityweb"
    })) {
    updateLoading(down_take_rate);
    execUnity();
    return
  }
  if (cache_key !== 'default') {
    window.qg.rmdir({
      uri: window.qg.env.USER_DATA_PATH + "/" + cache_key
    })
  }
  qg.setStorage({
    key: 'mini_wasm_cache_url_md5',
    value: key,
    success: function (data) {
      console.log('mini_wasm_cache_url_md5 cache success')
    },
    fail: function (data, code) {
      console.error(`mini_wasm_cache_url_md5 cache fail, code = ${code}`)
    }
  })
  var downPath = window.qg.env.USER_DATA_PATH + "/" + key + "/wasm_zipsource"
  var downloadTask = window.qg.downloadFile({
    url: sourceUrl,
    filePath: downPath,
    success: function () {
      window.qg.unzipFile({
        srcUri: downPath,
        dstUri: window.qg.env.USER_DATA_PATH + "/" + key + "/",
        success: function (uri) {
          updateLoading(down_take_rate)
          execUnity()
        },
        fail: function (data, code) {
          console.error(`wasm unzip handling fail, code = ${code}`)
          updateLoadingError()
        }
      })
    },
    fail: function (e) {
      console.error("wasm download file fail " + JSON.stringify(e))
      updateLoadingError()
    }
  });
  downloadTask.onProgressUpdate(function (msg) {
    var progress = msg["progress"];
    updateLoading(progress / 100 * down_take_rate)
  });
}

function preloadAssets(){
  var preloadUrl = UnityLoader.EnvConfig.getConfig("preloadUrl")
  var preloadUrlList = preloadUrl.split(';')
  preloadUrlList.forEach((url, index) => {
    // 创建网络请求
    qg.request({
      url: url,
      dataType: 'arraybuffer',
      success: function(ret) {
        console.log("preloadAssets request success " + " url = " + url)
      },
      fail: function(error, code) {
        console.error("preloadAssets request fail " + " url = " + url + " code = " + code)
      }
    });
  });
}

var wasmUrl = UnityLoader.EnvConfig.getConfig("wasmUrl")
downloadSource(wasmUrl)
//execUnity()