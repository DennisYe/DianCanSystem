function resizeIframe(ifr, minheight, callback) {
    //ifr.style.height = minheight + "px";
    var bHeight = ifr.contentWindow.document.body.scrollHeight || minheight;
    if (window.debug) alert("1:" + bHeight);
    var dHeight = ifr.contentWindow.document.documentElement.scrollHeight || minheight;
    if (window.debug) alert("2:" + dHeight);
    var height = Math.min(bHeight, dHeight);
    if (window.debug) alert("3:" + height);
    if (height < minheight) { height = minheight };
    if (window.debug) alert("4:" + height);
    ifr.style.height = height + "px";
    if (callback) {
        callback(height);
    }
}