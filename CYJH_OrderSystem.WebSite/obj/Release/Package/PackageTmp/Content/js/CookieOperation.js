function SetCookie(name, value) {
    var days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + days * 24 * 3600 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString()+";path=/";
}

function GetCookie(name) {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        if ($.trim(cookies[i].split("=")[0]) == name) {
            return unescape($.trim(cookies[i].split("=")[1]));
        }
    }
    return null;
}

function ClearCookie() {

}