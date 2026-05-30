
let _DEBUG_MODE = false;
let ___UNITY_OPPO_BRIDGE = "___UNITY_OPPO_BRIDGE";

//字符串常量
class OppoStrings { }
OppoStrings.success = "success";
OppoStrings.filePath = "filePath";
OppoStrings.arg_1 = "arg_1";
OppoStrings.arg_2 = "arg_2";
OppoStrings.arg_3 = "arg_3";


//bgm状态
let UNITY_OPPO_BRIDGE_BGM_STATE_PLAY = 0;//播放
let UNITY_OPPO_BRIDGE_BGM_STATE_STOP = 1;//停止状态

//audio片段字典
let _untiy_oppo_bridge_bgm_audios = new Map();

//bgm audio 可播放
let _untiy_oppo_bridge_bgm_audio_ready = new Map();

//bgm 片段字典
let _unity_oppo_bridge_bgm_files = new Map();
//bgm播放状态
let _untiy_oppo_bridge_bgm_state = UNITY_OPPO_BRIDGE_BGM_STATE_PLAY;
//bgm资源路径
let _unity_oppo_bridge_bgm_url = "";
//当前播放的bgm
let _unity_oppo_bridge_current_play_bgm = "";



//下载文件
function downloadFile (json) {
  this.oppo_bridge_log("downloadFile", json);
  var data = JSON.parse(json);
  var arg_url = data[OppoStrings.arg_1];
  var arg_filePath = data[OppoStrings.arg_2];
  var arg_callbackCode = data[OppoStrings.arg_3].toString();

  // var dirPath = getUserDataPath() + "/____com_typhoon_assetbundle/153";
  // try {
  //   fs.accessSync(dirPath);
  // } catch (error) {
  //   this.oppo_bridge_log ("create folder", dirPath);
  //   var fs = qg.getFileSystemManager();
  //   fs.mkdirSync(dirPath, true);
  // }
  var param = {
    url: arg_url,
    filePath: arg_filePath,
    success: (res) => { this.onFileDownloadSuccess(res, arg_callbackCode); },
    fail: (res) => { this.onFileDownLoadFailed(res, arg_callbackCode); }
  };
  qg.downloadFile(param);
}

//文件下载成功
function onFileDownloadSuccess (arg_res, arg_callbackCode) {
  this.oppo_bridge_log("onFileDownloadSuccess", arg_res, arg_callbackCode);
  let result = {
    callbackCode: arg_callbackCode,
    success: true,
    tempFilePath: arg_res.tempFilePath,
    errCode: arg_res.errCode,
    errMsg: arg_res.errMsg,
  }
  var msg = objectToJson(result);
  //通知unity,下载成功  
  this.bridge_noticeUnityJsCallBack(msg);

}

//文件下载失败
function onFileDownLoadFailed (arg_res, arg_callbackCode) {
  this.oppo_bridge_log("onFileDownLoadFailed", arg_res, arg_callbackCode);
  let result = {
    callbackCode: arg_callbackCode,
    success: false,
    errCode: arg_res.errCode,
    errMsg: arg_res.errMsg,
  }
  this.oppo_bridge_log("onFileDownLoadFailed result", result.errCode, result.errMsg);
  var msg = objectToJson(result);
  //通知unity,下载成功  
  this.bridge_noticeUnityJsCallBack(msg);
}


//是否有文件or文件夹
function hasFileOrDirectory (json) {
  this.oppo_bridge_log("hasFileOrFolderPath", json);
  var data = JSON.parse(json);
  var arg_path = data[OppoStrings.arg_1];
  var fs = qg.getFileSystemManager();
  this.oppo_bridge_log("hasFileOrFolderPath", 22);
  try {
    fs.accessSync(arg_path);
    return "1";
  } catch (error) {

  }
  return "0";
}


//创建目录
function createDirectory (json) {
  this.oppo_bridge_log("createDirectory", json);
  var data = JSON.parse(json);
  var arg_dirPath = data[OppoStrings.arg_1];
  var arg_recursive = data[OppoStrings.arg_2];
  var fs = qg.getFileSystemManager();
  fs.mkdirSync(arg_dirPath, arg_recursive);
}

//删除目录
function deleteDirectory (json) {
  this.oppo_bridge_log("deleteDirectory", json);
  var data = JSON.parse(json);
  var arg_dirPath = data[OppoStrings.arg_1];
  var arg_recursive = data[OppoStrings.arg_2];
  var fs = qg.getFileSystemManager();
  fs.rmdirSync(arg_dirPath, arg_recursive);
}

//获取文件夹目录
function getDirectoryFiles (json) {
  this.oppo_bridge_log("getDirectoryFiles", json);
  var data = JSON.parse(json);
  var arg_dirPath = data[OppoStrings.arg_1];
  var fs = qg.getFileSystemManager();
  var result = fs.readdirSync(arg_dirPath);
  return this.objectToJson(result);
}

//删除文件
function deleteFile (json) {
  this.oppo_bridge_log("deleteFile", json);
  var data = JSON.parse(json);
  var arg_filePath = data[OppoStrings.arg_1];
  var fs = qg.getFileSystemManager();
  fs.unlinkSync(arg_filePath);
}

//读取文件
function readFile (json) {
  this.oppo_bridge_log("readFile", json);
  var data = JSON.parse(json);
  var arg_filePath = data[OppoStrings.arg_1];
  var arg_callbackCode = data[OppoStrings.arg_2];
  var fs = qg.getFileSystemManager();
  var param = {
    filePath: arg_filePath,
    success: (res) => { this.onReadFileSuccess(res, arg_callbackCode); },
    fail: (res) => { this.onReadFileFail(res, arg_callbackCode); },
  }
  fs.readFile(param);
}

//文件读取成功
function onReadFileSuccess (arg_res, arg_callbackCode) {
  this.oppo_bridge_log("onReadFileSuccess", arg_res, arg_callbackCode);
  // this.oppo_bridge_log (arg_res.data, arg_res.data.length);

  //byte[]转成string
  var bytes = buf2hex(arg_res.data);
  let result = {
    callbackCode: arg_callbackCode,
    success: true,
    byteString: bytes,
  }
  //this.oppo_bridge_log ("onReadFileSuccess bytestring", bytes);
  //this.oppo_bridge_log ("onReadFileSuccess result", arg_res);
  var msg = objectToJson(result);
  //通知unity,下载成功  
  this.bridge_noticeUnityJsCallBack(msg);
}


//文件读取失败
function onReadFileFail (arg_res, arg_callbackCode) {
  this.oppo_bridge_log("onReadFileFail", arg_res, arg_callbackCode);
  let result = {
    callbackCode: arg_callbackCode,
    success: false,
  }
  var msg = objectToJson(result);
  //通知unity,下载成功  
  this.bridge_noticeUnityJsCallBack(msg);
}

//byte[]转16进制string
function buf2hex (buffer) {
  var array = new Uint8Array(buffer);
  var str = array.toString();
  str = '[' + str + ']';
  array = null;
  return str;
};

//获取用户Data路径
function getUserDataPath (json) {
  this.oppo_bridge_log("getUserDataPath");
  return qg.env.USER_DATA_PATH;
}


//object转json  
function objectToJson (arg) {
  return JSON.stringify(arg);
}


//打印日志
function oppo_bridge_log (...args) {
  if (_DEBUG_MODE) {
    console.log("## unity-oppo-bridge ", ...args);
  }
}


//向unity发送消息
function bridge_noticeUnity (arg_method, arg_msg) {
  window.unityInstance.SendMessage(___UNITY_OPPO_BRIDGE, arg_method, arg_msg);
}


//执行OnJsCallBack回调  
function bridge_noticeUnityJsCallBack (arg_msg) {
  this.bridge_noticeUnity("OnJsCallBack", arg_msg)
}


//call方法
function _oppo_bridge_call (arg_method_id, json) {
  this.oppo_bridge_log("_oppo_bridge_call", arg_method_id, json);
  switch (arg_method_id) {
    case 1001:
      this.downloadFile(json);
      return;
    case 1002:
      this.createDirectory(json);
      return;
    case 1003:
      this.deleteDirectory(json);
      return;
    case 1004:
      this.deleteFile(json);
      return;
    case 1005:
      this.readFile(json);
      return;
    case 1006://播放bgm
      this.playBgm(json);
      return;
    case 1007://停止bgm
      this.stopBgm();
      return;

  }

  throw new Error("## unity-oppo-bridge  error:" + "no find call method id:" + arg_method_id);
}

//get方法
function _oppo_bridge_get (arg_method_id, json) {

  this.oppo_bridge_log("_oppo_bridge_get", arg_method_id, json);
  this.oppo_bridge_log(qg.env.USER_DATA_PATH);
  switch (arg_method_id) {
    case 2001:
      return this.getUserDataPath(json);
    case 2002:
      return this.hasFileOrDirectory(json);
    case 2003:
      return this.getDirectoryFiles(json);
  }
  throw new Error("## unity-oppo-bridge  error:" + "no find get method id:" + arg_method_id);

}


//播放bgm
function playBgm (json) {
  this.oppo_bridge_log("playBgm", json);
  //播放音频
  var data = JSON.parse(json);
  //音效地址
  var arg_url = data[OppoStrings.arg_1];
  //设置bgm状态
  _untiy_oppo_bridge_bgm_state = UNITY_OPPO_BRIDGE_BGM_STATE_PLAY;
  _unity_oppo_bridge_bgm_url = arg_url;
  //加载音效
  loadBgmFile(arg_url);
  //触发变化事件
  onBgmStageChanged();
}

//停止bgm
function stopBgm () {
  this.oppo_bridge_log("stopBgm");
  //设置bgm状态
  _untiy_oppo_bridge_bgm_state = UNITY_OPPO_BRIDGE_BGM_STATE_STOP;
  //触发变化事件
  onBgmStageChanged();
}

//当bgm状态改变时
function onBgmStageChanged () {
  this.oppo_bridge_log("onBgmStageChanged");
  switch (_untiy_oppo_bridge_bgm_state) {
    case UNITY_OPPO_BRIDGE_BGM_STATE_PLAY://播放状态
      {
        //目标bgm url
        var target_bgm_url = _unity_oppo_bridge_bgm_url;
        //停止所有正在播放的音频
        for (let [k, v] of _untiy_oppo_bridge_bgm_audios.entries()) {
          //如果不是目标bgm
          if (k != target_bgm_url && this.bgmIsReady(k)) {
            //停止播放,不是目标bgm
            v.stop();
            if (_unity_oppo_bridge_current_play_bgm == k) {
              _unity_oppo_bridge_current_play_bgm = "";
            }
          }
        }


        if (_untiy_oppo_bridge_bgm_audios.has(target_bgm_url)) {
          if (!this.bgmIsPlaying(target_bgm_url)) {
            if (this.bgmIsReady(target_bgm_url)) {
              //播放bgm
              _untiy_oppo_bridge_bgm_audios.get(target_bgm_url).play();
              //标记正在播放的bgm
              _unity_oppo_bridge_current_play_bgm = target_bgm_url;
            }
          }
        }

      }
      break;
    case UNITY_OPPO_BRIDGE_BGM_STATE_STOP://暂停状态
      {
        //遍历所有的音频片段,停止播放
        for (let [k, v] of _untiy_oppo_bridge_bgm_audios.entries()) {
          if (this.bgmIsReady(k)) {
            v.stop();
          }
        }
        //置空当前播放的bgm
        _unity_oppo_bridge_current_play_bgm = "";
      }

      break;
  }

}


//当bgm下载完毕
function onBgmFilesStageChanged (arg_success, arg_url) {
  this.oppo_bridge_log("onBgmFilesStageChanged", arg_success, arg_url);
  if (arg_success) {
    //下载成功
    _unity_oppo_bridge_bgm_files.set(arg_url, true);
  } else {
    //下载失败,删除key

    if (_unity_oppo_bridge_bgm_files.has(arg_url)) {
      _unity_oppo_bridge_bgm_files.delete(arg_url);
    }
  }
  loadBgmAudio();
}

//记载bgm文件
function loadBgmFile (arg_url) {
  this.oppo_bridge_log("loadBgmFile", arg_url, _unity_oppo_bridge_bgm_files);
  if (!_unity_oppo_bridge_bgm_files.has(arg_url)) {
    //默认bgm文件不存在
    _unity_oppo_bridge_bgm_files.set(arg_url, false);
    //本地加载路径
    var tempFilePath = this.toCachePath(arg_url);
    var fs = qg.getFileSystemManager();
    var hasFileInLocal = false;
    try {
      fs.accessSync(tempFilePath);
      hasFileInLocal = true;
    } catch (error) {
      //无文件
    }

    if (hasFileInLocal) {
      //本地文件存在
      this.onBgmFilesStageChanged(true, arg_url);
    } else {
      this.oppo_bridge_log("hasFileInLocal false qg.downloadFile", arg_url, tempFilePath);
      //从云端下载
      qg.downloadFile({
        url: arg_url,
        filePath: tempFilePath,
        success (msg) {
          this.oppo_bridge_log("qg.downloadFile success", msg, arg_url);
          //下载成功
          this.onBgmFilesStageChanged(true, arg_url);
        },
        fail (msg) {
          this.oppo_bridge_log("qg.downloadFile fail", msg.errCode, msg.errMsg, arg_url);
          //下载失败
          this.onBgmFilesStageChanged(false, arg_url);
        },
        complete () { },
      });
    }
  }

}

//加载bgm audio

function loadBgmAudio () {
  this.oppo_bridge_log("loadBgmAudio");
  //遍历音频文件字典，加载需要的音频片段
  for (let [k, v] of _unity_oppo_bridge_bgm_files.entries()) {
    //补充 aduio
    if (v == true) {
      var arg_url = k;
      //是否存在实例
      if (!_untiy_oppo_bridge_bgm_audios.has(arg_url)) {
        var loadPath = this.toCachePath(arg_url);
        //不存在实例，加载音频实例
        var audio = qg.createInnerAudioContext();
        audio.loop = true;
        audio.autoplay = false;
        var playSound = function () {
          this.onBgmAudioLoadDone(arg_url);
          audio.offCanplay(playSound);
        };
        //触发回调
        audio.onCanplay(playSound);
        audio.src = loadPath;
        _untiy_oppo_bridge_bgm_audios.set(k, audio);
      }

    }
  }
}

//bgm audio 加载完毕
function onBgmAudioLoadDone (arg_url) {
  this.oppo_bridge_log("onBgmAudioLoadDone", arg_url);
  //标记bgm可播放  
  _untiy_oppo_bridge_bgm_audio_ready.set(arg_url, true);
  this.onBgmStageChanged();
}


//bgm是否准备完毕
function bgmIsReady (arg_url) {
  if (_untiy_oppo_bridge_bgm_audio_ready.has(arg_url)) {
    return _untiy_oppo_bridge_bgm_audio_ready.get(arg_url);
  }
  return false;

}

//bgm是否在播放中
function bgmIsPlaying (arg_url) {
  return _untiy_oppo_bridge_bgm_audios.has(arg_url) && this.bgmIsReady(arg_url) && _unity_oppo_bridge_current_play_bgm == arg_url;

}



function toCachePath (url) {
  var cachePath = this._uob_stringReplaceAll(url, "/", "___");
  cachePath = this._uob_stringReplaceAll(cachePath, ":", "_#_");
  if (cachePath.length > 200) {
    cachePath = cachePath.substring(0, 200);
  }
  return qg.env.USER_DATA_PATH + "/" + cachePath;
}

//替换字符串
function _uob_stringReplaceAll (str, s1, s2) {
  return str.replace(new RegExp(s1, "gm"), s2);
}

window["_oppo_bridge_call"] = _oppo_bridge_call;
window["_oppo_bridge_get"] = _oppo_bridge_get;
console.log("## unity-oppo-bridge  load done");
