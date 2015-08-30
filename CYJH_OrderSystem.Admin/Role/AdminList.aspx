<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminList.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Role.AdminList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>管理员列表</title>
    <link type="text/css" rel="stylesheet" href="../css/all.css" />

    <script src="../js/childuseonly.js" type="text/javascript"></script>

    <script src="../js/comm.js" type="text/javascript"></script>

    <script type="text/javascript">
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="top-bar">
        <h1>管理员管理</h1>
    </div>
    <br />
    <div class="select-bar">
        <label for="tb_NickName">
            昵称：<asp:TextBox runat="server" ID="tb_NickName" CssClass="text"></asp:TextBox>
        </label>
        <label>
            <asp:Button runat="server" ID="btn_search" Text="搜索" 
            onclick="btn_search_Click"></asp:Button>
        </label>
        <label>
            <asp:Button runat="server" ID="btn_all" Text="所有" onclick="btn_all_Click"></asp:Button>
        </label>
    </div>
    <div class="table">
        保留权限：用户更换组时，保留所有已设置的权限<br>
        追加权限：用户更换组时，会加入原来没有设置过的权限，不更新所有已设权限<br>
        更替权限：用户更换组时，权限相应变化<br>
        <asp:GridView runat="server" CellPadding="0" CssClass="listing" ID="gvList" DataKeyNames="AID"
            AutoGenerateColumns="False" OnRowEditing="gvList_RowEditing" OnRowCancelingEdit="gvList_RowCancelingEdit"
            OnRowDataBound="gvList_RowDataBound" OnRowUpdating="gvList_RowUpdating" 
            OnRowDeleting="gvList_RowDeleting" onrowcommand="gvList_RowCommand">
            <AlternatingRowStyle CssClass="bg" />
            <Columns>
                <asp:TemplateField HeaderText="昵称" HeaderStyle-CssClass="first" HeaderStyle-Width="177px">
                    <HeaderStyle CssClass="first" Width="87px"></HeaderStyle>
                    <ItemStyle Width="87px" HorizontalAlign="Left" CssClass="first" />
                    <EditItemTemplate>
                        <asp:TextBox ID="tb_NickName" Width="80px" CssClass="linetext" Text='<%# Eval("ANickName") %>'
                            runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox Width="80px" ID="cb_NickName" Text='<%# Eval("ANickName") %>' runat="server">
                        </asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AName" HeaderText="登录名" ReadOnly="true" />
                <asp:TemplateField HeaderText="预设组">
                    <ItemStyle Width="165px" />
                    <EditItemTemplate>
                        <asp:DropDownList Width="90px" runat="server" ID="ddl_GroupList">
                        </asp:DropDownList>
                        <asp:DropDownList Width="70px" runat="server" ID="ddl_rolearg">
                            <asp:ListItem Value="0" Selected="True">保留权限</asp:ListItem>
                            <asp:ListItem Value="1">更替权限</asp:ListItem>
                            <asp:ListItem Value="2">追加权限</asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("GName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="邮箱">
                    <ItemStyle Width="145px" />
                    <EditItemTemplate>
                        <asp:TextBox ID="tb_Email" Width="140px" CssClass="linetext" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ALastTime" HeaderText="登录时间" ReadOnly="True" DataFormatString="{0:yy-MM-dd HH:mm}" />
                <asp:BoundField DataField="IP" HeaderText="登录IP" ReadOnly="true" />
                <asp:TemplateField HeaderText="编辑">
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnupdate_Update" runat="server" CausesValidation="True" CommandName="Update"
                            CommandArgument='<%# Bind("GID") %>' Text="更新"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Cancel"
                            Text="取消"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnupdate_Update" runat="server" CausesValidation="False" CommandName="Edit"
                            Text="编辑"></asp:LinkButton>
                        <asp:LinkButton ID="btndel_admin" runat="server" CausesValidation="False" CommandName="Delete"
                            Text="删除" OnClientClick="return confirm('是否要删除？')"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="权限">
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hl" NavigateUrl='<%#"RoleManage.aspx?adminid="+ Eval("AID") %>'
                            Text="设置"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="密码">
                    <ItemTemplate>
                    <asp:LinkButton runat="server" ID="lab_resetpwd" Text="重置" CommandName="resetpwd" CommandArgument='<%# Eval("AID") %>' OnClientClick="return confirm('要重置密码？')"></asp:LinkButton>    
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="leftFootBar">
            <div class="select center">
                <asp:LinkButton runat="server" ID="btnupdate_save" Text="批量设置权限" OnClick="btn_save_Click"></asp:LinkButton>
            </div>
            <label for="checkAll">
                <input type="checkbox" id="checkAll" onclick="$CheckAll('gvList',this.checked)" />全选/不选</label>
        </div>
    </div>
    <div class="table">
        <table class="listing form" cellpadding="0" cellspacing="0">
            <tr>
                <th class="full" colspan="2">
                    添加新管理员  <a href="javascript:;" onclick='showFloat("快速添加新管理员", "Role/FastAddAdmin.aspx", "true", 400, 500, false);'>快速添加</a>
                </th>
            </tr>
            <tr>
                <td class="first" width="172">
                    <strong>登录名</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_LoginName" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr>
            <tr class="bg">
                <td class="first">
                    <strong>昵称</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_ANickName" CssClass="text linetext"></asp:TextBox>
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
                <td class="first">
                    <strong>预设组</strong>
                </td>
                <td class="last">
                    <asp:DropDownList runat="server" ID="ddl_GroupList">
                    </asp:DropDownList>
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
        </table>
        <div class="leftFootBar">
            <div class="select center">
                <asp:LinkButton runat="server" ID="btnadd_save" Text="保存" OnClick="lb_save_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
