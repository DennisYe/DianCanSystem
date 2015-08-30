<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoRule.aspx.cs" Inherits="CYJH_OrderSystem.Admin.NoRule" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
<%
    string refUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : "";
    
 %>
    对不起,您没有访问本页面的权限!
    <%
        if (!string.IsNullOrEmpty(refUrl)) {
%>
    <a href="<%=refUrl %>">返回</a>
<%
        }
         %>
</body>
</html>
