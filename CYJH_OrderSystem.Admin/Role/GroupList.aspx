<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GroupList.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Role.GroupList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>用户组列表</title>
    <link type="text/css" rel="stylesheet" href="../css/all.css" />
    <script src="../js/childuseonly.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script type="text/javascript">
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="top-bar">
        <h1>
            用户组管理</h1>
    </div>
    <br />
    <div class="select-bar-empty"></div>
     
    <div class="table">
        <asp:GridView runat="server" CellPadding="0" CssClass="listing" ID="gvList" DataKeyNames="GID" OnRowEditing="gvList_RowEditing" 
            OnRowCancelingEdit="gvList_RowCancelingEdit"  onrowupdating="gvList_RowUpdating"   onrowdeleting="gvList_RowDeleting" AutoGenerateColumns="False">
            <AlternatingRowStyle CssClass="bg" />
            <Columns>
                <asp:TemplateField HeaderText="编号" HeaderStyle-CssClass="first" HeaderStyle-Width="177px">
                    <HeaderStyle CssClass="first" Width="87px"></HeaderStyle>
                    <ItemStyle Width="87px" HorizontalAlign="Left" CssClass="first" />
                    
                    <ItemTemplate>
                        <asp:CheckBox Width="80px" ID="cb_GID" Text='<%# Eval("GID") %>' runat="server">
                        </asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="组名">
                    <ItemStyle CssClass="left" />
                    <EditItemTemplate>
                        <asp:TextBox runat="server"  CssClass="linetext" ID="tb_Gname_Editor" Text='<%# Eval("GName") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("GName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 
                <asp:TemplateField ShowHeader="False" HeaderText="调整">
                    <ItemStyle Width="70px" />
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnupdate_Update" runat="server" CausesValidation="True" CommandName="Update" CommandArgument='<%# Bind("GID") %>' Text="更新"></asp:LinkButton>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Cancel" Text="取消"></asp:LinkButton>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnupdate_Update" runat="server" CausesValidation="False" CommandName="Edit" Text="编辑"></asp:LinkButton>
                        <asp:LinkButton ID="btndel_admin" runat="server" CausesValidation="False" CommandName="Delete" Text="删除" OnClientClick="return confirm('是否要删除？')" ></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="权限">
                    <ItemStyle Width="40px" />
                    <ItemTemplate>
                        <asp:HyperLink runat="server" ID="hl" NavigateUrl='<%#"RoleManage.aspx?groupid="+ Eval("GID") %>' Text="设置" ></asp:HyperLink>
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
                                        添加新组
                </th>
            </tr>
            <tr>
                <td class="first" width="172">
                    <strong>组名</strong>
                </td>
                <td class="last">
                    <asp:TextBox runat="server" ID="tb_GroupName" CssClass="text linetext"></asp:TextBox>
                </td>
            </tr>
 
        </table>
        <div class="leftFootBar">
            <div class="select center">
                <asp:LinkButton runat="server" ID="btnadd_save" Text="保存" onclick="lb_save_Click"></asp:LinkButton>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
