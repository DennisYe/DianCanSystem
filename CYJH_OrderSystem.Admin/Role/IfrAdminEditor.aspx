<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IfrAdminEditor.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Role.IfrAdminEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <title>管理员信息编辑</title>
    <link type="text/css" rel="stylesheet" href="../css/all.css" />
</head>
<body>
    <form id="form1" runat="server">
      <div class="top-bar">
        <h1>修改信息</h1>
    </div>
      <br />
    <div class="select-bar-empty">
        
    </div>
    <div class="table">
        <table class="listing form" cellpadding="0" cellspacing="0">
            <tr>
                <th class="full" colspan="2">
                    基本信息
                </th>
            </tr>
            <tr>
                <td class="first">
                    <strong>昵称</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_NickName" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr>
           
            <tr class="bg">
                <td class="first"">
                    <strong>邮箱</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_Email" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr> 
            <tr>
                <td class="first">
                    <strong>登录密码</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_LoginPwd1" TextMode="Password" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr>
            <tr class="bg">
                <td class="first"">
                    <strong>确认密码</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_LoginPwd2" TextMode="Password" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="first"">
                    <strong>旧密码</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_OldPwd" TextMode="Password" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="leftFootBar">
            <div class="select center">
                <asp:LinkButton runat="server" ID="btn_save" Text="保存" 
                    onclick="btn_save_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
