/*
 * JavaScript MD5
 * https://github.com/blueimp/JavaScript-MD5
 *
 * Copyright 2011, Sebastian Tschan
 * https://blueimp.net
 *
 * Licensed under the MIT license:
 * https://opensource.org/licenses/MIT
 *
 * Based on
 * A JavaScript implementation of the RSA Data Security, Inc. MD5 Message
 * Digest Algorithm, as defined in RFC 1321.
 * Version 2.2 Copyright (C) Paul Johnston 1999 - 2009
 * Other contributors: Greg Holt, Andrew Kepert, Ydnar, Lostinet
 * Distributed under the BSD License
 * See http://pajhome.org.uk/crypt/md5 for more info.=
 */

/* global define */

/* eslint-disable strict */
let md5
  ; (function ($) {
    'use strict'

    /**
     * Add integers, wrapping at 2^32.
     * This uses 16-bit operations internally to work around bugs in interpreters.
     *
     * @param {number} x First integer
     * @param {number} y Second integer
     * @returns {number} Sum
     */
    function safeAdd (x, y) {
      var lsw = (x & 0xffff) + (y & 0xffff)
      var msw = (x >> 16) + (y >> 16) + (lsw >> 16)
      return (msw << 16) | (lsw & 0xffff)
    }

    /**
     * Bitwise rotate a 32-bit number to the left.
     *
     * @param {number} num 32-bit number
     * @param {number} cnt Rotation count
     * @returns {number} Rotated number
     */
    function bitRotateLeft (num, cnt) {
      return (num << cnt) | (num >>> (32 - cnt))
    }

    /**
     * Basic operation the algorithm uses.
     *
     * @param {number} q q
     * @param {number} a a
     * @param {number} b b
     * @param {number} x x
     * @param {number} s s
     * @param {number} t t
     * @returns {number} Result
     */
    function md5cmn (q, a, b, x, s, t) {
      return safeAdd(bitRotateLeft(safeAdd(safeAdd(a, q), safeAdd(x, t)), s), b)
    }
    /**
     * Basic operation the algorithm uses.
     *
     * @param {number} a a
     * @param {number} b b
     * @param {number} c c
     * @param {number} d d
     * @param {number} x x
     * @param {number} s s
     * @param {number} t t
     * @returns {number} Result
     */
    function md5ff (a, b, c, d, x, s, t) {
      return md5cmn((b & c) | (~b & d), a, b, x, s, t)
    }
    /**
     * Basic operation the algorithm uses.
     *
     * @param {number} a a
     * @param {number} b b
     * @param {number} c c
     * @param {number} d d
     * @param {number} x x
     * @param {number} s s
     * @param {number} t t
     * @returns {number} Result
     */
    function md5gg (a, b, c, d, x, s, t) {
      return md5cmn((b & d) | (c & ~d), a, b, x, s, t)
    }
    /**
     * Basic operation the algorithm uses.
     *
     * @param {number} a a
     * @param {number} b b
     * @param {number} c c
     * @param {number} d d
     * @param {number} x x
     * @param {number} s s
     * @param {number} t t
     * @returns {number} Result
     */
    function md5hh (a, b, c, d, x, s, t) {
      return md5cmn(b ^ c ^ d, a, b, x, s, t)
    }
    /**
     * Basic operation the algorithm uses.
     *
     * @param {number} a a
     * @param {number} b b
     * @param {number} c c
     * @param {number} d d
     * @param {number} x x
     * @param {number} s s
     * @param {number} t t
     * @returns {number} Result
     */
    function md5ii (a, b, c, d, x, s, t) {
      return md5cmn(c ^ (b | ~d), a, b, x, s, t)
    }

    /**
     * Calculate the MD5 of an array of little-endian words, and a bit length.
     *
     * @param {Array} x Array of little-endian words
     * @param {number} len Bit length
     * @returns {Array<number>} MD5 Array
     */
    function binlMD5 (x, len) {
      /* append padding */
      x[len >> 5] |= 0x80 << len % 32
      x[(((len + 64) >>> 9) << 4) + 14] = len

      var i
      var olda
      var oldb
      var oldc
      var oldd
      var a = 1732584193
      var b = -271733879
      var c = -1732584194
      var d = 271733878

      for (i = 0; i < x.length; i += 16) {
        olda = a
        oldb = b
        oldc = c
        oldd = d

        a = md5ff(a, b, c, d, x[i], 7, -680876936)
        d = md5ff(d, a, b, c, x[i + 1], 12, -389564586)
        c = md5ff(c, d, a, b, x[i + 2], 17, 606105819)
        b = md5ff(b, c, d, a, x[i + 3], 22, -1044525330)
        a = md5ff(a, b, c, d, x[i + 4], 7, -176418897)
        d = md5ff(d, a, b, c, x[i + 5], 12, 1200080426)
        c = md5ff(c, d, a, b, x[i + 6], 17, -1473231341)
        b = md5ff(b, c, d, a, x[i + 7], 22, -45705983)
        a = md5ff(a, b, c, d, x[i + 8], 7, 1770035416)
        d = md5ff(d, a, b, c, x[i + 9], 12, -1958414417)
        c = md5ff(c, d, a, b, x[i + 10], 17, -42063)
        b = md5ff(b, c, d, a, x[i + 11], 22, -1990404162)
        a = md5ff(a, b, c, d, x[i + 12], 7, 1804603682)
        d = md5ff(d, a, b, c, x[i + 13], 12, -40341101)
        c = md5ff(c, d, a, b, x[i + 14], 17, -1502002290)
        b = md5ff(b, c, d, a, x[i + 15], 22, 1236535329)

        a = md5gg(a, b, c, d, x[i + 1], 5, -165796510)
        d = md5gg(d, a, b, c, x[i + 6], 9, -1069501632)
        c = md5gg(c, d, a, b, x[i + 11], 14, 643717713)
        b = md5gg(b, c, d, a, x[i], 20, -373897302)
        a = md5gg(a, b, c, d, x[i + 5], 5, -701558691)
        d = md5gg(d, a, b, c, x[i + 10], 9, 38016083)
        c = md5gg(c, d, a, b, x[i + 15], 14, -660478335)
        b = md5gg(b, c, d, a, x[i + 4], 20, -405537848)
        a = md5gg(a, b, c, d, x[i + 9], 5, 568446438)
        d = md5gg(d, a, b, c, x[i + 14], 9, -1019803690)
        c = md5gg(c, d, a, b, x[i + 3], 14, -187363961)
        b = md5gg(b, c, d, a, x[i + 8], 20, 1163531501)
        a = md5gg(a, b, c, d, x[i + 13], 5, -1444681467)
        d = md5gg(d, a, b, c, x[i + 2], 9, -51403784)
        c = md5gg(c, d, a, b, x[i + 7], 14, 1735328473)
        b = md5gg(b, c, d, a, x[i + 12], 20, -1926607734)

        a = md5hh(a, b, c, d, x[i + 5], 4, -378558)
        d = md5hh(d, a, b, c, x[i + 8], 11, -2022574463)
        c = md5hh(c, d, a, b, x[i + 11], 16, 1839030562)
        b = md5hh(b, c, d, a, x[i + 14], 23, -35309556)
        a = md5hh(a, b, c, d, x[i + 1], 4, -1530992060)
        d = md5hh(d, a, b, c, x[i + 4], 11, 1272893353)
        c = md5hh(c, d, a, b, x[i + 7], 16, -155497632)
        b = md5hh(b, c, d, a, x[i + 10], 23, -1094730640)
        a = md5hh(a, b, c, d, x[i + 13], 4, 681279174)
        d = md5hh(d, a, b, c, x[i], 11, -358537222)
        c = md5hh(c, d, a, b, x[i + 3], 16, -722521979)
        b = md5hh(b, c, d, a, x[i + 6], 23, 76029189)
        a = md5hh(a, b, c, d, x[i + 9], 4, -640364487)
        d = md5hh(d, a, b, c, x[i + 12], 11, -421815835)
        c = md5hh(c, d, a, b, x[i + 15], 16, 530742520)
        b = md5hh(b, c, d, a, x[i + 2], 23, -995338651)

        a = md5ii(a, b, c, d, x[i], 6, -198630844)
        d = md5ii(d, a, b, c, x[i + 7], 10, 1126891415)
        c = md5ii(c, d, a, b, x[i + 14], 15, -1416354905)
        b = md5ii(b, c, d, a, x[i + 5], 21, -57434055)
        a = md5ii(a, b, c, d, x[i + 12], 6, 1700485571)
        d = md5ii(d, a, b, c, x[i + 3], 10, -1894986606)
        c = md5ii(c, d, a, b, x[i + 10], 15, -1051523)
        b = md5ii(b, c, d, a, x[i + 1], 21, -2054922799)
        a = md5ii(a, b, c, d, x[i + 8], 6, 1873313359)
        d = md5ii(d, a, b, c, x[i + 15], 10, -30611744)
        c = md5ii(c, d, a, b, x[i + 6], 15, -1560198380)
        b = md5ii(b, c, d, a, x[i + 13], 21, 1309151649)
        a = md5ii(a, b, c, d, x[i + 4], 6, -145523070)
        d = md5ii(d, a, b, c, x[i + 11], 10, -1120210379)
        c = md5ii(c, d, a, b, x[i + 2], 15, 718787259)
        b = md5ii(b, c, d, a, x[i + 9], 21, -343485551)

        a = safeAdd(a, olda)
        b = safeAdd(b, oldb)
        c = safeAdd(c, oldc)
        d = safeAdd(d, oldd)
      }
      return [a, b, c, d]
    }

    /**
     * Convert an array of little-endian words to a string
     *
     * @param {Array<number>} input MD5 Array
     * @returns {string} MD5 string
     */
    function binl2rstr (input) {
      var i
      var output = ''
      var length32 = input.length * 32
      for (i = 0; i < length32; i += 8) {
        output += String.fromCharCode((input[i >> 5] >>> i % 32) & 0xff)
      }
      return output
    }

    /**
     * Convert a raw string to an array of little-endian words
     * Characters >255 have their high-byte silently ignored.
     *
     * @param {string} input Raw input string
     * @returns {Array<number>} Array of little-endian words
     */
    function rstr2binl (input) {
      var i
      var output = []
      output[(input.length >> 2) - 1] = undefined
      for (i = 0; i < output.length; i += 1) {
        output[i] = 0
      }
      var length8 = input.length * 8
      for (i = 0; i < length8; i += 8) {
        output[i >> 5] |= (input.charCodeAt(i / 8) & 0xff) << i % 32
      }
      return output
    }

    /**
     * Calculate the MD5 of a raw string
     *
     * @param {string} s Input string
     * @returns {string} Raw MD5 string
     */
    function rstrMD5 (s) {
      return binl2rstr(binlMD5(rstr2binl(s), s.length * 8))
    }

    /**
     * Calculates the HMAC-MD5 of a key and some data (raw strings)
     *
     * @param {string} key HMAC key
     * @param {string} data Raw input string
     * @returns {string} Raw MD5 string
     */
    function rstrHMACMD5 (key, data) {
      var i
      var bkey = rstr2binl(key)
      var ipad = []
      var opad = []
      var hash
      ipad[15] = opad[15] = undefined
      if (bkey.length > 16) {
        bkey = binlMD5(bkey, key.length * 8)
      }
      for (i = 0; i < 16; i += 1) {
        ipad[i] = bkey[i] ^ 0x36363636
        opad[i] = bkey[i] ^ 0x5c5c5c5c
      }
      hash = binlMD5(ipad.concat(rstr2binl(data)), 512 + data.length * 8)
      return binl2rstr(binlMD5(opad.concat(hash), 512 + 128))
    }

    /**
     * Convert a raw string to a hex string
     *
     * @param {string} input Raw input string
     * @returns {string} Hex encoded string
     */
    function rstr2hex (input) {
      var hexTab = '0123456789abcdef'
      var output = ''
      var x
      var i
      for (i = 0; i < input.length; i += 1) {
        x = input.charCodeAt(i)
        output += hexTab.charAt((x >>> 4) & 0x0f) + hexTab.charAt(x & 0x0f)
      }
      return output
    }

    /**
     * Encode a string as UTF-8
     *
     * @param {string} input Input string
     * @returns {string} UTF8 string
     */
    function str2rstrUTF8 (input) {
      return unescape(encodeURIComponent(input))
    }

    /**
     * Encodes input string as raw MD5 string
     *
     * @param {string} s Input string
     * @returns {string} Raw MD5 string
     */
    function rawMD5 (s) {
      return rstrMD5(str2rstrUTF8(s))
    }
    /**
     * Encodes input string as Hex encoded string
     *
     * @param {string} s Input string
     * @returns {string} Hex encoded string
     */
    function hexMD5 (s) {
      return rstr2hex(rawMD5(s))
    }
    /**
     * Calculates the raw HMAC-MD5 for the given key and data
     *
     * @param {string} k HMAC key
     * @param {string} d Input string
     * @returns {string} Raw MD5 string
     */
    function rawHMACMD5 (k, d) {
      return rstrHMACMD5(str2rstrUTF8(k), str2rstrUTF8(d))
    }
    /**
     * Calculates the Hex encoded HMAC-MD5 for the given key and data
     *
     * @param {string} k HMAC key
     * @param {string} d Input string
     * @returns {string} Raw MD5 string
     */
    function hexHMACMD5 (k, d) {
      return rstr2hex(rawHMACMD5(k, d))
    }

    /**
     * Calculates MD5 value for a given string.
     * If a key is provided, calculates the HMAC-MD5 value.
     * Returns a Hex encoded string unless the raw argument is given.
     *
     * @param {string} string Input string
     * @param {string} [key] HMAC key
     * @param {boolean} [raw] Raw output switch
     * @returns {string} MD5 output
     */
    md5 = function (string, key, raw) {
      if (!key) {
        if (!raw) {
          return hexMD5(string)
        }
        return rawMD5(string)
      }
      if (!raw) {
        return hexHMACMD5(key, string)
      }
      return rawHMACMD5(key, string)
    }

    if (typeof define === 'function' && define.amd) {
      define(function () {
        return md5
      })
    } else if (typeof module === 'object' && module.exports) {
      module.exports = md5
    } else {
      $.md5 = md5
    }
  })(this)


//音频行为类型
let XGUA_CLIP_ACTION_TYPE = {
  Play: 1,//播放
  Stop: 2,//停止
  Pause: 3,//暂停  
  Seek: 4,//跳转到指定位置
  SetVolume: 5,//设置音量
  SetLoop: 6,//设置loop
  SetAutoPlay: 7,//设置AutoPlay
  SetStartTime: 8,//设置StartTime
}

//小游戏音频
let XGameUnityAudio = {
  _logMode: false,
  _bgms: new Map(),
  _preloadQueue: new Map(),
  _downloadTasks: new Map(),
  _clips: new Map(),
  _fileMd5Map: new Map(),
  _PRELOAD_STATE_TYPE: {
    HasCacheFile: 0,   //有缓存文件
    Downloading: 1,   //下载中  
    DownloadFail: 2,   //下载失败
  },
  //队列加载结果
  _PRELOAD_QUEUE_RESULT_TYPE: {
    Loading: 0,//加载中
    LoadingSuccess: 1,//加载成功
    LoadingFail: 2,//加载失败
  },
  _onPreDownloadCompleteCallback: null,
  init: function (preDownloadCompleteCallBack) {
    this._onPreDownloadCompleteCallback = preDownloadCompleteCallBack;
  },
  //新的音频实例
  newClipInstance: function (param_url, param_isLong) {
    //默认为长音频
    if (param_isLong == undefined) {
      param_isLong = true;
    }
    var clip = {
      url: param_url,
      isLong: param_isLong,
      startTime: 0,
      autoplay: false,
      loop: false,
      volume: 1,
      context: null,
      actions: [],
      ready: false,
      triggerPaused: false,
      //设置音频
      setVolume: function (param_volume) {
        if (this.context) {
          this.context.volume = param_volume;
        } else {
          this.volume = param_volume;
          this.actions.push({ type: XGUA_CLIP_ACTION_TYPE.SetVolume, value: param_volume });
        }
      },
      //设置自动播放
      setAutoPlay: function (param_autoPlay) {
        if (this.context) {
          this.context.autoplay = param_autoPlay;
        } else {
          this.autoplay = param_autoPlay;
          this.actions.push({ type: XGUA_CLIP_ACTION_TYPE.SetAutoPlay, value: param_autoPlay });
        }
      },
      //循环
      setLoop: function (param_loop) {
        if (this.context) {
          this.context.loop = param_loop;
        } else {
          this.loop = param_loop;
          this.actions.push({ type: XGUA_CLIP_ACTION_TYPE.SetLoop, value: param_loop });
        }
      },
      setStartTime: function (param_startTime) {
        if (this.context) {
          this.context.startTime = param_startTime;
        } else {
          this.startTime = param_startTime;
          this.actions.push({ type: XGUA_CLIP_ACTION_TYPE.SetStartTime, value: param_startTime });
        }
      },
      //音频长度
      getClipLength: function () {
        if (this.context && this.ready) {
          return this.context.duration;
        }
        return -1;
      },
      //当前播放位置
      getCurrentTime: function () {
        if (this.context) {
          return this.context.currentTime;
        }
        return 0;
      },
      //音量大小
      getVolume: function () {
        if (this.context) {
          return this.context.volume;
        }
        return this.volume;
      },
      //播放
      play: function () {
        if (this.context && this.ready) {
          this.context.play();
          if (!this.triggerPaused) {
            this.context.seek(this.startTime);
          }
          this.triggerPaused = false;
        } else {
          this.actions.push({ type: XGUA_CLIP_ACTION_TYPE.Play, value: null });
        }
      },
      //停止播放
      stop: function () {
        if (this.context && this.ready) {
          this.context.stop();
          this.triggerPaused = false;
        } else {
          this.actions.push({ type: XGUA_CLIP_ACTION_TYPE.Stop, value: null });
        }
      },
      //暂停
      pause: function () {
        if (this.context && this.ready) {
          this.context.pause();
          this.triggerPaused = true;
        } else {
          this.actions.push({ type: XGUA_CLIP_ACTION_TYPE.Pause, value: null });
        }
      },
      //跳转到指定位置
      seek: function (param_position) {
        if (this.context && this.ready) {
          this.context.seek(param_position);
        } else {
          this.actions.push({ type: XGUA_CLIP_ACTION_TYPE.Seek, value: param_position });
        }
      }
    };
    return clip;
  },
  //预下载音频资源
  preDownloadAudioFiles: function (param_urls, param_callBackId) {
    this.xgua_log("preDownloadAudioFiles", param_urls, param_callBackId);
    //callbackId就是队列ID
    var queueId = param_callBackId;
    var data = new Map();
    //创建预加载状态
    param_urls.forEach(element => {
      var cachePath = this.getAudioCachePath(element);
      if (this.fileExists(cachePath)) {
        data.set(element, this._PRELOAD_STATE_TYPE.HasCacheFile);
      } else {
        data.set(element, this._PRELOAD_STATE_TYPE.Downloading);
      }
    });

    //加入队列
    this._preloadQueue.set(queueId, data);
    //判断哪些bgm需要下载
    for (let [url, state] of data) {
      switch (state) {
        case this._PRELOAD_STATE_TYPE.Downloading:
          this.tryCreateDownloadTask(url);
          break;
      }
    }
    //检查队列
    this.checkPreloadQueueResult(queueId);
  },

  //准备音频实例
  prepareAudioInstance: function (param_options) {
    this.xgua_log("perpareAudioInstance", param_options);
    param_options.forEach(element => {
      const url = element.url;
      const isLong = element.isLong;
      this.createOrGetAudioClip(url, isLong);
    });
  },

  //加载一个音频资源
  loadOneAudioSource: function (param_url) {
    var cachePath = this.getAudioCachePath(param_url);
    if (this.fileExists(cachePath)) {
      //有缓存，触发下载成功事件
      this.onAudioSourceDownloadSuccess(param_url);
    } else {
      //下载
      this.tryCreateDownloadTask(param_url);
    }
  },

  //获取音效大小
  getVolume: function (param_url) {
    return this.createOrGetAudioClip(param_url).getVolume();
  },

  //获取当前播放进度
  getCurrentTime: function (param_url) {
    return this.createOrGetAudioClip(param_url).getCurrentTime();
  },

  //获取音频长度，未完成加载时返回-1
  getClipLength: function (param_url) {
    return this.createOrGetAudioClip(param_url).getClipLength();
  },

  //设置音量
  setVolume: function (param_url, param_volume) {
    this.xgua_log("setVolume", param_url, param_volume);
    this.createOrGetAudioClip(param_url).setVolume(param_volume);
  },


  //是否自动播放
  setAutoPlay: function (param_url, param_autoPlay) {
    this.xgua_log("setAutoPlay", param_url, param_autoPlay);
    this.createOrGetAudioClip(param_url).setAutoPlay(param_autoPlay);
  },


  //设置loop
  setLoop: function (param_url, param_loop) {
    this.xgua_log("setLoop", param_url, param_loop);
    this.createOrGetAudioClip(param_url).setLoop(param_loop);
  },

  //设置开始时间
  setStartTime: function (param_url, param_startTime) {
    this.xgua_log("setStartTime", param_url, param_startTime);
    this.createOrGetAudioClip(param_url).setStartTime(param_startTime);
  },

  // 播放
  play: function (param_url) {
    this.xgua_log("play", param_url);
    this.createOrGetAudioClip(param_url).play();
  },

  //暂停
  pause: function (param_url) {
    this.xgua_log("pause", param_url);
    this.createOrGetAudioClip(param_url).pause();
  },

  //停止音频
  stop: function (param_url) {
    this.xgua_log("stop", param_url);
    this.createOrGetAudioClip(param_url).stop();
  },

  //跳转到指定位置单位秒
  seek: function (param_url, param_position) {
    this.xgua_log("seek", param_url, param_position);
    this.createOrGetAudioClip(param_url).seek(param_position);
  },

  //销毁音频
  destroy: function (param_url) {
    this.xgua_log("destroy", param_url);
    if (this._bgms.has(param_url)) {
      this._bgms.set(param_url, false);
    }

    if (this._clips.has(param_url)) {
      var clip = this._clips.get(param_url);
      this.xgua_log("clip.context 1", clip.context);
      clip.stop();
      if (clip.context) {
        clip.context.destroy();
      }
      this.xgua_log("clip.context 2", clip.context);
      // this._clips.delete(param_url);
    }
  },


  //快捷播放bgm
  playBgm: function (param_url) {
    this.xgua_log("playBgm", param_url);
    if (!this._clips.has(param_url)) {
      //创建片段
      this.createOrGetAudioClip(param_url).setLoop(true);
    }
    if (!this._bgms.has(param_url)) {
      this._bgms.set(param_url, false);
    }
    //播放bgm
    for (let [url, state] of this._bgms) {
      if (url == param_url) {
        if (state == false) {
          //当前不在播放状态,播放音频        
          this.seek(url, 0);
          this.play(url);
          this._bgms.set(param_url, true);
        }
      } else {
        //停止音频
        this.stop(url);
        this._bgms.set(param_url, false);
      }
    }
  },

  //停止bgm
  stopBgm: function () {
    //停止所有bgm
    for (let [url, state] of this._bgms) {
      if (this._clips.has(url)) {
        //存在片段
        this.stop(url);
      }
      this._bgms.set(url, false);
    }
  },

  //创建音频片段
  createOrGetAudioClip: function (param_url, param_isLong) {
    if (param_isLong == undefined) {
      param_isLong = true;
    }
    if (this._clips.has(param_url)) {
      return this._clips.get(param_url);
    }
    var clip = this.newClipInstance(param_url, param_isLong);
    this.xgua_log("createOrGetAudioClip", clip);
    //加入字典
    this._clips.set(param_url, clip);
    //加载音频
    this.loadOneAudioSource(param_url);

    return clip;
  },

  //处理clip行为队列
  handleClipActions: function (param_url) {
    this.xgua_log("onAudioContextReady", param_url);
    if (this._clips.has(param_url)) {
      var clip = this._clips.get(param_url);
      if (clip.context != null) {
        var context = clip.context;
        clip.actions.forEach(element => {
          switch (element.type) {
            case XGUA_CLIP_ACTION_TYPE.Pause://停止播放
              context.pause();
              break;
            case XGUA_CLIP_ACTION_TYPE.Play://播放
              context.play();
              break;
            case XGUA_CLIP_ACTION_TYPE.Stop://停止
              context.stop();
              break;
            case XGUA_CLIP_ACTION_TYPE.Seek://跳转到指定位置
              context.seek(element.value);
              break;
            case XGUA_CLIP_ACTION_TYPE.SetAutoPlay://设置自动播放   
              context.autoplay = element.value;
              break;
            case XGUA_CLIP_ACTION_TYPE.SetLoop://是否循环            
              context.loop = element.value;
              break;
            case XGUA_CLIP_ACTION_TYPE.SetStartTime://设置开始时间
              context.startTime = element.value;
              break;
            case XGUA_CLIP_ACTION_TYPE.SetVolume://设置音量
              context.volume = element.value;
              break;
          }
        });
        //清空堆积的行为
        clip.actions = [];
      }
    }
  },

  //音效context ready
  onAudioContextReady: function (param_url, param_context) {
    this.xgua_log("onAudioContextReady", param_url, param_context);
    if (this._clips.has(param_url)) {
      var clip = this._clips.get(param_url);
      //设置
      clip.ready = true;
      //处理堆积的音频行为
      this.handleClipActions(param_url);
    } else {
      //不存在,销毁音频
      param_context.destroy();
    }
  },

  //音频资源下载成功
  onAudioSourceDownloadSuccess: function (param_url) {
    // this.xgua_log("onAudioSourceDownloadSuccess", param_url);
    //尝试创建AudioContext
    if (this._clips.has(param_url)) {
      const clip = this._clips.get(param_url);

      this.xgua_log("createInnerAudioContext------1", clip);
      if (clip.context == null) {
        // this.xgua_log("createInnerAudioContext------2", clip.context);
        //进行创建
        var context = qg.createInnerAudioContext({ useWebAudioImplement: !clip.isLong });
        context.src = this.getAudioCachePath(clip.url);
        this.xgua_log("createInnerAudioContext------3", this.getAudioCachePath(clip.url));
        var readyFunc = () => {
          // XGameUnityAudio.xgua_log("readyFunc", param_url, context);
          XGameUnityAudio.onAudioContextReady(param_url, context);
          context.offCanplay(readyFunc);
        }
        context.onCanplay(readyFunc);
        clip.context = context;
        this.xgua_log("createInnerAudioContext------4", clip);
      }
    }
  },

  //尝试创建加载任务
  tryCreateDownloadTask: function (param_url) {
    // this.xgua_log("tryCreateDownloadTask", param_url, "start");
    if (this._downloadTasks.has(param_url)) {
      //存在下载任务,跳过
      // this.xgua_log("tryCreateDownloadTask", param_url, "return");
      return;
    }
    this._downloadTasks.set(param_url, false);
    var cachePath = this.getAudioCachePath(param_url);
    // this.xgua_log("downloadFile:", param_url, cachePath);

    qg.downloadFile({
      url: param_url,
      success (res) {
        // XGameUnityAudio.xuaw_log("downloadFile succecss:", res);
        let fs = qg.getFileSystemManager();
        if (res.statusCode == 200) {
          fs.saveFile({
            tempFilePath: res.tempFilePath,
            filePath: cachePath,
            success: () => {
              XGameUnityAudio.xgua_log("保存到：", cachePath)
              XGameUnityAudio.xgua_onDownloadTaskComplete(param_url, true);
            }, fail: () => {
              XGameUnityAudio.xgua_onDownloadTaskComplete(param_url, false, "写入文件失败");
            }
          });
        } else {
          XGameUnityAudio.xgua_onDownloadTaskComplete(param_url, false, `statusCode:${res.statusCode}`);
        }
      },
      fail (err) {
        //下载失败
        XGameUnityAudio.xgua_onDownloadTaskComplete(param_url, false, err);
      }
    });

  },

  //下载回调
  xgua_onDownloadTaskComplete: function (param_url, param_success, param_error) {
    // this.xgua_log("xuaw_onDownloadTaskComplete", param_url, param_success);
    if (!param_success) {
      //失败
      this.xgua_log("xgua_onDownloadTaskComplete error", { url: param_url, result: param_success, error: param_error });
    }
    if (param_success) {
      //触发下载成功，尝试创建音频context
      this.onAudioSourceDownloadSuccess(param_url);
    }
    this.xgua_onAudioClipDownloadResult(param_url, param_success);
    this._downloadTasks.delete(param_url);
  },


  xgua_onAudioClipDownloadResult: function (param_url, param_success) {
    // this.xgua_log("xuaw_onAudioClipDownloadResult", param_url, param_success);
    var interest = [];
    //更新预加载歌曲的状态
    for (let [queueId, map] of this._preloadQueue) {
      if (map.has(param_url)) {
        //如果是下载中的状态
        if (map.get(param_url) == this._PRELOAD_STATE_TYPE.Downloading) {
          //更新预加载状态值
          map.set(param_url, param_success == true ? this._PRELOAD_STATE_TYPE.HasCacheFile : this._PRELOAD_STATE_TYPE.DownloadFail);
          //加入兴趣列表
          interest.push(queueId);
        }
      }
    }

    //检查有变动的队列
    interest.forEach(element => {
      //检查结果
      this.checkPreloadQueueResult(element);
    });

  },

  //获取预加载队列状态
  getPreloadQueueResult: function (param_queue_id) {
    var map = this._preloadQueue.get(param_queue_id);
    var doneCount = 0;
    //k:url v:预加载状态
    for (let [url, state] of map) {
      // this.xuaw_log(url, state);
      switch (state) {
        case this._PRELOAD_STATE_TYPE.HasCacheFile://有缓存文件，完成数+1
          doneCount += 1;
          break;
        case this._PRELOAD_STATE_TYPE.Downloading://下载中        
          break;
        case this._PRELOAD_STATE_TYPE.DownloadFail://有一个失败,直接返回失败      
          return this._PRELOAD_QUEUE_RESULT_TYPE.LoadingFail;
      }
    }
    // this.xuaw_log("donecount:", doneCount, "total:", map.size);
    if (doneCount >= map.size) {
      //加载完毕
      return this._PRELOAD_QUEUE_RESULT_TYPE.LoadingSuccess;
    }
    //在加载中
    return this._PRELOAD_QUEUE_RESULT_TYPE.Loading;
  },

  //检查预加载队列结果
  checkPreloadQueueResult: function (param_ququeId) {
    // this.xuaw_log("checkPreloadQueueResult", param_ququeId);
    if (this._preloadQueue.has(param_ququeId)) {
      //计算结果
      var preloadResult = this.getPreloadQueueResult(param_ququeId);
      // this.xuaw_log("preloadResult：", preloadResult);
      switch (preloadResult) {
        case this._PRELOAD_QUEUE_RESULT_TYPE.LoadingFail://加载失败
          this.onPreloadQueueComplete(param_ququeId, false);
          break;
        case this._PRELOAD_QUEUE_RESULT_TYPE.LoadingSuccess://全加载成功
          this.onPreloadQueueComplete(param_ququeId, true);
          break;
      }
    }
  },

  //预加载完成回调
  onPreloadQueueComplete: function (param_queue_id, param_success) {
    // this.xgua_log("onPreloadQueueComplete", param_queue_id, param_success);
    if (this._preloadQueue.has(param_queue_id)) {
      //触发回调
      if (this._onPreDownloadCompleteCallback != null) {
        this._onPreDownloadCompleteCallback(param_queue_id, param_success);
      }
      this._preloadQueue.delete(param_queue_id);
    }
  },

  //输出日志
  xgua_log: function (...args) {
    if (this._logMode) {
      console.log("##XGUA: ", ...args);
    }
  },
  //计算音频缓存地址
  getAudioCachePath: function (url) {
    if (this._fileMd5Map.has(url)) {
      return this._fileMd5Map.get(url);
    }
    var hash = `_xgua_${url.length}_${md5(url)}`;
    var result = `${qg.env.USER_DATA_PATH}/${hash}`;
    this._fileMd5Map.set(url, result);
    return result;
  },
  //替换字符串
  xgua_stringReplaceAll: function (str, s1, s2) {
    return str.replace(new RegExp(s1, "gm"), s2);
  },
  //是否有文件
  fileExists: function (params) {
    var fs = qg.getFileSystemManager();
    try {
      fs.accessSync(params);
      return true;
    } catch (e) {
      return false;
    }
    return false;
  },
  //通知unity
  noticeUnity: function (method, msg) {
    window.unityInstance.SendMessage("_XGAME_H5_AUDIO", method, msg);
  },
  //获取缓存大小
  getCacheSize: function () {
    this.xgua_log("getCacheSize");
    var size = 0;
    let fs = qg.getFileSystemManager()
    var stats = fs.statSync(qg.env.USER_DATA_PATH, true);
    var prefix = `_xgua_`;
    this.xgua_log("stats", stats);
    for (const element of stats) {
      this.xgua_log("element", element.path, "  size:", element.stats.size);
      if (element.path.startsWith(prefix)) {
        size += element.stats.size;
      }
    }
    return size;
  }, //释放缓存音频
  clearCacheAudioFile: function (ignores) {
    this.xgua_log("clearCacheAudioFile", ignores);
    var map = new Map();
    ignores.forEach(element => {
      var p = this.getAudioCachePath(element);
      map.set(p, 1);
    });

    //遍历获取所有文件
    var fs = qg.getFileSystemManager()
    var stats = fs.statSync(qg.env.USER_DATA_PATH, true);
    var deleteList = [];
    var prefix = `_xgua_`;
    stats.forEach(element => {
      var fPath = element.path;
      if (fPath.startsWith(prefix)) {
        this.xgua_log("path", fPath);
        var fullPath = `${qg.env.USER_DATA_PATH}/${fPath}`;
        this.xgua_log("fullPath:", fullPath);
        if (!map.has(fullPath)) {
          this.xgua_log("加入删除列表:", fullPath);
          deleteList.push(fullPath);
        }
      }
    });
    //删除文件
    for (const element of deleteList) {
      this.xgua_log("删除文件:", element);
      fs.unlinkSync(element);
    }
  }
}

//方法ID
let METHOD_ID = {
  PRE_DOWNLOAD_AUDIO_FILES: "PRE_DOWNLOAD_AUDIO_FILES",
  PREPARE_AUDIO_INSTANCE: "PREPARE_AUDIO_INSTANCE",
  SET_VOLUME: "SET_VOLUME",
  SET_LOOP: "SET_LOOP",
  PLAY: "PLAY",
  STOP: "STOP",
  PAUSE: "PAUSE",
  SEEK: "SEEK",
  DESTROY: "DESTROY",
  PLAY_BGM: "PLAY_BGM",
  STOP_BGM: "STOP_BGM",
  INIT: "INIT",
  GET_CACHE_SIZE: "GET_CACHE_SIZE",
  CLEAR_CACHE_AUDIO: "CLEAR_CACHE_AUDIO",
}

//字符串
let STRINGS = {
  urls: "urls",
  call_back_id: "call_back_id",
  success: "success",
  error: "error",
  options: "options",
  url: "url",
  position: "position",
  loop: "loop",
  volume: "volume",
  ignores: "ignores",
  size: "size",
}

window["XGameUnityAudio"] = XGameUnityAudio;

//获取音量
window["XGameH5Audio_GetVolume"] = (url) => {
  XGameUnityAudio.xgua_log("XGameH5Audio_GetVolume", url);
  return XGameUnityAudio.getVolume(url)
};

//获取当前播放进度
window["XGameH5Audio_GetCurrentTime"] = (url) => {
  XGameUnityAudio.xgua_log("XGameH5Audio_GetCurrentTime", url);
  return XGameUnityAudio.getCurrentTime(url)
};

//获取音频长度
window["XGameH5Audio_GetClipLength"] = (url) => {
  XGameUnityAudio.xgua_log("XGameH5Audio_GetClipLength", url);
  return XGameUnityAudio.getClipLength(url)
};

//调用方法
window["XGameH5Audio_CallMethod"] = (method, json) => {
  XGameUnityAudio.xgua_log("XGameH5Audio_CallMethod", method, json);
  switch (method) {
    case METHOD_ID.INIT:
      {
        //初始化，绑定回调
        XGameUnityAudio.init((param_queue_id, param_success) => {
          var json = JSON.stringify({ call_back_id: param_queue_id, success: param_success });
          XGameUnityAudio.noticeUnity("OnPreDownloadAudioFilesCallBack", json);
        });
      }
      break
    case METHOD_ID.PRE_DOWNLOAD_AUDIO_FILES:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.preDownloadAudioFiles(data[STRINGS.urls], data[STRINGS.call_back_id]);
      }
      break;
    case METHOD_ID.PREPARE_AUDIO_INSTANCE:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.prepareAudioInstance(data[STRINGS.options]);
      }
      break;
    case METHOD_ID.SET_VOLUME:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.setVolume(data[STRINGS.url], data[STRINGS.volume]);
      }
      break;
    case METHOD_ID.SET_LOOP:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.setLoop(data[STRINGS.url], data[STRINGS.loop]);
      }
      break;
    case METHOD_ID.PLAY:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.play(data[STRINGS.url]);
      }
      break;
    case METHOD_ID.STOP:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.stop(data[STRINGS.url]);
      }
      break;
    case METHOD_ID.PAUSE:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.pause(data[STRINGS.url]);
      }
      break;
    case METHOD_ID.SEEK:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.seek(data[STRINGS.url], data[STRINGS.position]);
      }
      break;
    case METHOD_ID.DESTROY:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.destroy(data[STRINGS.url]);
      }
      break;
    case METHOD_ID.PLAY_BGM:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.playBgm(data[STRINGS.url]);
      }
      break;
    case METHOD_ID.STOP_BGM:
      {
        XGameUnityAudio.stopBgm();
      }
      break;
    case METHOD_ID.CLEAR_CACHE_AUDIO:
      {
        var data = JSON.parse(json);
        XGameUnityAudio.clearCacheAudioFile(data[STRINGS.ignores]);
      }
      break;
  }
};

//获取缓存大小
window["XGameH5Audio_GetCacheSize"] = () => {
  XGameUnityAudio.xgua_log("XGameH5Audio_GetCacheSize");
  return XGameUnityAudio.getCacheSize();
};


console.log("xgame-unity-audio-oppo load done");
