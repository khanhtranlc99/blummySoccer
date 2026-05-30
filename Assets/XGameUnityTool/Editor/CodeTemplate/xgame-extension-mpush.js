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
define("extension/mpush/src/Mpush", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ReqItemsRsp = void 0;
    class ReqItemsRsp {
        constructor() {
            this.contents = [];
        }
    }
    exports.ReqItemsRsp = ReqItemsRsp;
    class Mpush {
        onInit(sdk) {
            this.user = sdk.User();
            this.plat = sdk.Platform();
        }
        onInitAfter() {
        }
        apiMpush(url, data, cb) {
            url = "/xgp_mpush" + url;
            this.user.api(url, data, cb, true);
        }
        onLogin() {
        }
        reqItems(planId, count, cb) {
            let req = {
                plan_id: planId,
                count: count,
            };
            this.apiMpush("/user/mpush/list_item", req, (ret, res) => {
                if (ret != "ok") {
                    cb(ret, null);
                    return;
                }
                let rsp = new ReqItemsRsp();
                let data = res.data;
                if (data.list_count > 0) {
                    rsp.contents = data.list;
                }
                cb("ok", rsp);
            });
        }
        clickItem(content, cb) {
            this.plat.openMpushContent(content, cb);
        }
    }
    exports.default = Mpush;
});
define("extension/mpush/src/config", ["require", "exports", "extension/mpush/src/Mpush"], function (require, exports, Mpush_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    {
        XGameGlobal["extension/mpush"] = Mpush_1.default;
    }
});
