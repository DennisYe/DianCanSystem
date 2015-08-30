var ValueChecked = function(uncheckdimidarr, uncheckinput, uncheckselect, unchecktextarea) {
    this.whenload = [];
    this.nowload = [];
    var randowidpre = 1;
    uncheckinput = uncheckinput ? uncheckinput : false;
    uncheckselect = uncheckselect ? uncheckselect : false;
    unchecktextarea = unchecktextarea ? unchecktextarea : false;


    this.GetCKValue = function() {
        function gval(tagname, notinid, val) {
            var tmpstr = "";
            var tdoms = document.getElementsByTagName(tagname);
            for (var j = tdoms.length - 1; j >= 0; j--) {
                var ttype = tdoms[j].type;
                var ttid = tdoms[j].id;
                if (ttid == "") {
                    ttid = randowidpre + "" + Math.random() + "";
                    ttid = ttid.replace(".", "_");
                    tdoms[j].id = ttid;
                    randowidpre++;
                }
                if (!notinid[ttid]) {
                    if (ttype == "radio" || ttype == "checkbox") {
                        val[ttid] = tdoms[j].checked ? "选择" : "不选";
                    } else {
                        val[ttid] = tdoms[j].value;
                    }
                }
                tmpstr += val[ttid];
            }
            return tmpstr;
        }

        var tmparr = [];
        tmparr["__VIEWSTATE"] = true;
        tmparr["__EVENTARGUMENT"] = true;
        tmparr["__EVENTTARGET"] = true;
        tmparr["__EVENTVALIDATION"] = true;
        if (uncheckdimidarr) {
            if (typeof uncheckdimidarr == "string") {
                tmparr[uncheckdimidarr] = true;
            } else if (uncheckdimidarr.length > 0) {
                for (var len = uncheckdimidarr.length - 1; len >= 0; len--) {
                    tmparr[uncheckdimidarr[len]] = true;
                }
            } else {
                for (var tmpv in uncheckdimidarr) {
                    tmparr[uncheckdimidarr[tmpv]] = true;
                }
            }
        }


        var tmpstr = "";
        var vallist = [];
        if (!uncheckinput) {
            tmpstr += gval("input", tmparr, vallist);
        }

        if (!uncheckselect) {
            tmpstr += gval("select", tmparr, vallist);
        }

        if (!unchecktextarea) {
            tmpstr += gval("textarea", tmparr, vallist);
        }
        tmparr = [];

        vallist["___valuestr"] = tmpstr;
        return vallist;
    }
}

ValueChecked.prototype.CheckWhenUnLoad = function() {
    this.nowload = this.GetCKValue();
    return (this.nowload["___valuestr"] == this.whenload["___valuestr"]);
}

ValueChecked.prototype.GetInitValue = function(id) {
    return this.whenload[id];
}

ValueChecked.prototype.GetChangedIdAndValue = function() {
    var tmp = [];
    tmp["updateCount"] = 0;
    if (this.nowload == []) {
        this.nowload = this.GetCKValue();
    }

    for (var i in this.nowload) {
        if (i && i != "___valuestr") {
            if (this.nowload[i] != this.whenload[i]) {
                tmp[i] = { id: i, o: this.whenload[i], n: this.nowload[i] };
                tmp["updateCount"]++;
            }
        }
    }
    return tmp;
}

ValueChecked.prototype.FillWhenLoad = function() {
    this.whenload = this.GetCKValue();
}
 