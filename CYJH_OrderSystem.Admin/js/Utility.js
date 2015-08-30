

//设置文本框高度自动(随着输入的内容而增加高度)
/*
opt:
freeHeight:空余高度
*/
function SetTextAreaAutoHeight(textareaJId, opt) {
    opt = $.extend({
        minHeight: 100,
        freeHeight: 18
    }
	, opt);
    var changeFunc = function() {
        var toHeight = this.scrollHeight;
        if (toHeight < opt.minHeight)
            toHeight = opt.minHeight;
        this.style.height = (toHeight + opt.freeHeight) + 'px';
    };
    $(textareaJId).bind("propertychange", changeFunc).height(0).bind("input", changeFunc).bind("keydown", changeFunc).bind("keyup", changeFunc).bind("keypress", changeFunc).trigger("keydown");
}

function imgOnLoadMiddle(img) {
    $(img).css({ "margin-top": (img.parentNode.offsetHeight - img.offsetHeight) / 2 + "px" });
}

//获取滚动事件滚轴的方向，上为-1，下为1
function getWheelV(e) { e = window.event || e.originalEvent; return -e.wheelDelta / Math.abs(e.wheelDelta) || e.detail / Math.abs(e.detail) || e.deltaY / Math.abs(e.deltaY) }

function tryGetObj(str) {
    var dv = null;
    try { eval("dv=" + str); } catch (e) { }
    return dv;
}
function getJsonCallBack(succeedBack, errBack, aftBack) {
    return function(data) {
        if (aftBack) aftBack();
        var dv = tryGetObj(data);
        if (dv == null) {
            if (errBack) errBack();
            alert("网页发生错误，部分功能暂时无法使用！如有疑问请联系客服！");
        }
        else {
            if (succeedBack) succeedBack(dv);
        }
    };
}
function callParentFormSubmit(dom) {
    $(dom).parents("form:first").find("input[type='submit']").click();
    return false;
}
//未测试
function setTxtDefVal(inp, defTxt, defClassName) {
    var inp = $(inp).val(defTxt).removeClass(defClassName).addClass(defClassName)
    .focus(function() { if (this.value == defTxt) $(this).val("").removeClass(defClassName) })
    .blur(function() { if (this.value == "") $(this).val(defTxt).removeClass(defClassName).addClass(defClassName) })[0];
    $(inp.form).submit(function() { if (inp.value == defTxt) $(inp).val("").removeClass(defClassName) });
}


//刷新图片的验证码
function reflushImgCode(img) {
    var url = img.src;
    var timestamp = new Date().getTime();
    if (url.indexOf("?") <= -1)
        url += "?";
    if (url.substr(url.length - 1, 1) != "?")
        url += "&";
    if (url.indexOf("&r=") > -1) {
        url = url.replace(/\&r\=.*/, '&r=' + timestamp);
    }
    else {
        url += 'r=' + timestamp;
    }
    img.src = url;
}


/********************************************
* Tab切换
* jList:要导航的jQuery对象
* idAttr:要显示的对象存放在哪个属性里
* currClass:切换的元素,被切换到的附加class
* autoShowFirst:是否默认显示第一个元素
* autoPlay:是否自动轮转播放
* autoPlaySpeed:启用autoPlay时有效,播放延迟
* onTabChange:触发选项卡切换事件(切换后)
* showContentEvent:触发选项卡切换的事件(被什么事件触发切换的.如click,点击后触发切换)
********************************************/
var TabInit = function(jList, opt) {
    opt = $.extend({
        idAttr: "href",
        currClass: "",
        autoShowFirst: true,
        autoPlay: false,
        autoPlaySpeed: 3000,
        onTabChange: function(btn, show) { },
        showContentEvent: "click",
        useRepClass: false,
        olClassAttr: "olclass", //原class存放的属性
        repClassAttr: "hoverclass"//如果经过需要替换的class所存放的属性
    }, opt);

    function autoPlayInit() {
        if (opt.autoPlay) {
            clearTimeout(autoPlayHandle);
            autoPlayHandle = setTimeout(autoPlay, opt.autoPlaySpeed);
        }
    }
    function autoPlay() {
        if (thisCurrBtn) {
            var next = thisCurrBtn.next(":first");
            if (next.length <= 0) next = thisCurrBtn.parent().children(":first");
            next.trigger(opt.showContentEvent);
        }
    }

    var firstShowBtn = null;
    var thisCurrShow = null;
    var thisCurrBtn = null;
    var autoPlayHandle = 0;
    var j = $(jList);
    j.each(function(i, n) {
        var jn = $(n);
        var toId = jn.attr(opt.idAttr), tjIndex = toId.indexOf("#");
        if (tjIndex > -1) toId = toId.substr(tjIndex, toId.length - tjIndex);
        var toObj = null;
        try { toObj = $(toId); } catch (e) { }
        if (toObj == null || toObj.length <= 0) return;
        jn.attr("showId", toId);
        toObj.hide();
        if (firstShowBtn == null) firstShowBtn = jn;
        var tabFn = function(e) {
            var btn = $(this);
            thisCurrBtn = btn;
            if (thisCurrShow) thisCurrShow.hide();
            thisCurrShow = $(btn.attr("showId")).show();
            if (opt.currClass) {
                j.filter("." + opt.currClass).removeClass(opt.currClass);
                btn.addClass(opt.currClass);
            }
            if (opt.useRepClass) {
                j.each(function() {
                    var el = $(this);
                    el.removeClass(el.attr(opt.repClassAttr)).addClass(el.attr(opt.olClassAttr));
                });
                btn.removeClass(btn.attr(opt.olClassAttr)).addClass(btn.attr(opt.repClassAttr));
            }
            autoPlayInit();
            if (opt.onTabChange) opt.onTabChange(thisCurrBtn, thisCurrShow);
            return false;
        };
        jn.bind("goTab", tabFn).bind(opt.showContentEvent, function() { $(this).trigger("goTab"); return false }).focus(function() { $(this).blur() });
    });
    if (opt.autoShowFirst && firstShowBtn) firstShowBtn.trigger("goTab");

    this.hideCurr = function() {
        if (thisCurrShow) thisCurrShow.hide();
        if (opt.currClass) j.filter("." + opt.currClass).removeClass(opt.currClass);
    }
    this.toTabByToId = function(toId) {
        j.filter("[showId='" + toId + "']").trigger("goTab");
    }
};

/********************************************
* 获取元素绝对位置
********************************************/
function GetPos(obj, dis) {
    if (!dis && obj.getBoundingClientRect) {
        var pos = obj.getBoundingClientRect(), d = obj.ownerDocument;
        return { left: pos.left + (d.documentElement.scrollLeft | d.body.scrollLeft)
				, top: pos.top + (d.documentElement.scrollTop | d.body.scrollTop)
        }
    }
    var curleft = obj.offsetLeft || 0;
    var curtop = obj.offsetTop || 0;
    while (obj = obj.offsetParent) { curleft += eval(obj.offsetLeft); curtop += obj.offsetTop; }
    return { left: curleft, top: curtop };
}



/********************************************
* 层级进入功能
********************************************/
function LevelSelect(opt) {
    opt = $.extend({
        idSplit: "/",
        eventName: "click"
    }, opt);

    this.parse = function(has) {
        if (!has) has = location.hash;
        if (has && has.length > 1) {
            var idArr = has.substr(1).split(opt.idSplit);
            for (var i = 0; i < idArr.length; i++) {
                var id = idArr[i];
                if (id) {
                    var j = $(id), el = j[0];
                    if (el) {
                        j.trigger(opt.eventName);
                    }
                }
            }
        }
    };
};
var singleLevelSelect = new LevelSelect();
LevelSelect.parse = singleLevelSelect.parse;


/********************************************
* 复制到剪切板
********************************************/
//function copyToClipBoard(text) {
//    window.clipboardData.setData('text', text);
//}
function copyToClipBoard(copy) {
    if (window.clipboardData) {
        window.clipboardData.setData("Text", copy);
        return true;
    }
    else if (window.netscape) {
        try {
            netscape.security.PrivilegeManager.enablePrivilege('UniversalXPConnect');
            var clip = Components.classes['@mozilla.org/widget/clipboard;1'].createInstance(Components.interfaces.nsIClipboard);
            if (!clip) return;
            var trans = Components.classes['@mozilla.org/widget/transferable;1'].createInstance(Components.interfaces.nsITransferable);
            if (!trans) return;
            trans.addDataFlavor('text/unicode');
            var str = new Object();
            var len = new Object();
            var str = Components.classes["@mozilla.org/supports-string;1"].createInstance(Components.interfaces.nsISupportsString);
            var copytext = copy;
            str.data = copytext;
            trans.setTransferData("text/unicode", str, copytext.length * 2);
            var clipid = Components.interfaces.nsIClipboard;
            if (!clip) return false;
            clip.setData(trans, null, clipid.kGlobalClipboard);
        } catch (e) { return false; }
        return true;
    }
    return false;
}

/********************************************
* 获取页面参数
********************************************/
function request(paras) {
    var paraObj = window.RequestParam;
    if (!paraObj) {
        paraObj = {};
        var url = location.href;
        var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
        for (i = 0; j = paraString[i]; i++) {
            paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
        }
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}
function getRequestParams(oUrl) {
    //如果有指定url那么就实时获取,否则就用缓存的对象
    var paraObj = oUrl ? null : window.RequestParam;
    if (!paraObj) {
        paraObj = {};
        var url = oUrl || location.search;
        if (url.indexOf("#") > -1) url = url.substr(0, url.indexOf("#"));
        var fi = url.indexOf("?");
        if (fi > -1 && fi + 1 <= url.length) url = url.substr(fi + 1, url.length - fi - 1);
        var paraString = url.split("&");
        for (i = 0; i < paraString.length; i++) {
            var tmp = paraString[i].split("=");
            paraObj[tmp[0]] = decodeURIComponent(tmp[1]);
        }
        if (oUrl) window.RequestParam = paraObj;
    }
    return paraObj;
}
function urlParamToStr(req) {
    var pas = req || getRequest();
    var paStr = "";
    for (var i in pas) {
        paStr += i + "=" + encodeURIComponent(pas[i]) + "&";
    }
    if (paStr.length > 0) paStr = paStr.substr(0, paStr.length - 1);
    return paStr;
}
function replaceURLParam(name, value) {
    var ps = getRequestParams();
    ps[name] = value;
    return "?" + urlParamToStr(ps);
}

/********************************************
* 浮动显示页面(DIV显示)
* closing:返回true表示停止关闭
********************************************/
function FloatDiv(div, opt) {
    var t = this;
    if (!window.FDBgZIndex) window.FDBgZIndex = 999;
    opt = $.extend({
        closeSel: "",
        vPosIsParentIfrWin: false,//Iframe时,垂直位置是否用父级窗口的来计算
        bgClass: "floatBoxBg", autoShow: false, prependTo: document.body, closing: null, closeBack: null, useOvDiv: true, defindTop: null
    }, opt);
    div = t.jq = $(div);
    var ov = $("<div class='" + opt.bgClass + "' style='position:absolute;z-index:998;display:none;background:#000;filter:Alpha(opacity=50);  opacity:0.5;'></div>");
    div.css({ "position": "absolute", "z-index": "999", "display": "none" });
    var setPos = function () {
        var winSize = getWinSize();
        var topValue;
        if (opt.defindTop != null) {
            topValue = opt.defindTop
        }
        else {
            topValue = (winSize.winH - div[0].offsetHeight) / 2 + winSize.scrollT;
        }
        var left = ((winSize.winW - div[0].offsetWidth) / 2 + winSize.scrollL), top;
        if (opt.vPosIsParentIfrWin) {
            var ifrs = parent.document.getElementsByTagName("iframe");
            for (var i = 0; i < ifrs.length; i++) {
                try {
                    if (ifrs[i].contentWindow == window) {
                        var pos = GetPos(ifrs[i]);
                        var ifrWinSize = getWinSize(parent.document);
                        topValue = (ifrWinSize.winH - div[0].offsetHeight) / 2 + ifrWinSize.scrollT - pos.top;
                        if (topValue < 0) topValue = 0;
                        if (topValue > winSize.pageH + div[0].offsetHeight) topValue = winSize.pageH + div[0].offsetHeight;
                        break;
                    }
                } catch (e) { }
            }
        }

        ov.css({ width: winSize.winW, height: winSize.pageH, "z-index": ++window.FDBgZIndex });
        div.css({
            left: left + "px", top: topValue + "px"
            , "z-index": ++window.FDBgZIndex
        });
    }
    t.resetPos = setPos;
    t.show = function() {
        ov.show();
        div.show();
        setPos();
    };
    t.fadeIn = function(time) {
        ov.fadeIn("slow");
        div.fadeIn("slow");
        setPos();
    }
    t.hide = function() {
        if (opt.closing && opt.closing()) return;
        ov.hide();
        div.hide();
        if (opt.closeBack) opt.closeBack();
    };
    if (opt.useOvDiv) $(document.body).prepend(ov);
    if (opt.prependTo) $(opt.prependTo).prepend(div);
    //ov.click(t.hide);
    if (opt.autoShow) t.show();
    div.resize(setPos)
    $(window).resize(setPos).resize();
    t.setCloseBack = function(fn) {
        opt.closeBack = fn;
    }
    if (opt.closeSel) div.find(opt.closeSel).click(t.hide);
}

/********************************************
* 浮动显示页面(iframe显示),
********************************************/
function FloatIfrUrl(url, opt) {
    var winSize = getWinSize();
    opt = $.extend({
        maxWidth: winSize.winW
        , maxHeight: winSize.winH
        , useMaxSize: true
        , autoShow: true
        , regIfrCloseFunc: "contentWindow.close"
    }, opt);

    var t = this, c = FloatIfrUrl;
    t.opt = opt;

    if (!c.prototype.fCount) c.prototype.fCount = 0;
    var fid = ++c.prototype.fCount;
    if (!c.prototype.allLoadFunc) c.prototype.allLoadFunc = [];
    c.prototype.allLoadFunc[fid] = function(ifr) {
        try {
            eval("ifr." + opt.regIfrCloseFunc + "=t.hide;");
        } catch (e) { alert(e + ":" + e.message); }
        if (ifr.floatObject.opt.useMaxSize) {
            var size = getIfrCntSize(ifr);
            if (size.width > ifr.floatObject.opt.maxWidth) size.width = ifr.floatObject.opt.maxWidth;
            if (size.height > ifr.floatObject.opt.maxHeight) size.height = ifr.floatObject.opt.maxHeight;
            ifr.style.height = size.height + "px";
            ifr.style.width = size.width + "px";
        }
        else {
            resizeIframeHeight(ifr);
        }
        ifr.floatObject.show();
    }
    var ifr = $("<iframe frameborder='0' id='__FloatIfrUrl_" + fid + "' onload='FloatIfrOnload(" + fid + ")' style='border:none;position:absolute;z-index:999999;display:none;overflow:hidden;' scrolling='auto' allowtransparency='true'></iframe>")
            .prependTo(document.body);
    //属性复制实现继承
    var base = new FloatDiv(ifr, opt);
    for (var p in base) t[p] = base[p];
    ifr.css({ width: opt.width + "px", height: opt.height + "px" }).attr("src", url);
    ifr[0].floatObject = t;
    t['show'] = function() { base.show(); resizeIframeHeight(ifr[0]); };
}
function FloatIfrOnload(fid) {
    var ifr = document.getElementById("__FloatIfrUrl_" + fid);
    var func = FloatIfrUrl.prototype.allLoadFunc[fid];
    if (func) func(ifr);
}

/********************************************
* 浮动显示页面(加载页面显示),
********************************************/
function FloatUrl(url, opt) {
    opt = $.extend({
        loadFn: function() { }
    }, opt);

    var t = this, div = $("<div></div>").prependTo(document.body).html("加载中...");
    //属性复制实现继承
    var base = new FloatDiv(div, opt);
    for (var p in base) t[p] = base[p];
    div.css({ width: opt.width + "px", height: opt.height + "px" });
    $.get(url, { rnd: Math.random() }, function(data) {
        div.html(data);
        if (opt.autoShow) base.show();
        if (opt.loadFn) opt.loadFn();
    })
}


/********************************************
* 获取窗体/页面尺寸,W3C标准
********************************************/
function getWinSize(doc) {
    var d = doc || document;
    var pageH = d.documentElement.offsetHeight;
    if (d.body.offsetHeight > pageH) pageH = d.body.offsetHeight;
    if (d.documentElement.scrollHeight > pageH) pageH = d.documentElement.scrollHeight;
    if (d.body.scrollHeight > pageH) pageH = d.body.scrollHeight;

    if (d.documentElement.clientHeight > pageH) pageH = d.documentElement.clientHeight;
    if (d.body.clientHeight > pageH) pageH = d.body.clientHeight;

    return {
        winW: d.documentElement.clientWidth,
        winH: d.documentElement.clientHeight,
        pageW: Math.max(d.documentElement.offsetWidth, d.body.offsetWidth),
        pageH: pageH,
        scrollT: Math.max(d.documentElement.scrollTop, d.body.scrollTop),
        scrollL: Math.max(d.documentElement.scrollLeft, d.body.scrollLeft)
    };
}

//加入浏览器收藏夹
function AddFavorite(sURL, sTitle) {
    try {
        window.external.addFavorite(sURL, sTitle);
    }
    catch (e) {
        try {
            window.sidebar.addPanel(sTitle, sURL, "");
        }
        catch (e) {
            alert("加入收藏失败，请使用Ctrl+D进行添加");
        }
    }
}
function getIfrCntSize(iframe) {
    var bHeight = iframe.contentWindow.document.body.scrollHeight;
    var dHeight = iframe.contentWindow.document.documentElement.scrollHeight;
    var height = Math.max(bHeight, dHeight);
    var width = Math.max(iframe.contentWindow.document.body.scrollWidth, iframe.contentWindow.document.documentElement.scrollWidth);
    return { width: width, height: height };
}
function resizeIframeHeight(iframe) {
    try {
        var size = getIfrCntSize(iframe);
        iframe.style.height = size.height + "px";
        iframe.style.width = size.width + "px";
    } catch (e) { }
}
function GetDate() {
    var week;
    if (new Date().getDay() == 0) week = "周日";
    if (new Date().getDay() == 1) week = "周一";
    if (new Date().getDay() == 2) week = "周二";
    if (new Date().getDay() == 3) week = "周三";
    if (new Date().getDay() == 4) week = "周四";
    if (new Date().getDay() == 5) week = "周五";
    if (new Date().getDay() == 6) week = "周六";
    return { year: new Date().getFullYear(), month: new Date().getMonth() + 1, day: new Date().getDate(), week: week };
}
/*
功能函数:解析字符串为日期类型
将String类型解析为Date类型.不能解析返回null
2006-/1-/1 [15:14:16[.254]]
*/
function parseDate(str) {
    if (typeof str == 'string') {
        var results = str.match(/^ *(\d{4})[\-\/]{1}0?(\d{1,2})[\-\/]{1}0?(\d{1,2}) *$/);
        if (results && results.length > 3)
            return new Date(parseInt(results[1]), parseInt(results[2]) - 1, parseInt(results[3]));
        results = str.match(/^ *(\d{4})[\-\/]{1}0?(\d{1,2})[\-\/]{1}0?(\d{1,2}) +0?(\d{1,2})\:0?(\d{1,2})\:0?(\d{1,2}) *$/);
        if (results && results.length > 6)
            return new Date(parseInt(results[1]), parseInt(results[2]) - 1, parseInt(results[3]), parseInt(results[4]), parseInt(results[5]), parseInt(results[6]));
        results = str.match(/^ *(\d{4})[\-\/]{1}0?(\d{1,2})[\-\/]{1}0?(\d{1,2}) +0?(\d{1,2})\:0?(\d{1,2})\:0?(\d{1,2})\.0?(\d{1,9}) *$/);
        if (results && results.length > 7)
            return new Date(parseInt(results[1]), parseInt(results[2]) - 1, parseInt(results[3]), parseInt(results[4]), parseInt(results[5]), parseInt(results[6]), parseInt(results[7]));
    }
    return null;
}
String.prototype.Trim = function() {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}