<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <title>管理后台V<%=CYJH_OrderSystem.Admin.Base.AdminAppSetting.Version%></title>
    
    <script type="text/javascript">
        if (window.top != window) {
            window.top.location = window.location;
        }
    </script>

    <link href="css/thickbox.css" rel="stylesheet" type="text/css" />
    <style media="all" type="text/css">
        @import "css/all.css?v=4";
    </style>

    <script src="js/comm.js" type="text/javascript"></script>

    <script src="js/autoResizeIframe.js" type="text/javascript"></script>

    <script src="js/jquery.min.js" type="text/javascript"></script>

    <script src="js/thickbox.js" type="text/javascript"></script>

    <script type="text/javascript">
        //这里配置管理首页的ID,默认为6,默认不启用该功能
        var ManagerPageId = "";

        function showEditor() {
            showFloat("修改信息", "Role/IfrAdminEditor.aspx?editor=1", "true", 500, 300, false);
        }

        function showFloat(title, src, bgclose, width, height, modal, reloadurl, target) {
            if (src.indexOf("?") == -1) {
                src += "?";
            } else {
                src += "&";
            }
            width = width ? width : 500;
            height = height ? height : 500;
            var modalstr = modal ? "true" : "false";
            src += "keepThis=true&TB_iframe=true&height=" + height + "&width=" + width + "&modal=" + modalstr;
            var evalstr = null;

            if (reloadurl) {
                evalstr = "window.parent.reloadMainIframe('" + reloadurl + "','" + (target ? target : "main") + "')";
            }

            tb_show(title, src, bgclose, null, evalstr);
        }

        function reloadMainIframe(reloadurl, target) {

            if ((!reloadurl) || reloadurl == "") return;

            reloadurl = $ReplaceRndQueryString(reloadurl);

            var ttarget = "main";
            if (target != "") { ttarget = target; }

            var alink = document.getElementById("__maintarget");
            if (!alink) {
                alink = document.createElement("a");
                alink.id = "__maintarget";
                document.body.appendChild(alink);
            }

            alink.target = ttarget;
            alink.href = reloadurl;

            alink.click();

        }

        function ToggleRight(dom) {
            var rdom = document.getElementById("right-column");
            var cdom = document.getElementById("center-column");
            var className = "extend";
            var innerHtml = "显示"
            if (rdom.className == "extend") {
                className = "";
                innerHtml = "隐藏"
            }

            rdom.className = className;
            cdom.className = className;
            document.getElementById("mainiframe").style.width = "100%";
            dom.innerHTML = innerHtml;
        }

        var MenusCreater = function (menus, size) {
            this._menus = menus;
            this._size = size ? size : 9;
            this._index = 1;
            this._recordcount = menus.length;
            this._pagecount = Math.ceil(this._recordcount / this._size);
            this.nowact = menus[0];
            var tmp = this;
            this.getUrl = function (menuItem) {
                return (menuItem.URL ? menuItem.URL : 'Left.aspx?id=' + menuItem.Id + (menuItem.ProjectId ? '&ProjectId=' + menuItem.ProjectId : ""));
            };
            this.CreateMenuItem = function (cfg, isact) {
                var li = document.createElement("li");
                li.onclick = function () {
                    tmp.OnItemClick(cfg, this);
                }
                if (!cfg) {
                    li.className = "floatright";
                    li.innerHTML = '<span><span><a style="color:red" href="javascript:void(0)">切换菜单</a></span></span>';
                } else if (cfg.Id == -1) {
                    li.className = "floatright";
                    li.innerHTML = '<span><span><em>当前用户：<b class="pointer" onclick="showEditor()"><%=this.AdminUser%></b><font  onclick="showEditor()" class="changepwd">(修改密码)</font></em><a target="_self" class="logout" href="Logout.aspx">点击退出</a></span></span>';
                } else {
                    if (isact) li.className = "active";
                    li.innerHTML = '<span><span><a target="leftmenu" href="' + this.getUrl(cfg) + '">' + cfg.Text + '</a></span></span>';
                }
                return li;
            };

            this.CreaterMenu = function () {
                var begin = (this._index - 1) * this._size;
                var end = Math.min(this._index * this._size, this._recordcount) - 1;
                var mbar = document.getElementById('top-navigation');
                mbar.innerHTML = '';
                mbar.appendChild(this.CreateMenuItem());
                mbar.appendChild(this.CreateMenuItem({ Id: -1 }));
                for (var tmp = begin; tmp <= end; tmp++) {
                    if (!this._menus[tmp].Text) continue;
                    if (this.nowact == this._menus[tmp])
                        mbar.appendChild(this.CreateMenuItem(this._menus[tmp], true));
                    else
                        mbar.appendChild(this.CreateMenuItem(this._menus[tmp]));
                }
                var li = document.createElement("li");
                li.className = "clear";
                mbar.appendChild(li);
            };
            this.OnItemClick = function (cfg, li) {
                if (!cfg)
                    this._index++;
                else
                    this.nowact = cfg;

                if (this._pagecount < this._index) {
                    this._index = 1;
                }
                try {
                    if (cfg.Id > 0) {
                        li.getElementsByTagName("a")[0].click();
                    }
                } catch (e) { }

                this.CreaterMenu();

            };

            this.CreaterMenu();
            return false;
        }

        function resetmiddle(height) {

        }


        function mainiframeonload(ifr) {
            resizeIframe(ifr, 600, function (h) { resetmiddle(h) })
        }

        function LoadDefaultMenu() {
            if (nowact) {
                $("#leftiframe").attr("src", getUrl(nowact));
            }
        }
        function ReloadMainFrameSize() {
            mainiframeonload(document.getElementById("mainiframe"));
        }
        function ReloadLeftFrameSize() {
            mainiframeonload(document.getElementById("leftiframe"));
        }

        var isFirstLoad = true;
        function loadMainDefaultPage(ifr) {
            if (isFirstLoad) {
                if (!ifr.contentWindow.$) setTimeout(function () { loadMainDefaultPage(ifr) }, 100);
                else {
                    isFirstLoad = false;
                    //如果有快速登录界面,则直接加载
                    ifr.contentWindow.$("a[url^='FastJump.aspx']:first").click().each(function () { eval(this.target + '.location="' + this.href + '";') });
                }
            }
        }

        //检测环境
        var dTitle = document.title;
        function chkEnvironment() {
            $.ajax({
                type: "HEAD"
                , url: "TestPage.html?rnd=" + new Date().getTime()
                , cache: false
                , complete: function (XMLHttpRequest, textStatus) {
                    var en = XMLHttpRequest.getResponseHeader("Environment");
                    var enStr = "";
                    switch (en) {
                        case "Tester":
                            enStr = "【准生产】";
                            break;
                        case "Local":
                            enStr = "【本地】";
                            break;
                        case "Online":
                            enStr = "【线上】";
                            break;
                        default:
                            enStr = "";
                            break;
                    }
                    document.title = enStr + dTitle;
                    setTimeout(chkEnvironment, 3000);
                }
            });
        }
        chkEnvironment();
    </script>

</head>
<body class="mainbody">
    <form id="form1" runat="server">
    <div id="main">
        <div id="header">
            <a href="Default.aspx" class="logo">
                <img src="Images/logo.gif" width="101" height="29" alt="" /></a>
            <ul id="top-navigation">
            </ul>
            <script type="text/javascript">
                var Menus = <%= this.Menus  %>;
                for(var i=0;i<Menus.length;i++){
                    if(Menus[i].Id==ManagerPageId){
                        Menus.splice(i,1);
                        i--;
                    }
                }
                MenusCreater(Menus);
            </script>

        </div>
        <div id="middle">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td width="200px" valign="top" align="left">
                    <iframe name="leftmenu" width="100%" id="leftiframe" scrolling="no"
                        height="630px" frameborder="0" onload="loadMainDefaultPage(this);mainiframeonload(this)"></iframe>
                        <script type="text/javascript">
                            LoadDefaultMenu(Menus);
                        </script>
                </td>
                <td valign="top" align="left">
                <div style="padding:10px;">
                <iframe name="main" width="100%" id="mainiframe" src="Welcome.html" scrolling="no" frameborder="0" onload="mainiframeonload(this)"></iframe>
                </div>
                </td>
            </tr>
        </table>
        </div>
        <div id="footer">
        </div>
    </div>
    </form>
    <script type="text/javascript">
        window.middlehight = document.getElementById("middle").offsetHeight;
        window.headerhight = document.getElementById("header").offsetHeight;
    </script>
</body>
</html>


