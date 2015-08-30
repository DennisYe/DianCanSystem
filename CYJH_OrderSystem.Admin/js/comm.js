if (window.top.location.href.toLowerCase().indexOf("main.aspx") == -1) {
    window.top.location.href = "/main.aspx";
}


/* JS Cache  */
var $PageCache = function(key, value) {
    var topw = window.top;
    if (!topw.__hidcache) {
        topw.__hidcache = [];
    }
    var arglength = arguments.length;
    if (arglength == 2) {
        topw.__hidcache[key] = value;
    } else if (arglength == 1) {
        return topw.__hidcache[key];
    } else {
        alert("argments err");
    }
}


/* URL */
var $ReplaceRndQueryString = function(url) {
    if (url.indexOf("rnd=") > 0) {
        var reg = /rnd=([^&?]+)/ig;
        url = url.replace(reg, "rnd=" + Math.random());
    } else {
        if(url.indexOf("?")>=0)
            url += "&rnd=" + Math.random();
        else
            url += "?rnd=" + Math.random();
    }
    return url;
}
 
  
/* RegExp */
var $IsInt = function(input) {
    return $IsMatch("^[0-9]+$", input);
}

var $IsMatch = function(reg, input) {
    var regobj = new RegExp(reg);
    return regobj.test(input);
}

/* GetElement */
var $Id = function(idexp) {
    if (typeof idexp == "string")
        return document.getElementById(idexp);
    return idexp;
}
/* GetValue */
var $Val = function(idexp) {
    var dom = $Id(idexp);
    if (dom)
        return dom.value;
    return null;
}

var $Hide = function(idexp) {
    var dom = $Id(idexp);
    if (dom)
        dom.style.display = 'none';
}

var $Show = function(idexp) {
    var dom = $Id(idexp);
    if (dom)
        dom.style.display = 'block';
}

/* CheckBox */
var $CheckAll = function(dom, checked) {
    dom = $Id(dom);
    if (!dom) return;
    var inputs = dom.getElementsByTagName("input");
    for (var i = inputs.length - 1; i >= 0; i--) {
        if (inputs[i].type == "checkbox") inputs[i].checked = checked;
    }
}

/* extend */
var $SimpleExtend = function(defaultobj, newobj) {
    var ret = defaultobj;
    for (var tmp in newobj) {
        ret[tmp] = newobj[tmp];
    }
    return ret;
}

/* TableHightLight */
var $OverLight = function(obj, beginIndex, endIndex) {
    function RowBind(row, index) {
        row.onmouseover = function() {
           this.className += " light";
        }; 
        row.onmouseout = function() {
            this.className = this.className.replace(" light", "");
        }
    }

    var objitem = $Id(obj);
    if (!objitem)
        return;
        
    objitemChild = objitem.getElementsByTagName("tr");
    var childCount = objitemChild.length;
    if (!beginIndex)
        beginIndex = 0;
    else if (beginIndex >= childCount)
        return;
    if (!endIndex)
        endIndex = childCount;
    else if (endIndex <= 0)
        endIndex = childCount + endIndex;

    if (beginIndex > endIndex)
        return;

    for (var tmp = beginIndex; tmp < endIndex; tmp++ ) {
        RowBind(objitemChild[tmp], tmp);
    }
}

/* addEvent */
$addEvent = function(obj, type, fn) {
    var obj = $Id(obj);
    if (!obj)
        return;
    if ( obj.attachEvent ) { 
        obj['e' + type + fn ] = fn; 
        obj[ type + fn ] = function(){ obj[ 'e' + type + fn ]( window.event );} 
        obj.attachEvent('on'+ type , obj[type+fn]); 
   } else 
     obj.addEventListener( type, fn, false );
}

$removeEvent = function(obj, type, fn) {
    var obj = $Id(obj);
    if (!obj)
        return;
    if (obj.detachEvent) {
        obj.detachEvent('on' + type, obj[type + fn]);
        obj[type + fn] = null;
    } else
        obj.removeEventListener(type, fn, false);
}
