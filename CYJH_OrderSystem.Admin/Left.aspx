<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
      <title></title>
    <style media="all" type="text/css">
        @import "css/all.css?v2";
    </style>
    
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function initLink() {
            var alist = document.getElementsByTagName("a");
            for (var i = alist.length - 1; i >= 0; i--) {
                var tmpa = alist[i];
                if (tmpa.target == "main") {
                    var cp = $(tmpa).next(".childItem");
                    if (cp.length > 0) {
                        checkShowNav($(tmpa), cp);
                    }
                    tmpa.onclick = function () {
                        return clickLink(this);
                    }
                }
            }
        }
        var nowlink;
        function clickLink(a) {
            $(a).removeClass("hover").addClass("hover");
            if (nowlink) {
                try {
                    $(nowlink).removeClass("hover");
                } catch (e) { }
            }

            if (a.href.indexOf("rnd=") > 0)
                a.href = a.href.substr(0, a.href.indexOf("rnd=") - 1);
            if (a.href.indexOf("?") > 0)
                a.href += "&rnd=" + Math.random();
            else
                a.href += "?rnd=" + Math.random();

            nowlink = a;
            return navClick(a);
        }
        function navClick(nav) {
            nav = $(nav);
            var cp = nav.next(".childItem");
            var re = false;
            if (cp.length > 0) {
                cp.toggle();
                checkShowNav(nav, cp);
            } else {
                if (nav.attr("url")) re = true;
            }
            parent.ReloadLeftFrameSize();
            return re;
        }
        function checkShowNav(nav, childPanel) {
            nav.removeClass("hasChildsJia").removeClass("hasChildsJian");
            if (childPanel.is(":hidden")) {
                nav.addClass("hasChildsJia");
            } else {
                nav.addClass("hasChildsJian");
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3><%= this.MenuTitle%></h3>
        <asp:Repeater ID="rep_Nav" runat="server" onitemdatabound="rep_Nav_ItemDataBound">
        <HeaderTemplate>
        <ul class="nav">
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href='<%#AppCurrQueryString(Eval("PUrl")) %>' url="<%#Eval("PUrl") %>" target="main" class="<%#HasChilds(Eval("Childs"))?"hasChildsJia":"" %>"><%#Eval("PName")%></a>
            <div id="childItem" runat="server" class="childItem"></div>
            </li>
        </ItemTemplate>
        <FooterTemplate>
        </ul>
        </FooterTemplate>
        </asp:Repeater>
    </div>
    </form>
    <script type="text/javascript">
        initLink();
    </script>
</body>
</html>