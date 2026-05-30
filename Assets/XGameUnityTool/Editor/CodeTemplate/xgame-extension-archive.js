var g = globalThis || window || global || GameGlobal;
var XGameGlobal = g["XGameGlobal"] ? g["XGameGlobal"] : {};
g["XGameGlobal"] = XGameGlobal;
var xgame_defined = XGameGlobal["definded"] ? XGameGlobal["definded"] : {};
XGameGlobal["definded"] = xgame_defined;
XGameGlobal["extensions"] = XGameGlobal["extensions"] ? XGameGlobal["extensions"] : [];
var ctor_list = [];
xgame_defined.require = (name) => {
    return xgame_defined[name];
};
xgame_defined.exports = {};
var define = (name, requires, ctor) => {
    let exports = {};
    xgame_defined.exports = exports;
    xgame_defined[name] = exports;
    ctor_list.push([ctor, exports, requires]);
};
var amd_init = () => {
    for (let i in ctor_list) {
        let c = ctor_list[i];
        let ctor = c[0];
        xgame_defined.exports = c[1];
        let requires = c[2];
        let ps = [];
        for (let j in requires) {
            ps.push(xgame_defined[requires[j]]);
        }
        ctor(...ps);
    }
};
XGameGlobal["extensions"].push(amd_init);
define("extension/archive/src/Archive", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class ArchiveValue {
    }
    let toUint8Arr = (str) => {
        const buffer = [];
        for (let i of str) {
            const _code = i.charCodeAt(0);
            if (_code < 0x80) {
                buffer.push(_code);
            }
            else if (_code < 0x800) {
                buffer.push(0xc0 + (_code >> 6));
                buffer.push(0x80 + (_code & 0x3f));
            }
            else if (_code < 0x10000) {
                buffer.push(0xe0 + (_code >> 12));
                buffer.push(0x80 + (_code >> 6 & 0x3f));
                buffer.push(0x80 + (_code & 0x3f));
            }
        }
        return Uint8Array.from(buffer);
    };
    let toUint8Arr2 = (str) => {
        const arr = [...str];
        const buffer = new Uint8Array(arr.length * 4);
        let index = 0;
        for (let i = 0; i < arr.length; i++) {
            const codePoint = arr[i].codePointAt(0);
            // 四字节字符
            if (codePoint >= 0x10000) {
                buffer[index++] = (codePoint >> 18) & 0x7 | 0xf0;
                buffer[index++] = (codePoint >> 12) & 0x3f | 0x80;
                buffer[index++] = (codePoint >> 6) & 0x3f | 0x80;
                buffer[index++] = codePoint & 0x3f | 0x80;
            }
            else if (codePoint >= 0x800) {
                // 三字节字符
                buffer[index++] = (codePoint >> 12) & 0xf | 0xe0;
                buffer[index++] = (codePoint >> 6) & 0x3f | 0x80;
                buffer[index++] = codePoint & 0x3f | 0x80;
            }
            else if (codePoint >= 0x80) {
                // 两字节字符
                buffer[index++] = (codePoint >> 6) & 0x1f | 0xc0;
                buffer[index++] = codePoint & 0x3f | 0x80;
            }
            else {
                // 单字节字符
                buffer[index++] = codePoint;
            }
        }
        return buffer.slice(0, index);
    };
    let cala_magic = (signs, v) => {
        for (let i = 0; i < v.length; i++) {
            let c = v[i];
            for (let j = 0; j < signs.length; j++) {
                let s = signs[j];
                s = s ^ (c + j + 1);
                signs[j] = s & 0xFF;
            }
        }
    };
    let gen_magic_sign = (user_id, req) => {
        let content = toUint8Arr2(req.content);
        let nonce = toUint8Arr2(req.nonce);
        let signs = [
            user_id & 0xFF,
            req.version & 0xFF,
            content.length & 0xFF,
            nonce.length & 0xFF,
        ];
        cala_magic(signs, content);
        cala_magic(signs, nonce);
        return signs.join("");
    };
    class Archive {
        constructor() {
            // private userId: number;
            // private userToken: string;
            this.syncCache = {};
            this.syncCacheList = [];
            this.isSyncing = false;
        }
        onInit(sdk) {
            this.user = sdk.User();
            this.plat = sdk.Platform();
        }
        onInitAfter() {
            this.plat.setTimeout(() => {
                this.checkSyncCache();
            }, 5000);
            this.plat.setInterval(() => {
                this.checkSyncCache();
            }, 20000);
        }
        onLogin() {
            // this.userId = this.user.getUserId();
            // this.userToken = this.user.getUserToken();
        }
        // private api(url: string, data, cb:(ret:boolean, res)=>void){
        //     if(!this.user.getLogined()){
        //         cb(false, { ret: "userUnLogin"});
        //         return;
        //     }
        //     let userId = this.user.getUserId();
        //     let userToken = this.user.getUserToken();
        //     let req = new AjaxOpt();
        //     req.url = this.serverUrl + url;
        //     req.method = "POST";
        //     req.dataType = "json";
        //     let req_data = {
        //         user_id: userId,
        //         user_token: userToken,
        //         data: data,
        //     }
        //     req.data = JSON.stringify(req_data);
        //     req.error = (msg)=>{
        //         cb(false, {ret: "ajaxError:" + msg});
        //     }
        //     req.success = (res)=>{
        //         if(res.status == 200){
        //             cb(res.data.ret == "ok", res.data);
        //         }else{
        //             cb(false, {ret: "httpStatusError:" + res.status});
        //         }
        //     }
        //     this.plat.ajax(req);
        // }
        apiArchive(url, data, cb) {
            url = "/xgp_archive" + url;
            this.user.api(url, data, cb, true);
        }
        rebackCache(list) {
            for (let i in list) {
                let d = list[i];
                if (!this.syncCache[d.key]) {
                    this.syncCache[d.key] = d;
                    this.syncCacheList.push(d);
                }
            }
            console.log("rebackCache:", list, this.syncCacheList);
        }
        checkSyncCache() {
            if (this.syncCacheList.length == 0 || this.isSyncing) {
                return;
            }
            this.isSyncing = true;
            let list = this.syncCacheList;
            this.syncCacheList = [];
            this.syncCache = {};
            this.beginSyncCb && this.beginSyncCb();
            let workId = "" + (new Date().getTime()) + "" + Math.random();
            let index = 0;
            let cb = (ret) => {
                if (!ret) {
                    this.isSyncing = false;
                    this.rebackCache(list);
                    return;
                }
                if (index >= list.length) {
                    this.isSyncing = false;
                    this.finishSyncCb && this.finishSyncCb();
                    return;
                }
                let work = (index == (list.length - 1)) ? "end" : "do";
                this.syncData(list[index], work, workId, cb);
                index++;
            };
            cb(true);
        }
        syncData(d, work, workId, cb) {
            if (!this.user.getLogined()) {
                cb(false);
                return;
            }
            let nonce = "" + (new Date().getTime()) + "|" + (Math.random() * 99999999);
            let req = {
                key: d.key,
                content: d.content,
                version: d.version,
                work: work,
                work_id: workId,
                nonce: nonce,
                sign: "",
            };
            let userId = this.user.getUserId();
            req.sign = gen_magic_sign(userId, req);
            this.apiArchive("/user/archive/save_64k_work", req, (ret, res) => {
                console.log("syncData:", req, res);
                cb(ret == "ok");
            });
        }
        reqAllKeys(cb) {
            this.apiArchive("/user/archive/req_allkey", {}, (ret, res) => {
                if (!ret) {
                    cb(res.ret, null);
                    return;
                }
                let data = res.data;
                let keys = data.keys;
                let count = data.count;
                let av = [];
                if (count > 0) {
                    for (let i in keys) {
                        av.push(keys[i]);
                    }
                }
                cb("ok", av);
            });
        }
        save64k(key, version, content) {
            if (!key || !version || !content) {
                throw new Error("params can not be undefine");
            }
            let cache = {
                content: content,
                dtype: "64k",
                version: version,
                key: key,
            };
            if (!this.syncCache[key]) {
                this.syncCacheList.push(cache);
            }
            this.syncCache[key] = cache;
        }
        req64k(key, cb) {
            if (key.length <= 0 || key.length > 128) {
                throw new Error("invalid key");
            }
            let req = {
                key: key,
            };
            this.apiArchive("/user/archive/req_64k", req, (ret, res) => {
                if (ret != "ok") {
                    cb(ret, null);
                    return;
                }
                cb("ok", res.data);
            });
        }
        setOnSync(begin, finish) {
            this.beginSyncCb = begin;
            this.finishSyncCb = finish;
        }
        forceSync() {
            this.checkSyncCache();
        }
        testGenSign(user_id, req) {
            return gen_magic_sign(user_id, req);
        }
    }
    exports.default = Archive;
});
define("extension/archive/src/config", ["require", "exports", "extension/archive/src/Archive"], function (require, exports, Archive_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    {
        XGameGlobal["extension/archive"] = Archive_1.default;
    }
});
