<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleManage.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Role.RoleManage" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>权限管理</title>
    <link type="text/css" rel="stylesheet" href="../css/all.css" />
    <script src="../js/childuseonly.js" type="text/javascript"></script>
    <script src="../js/comm.js" type="text/javascript"></script>
    <script src="../js/ValueChecked.js" type="text/javascript"></script>
    <script type="text/javascript">
        var valueChecked = new ValueChecked(['checkAll']);

        function showAdd() {
            if (checkUpdate()) {
                showFloat("添加站点", "role/IfrAdminEditor.aspx", true, 900, 500, false, location.href);
            }
        }
        function savenow() {
            valueChecked.FillWhenLoad();
            
        }
        function checkUpdate() {
            if (!valueChecked.CheckWhenUnLoad()) {
                    var res = confirm("当前内容有修改，离开将不会保存，点击是离开并不保存，如果要保存，请点击否!");
                if (res) {
                    return true;
                } else {
                    var changed = valueChecked.GetChangedIdAndValue();
                    var str = "";
                    for (var i in changed) {
                        if (changed[i].id) {
                            str += "id 为 “" + changed[i].id + "”， 旧值为 “" + changed[i].o + "”，新值为 “" + changed[i].n + '”\r\n';
                        }
                    }
                    alert(str);
                    return false;
                }

            }
            return true;
        }
    </script>

</head>
<body onload="savenow()">
    <form id="form1" runat="server">
        <div class="top-bar">
           <!-- <a href="javascript:void(0)" onclick="showAdd()" class="button">新用户</a>
            <a href="javascript:void(0)" onclick="showAdd()" class="button">新预设组</a>-->
            <h1>权限分配</h1>
            <div class="breadcrumbs">
                <a href="#">Homepage</a> / <a href="#">Contents</a></div>
        </div>
        <br />
        <div class="select-bar">
           
            
            <label for="ddl_Root">
                主栏目
                <asp:DropDownList ID="ddl_Root" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddl_Root_SelectedIndexChanged">
                    <asp:ListItem Value="1">用户管理</asp:ListItem>
                </asp:DropDownList>
            </label>
            <label for="ddl_Type">
                类型
                <asp:DropDownList ID="ddl_Type" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddl_Type_SelectedIndexChanged">
                    <asp:ListItem Value="0">请选择</asp:ListItem>
                    <asp:ListItem Value="1">用户组</asp:ListItem>
                    <asp:ListItem Value="2">用户</asp:ListItem>
                </asp:DropDownList>
            </label>
            <label for="ddl_For">
                对象
                <asp:DropDownList ID="ddl_For" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddl_For_SelectedIndexChanged">
                    <asp:ListItem Value="0">请选择</asp:ListItem>
                </asp:DropDownList>
            </label>
            <label >
                <asp:LinkButton runat="server" ID="lb_reload" onclick="lb_reload_Click">刷新对象</asp:LinkButton>
            </label>
        </div>
        <div class="table">
            
            <asp:GridView runat="server" CellPadding="0" CssClass="listing" ID="gvInRole" 
                AutoGenerateColumns="False" DataKeyNames="PID" 
                onrowdeleting="gvInRole_RowDeleting" onrowcommand="gvInRole_RowCommand">
                <AlternatingRowStyle CssClass="bg" />
               
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="first" HeaderStyle-Width="177px">
                        <HeaderStyle CssClass="first" Width="177px"></HeaderStyle>
                        <ItemStyle Width="177px" HorizontalAlign="Left" CssClass="first" />
                        <HeaderTemplate>描述 &nbsp;-&nbsp;
                           <asp:LinkButton runat="server" ID="lb_ChildName"  CommandArgument='0' CommandName="ChangeRoot">根目录</asp:LinkButton> </HeaderTemplate>
                        <ItemTemplate>
                            <strong><%# this.NowSelectRootName %></strong>
                            &nbsp;-&nbsp;
                           <asp:LinkButton runat="server" ID="lb_ChildName"  CommandArgument='<%# Eval("PId") %>' CommandName="ChangeRoot"><%# Eval("PName") %></asp:LinkButton> 
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="增加">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="r_1" Enabled='<%#Eval("IsUrl") %>' Checked='<%# Bind("RoleAdd") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="删除">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="r_2" Enabled='<%#Eval("IsUrl") %>' Checked='<%# Bind("RoleDelete") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="修改">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="r_3" Enabled='<%#Eval("IsUrl") %>' Checked='<%# Bind("RoleUpdate") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="查询">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="r_4"  Enabled='<%#Eval("IsUrl") %>' Checked='<%# Bind("RoleSelect") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="特殊1">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="r_5" Enabled='<%#Eval("IsUrl") %>' Checked='<%# Bind("RoleSpec1") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="特殊2">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="r_6" Enabled='<%#Eval("IsUrl") %>' Checked='<%# Bind("RoleSpec2") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="特殊3">
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="r_7" Enabled='<%#Eval("IsUrl") %>' Checked='<%# Bind("RoleSpec3") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="移除" ShowHeader="False">
                        <ItemStyle CssClass="last" />
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClientClick="return confirm('确定要移除？')"
                                CommandName="Delete" Text="移除"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView> 
            <div class="leftFootBar" >
                <div class="select center">
                    <asp:LinkButton  runat="server" ID="btn_save" Text="保存权限" OnClientClick="return confirm('保存后，将会覆盖原来的权限，是否保存？')"
                        onclick="btn_save_Click"></asp:LinkButton>
                </div> 
                <label for="checkAll"><input type="checkbox" id="checkAll" onclick="$CheckAll('gvInRole',this.checked)" />全选/不选</label>
            </div>
        </div>
        
        <div class="table">
            <asp:DataList runat="server" ID="dl_MenuList" CellPadding="0" CellSpacing="0"  DataKeyField="PID"
                CssClass="listing lefttd" RepeatColumns="4" RepeatDirection="Horizontal" 
                onitemcommand="dl_MenuList_ItemCommand">
                <HeaderStyle CssClass="tablehead left"  HorizontalAlign="Left" Width="25%" />
                <ItemStyle Height="27px" Width="25%"/>
                <HeaderTemplate>
                    <strong>待选项目</strong>
                </HeaderTemplate>
                <ItemTemplate>
                    <span class="floatright">
                        <asp:LinkButton runat="server" CommandName="Insert" CommandArgument='<%#Eval("BtnRightExp") %>' Enabled='<%# GetEnable( Convert.ToInt32(Eval("PID")))%>' Text="加入"></asp:LinkButton>
                    </span> <%#Eval("PName") %>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </form>
</body>
</html>
