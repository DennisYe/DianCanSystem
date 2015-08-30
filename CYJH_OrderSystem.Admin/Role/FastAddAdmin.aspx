<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FastAddAdmin.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Role.FastAddAdmin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>快速添加管理员</title>
    <style type="text/css">
    body{font-size:10pt;}
    </style>
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ChkAddAdmin() {
            var count = $(":checkbox[name$=cb_AName]:checked").length;
            if (count <= 0) {
                alert('请先选择要添加的管理员');return false;
            }
            return confirm('确认添加所选的' + $(":checkbox[name$=cb_AName]:checked").length + '项?');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <strong>↓请选勾选管理员,拉到页面底部添加↓</strong>
    </div>
    
    <div>
        <asp:GridView ID="adminList" runat="server" AutoGenerateColumns="False" 
            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
            CellPadding="4" ForeColor="Black" GridLines="Vertical">
            <RowStyle BackColor="#F7F7DE" />
        <Columns>
            <asp:TemplateField HeaderText="账号">
                <ItemTemplate>
                    <asp:CheckBox Text='<%# Eval("AName") %>' runat="server" ID="cb_AName" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="昵称">
                <ItemTemplate>
                    <asp:Label ID="lab_ANickName" Text='<%# Eval("ANickName") %>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="邮箱">
                <ItemTemplate>
                    <asp:Label ID="lab_Email" Text='<%# Eval("Email") %>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
            <FooterStyle BackColor="#CCCC99" />
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
    
    <div>
        <asp:Button ID="btnadd_AddUser" runat="server" Text="添加所选到预设组" 
            OnClientClick="return ChkAddAdmin()" onclick="Button1_Click" />=&gt;
    <asp:DropDownList runat="server" ID="ddl_GroupList">
    </asp:DropDownList>
    </div>
    </form>
</body>
</html>
