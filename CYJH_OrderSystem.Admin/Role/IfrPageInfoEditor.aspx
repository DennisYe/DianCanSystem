<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IfrPageInfoEditor.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Role.IfrPageInfoEditor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>隐藏页设置</title>
    <link type="text/css" rel="stylesheet" href="../css/all.css" />
    <script src="../js/childuseonly.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="top-bar">
        <h1>隐藏页设置</h1>
        <div class="breadcrumbs">
            当前页面 / <a href="javascript:void(0)" title='页面地址：<%= this.NodeURL %>' class="red"><%= this.NodeName %></a>
        </div>
    </div>
    <br />
    <div class="select-bar-empty">
        
    </div>
     
    <div class="table">
         <asp:DataList runat="server" ID="dl_InChild" CellPadding="0" CellSpacing="0" DataKeyField="PID"
            CssClass="listing lefttd" RepeatColumns="4" RepeatDirection="Horizontal" 
             onitemcommand="dl_InChild_ItemCommand"  >
            <HeaderStyle CssClass="tablehead left" Width="25%" HorizontalAlign="Left" />
            <ItemStyle Height="27px" Width="25%"/>
            <HeaderTemplate>
                <strong>已选页面</strong>
            </HeaderTemplate>
            <ItemTemplate>
                <span class="floatright">
                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Remove"  Text="移除"></asp:LinkButton>
                </span> <%#Eval("PName") %>
            </ItemTemplate>
        </asp:DataList> 
        
    </div>
    
    
    <div class="table">
        <asp:DataList runat="server" ID="dl_MenuList" CellPadding="0" CellSpacing="0"  DataKeyField="PID"
            CssClass="listing lefttd" RepeatColumns="4" RepeatDirection="Horizontal" 
            onitemcommand="dl_MenuList_ItemCommand">
            <HeaderStyle CssClass="tablehead left"  Width="25%" HorizontalAlign="Left" />
            <ItemStyle Height="27px"  Width="25%" />
            <HeaderTemplate>
                <strong>待选页面</strong>
            </HeaderTemplate>
            <ItemTemplate>
                <span class="floatright">
                    <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Insert" Enabled='<%# GetEnable( Convert.ToInt32(Eval("PID")))%>' Text="加入"></asp:LinkButton>
                </span> <%#Eval("PName") %>
            </ItemTemplate>
        </asp:DataList>
    </div>
    
    
    <div class="table">
        <table class="listing form" cellpadding="0" cellspacing="0">
            <tr>
                <th class="full" colspan="2">
                    添加新隐藏页                 </th>
            </tr>
            <tr>
                <td class="first" width="172">
                    <strong>名称</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_add_pname" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr>
            <tr class="bg">
                <td class="first">
                    <strong>链接</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_add_url" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="leftFootBar">
            <div class="select center">
                <asp:LinkButton runat="server" ID="lb_save" Text="添加" onclick="lb_save_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
