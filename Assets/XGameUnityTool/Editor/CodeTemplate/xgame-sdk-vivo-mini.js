var g = globalThis || window || global || GameGlobal;
var XGameGlobal = g["XGameGlobal"] ? g["XGameGlobal"] : {};
g["XGameGlobal"] = XGameGlobal;
var xgame_defined = XGameGlobal["definded"] ? XGameGlobal["definded"] : {};
XGameGlobal["definded"] = xgame_defined;
XGameGlobal["extensions"] = XGameGlobal["extensions"] ? XGameGlobal["extensions"] : [];
xgame_defined.require = (name) => {
    return xgame_defined[name];
};
xgame_defined.exports = {};
var define = (name, requires, ctor) => {
    let exports = {};
    xgame_defined.exports = exports;
    xgame_defined[name] = exports;
    let ps = [];
    for (let i in requires) {
        ps.push(xgame_defined[requires[i]]);
    }
    ctor(...ps);
};
var extension_amd_init = () => {
    let extensions = XGameGlobal["extensions"];
    for (let i in extensions) {
        extensions[i]();
    }
};
define("sdk/src/Const", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.AdType = exports.PLUG = exports.PLATFORM = void 0;
    var PLATFORM;
    (function (PLATFORM) {
        PLATFORM[PLATFORM["inner"] = 1] = "inner";
        PLATFORM[PLATFORM["wechat_dev"] = 2] = "wechat_dev";
        PLATFORM[PLATFORM["wechat_mini"] = 3] = "wechat_mini";
        PLATFORM[PLATFORM["wechat_pay"] = 4] = "wechat_pay";
        PLATFORM[PLATFORM["bytedance_mini"] = 5] = "bytedance_mini";
        PLATFORM[PLATFORM["oppo_mini"] = 6] = "oppo_mini";
        PLATFORM[PLATFORM["vivo_mini"] = 7] = "vivo_mini";
        PLATFORM[PLATFORM["qq_mini"] = 9] = "qq_mini";
        PLATFORM[PLATFORM["huawei_mini"] = 10] = "huawei_mini";
    })(PLATFORM = exports.PLATFORM || (exports.PLATFORM = {}));
    var PLUG;
    (function (PLUG) {
        PLUG[PLUG["inner_platform"] = 1] = "inner_platform";
        PLUG[PLUG["wechat_mini"] = 2] = "wechat_mini";
        PLUG[PLUG["wechat_pay"] = 3] = "wechat_pay";
        PLUG[PLUG["mini_ad_control"] = 4] = "mini_ad_control";
        PLUG[PLUG["bytedance_mini"] = 5] = "bytedance_mini";
        PLUG[PLUG["oppo_mini"] = 6] = "oppo_mini";
        PLUG[PLUG["vivo_mini"] = 7] = "vivo_mini";
        PLUG[PLUG["qq_mini"] = 9] = "qq_mini";
        PLUG[PLUG["huawei_mini"] = 10] = "huawei_mini";
    })(PLUG = exports.PLUG || (exports.PLUG = {}));
    // 广告类型
    var AdType;
    (function (AdType) {
        AdType[AdType["unknow"] = 0] = "unknow";
        AdType[AdType["banner"] = 1] = "banner";
        AdType[AdType["inters"] = 2] = "inters";
        AdType[AdType["video"] = 3] = "video";
        AdType[AdType["custom"] = 4] = "custom";
        AdType[AdType["native"] = 5] = "native";
        AdType[AdType["splash"] = 6] = "splash";
    })(AdType = exports.AdType || (exports.AdType = {}));
});
// export enum InstType {
//     Banner,
//     NativeBanner,
//     NativeTmpBanner,
//     NativeBig,
//     NativeTmpBig,
//     Inters,
//     NativeInters,
//     NativeTmpInters,
//     IntersVideo,
//     Video,
//     Splash,
//     NativeTmp,
//     NavigateDrawer,
//     Custom
// }
// let formats = {
//     [InstType.Banner]: "Banner",
//     [InstType.NativeBanner]: "NativeBanner",
//     [InstType.NativeTmpBanner]: "NativeTmpBanner",
//     [InstType.NativeBig]: "NativeBig",
//     [InstType.NativeTmpBig]: "NativeTmpBig",
//     [InstType.Inters]: "Inters",
//     [InstType.NativeInters]: "NativeInters",
//     [InstType.NativeTmpInters]: "NativeTmpInters",
//     [InstType.IntersVideo]: "IntersVideo",
//     [InstType.Video]: "Video",
//     [InstType.Splash]: "Splash",
//     [InstType.NativeTmp]: "NativeTmp",
//     [InstType.NavigateDrawer]: "NavigateDrawer",
//     [InstType.Custom]: "Custom",
// }
// export class InstTypeName {
//     static getName(t): string{
//         return formats[t];
//     }
// }
define("sdk/src/Component", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.ShowResult = exports.AdEvent = void 0;
    class AdEvent {
        constructor() {
            this.revenue = 0.0;
        }
    }
    exports.AdEvent = AdEvent;
    class ShowResult {
    }
    exports.ShowResult = ShowResult;
});
define("sdk/src/IExtension", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
define("sdk/src/IModule", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class IModule {
        getType() { return 0; }
        onModule(sdk) { }
        onInit() { }
        ;
        onInitAfter() { }
        ;
        onLogin() { }
        ;
    }
    exports.default = IModule;
    IModule.MODULE_UNKNOW = 0;
    IModule.MODULE_PLATFORM = 1;
    IModule.MODULE_USER = 2;
    IModule.MODULE_AD = 3;
    IModule.MODULE_CHANNEL = 4;
});
define("sdk/src/ad/IAdPos", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
define("sdk/src/ad/IAdBannerPos", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
define("sdk/src/ad/IAdVideoPos", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
define("sdk/src/ad/IAdIntersPos", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
define("sdk/src/ad/IAdCustomPos", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
define("sdk/src/IAd", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    const TAG = "XGameH5-IAd";
});
define("sdk/src/Structs", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Bridge = exports.BridgeCallNativeStatic = exports.Plug = exports.AjaxOpt = exports.AjaxRes = exports.PlatformInfo = exports.OrderInfo = exports.ProductInfo = void 0;
    class ProductInfo {
        constructor() {
            this.id = "";
            this.name = "";
            this.desc = "";
            this.price = 0;
        }
    }
    exports.ProductInfo = ProductInfo;
    class OrderInfo {
    }
    exports.OrderInfo = OrderInfo;
    class PlatformInfo {
        constructor() {
            this.platform = "";
            this.win_width = 0;
            this.win_height = 0;
            this.brand = "";
            this.model = "";
            this.language = "";
            this.system_version = "";
            this.app_version = "";
            this.bundle_id = "";
            this.core_version = "";
            this.platform_version_name = "";
            this.platform_version_code = 0;
        }
    }
    exports.PlatformInfo = PlatformInfo;
    class AjaxRes {
    }
    exports.AjaxRes = AjaxRes;
    class AjaxOpt {
    }
    exports.AjaxOpt = AjaxOpt;
    class Plug {
    }
    exports.Plug = Plug;
    class BridgeCallNativeStatic {
    }
    exports.BridgeCallNativeStatic = BridgeCallNativeStatic;
    class Bridge {
    }
    exports.Bridge = Bridge;
});
define("sdk/src/IChannel", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
define("sdk/src/IPlatform", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
});
define("sdk/src/IUser", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    let K_USER_ID = "xgame.user_id";
    let K_USER_TOKEN = "xgame.user_token";
    let K_ACCOUNT_ID = "xgame.account_id";
});
define("sdk/src/XGame", ["require", "exports", "sdk/src/IModule"], function (require, exports, IModule_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class XGame {
        constructor() {
            this.modules = [];
            this.channelId = 0;
            this.extensions = [];
        }
        initModules(modules) {
            this.modules = modules;
            for (let i = 0; i < modules.length; i++) {
                let mod = modules[i];
                switch (mod.getType()) {
                    case IModule_1.default.MODULE_PLATFORM:
                        this.plat = mod;
                        break;
                    case IModule_1.default.MODULE_USER:
                        this.user = mod;
                        break;
                    case IModule_1.default.MODULE_AD:
                        this.ad = mod;
                        break;
                    case IModule_1.default.MODULE_CHANNEL:
                        this.channel = mod;
                        break;
                }
            }
            for (let i = 0; i < modules.length; i++) {
                modules[i].onModule(this);
            }
        }
        Platform() {
            return this.plat;
        }
        User() {
            return this.user;
        }
        Ad() {
            return this.ad;
        }
        Channel() {
            return this.channel;
        }
        onLogin() {
            for (let i = 0; i < this.modules.length; i++) {
                this.modules[i].onLogin();
            }
            for (let i in this.extensions) {
                this.extensions[i].onLogin();
            }
        }
        getChannelId() { return this.channelId; }
        init(channelId, success) {
            console.log("init:", channelId);
            this.channelId = channelId;
            for (let i in this.modules) {
                this.modules[i].onInit();
            }
            for (let i in this.modules) {
                this.modules[i].onInitAfter();
            }
            for (let i in this.extensions) {
                this.extensions[i].onInit(this);
            }
            for (let i in this.extensions) {
                this.extensions[i].onInitAfter();
            }
            this.Platform().onInitEnd(success);
        }
        regExtension(ext) {
            this.extensions.push(ext);
        }
    }
    exports.default = XGame;
});
define("h5core/src/H5AdInst", ["require", "exports", "sdk/src/Component"], function (require, exports, Component_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.LoadState = void 0;
    // 广告加载状态
    var LoadState;
    (function (LoadState) {
        LoadState[LoadState["UNLOAD"] = 0] = "UNLOAD";
        LoadState[LoadState["LOADING"] = 1] = "LOADING";
        LoadState[LoadState["LOADED"] = 2] = "LOADED";
    })(LoadState = exports.LoadState || (exports.LoadState = {}));
    class H5AdInst {
        constructor(sdk, type, instType) {
            this.TAG = "XGameH5-AdInst";
            // 广告id数组当前索引
            this.idListIndex = 0;
            // 广告当前加载状态
            this.loadState = LoadState.UNLOAD;
            // 广告当前正在展示
            this.isShow = false;
            // 广告成功展示
            this.sucShow = false;
            // 广告加载成功即展示
            this.ready2show = false;
            // 广告加载错误次数
            this.errorLoadTimes = 0;
            // 广告最后一次展示的时间戳
            this.lastShowTime = 0;
            // 广告当前调用展示时候的时间戳
            this.recordShowTime = 0;
            // 广告当前调用隐藏-再次展示的间隔时间 在该时间内则复用之前隐藏的广告
            this.setting_reloadTimeSpace = 0;
            this.sdk = sdk;
            this.type = type;
            this.instType = instType;
        }
        init(idList) {
            this.shuffleIds(idList);
            this.idList = idList;
            this.doInit();
        }
        shuffleIds(ids) {
            if (ids.length <= 1) {
                return;
            }
            for (let i = 0; i < ids.length; i++) {
                let r = Math.floor((Math.random() * ids.length));
                let t = ids[i];
                ids[i] = ids[r];
                ids[r] = t;
            }
        }
        // setListener(listener: Listener): void {
        //     this.listener = listener;
        // }
        logD(msg) {
            console.log(this.TAG, "[" + this.instType + "] " + msg);
        }
        load() {
            if (this.loadState != LoadState.UNLOAD) {
                return;
            }
            this.loadState = LoadState.LOADING;
            if (this.idListIndex >= this.idList.length) {
                this.idListIndex = 0;
            }
            this.logD("load:" + this.idListIndex + " : " + this.idList[this.idListIndex]);
            this.doLoad(this.idList[this.idListIndex]);
            this.idListIndex++;
        }
        genNextAdId() {
            if (this.idListIndex >= this.idList.length) {
                this.idListIndex = 0;
            }
            this.logD("genNextAdId: " + this.idListIndex + ":" + this.idList[this.idListIndex]);
            let adId = this.idList[this.idListIndex];
            this.idListIndex++;
            return adId;
        }
        onLoad(suc, msg) {
            this.logD("onLoad:" + suc + ":" + (msg != null ? msg : ""));
            if (suc) {
                this.loadState = LoadState.LOADED;
                this.errorLoadTimes = 0;
                if (this.ready2show && !this.sucShow) {
                    this.doShow();
                }
            }
            else {
                this.loadState = LoadState.UNLOAD;
                this.errorLoadTimes++;
            }
        }
        show(result) {
            if (this.isShow || this.ready2show) {
                this.errorLoadTimes = 0;
                if (result) {
                    result.onError();
                }
                return;
            }
            if (this.sucShow) {
                if (result) {
                    result.onError();
                }
                return;
            }
            // let ret: boolean = this.listener.show(this);
            // if (!ret) {
            //     if (result) { result.onError(); }
            //     return;
            // }
            this.result = result;
            this.isShow = true;
            this.errorLoadTimes = 0;
            this.logD("show: " + this.loadState);
            if (this.loadState != LoadState.LOADED) {
                this.ready2show = true;
                this.errorLoadTimes = 0;
                return;
            }
            this.doShow();
        }
        onShow(suc, msg) {
            if (!this.isShow) {
                this.isShow = true;
                this.hide();
                return;
            }
            if (this.sucShow) {
                return;
            }
            this.logD("onShow: " + suc + " : " + (msg != null ? msg : ""));
            this.sucShow = suc;
            if (suc) {
                this.lastShowTime = new Date().getTime();
            }
            else {
                this.loadState = LoadState.UNLOAD;
                this.ready2show = false;
                if (this.isShow) {
                    this.hide();
                    this.load();
                    //this.show(this.result);
                }
                this.result && this.result.onError();
            }
            // this.listener.onShow(this, suc);
        }
        hide() {
            if (!this.isShow) {
                return;
            }
            this.doHide();
        }
        onHide() {
            if (!this.isShow) {
                return;
            }
            this.sucShow = false;
            this.logD("onHide");
            if (this.loadState != LoadState.LOADING && this.lastShowTime > 0) {
                this.recordShowTime += Math.abs(new Date().getTime() - this.lastShowTime);
                console.log("onHide:", this.recordShowTime, this.setting_reloadTimeSpace);
                if (this.recordShowTime > this.setting_reloadTimeSpace) {
                    this.recordShowTime = 0;
                    this.loadState = LoadState.UNLOAD;
                }
                this.lastShowTime = 0;
            }
            this.ready2show = false;
            this.isShow = false;
            // this.listener.onHide(this);
            if (this.result) {
                this.result.onHide();
            }
        }
        genAdEvent() {
            let event = new Component_1.AdEvent();
            event.type = this.type;
            event.instType = this.instType;
            // event.eventParams = this.eventParams;
            return event;
        }
        tick() {
            if (this.loadState != LoadState.UNLOAD) {
                this.doTick();
                return;
            }
            if (this.errorLoadTimes >= (this.idList.length * 2)) {
                this.doTick();
                return;
            }
            this.load();
        }
        doInit() {
        }
        doLoad(adId) {
        }
        doShow() {
        }
        doHide() {
            this.onHide();
        }
        doTick() {
        }
        getFlag() {
            this.errorLoadTimes = 0;
            return this.loadState == LoadState.LOADED;
        }
    }
    exports.default = H5AdInst;
});
define("h5core/src/H5AdInstMgr", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class H5AdInstMgr {
        constructor(sdk) {
            this.TAG = "XGPH5-AdInstMgr";
            this.inst_list = [];
            this.sdk = sdk;
            this.platform = sdk.Platform();
        }
        // private shuffleIds(ids: string[]): void {
        //     if (ids.length <= 1) {
        //         return;
        //     }
        //     for (let i = 0; i < ids.length; i++) {
        //         let r: number = Math.floor((Math.random() * ids.length));
        //         let t: string = ids[i];
        //         ids[i] = ids[r];
        //         ids[r] = t;
        //     }
        // }
        init() {
            this.tickCounter = this.platform.setInterval(() => {
                for (let i = 0; i < this.inst_list.length; i++) {
                    this.inst_list[i].tick();
                }
            }, 5000);
        }
        reg_inst(inst) {
            this.inst_list.push(inst);
        }
    }
    exports.default = H5AdInstMgr;
});
define("sdk/src/ad/AdStruct", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.AdMediaData = exports.AdPosData = void 0;
    class AdPosData {
    }
    exports.AdPosData = AdPosData;
    class AdMediaData {
    }
    exports.AdMediaData = AdMediaData;
});
define("h5core/src/H5AdPos", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class H5AdPos {
        constructor(plan, pos) {
            this.plan = plan;
            this.pos = pos;
            this.init();
        }
        init() {
        }
        getFlag(cb) {
            cb(false);
        }
        create() {
            return this;
        }
        show(result) {
        }
        hide() {
        }
    }
    exports.default = H5AdPos;
});
define("h5core/src/H5AdCustomPos", ["require", "exports", "h5core/src/H5AdPos"], function (require, exports, H5AdPos_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class H5AdCustomPos extends H5AdPos_1.default {
        constructor() {
            super(...arguments);
            this.positionX = 0;
            this.positionY = 0;
            this.anchorX = 0;
            this.anchorY = 0;
        }
        setPosition(x, y) {
            this.positionX = x;
            this.positionY = y;
        }
        setAnchor(x, y) {
            this.anchorX = x;
            this.anchorY = y;
        }
    }
    exports.default = H5AdCustomPos;
});
define("h5core/src/H5AdPlan", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    const TAG = "XGameH5-AdPlan";
    class H5AdPlan {
        constructor(sdk) {
            this.pos_map = {};
            this.media_map = {};
            this.inst_map = {};
            this.pos_ad_map = {};
            this.default_custom_pos_no = [];
            this.sdk = sdk;
        }
        parse_data(data) {
            let media_count = data.media_count;
            if (media_count > 0) {
                let media_list = data.media_list;
                for (let i in media_list) {
                    let v = media_list[i];
                    this.media_map[v.media_id] = v;
                }
            }
            let pos_count = data.pos_count;
            if (pos_count > 0) {
                let pos_list = data.pos_list;
                for (let i in pos_list) {
                    let v = pos_list[i];
                    this.pos_map[v.pos_no] = v;
                }
            }
            let params = data.params ? data.params : {};
            if (!params) {
                console.error(TAG, "ad plan params is null.");
            }
            this.default_banner_pos_no = params["default_banner_pos_no"];
            this.default_inters_pos_no = params["default_inters_pos_no"];
            this.default_video_pos_no = params["default_video_pos_no"];
            for (let i = 1; i <= 5; i++) {
                this.default_custom_pos_no.push(params["default_custom_pos_no_" + i]);
            }
        }
        get_inst(media_id) {
            let media = this.media_map[media_id];
            if (!media) {
                console.error(TAG, "no media.");
                return null;
            }
            let inst = this.inst_map[media_id];
            if (!inst) {
                inst = this.gen_inst(this.sdk, media);
            }
            if (inst) {
                this.inst_map[media_id] = inst;
            }
            return inst;
        }
        get_pos(pos_no) {
            let pos = this.pos_map[pos_no];
            if (!pos) {
                console.error(TAG, "no pos.");
                return null;
            }
            let pos_ad = this.pos_ad_map[pos_no];
            if (!pos_ad) {
                pos_ad = this.gen_pos(this.sdk, pos);
            }
            if (pos_ad) {
                this.pos_ad_map[pos_no] = pos_ad;
            }
            return pos_ad;
        }
        gen_inst(sdk, media) {
            return null;
        }
        gen_pos(sdk, pos) {
            return null;
        }
        get_default_banner() {
            if (!this.default_banner_pos_no) {
                console.error(TAG, "not default banner pos no.");
                return null;
            }
            return this.get_pos(this.default_banner_pos_no);
        }
        get_default_inters() {
            if (!this.default_inters_pos_no) {
                console.error(TAG, "not default inters pos no.");
                return null;
            }
            return this.get_pos(this.default_inters_pos_no);
        }
        get_default_video() {
            if (!this.default_video_pos_no) {
                console.error(TAG, "not default video pos no.");
                return null;
            }
            return this.get_pos(this.default_video_pos_no);
        }
        get_default_custom(index) {
            let pos_no = this.default_custom_pos_no[index - 1];
            if (!pos_no) {
                console.error(TAG, "not the pos_no. index:", index);
                return null;
            }
            return this.get_pos(pos_no);
        }
        check_pos_no(type, pos_no) {
            let pos = this.pos_map[pos_no];
            if (!pos) {
                return false;
            }
            return pos.ad_type == type;
        }
        getCustomId(index) {
            return "";
        }
    }
    exports.default = H5AdPlan;
});
define("platform/vivo-mini/src/ad/BannerInst", ["require", "exports", "h5core/src/H5AdInst", "sdk/src/Const"], function (require, exports, H5AdInst_1, Const_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class BannerInst extends H5AdInst_1.default {
        constructor(sdk, media) {
            super(sdk, Const_1.AdType.banner, "sysBanner");
            this.bannerAd = null;
        }
        doLoad(adId) {
            if (this.bannerAd) {
                this.bannerAd.destroy();
                this.bannerAd = null;
                // this.onLoad(true, null);
                // return;
            }
            this.bannerAd = qg.createBannerAd({
                posId: adId,
                style: {},
                adIntervals: 30,
            });
            let ad = this.bannerAd;
            let loaded = false;
            // 监听系统banner加载
            this.bannerAd.onLoad(() => {
                loaded = true;
                this.onLoad(true, null);
            });
            // 监听系统banner错误
            this.bannerAd.onError((err) => {
                loaded = true;
                this.onLoad(false, JSON.stringify(err));
            });
            // 监听系统banner隐藏
            this.bannerAd.onClose(() => {
                this.hide();
            });
            setTimeout(() => {
                if (loaded || (ad != this.bannerAd)) {
                    return;
                }
                this.bannerAd && this.bannerAd.destroy();
                this.onLoad(false, "timeout...");
            }, 5000);
        }
        doShow() {
            this.bannerAd.show().then(() => {
                this.onShow(true, null);
            }).catch((e) => {
                this.onShow(false, "catch");
            });
        }
        doHide() {
            if (this.sucShow) {
                this.bannerAd.hide();
                this.bannerAd.destroy();
            }
            this.onHide();
        }
    }
    exports.default = BannerInst;
});
define("h5core/src/H5AdBannerPos", ["require", "exports", "h5core/src/H5AdPos"], function (require, exports, H5AdPos_2) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class H5AdBannerPos extends H5AdPos_2.default {
    }
    exports.default = H5AdBannerPos;
});
define("platform/vivo-mini/src/ad/BannerPos", ["require", "exports", "h5core/src/H5AdBannerPos"], function (require, exports, H5AdBannerPos_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.BannerPos = void 0;
    class BannerPos extends H5AdBannerPos_1.default {
        create() {
            if (!this.plan) {
                return this;
            }
            this.banner_inst = this.plan.get_inst(this.pos.media_id);
            return this;
        }
        show(result) {
            if (!this.banner_inst) {
                result && result.onError("notAdInst");
                return;
            }
            this.banner_inst.show(result);
        }
        getFlag(cb) {
            if (this.banner_inst && this.banner_inst.getFlag()) {
                cb(true);
            }
            else {
                cb(false);
            }
        }
        hide() {
            this.banner_inst && this.banner_inst.hide();
        }
    }
    exports.BannerPos = BannerPos;
});
define("platform/vivo-mini/src/ad/IntersInst", ["require", "exports", "h5core/src/H5AdInst", "sdk/src/Const"], function (require, exports, H5AdInst_2, Const_2) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class IntersInst extends H5AdInst_2.default {
        constructor(sdk, media) {
            super(sdk, Const_2.AdType.inters, "sysInters");
            this.intersAd = null;
        }
        doLoad(adId) {
            this.intersAd = qg.createInterstitialAd({
                posId: adId,
            });
            // 监听系统插屏加载
            this.intersAd.onLoad(() => {
                this.onLoad(true, null);
            });
            // 监听系统插屏错误
            this.intersAd.onError((err) => {
                this.onLoad(false, JSON.stringify(err));
            });
            // 监听系统插屏隐藏
            this.intersAd.onClose(() => {
                this.onHide();
            });
        }
        doShow() {
            this.intersAd && this.intersAd.show().then(() => {
                this.onShow(true, null);
            }).catch((msg) => {
                this.onShow(false, JSON.stringify(msg));
            });
        }
        doHide() {
            super.onHide();
        }
    }
    exports.default = IntersInst;
});
define("h5core/src/H5AdIntersPos", ["require", "exports", "h5core/src/H5AdPos"], function (require, exports, H5AdPos_3) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class H5AdIntersPos extends H5AdPos_3.default {
    }
    exports.default = H5AdIntersPos;
});
define("platform/vivo-mini/src/ad/IntersPos", ["require", "exports", "h5core/src/H5AdIntersPos"], function (require, exports, H5AdIntersPos_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.IntersPos = void 0;
    class IntersPos extends H5AdIntersPos_1.default {
        create() {
            if (!this.plan) {
                return this;
            }
            this.main_inst = this.plan.get_inst(this.pos.media_id);
            let back_media_id = this.pos.params.back_media_id;
            let back_enable = this.pos.params.back_enable;
            if (back_enable && back_media_id) {
                this.back_inst = this.plan.get_inst(back_media_id);
            }
            return this;
        }
        getFlag(cb) {
            if (this.main_inst && this.main_inst.getFlag()) {
                cb(true);
                return;
            }
            if (this.back_inst && this.back_inst.getFlag()) {
                cb(true);
                return;
            }
            cb(false);
        }
        show(result) {
            if (!this.main_inst && !this.back_inst) {
                result && result.onError("notAdInst");
                return;
            }
            if (this.main_inst && this.main_inst.getFlag()) {
                let re = result.onError;
                result.onError = () => {
                    if (this.back_inst && this.back_inst.getFlag()) {
                        result.onError = re;
                        this.back_inst.show(result);
                    }
                };
                this.main_inst.show(result);
                return;
            }
            if (this.back_inst && this.back_inst.getFlag()) {
                this.back_inst.show(result);
                return;
            }
            result.onError();
        }
        hide() {
            this.main_inst && this.main_inst.hide();
            this.back_inst && this.back_inst.hide();
        }
    }
    exports.IntersPos = IntersPos;
});
define("platform/vivo-mini/src/ad/NativeTmpIntersInst", ["require", "exports", "h5core/src/H5AdInst", "sdk/src/Const"], function (require, exports, H5AdInst_3, Const_3) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class NativeTmpIntersInst extends H5AdInst_3.default {
        constructor(sdk, media) {
            super(sdk, Const_3.AdType.native, "tmpInters");
            this.default_wh = "1080;720";
            this.left = 0;
            this.top = 0;
            let platform = sdk.Platform();
            let info = platform.getInfo();
            let tmp_size = media.params.tmp_size;
            tmp_size = tmp_size ? tmp_size : this.default_wh;
            let wh = tmp_size.split(';');
            if (wh.length >= 2) {
                this.left = (info.win_width - Number(wh[0])) / 2;
                this.top = (info.win_height - Number(wh[1])) / 2;
            }
            // this.setting_reloadTimeSpace = 2000;
        }
        doLoad(adId) {
            this.nativeTemplateIntersAd = qg.createCustomAd({
                adUnitId: adId,
                style: {
                    left: this.left,
                    top: this.top
                }
            });
            this.nativeTemplateIntersAd.onLoad(() => {
                this.onLoad(true, null);
            });
            this.nativeTemplateIntersAd.onError((error) => {
                this.onLoad(false, JSON.stringify(error));
            });
            this.nativeTemplateIntersAd.onClose(() => {
                console.log(this.TAG, "NativeTmpAd onClose.");
                this.hide();
            });
        }
        doShow() {
            this.nativeTemplateIntersAd.show().then(() => {
                this.onShow(true, null);
            }).catch((err) => {
                this.onShow(false, JSON.stringify(err));
            });
        }
        doHide() {
            if (this.nativeTemplateIntersAd) {
                this.nativeTemplateIntersAd.destroy();
                this.nativeTemplateIntersAd = null;
            }
            this.onHide();
        }
    }
    exports.default = NativeTmpIntersInst;
});
define("platform/vivo-mini/src/ad/VideoInst", ["require", "exports", "h5core/src/H5AdInst", "sdk/src/Const"], function (require, exports, H5AdInst_4, Const_4) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class VideoInst extends H5AdInst_4.default {
        constructor(sdk) {
            super(sdk, Const_4.AdType.video, "video");
            this.videoAd = null;
        }
        doLoad(adId) {
            if (this.videoAd) {
                this.videoAd.load();
                return;
            }
            this.videoAd = qg.createRewardedVideoAd({
                posId: adId
            });
            //监听视频广告加载完成
            this.videoAd.onLoad(() => {
                this.onLoad(true, null);
            });
            //监听视频广告加载出错
            this.videoAd.onError((res) => {
                this.onLoad(false, JSON.stringify(res));
            });
            //监听视频广告播放完成
            this.videoAd.onClose((res) => {
                this.hide();
                this.callReward(res.isEnded);
            });
            // 加载一次
            this.videoAd.load();
        }
        doShow() {
            this.videoAd.show().then(() => {
                this.onShow(true, null);
            }).catch((msg) => {
                this.onShow(false, JSON.stringify(msg));
            });
        }
        callReward(result) {
            console.log(this.TAG, "video play complete? " + result);
            // 回调结果到游戏层
            let e = this.genAdEvent();
            e.isReward = result;
            this.result.onResult(e);
        }
    }
    exports.default = VideoInst;
});
define("h5core/src/H5AdVideoPos", ["require", "exports", "h5core/src/H5AdPos"], function (require, exports, H5AdPos_4) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class H5AdVideoPos extends H5AdPos_4.default {
    }
    exports.default = H5AdVideoPos;
});
define("platform/vivo-mini/src/ad/VideoPos", ["require", "exports", "h5core/src/H5AdVideoPos"], function (require, exports, H5AdVideoPos_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.VideoPos = void 0;
    class VideoPos extends H5AdVideoPos_1.default {
        create() {
            if (!this.plan) {
                return this;
            }
            this.video_inst = this.plan.get_inst(this.pos.media_id);
            return this;
        }
        show(result) {
            if (!this.video_inst) {
                result && result.onError("notAdInst");
                return;
            }
            this.video_inst && this.video_inst.show(result);
        }
        hide() {
            this.video_inst && this.video_inst.hide();
        }
        getFlag(cb) {
            if (this.video_inst && this.video_inst.getFlag()) {
                cb(true);
            }
            else {
                cb(false);
            }
        }
    }
    exports.VideoPos = VideoPos;
});
define("platform/vivo-mini/src/VivoAdPlan", ["require", "exports", "h5core/src/H5AdPlan", "sdk/src/Const", "platform/vivo-mini/src/ad/BannerInst", "platform/vivo-mini/src/ad/BannerPos", "platform/vivo-mini/src/ad/IntersInst", "platform/vivo-mini/src/ad/IntersPos", "platform/vivo-mini/src/ad/NativeTmpIntersInst", "platform/vivo-mini/src/ad/VideoInst", "platform/vivo-mini/src/ad/VideoPos"], function (require, exports, H5AdPlan_1, Const_5, BannerInst_1, BannerPos_1, IntersInst_1, IntersPos_1, NativeTmpIntersInst_1, VideoInst_1, VideoPos_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.VivoAdPlan = void 0;
    const TAG = "XGameH5-VivoAdPlan";
    class VivoAdPlan extends H5AdPlan_1.default {
        constructor(sdk, mgr) {
            super(sdk);
            this.mgr = mgr;
        }
        gen_pos(sdk, pos) {
            switch (pos.ad_type) {
                case Const_5.AdType.banner:
                    return new BannerPos_1.BannerPos(this, pos);
                case Const_5.AdType.inters:
                    return new IntersPos_1.IntersPos(this, pos);
                case Const_5.AdType.video:
                    return new VideoPos_1.VideoPos(this, pos);
            }
            return null;
        }
        gen_inst(sdk, media) {
            switch (media.ad_type) {
                case Const_5.AdType.banner:
                    return this.create_banner(sdk, media);
                case Const_5.AdType.inters:
                    return this.create_inters(sdk, media);
                case Const_5.AdType.video:
                    return this.create_video(sdk, media);
            }
            return null;
        }
        create_banner(sdk, media) {
            let params = media.params;
            let ad_id = params.ad_id;
            console.log(TAG, "create_banner media params:", params);
            if (ad_id) {
                let ad = null;
                if (params.banner_type == "1") {
                    ad = new BannerInst_1.default(sdk, media);
                }
                ad.init(ad_id.split(";"));
                ad.load();
                this.mgr.reg_inst(ad);
                return ad;
            }
            return null;
        }
        create_inters(sdk, media) {
            let params = media.params;
            let ad_id = params.ad_id;
            console.log(TAG, "create_inters media params:", params);
            if (ad_id) {
                let ad = null;
                if (params.inters_type == "1") {
                    ad = new IntersInst_1.default(sdk, media);
                }
                else if (params.inters_type == "2") {
                    ad = new NativeTmpIntersInst_1.default(sdk, media);
                }
                ad.init(ad_id.split(";"));
                ad.load();
                this.mgr.reg_inst(ad);
                return ad;
            }
            return null;
        }
        create_video(sdk, media) {
            let params = media.params;
            let ad_id = params.ad_id;
            if (ad_id) {
                let ad = new VideoInst_1.default(sdk);
                ad.init(ad_id.split(";"));
                ad.load();
                this.mgr.reg_inst(ad);
                return ad;
            }
            return null;
        }
    }
    exports.VivoAdPlan = VivoAdPlan;
});
define("h5core/src/H5AdControl", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    const TAG = "XGameH5-AdControl";
    class H5AdControl {
        constructor() {
            this.enable = false;
            this.banner_enable = false;
            this.banner_id = "";
            this.banner_refresh_time = 30;
            this.inters_enable = false;
            this.inters_id = "";
            this.inters_space_time = 0;
            // protected inters_space_show: number = 0;
            this.video_enable = false;
            this.video_id = "";
            this.custom_enable = false;
            this.custom_id_list = [];
            this.inters_last_show_time = 0;
            this.inters_now_space_show_count = 0;
        }
        parse_data(data) {
            if (!data["enable"] || data["enable"] === 0) {
                console.error(TAG, "param enable is undefined or enable not enable.");
                return;
            }
            this.enable = data["enable"];
            this.banner_enable = data["banner_enable"];
            this.banner_id = data["banner_id"];
            this.banner_refresh_time = data["banner_refresh_time"];
            this.inters_enable = data["inters_enable"];
            this.inters_id = data["inters_id"];
            // this.inters_space_show = data["inters_space_show"];
            this.inters_space_time = data["inters_space_time"];
            this.video_enable = data["video_enable"];
            this.video_id = data["video_id"];
            this.custom_enable = data["custom_enable"];
            if (this.custom_enable) {
                for (let i = 1; i <= 5; i++) {
                    this.custom_id_list.push(data["custom_id_" + i]);
                }
            }
        }
        getEnable() {
            return this.enable;
        }
        getBannerEnable() {
            return this.banner_enable;
        }
        getBannerId() {
            return this.banner_id;
        }
        getBannerRefreshTime() {
            return this.banner_refresh_time;
        }
        getIntersEnable() {
            return this.inters_enable;
        }
        getIntersId() {
            return this.inters_id;
        }
        getIntersSpaceTime() {
            return this.inters_space_time;
        }
        getIntersSpaceShow() {
            // return this.inters_space_show;
            return 0;
        }
        getVideoEnable() {
            return this.video_enable;
        }
        getVideoId() {
            return this.video_id;
        }
        getCustomEnable() {
            return this.custom_enable;
        }
        getCustomIdList() {
            return this.custom_id_list;
        }
        showInters() {
            if (this.inters_space_time > 0 && (new Date().getTime() - this.inters_last_show_time) < this.inters_space_time * 1000) {
                return "inters space time not limit.";
            }
            this.inters_last_show_time = new Date().getTime();
            // if (this.inters_space_show > 0 && this.inters_now_space_show_count < this.inters_space_show) {
            //     this.inters_now_space_show_count++;
            //     return "inters space count not limit.";
            // }
            // this.inters_now_space_show_count = 0;
            return "ok";
        }
    }
    exports.default = H5AdControl;
});
define("h5core/src/H5Ad", ["require", "exports", "sdk/src/Const", "sdk/src/Component", "sdk/src/IModule"], function (require, exports, Const_6, Component_2, IModule_2) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    const TAG = "XGameH5-IAd";
    class H5Ad {
        getType() {
            return IModule_2.default.MODULE_AD;
        }
        onModule(sdk) {
        }
        onInit() {
        }
        onInitAfter() {
        }
        onLogin() {
        }
        initAd() { }
        showBanner(result) {
            console.log(TAG, "showBanner");
            if (!this.ad_plan || !this.ad_contrl.getBannerEnable()) {
                console.error(TAG, "no ad plan or banner enable not enable.");
                return;
            }
            let banner = this.ad_plan.get_default_banner();
            if (banner) {
                banner.show(result);
            }
        }
        hideBanner() {
            console.log(TAG, "hideBanner");
            if (!this.ad_plan || !this.ad_contrl.getBannerEnable()) {
                return;
            }
            let banner = this.ad_plan.get_default_banner();
            if (banner) {
                banner.hide();
            }
        }
        getIntersFlag(cb) {
            if (!this.ad_plan || !this.ad_contrl.getIntersEnable()) {
                console.error(TAG, "no ad plan or inters enable not enable.");
                cb(false);
                return;
            }
            let inters = this.ad_plan.get_default_inters();
            if (!inters) {
                cb(false);
                return;
            }
            return inters.getFlag(cb);
        }
        showInters(result) {
            this.getIntersFlag((flag) => {
                if (!flag) {
                    console.error(TAG, "showInters fail, flag is false.");
                    return;
                }
                let ret = this.ad_contrl.showInters();
                if (ret != "ok") {
                    console.error(TAG, "showInters fail, control not pass,", ret);
                    return;
                }
                let inters = this.ad_plan.get_default_inters();
                inters && inters.show(result);
            });
        }
        getVideoFlag(cb) {
            if (!this.ad_plan || !this.ad_contrl.getVideoEnable()) {
                cb(false);
                return;
            }
            let video = this.ad_plan.get_default_video();
            if (!video) {
                cb(false);
                return;
            }
            video.getFlag(cb);
        }
        showVideo(result) {
            this.getVideoFlag((flag) => {
                if (!flag) {
                    result && result.onError && result.onError("disableFlag");
                    console.error(TAG, "no ad plan or video enable not enable.");
                    return;
                }
                let video = this.ad_plan.get_default_video();
                if (video) {
                    video.show(result);
                }
                else {
                    result && result.onError && result.onError("noDefaultVideoPos");
                }
            });
        }
        newResult() {
            let r = new Component_2.ShowResult();
            r.onClick = () => { };
            r.onError = () => { };
            r.onHide = () => { };
            r.onShow = () => { };
            return r;
        }
        createCustomAd(idIndex) {
            console.log("IAd", "idIndex:", idIndex);
            return null;
        }
        createBannerPos(pos_no) {
            if (!this.ad_plan) {
                return null;
            }
            if (!this.ad_plan.check_pos_no(Const_6.AdType.banner, pos_no)) {
                return null;
            }
            return this.ad_plan.get_pos(pos_no);
        }
        createIntersPos(pos_no) {
            if (!this.ad_plan) {
                return null;
            }
            if (!this.ad_plan.check_pos_no(Const_6.AdType.inters, pos_no)) {
                return null;
            }
            return this.ad_plan.get_pos(pos_no);
        }
        createVideoPos(pos_no) {
            if (!this.ad_plan) {
                return null;
            }
            if (!this.ad_plan.check_pos_no(Const_6.AdType.video, pos_no)) {
                return null;
            }
            return this.ad_plan.get_pos(pos_no);
        }
        createCustomPos(pos_no) {
            if (!this.ad_plan) {
                return null;
            }
            if (!this.ad_plan.check_pos_no(Const_6.AdType.custom, pos_no)) {
                return null;
            }
            return this.ad_plan.get_pos(pos_no);
        }
    }
    exports.default = H5Ad;
});
define("platform/vivo-mini/src/VivoMiniAd", ["require", "exports", "sdk/src/Const", "h5core/src/H5Ad", "platform/vivo-mini/src/VivoAdPlan", "h5core/src/H5AdControl", "h5core/src/H5AdInstMgr"], function (require, exports, Const_7, H5Ad_1, VivoAdPlan_1, H5AdControl_1, H5AdInstMgr_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    const TAG = "XGameH5-VivoMiniAd";
    class VivoMiniAd extends H5Ad_1.default {
        onModule(sdk) {
            this.sdk = sdk;
        }
        ;
        parse_ad_control(channel) {
            this.ad_contrl = new H5AdControl_1.default();
            let plug = channel.getPlug(Const_7.PLUG.mini_ad_control);
            if (!plug || !plug.params || !plug.params.enable) {
                return;
            }
            this.ad_contrl.parse_data(plug.params);
        }
        parse_ad_plug(channel) {
            if (!this.ad_contrl.getEnable()) {
                console.error(TAG, "ad enable not open.");
                return;
            }
            let plug = channel.getPlug(Const_7.PLUG.vivo_mini);
            if (!plug || !plug.params) {
                return;
            }
            let ad_plan = plug.params.ad_plan;
            if (!ad_plan) {
                return;
            }
            let sdk = this.sdk;
            this.inst_mgr = new H5AdInstMgr_1.default(sdk);
            this.inst_mgr.init();
            this.ad_plan = new VivoAdPlan_1.VivoAdPlan(sdk, this.inst_mgr);
            this.ad_plan.parse_data(ad_plan);
            let def_banner = this.ad_plan.get_default_banner();
            def_banner && def_banner.create();
            let def_inters = this.ad_plan.get_default_inters();
            def_inters && def_inters.create();
            let def_video = this.ad_plan.get_default_video();
            def_video && def_video.create();
        }
        onLogin() {
            let sdk = this.sdk;
            let channel = sdk.Channel();
            this.parse_ad_control(channel);
            this.parse_ad_plug(channel);
        }
    }
    exports.default = VivoMiniAd;
});
define("h5core/src/H5Platform", ["require", "exports", "sdk/src/IModule", "sdk/src/Structs"], function (require, exports, IModule_3, Structs_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    const SERVER_HOST = "https://api.xplaymobile.com";
    // const SERVER_HOST = "http://192.168.31.146/local_xgp";
    const SERVER_URL = SERVER_HOST + "/xgp_user";
    class H5Platform {
        getType() {
            return IModule_3.default.MODULE_PLATFORM;
        }
        onInit() {
        }
        onInitAfter() {
        }
        onLogin() {
        }
        onModule(sdk) {
        }
        onInitEnd(success) {
            success("ok");
        }
        login(success) {
            success("platformNotSupport", null);
        }
        pay(product_no, success) {
            success("platformNotSupport");
        }
        ajax(opt) {
            let xhr = new XMLHttpRequest();
            xhr.onreadystatechange = () => {
                if (xhr.readyState == 4) {
                    let res = new Structs_1.AjaxRes();
                    res.header = xhr.getAllResponseHeaders();
                    res.url = opt.url;
                    res.method = opt.method;
                    res.status = xhr.status;
                    if (opt.dataType == "json") {
                        res.data = JSON.parse(xhr.response);
                    }
                    else {
                        res.data = xhr.response;
                    }
                    opt.success(res);
                }
            };
            xhr.timeout = 10000;
            xhr.ontimeout = () => {
                opt.error("timeout...");
            };
            xhr.onerror = (e) => {
                opt.error("onerror");
            };
            if (opt.header) {
                for (let k in opt.header) {
                    xhr.setRequestHeader(k, opt.header[k]);
                }
            }
            xhr.open(opt.method, opt.url, true);
            xhr.send(opt.data);
        }
        getServerUrl() {
            return SERVER_URL;
        }
        getServerHost() {
            return SERVER_HOST;
        }
        saveLocal(name, value) {
            localStorage && localStorage.getItem(name);
        }
        getLocal(name) {
            return localStorage ? localStorage.getItem(name) : null;
        }
        removeLocal(name) {
            localStorage && localStorage.removeItem(name);
        }
        clearLocal() {
            localStorage && localStorage.clear();
        }
        saveSession(name, value) {
            sessionStorage && sessionStorage.setItem(name, value);
        }
        getSession(name) {
            return sessionStorage ? sessionStorage.getItem(name) : null;
        }
        removeSession(name) {
            sessionStorage && sessionStorage.removeItem(name);
        }
        clearSession() {
            sessionStorage && sessionStorage.clear();
        }
        getInfo() { return null; }
        shareApp() { }
        phoneVibrate(type) { }
        setTimeout(callback, delay, ...args) {
            setTimeout(callback, delay, ...args);
        }
        clearTimeout(id) {
            clearTimeout(id);
        }
        setInterval(callback, delay, ...args) {
            return setInterval(callback, delay, ...args);
        }
        clearInterval(id) { }
        exitTheGame() { }
    }
    exports.default = H5Platform;
});
define("sdk/src/Utils", ["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    exports.Queue = void 0;
    class Queue {
        constructor() {
            this.queue = [];
            this.starting = false;
        }
        complete(...args) {
            if (this.queue.length > 0) {
                let cb = this.queue[0];
                this.queue = this.queue.slice(1);
                cb(this, ...args);
            }
            else {
                this.starting = false;
                if (this.finishcb) {
                    this.finishcb(this);
                }
            }
        }
        then(cb) {
            this.queue.push(cb);
            return this;
        }
        start(cb = null, ...args) {
            this.finishcb = cb;
            if (!this.starting) {
                this.starting = true;
                this.complete(...args);
            }
        }
        cancel() {
            this.starting = false;
            this.queue = [];
        }
        static create() {
            return new Queue();
        }
    }
    exports.Queue = Queue;
});
define("platform/vivo-mini/src/VivoMiniPlatform", ["require", "exports", "sdk/src/Const", "h5core/src/H5Platform", "sdk/src/Structs", "sdk/src/Utils"], function (require, exports, Const_8, H5Platform_1, Structs_2, Utils_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    const TAG = "XGameH5-VivoMiniPlatform";
    class VivoMiniPlatform extends H5Platform_1.default {
        constructor() {
            super();
            this.needReqPayedOrders = 0;
            let sysInfo = qg.getSystemInfoSync();
            this.info = new Structs_2.PlatformInfo();
            this.info.win_width = sysInfo.screenWidth;
            this.info.win_height = sysInfo.screenHeight;
        }
        onModule(sdk) {
            this.sdk = sdk;
            this.user = sdk.User();
        }
        onInitEnd(success) {
            success("ok");
            let doTimeoutReq = () => {
                if (this.needReqPayedOrders > 0) {
                    this.needReqPayedOrders--;
                    this.user.reqPayedOrders();
                }
            };
            this.setInterval(doTimeoutReq, 5000);
            qg.onShow(() => {
                this.needReqPayedOrders = 2;
            });
        }
        login(success) {
            qg.login({
                success: (res) => {
                    console.log(TAG, JSON.stringify(res));
                    if (res.data.token) {
                        let data = {
                            plug: Const_8.PLUG.vivo_mini,
                            info: {
                                token: res.data.token,
                            }
                        };
                        success("ok", data);
                    }
                    else {
                        success("vivoLoginFaild", null);
                    }
                }
            });
        }
        prePayOrder(product_no, success) {
            let send = {
                "product_no": product_no
            };
            console.log("prePayOrder send", JSON.stringify(send));
            this.user.api("/user/vivo_mini/pre_order", send, (ret, res) => {
                console.log("pre_order:", JSON.stringify(ret), JSON.stringify(res));
                success(ret, res);
            });
        }
        pay(product_no, success) {
            let q = Utils_1.Queue.create();
            q.then(() => {
                this.prePayOrder(product_no, (ret, res) => {
                    if (ret != "ok") {
                        qg.showToast({ message: "创建订单失败", duration: 0 });
                        success(ret);
                        q.cancel();
                    }
                    else {
                        success(ret);
                        q.complete(res);
                    }
                });
            });
            q.then((q, res) => {
                let obj = res.data.obj;
                qg.pay({
                    orderInfo: JSON.stringify(obj),
                    success: (ret) => {
                        console.log("支付成功", JSON.stringify(ret));
                        success("ok");
                        q.complete();
                    },
                    fail: function (err) {
                        console.log("支付失败", JSON.stringify(err));
                        q.cancel();
                    },
                    cancel: function (ret) {
                        console.log("支付取消", JSON.stringify(ret));
                        q.cancel();
                    },
                    complete: function () {
                        console.log("支付完成");
                        q.complete();
                    }
                });
            });
            q.start();
        }
        ajax(opt) {
            qg.request({
                url: opt.url,
                data: opt.data,
                method: opt.method,
                dataType: opt.dataType,
                success: (res) => {
                    let result = new Structs_2.AjaxRes();
                    result.data = res.data;
                    result.header = res.header;
                    result.status = res.statusCode;
                    result.url = opt.url;
                    result.method = opt.method;
                    if (opt.success) {
                        opt.success(result);
                    }
                },
                fail: (res) => {
                    if (opt.error) {
                        opt.error(res);
                    }
                }
            });
        }
        saveLocal(name, value) {
            localStorage.setItem(name, value);
        }
        getLocal(name) {
            return localStorage.getItem(name);
        }
        removeLocal(name) {
            localStorage.removeItem(name);
        }
        saveSession(name, value) {
            localStorage.setItem(name, value);
        }
        getSession(name) {
            return localStorage.getItem(name);
        }
        removeSession(name) {
            localStorage.removeItem(name);
        }
        getInfo() {
            return this.info;
        }
        setTimeout(callback, delay, ...args) {
            return setTimeout(callback, delay, ...args);
        }
        clearTimeout(id) { clearTimeout(id); }
        setInterval(callback, delay, ...args) {
            return setInterval(callback, delay, ...args);
        }
        clearInterval(id) { clearInterval(id); }
    }
    exports.default = VivoMiniPlatform;
});
define("h5core/src/H5User", ["require", "exports", "sdk/src/IModule", "sdk/src/Structs"], function (require, exports, IModule_4, Structs_3) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    let K_USER_ID = "xgame.user_id";
    let K_USER_TOKEN = "xgame.user_token";
    let K_ACCOUNT_ID = "xgame.account_id";
    class H5User {
        constructor() {
            this.userId = 0;
            this.userToken = null;
            this.accountId = 0;
            this.channelId = 0;
            this.commitOrderMap = {};
        }
        onModule(sdk) {
            this.sdk = sdk;
            this.platform = sdk.Platform();
            this.channel = sdk.Channel();
        }
        onInit() {
            this.channelId = this.sdk.getChannelId();
        }
        onInitAfter() {
        }
        onLogin() {
        }
        getType() {
            return IModule_4.default.MODULE_USER;
        }
        getAccountId() {
            return this.accountId;
        }
        getUserId() {
            return this.userId;
        }
        getUserToken() {
            return this.userToken;
        }
        reqUserInfo(success) {
            this.api("/user/info/get_info", {}, (ret, res) => {
                if (ret == "ok") {
                    let data = res.data;
                    this.channel.loadInfo(data.channel_info);
                    this.orders = [];
                    if (data.orders_num && data.orders_num > 0) {
                        for (let i in data.orders) {
                            let od = data.orders[i];
                            let oi = new Structs_3.OrderInfo();
                            oi.orderId = od.order_id;
                            oi.status = 1;
                            oi.product = this.newProduct();
                            oi.product.id = od.product_id;
                            this.orders.push(oi);
                        }
                    }
                    this.sdk.onLogin();
                    success(ret, res);
                }
                else {
                    success(ret, res);
                }
            });
        }
        clearLogin() {
            this.userId = 0;
            this.userToken = null;
            this.accountId = 0;
            this.platform.removeSession("user_id");
            this.platform.removeSession("user_token");
        }
        login(success) {
            if (this.userId > 0) {
                console.log("重复登录");
                success("alreadyLogined", null);
                return;
            }
            this.platform.login((ret, data) => {
                if (ret != "ok") {
                    success(ret, null);
                    return;
                }
                let req = {
                    plug: data.plug,
                    info: data.info,
                };
                this.api("/login/platform_login", req, (ret, res) => {
                    if (ret == "ok") {
                        let user = res.data.user;
                        this.userId = user.user_id;
                        this.userToken = user.token;
                        this.accountId = res.data.account_id;
                        this.reqUserInfo(success);
                    }
                    else {
                        this.clearLogin();
                        success(ret, res);
                    }
                }, false);
            });
        }
        logout() {
            console.log("logout");
            this.clearLogin();
        }
        reqPayedOrders() {
            this.api("/user/order/get_payed_orders", {}, (ret, res) => {
                if (ret != "ok") {
                    return;
                }
                let data = res.data;
                let orders = [];
                if (data.orders_num && data.orders_num > 0) {
                    for (let i in data.orders) {
                        let v = data.orders[i];
                        let order_id = v.order_id;
                        if (!this.commitOrderMap[order_id]) {
                            let or = new Structs_3.OrderInfo();
                            or.orderId = v.order_id;
                            or.product = new Structs_3.ProductInfo();
                            or.product.id = v.product.product_no;
                            or.product.price = v.product.price;
                            orders.push(or);
                        }
                    }
                }
                this.onPayed("ok", orders);
            });
        }
        pay(product_no, success) {
            this.platform.pay(product_no, (ret) => {
                console.log("platform pay:", ret);
                success(ret);
                if (ret == "ok") {
                    this.reqPayedOrders();
                }
            });
        }
        setOnPayed(payed) {
            this.payedCb = payed;
        }
        onPayed(ret, orders) {
            if (this.payedCb) {
                this.payedCb(ret, orders);
                return true;
            }
            else {
                return false;
            }
        }
        getOrders() {
            return this.orders;
        }
        commitOrder(order_id) {
            this.commitOrderMap[order_id] = true;
            this.api("/user/order/commit_order", { order_id: order_id }, (ret, res) => {
                if (ret == "ok") {
                    delete this.commitOrderMap[order_id];
                }
            });
        }
        newProduct() {
            return new Structs_3.ProductInfo();
        }
        api(url, data, success, checkLogin = true) {
            if (!this.userId && checkLogin) {
                success("apiUnlogin", { ret: "apiUnlogin" });
                return;
            }
            let send = {
                user_id: this.userId,
                user_token: this.userToken,
                channel_id: this.channelId,
                account_id: this.accountId,
                data: null,
            };
            send.data = data;
            this.platform.ajax({
                method: "POST",
                url: this.platform.getServerUrl() + url,
                data: JSON.stringify(send),
                dataType: "json",
                success: (res) => {
                    let rd = res.data;
                    if (res.status != 200) {
                        success("statusError", res);
                        return;
                    }
                    if (rd.ret == "tokenExpired") {
                        this.clearLogin();
                        success("tokenExpired", rd);
                        return;
                    }
                    success(rd.ret, rd);
                },
                error: (e) => {
                    console.error("ajax error:", JSON.stringify(e));
                    success("ajaxError", { ret: "ajaxError" });
                },
            });
        }
        /*
         * 兼容Android marsdk 接口
         */
        androidMarPay(money, propId, propName) {
        }
    }
    exports.default = H5User;
});
define("h5core/src/H5Channel", ["require", "exports", "sdk/src/IModule", "sdk/src/Structs"], function (require, exports, IModule_5, Structs_4) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    class H5Channel {
        constructor() {
            this.plugMap = {};
        }
        onInitAfter() {
        }
        onLogin() {
        }
        getType() {
            return IModule_5.default.MODULE_CHANNEL;
        }
        onModule(sdk) {
            this.sdk = sdk;
        }
        onInit() {
            this.channelId = this.sdk.getChannelId();
        }
        loadInfo(info) {
            this.channelId = info.channelId;
            this.plugs = [];
            for (let i in info.plugs) {
                let p = info.plugs[i];
                let plug = new Structs_4.Plug();
                plug.params = p.params;
                plug.ptype = p.ptype;
                this.plugs.push(plug);
                this.plugMap[plug.ptype] = plug;
            }
        }
        getPlug(ptype) {
            return this.plugMap[ptype];
        }
    }
    exports.default = H5Channel;
});
define("platform/vivo-mini/src/config", ["require", "exports", "sdk/src/XGame", "h5core/src/H5User", "h5core/src/H5Channel", "platform/vivo-mini/src/VivoMiniAd", "platform/vivo-mini/src/VivoMiniPlatform"], function (require, exports, XGame_1, H5User_1, H5Channel_1, VivoMiniAd_1, VivoMiniPlatform_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    {
        XGameGlobal["xgame.sdk.init.platform"] = (cb) => {
            let sdk = new XGame_1.default();
            sdk.initModules([
                new H5User_1.default(),
                new H5Channel_1.default(),
                new VivoMiniPlatform_1.default(),
                new VivoMiniAd_1.default(),
            ]);
            extension_amd_init();
            cb(sdk);
        };
        XGameGlobal["xgame.sdk.init"] = (cb) => {
            XGameGlobal["xgame.sdk.init.platform"](cb);
        };
    }
});
