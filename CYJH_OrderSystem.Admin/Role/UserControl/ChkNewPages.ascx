<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChkNewPages.ascx.cs" Inherits="CYJH_OrderSystem.Admin.Role.UserControl.ChkNewPages" %>
<%@ Import Namespace="Shared" %>

<%foreach (var ckNewPage in CheckNewPagesItems) {%>
<fieldset>
    <legend><%=ckNewPage.Name %> <a href="javascript:;" class="LnkFoldCkNewTg" onclick="FoldCkNewPageInfoDiv(this,'折叠','展开')">展开</a></legend>
    <a class="CkNewPageHideTxt" href="javascript:;" onclick="FoldCkNewPageInfoDiv($(this).parent().find('.LnkFoldCkNewTg')[0],'折叠','展开')">
    [...]
    </a>
    <%var pages = ckNewPage.Impl.CheckNewMgrPages(); %>
    <%if (pages.Count > 0) { %>
    <%foreach (var page in pages) {%>
    <div class="CkNewPageInfo">
        <input type="hidden" name="PName" value="<%=page.PName.HtmlEncode() %>" />
        <input type="hidden" name="PUrl" value="<%=page.PUrl.HtmlEncode() %>" />
        <%=page.PName.IsEmpty() ? "<无页面名>" : page.PName%>[<%=page.PUrl.IsEmpty() ? "<无URL>" : page.PUrl%>]
        <a href="javascript:;" onclick="return MoveCkNewPageToAdd(this)">添加</a>
    </div>
    <%}%>
    <%} else {%>
    &lt;未检测到新页面&gt;
    <%} %>
</fieldset>
<%}%>