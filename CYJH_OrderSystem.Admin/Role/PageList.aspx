<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageList.aspx.cs" Inherits="CYJH_OrderSystem.Admin.Role.PageList" EnableViewState="false" EnableEventValidation="false" %>
<%@ Import Namespace="Shared" %>
<%@ Register Src="~/Role/UserControl/ChkNewPages.ascx" TagPrefix="uc1" TagName="ChkNewPages" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>页面管理</title>
    <link href="../Style/RoleV1/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" EnableViewState="false">
<select runat="server" id="List_AllPages" style="display:none;"></select>
    <div class="Wrap">
        <h3>页面管理</h3>
        
        <div class="Tools">
        </div>
        <div id="DivAllPageTreeP">
            <ul class="Pages" id="DivAllPagePanel">
            <li class='PageItem PageTypeShow'>
                <div class="PageTitle" dataPID='0'>
                    <ins class="PageItemIcon"></ins>
                    <span>根</span>
                    <span>/</span>
                    <span class="ProcRight">
                        <button class="BtnMini BtnAddChildPage" onclick="return ShowAddChildPage(this)">添加子页面</button>
                    </span>
                </div>
                <ul id="DivAllPages">
                    <asp:Repeater ID="rp_PageItem" runat="server" onitemdatabound="rp_PageItem_ItemDataBound">
                    <ItemTemplate>
                    <li class="PageItem PageType<%#Eval("HideParentID").ToString()!="0"?"Hide":"Show" %>">
                        <div class="PageTitle" id="PageTitle" runat="server"
                            dataPID='<%#Eval("PID") %>'
                            dataPName='<%#Eval("PName") %>'
                            dataPUrl='<%#Eval("PUrl") %>'
                            dataQueue='<%#Eval("Queue") %>'
                            dataDefShowChild='<%#Eval("DefShowChild") %>'
                            dataParentID='<%#Eval("ParentID") %>'
                            dataHideParentID='<%#Eval("HideParentID") %>'
                            >
                            <ins class="PageItemIcon"></ins>
                            <span><%#Eval("PName")%></span>
                            <span><%#GetPagURL(Eval("PUrl"),"[{0}]")%></span>
                            <span class="ProcRight">
                                <button class="BtnMini BtnEdit BtnByAll" onclick="return ShowEditPage(this)">编辑</button>
                                <button class="BtnMini BtnDelete BtnByAll" onclick="return DeletePage(this)">删除</button>
                                <button class="BtnMini BtnAdd BtnByShowPage" onclick="return ShowAddChildPage(this)">添加子页</button>
                                <button class="BtnMini BtnLink BtnByShowPage" onclick="return ShowAddHidePage(this)">关联隐藏页</button>
                                <button class="BtnMini BtnImport BtnByAll" onclick="return showImportPage(this)">导入</button>
                                <button class="BtnMini BtnExport BtnByAll" onclick="return showExportPage(this)">导出</button>
                            </span>
                        </div>
                        <ul id="DivChilds" class="PageChilds" runat="server"></ul>
                    </li>
                    </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </li>
        </ul>
        </div>
    </div>
    <div id="DivHidePages">
    <asp:Repeater ID="rep_HidePages" runat="server">
    <ItemTemplate>
    <div
        dataPID='<%#Eval("PID") %>'
        dataPName='<%#Eval("PName") %>'
        dataPUrl='<%#Eval("PUrl") %>'
        dataParentID='-1'
        ></div>
    </ItemTemplate>
    </asp:Repeater>
    </div>
    </form>
    

    <div class="FloatEmptyDiv" id="DivLoading">
        <div id="loaderImage" class="Loading"></div>
<script type="text/javascript">
    (function () {
        var cSpeed = 9;
        var cWidth = 50;
        var cHeight = 50;
        var cTotalFrames = 18;
        var cFrameWidth = 50;

        var cImageTimeout = false;
        var cIndex = 0;
        var cXpos = 0;
        var cPreloaderTimeout = false;
        var SECONDS_BETWEEN_FRAMES = 0;

        function startAnimation() {

            //FPS = Math.round(100/(maxSpeed+2-speed));
            FPS = Math.round(100 / cSpeed);
            SECONDS_BETWEEN_FRAMES = 1 / FPS;

            cPreloaderTimeout = setTimeout(continueAnimation, SECONDS_BETWEEN_FRAMES / 1000);

        }

        function continueAnimation() {

            cXpos += cFrameWidth;
            //increase the index so we know which frame of our animation we are currently on
            cIndex += 1;

            //if our cIndex is higher than our total number of frames, we're at the end and should restart
            if (cIndex >= cTotalFrames) {
                cXpos = 0;
                cIndex = 0;
            }

            if (document.getElementById('loaderImage'))
                document.getElementById('loaderImage').style.backgroundPosition = (-cXpos) + 'px 0';

            cPreloaderTimeout = setTimeout(continueAnimation, SECONDS_BETWEEN_FRAMES * 1000);
        }

        function stopAnimation() {//stops animation
            clearTimeout(cPreloaderTimeout);
            cPreloaderTimeout = false;
        }

        //The following code starts the animation
        startAnimation();
    })();
</script>
    </div>
    
    <div class="FloatDiv" id="DivEditPage">
        <div class="FloatTitle">
            <span>修改页面</span>
            <a href="javascript:;" class="FloatClose">关闭</a>
        </div>
        <div class="FloatCnt">
            <form>
            <input name="PID" style="display:none;" />
            所属父级：<select name="ParentID"><option value="0">根节点</option></select><br />
            <div class="ShowHidePageInfo">[隐藏页面]</div>
            页面名称：<input class="Inp" name="PName" /><br />
            页面地址：<input class="Inp" name="PUrl" /><br />
            页面顺序：<input class="Inp" name="Queue" /><br />
            默认展开：<input type="checkbox" name="DefShowChild" /><br />
            <a href="javascript:;" onclick="return EditPage()">保存</a><br />
            </form>
        </div>
    </div>

    <div class="FloatDiv" id="DivAddChildPage" style="width:500px;">
        <div class="FloatTitle">
            <span>添加子页面</span>
            <a href="javascript:;" class="FloatClose">关闭</a>
        </div>
        <div class="FloatCnt">
        <div>
            <ul class="TabNames Clear" id="UlAddCntTabs">
                <li class="Onthis"><a href="#" id="LnkAddPageFrom">手动输入</a></li>
                <li><a href="#">检测新页面</a></li>
                <li><a href="#">导入页面配置</a></li>
                <li class="Clear"></li>
            </ul>
            <div id="DivAddCnts">
            <div class="TabCnt">
                <!--手动输入添加子页面-->
                <form id="FormAddPage">
                <div id="DivAddPageFormInpParentID">
                所属父级：<select name="ParentID"><option class="fixed" value="0">根节点</option></select>
                </div>
                <div id="DivAddPageSelType">
                节点类型：
                <label><input type="radio" name="PageType" value="ShowPage" hideinp="#DivAddPageFormInpDefShowChild" />普通页面</label>
                <label><input type="radio" name="PageType" value="HidePage" hideinp="#DivAddPageFormInpDefShowChild,#DivAddPageFormInpQueue" />隐藏页面</label>
                </div>
                <div id="DivAddPageFormInpPName">
                页面名称：<input class="Inp" name="PName" />
                </div>
                <div id="DivAddPageFormInpPUrl">
                页面地址：<input class="Inp" name="PUrl" />
                </div>
                <div id="DivAddPageFormInpQueue">
                显示顺序：<input class="Inp" name="Queue" />
                </div>
                <div id="DivAddPageFormInpDefShowChild">
                默认展开:<input type="checkbox" name="DefShowChild" value="True" />
                </div>
                <a href="javascript:;" onclick="return AddChildPage()">添加</a><br />
                </form>
            </div>
            <div class="TabCnt">
                <!--检测新页面-->
                <div class="FloatScrollCnt" id="DivCkNewPages">
                    <uc1:ChkNewPages runat="server" id="ChkNewPages" />
                </div>
                <div>
                    <a href="javascript:;" onclick="return reflushChkNewPages()">刷新</a>
                </div>
            </div>
            <div class="TabCnt">
                <!--导入页面配置-->
                &lt;未实现&gt;
            </div>
            </div>
        </div>
        </div>
    </div>

    <div class="FloatDiv" id="DivFloatProc">
        <div class="FloatTitle">
            <span>确认操作</span>
            <a href="javascript:;" class="FloatClose">关闭</a>
        </div>
        <div class="ProcTitleTxt"></div>
        <div class="FloatCnt ProcCnt">
        </div>
    </div>
    
    <div class="FloatDiv" id="DivAddHidePage" style="width:700px;">
        <div class="FloatTitle">
            <span>关联隐藏页面</span>
            <a href="javascript:;" class="FloatClose">关闭</a>
        </div>
        <div class="FloatCnt">
            关联到:<select id="SelAddHideReParentId" name="ParentID"></select><br />
            <div id="DivAddHideRePageTpl" style="display:none" >
            <div class="AddHideRePageItem" dataPID='{PID}'>
            {PName}[{PUrl}]
            <a href="#" class="Btn" onclick="return AddHidePageRe($(this).parent().attr('dataPID'),$(this).parent().parent().parent().find('[name=ParentID]').val())">关联</a>
            <span class="DisabledShowTxt">
            <a href="#" class="Btn" onclick="return DeletePageDo($(this).parent().parent().attr('dataPID'),$(this).parent().parent().parent().parent().find('[name=ParentID]').val())">取消关联</a>
            </span>
            </div>
            </div>
            <div id="DivAddHideRePageList" class="FloatScrollCnt">
            </div>
        </div>
    </div>
    
    <div class="FloatDiv" id="DivImportPageCfg" style="width:700px;">
        <div class="FloatTitle">
            <span>导入页面</span>
            <a href="javascript:;" class="FloatClose">关闭</a>
        </div>
        <div class="FloatCnt">
            <div>
                导入到：<select id="SelImportPageParentId" name="ParentID"></select>（的子页面）
            </div>
            将导出的配置文本复制到下方：<br />
            <textarea id="TxtImportPageCfgTxt" style="width:670px;height:100px;"></textarea><br />
            <a href="javascript:;" onclick="return ImportPage();" >导入</a>
        </div>
    </div>
    
    <div class="FloatDiv" id="DivExportPageCfg" style="width:700px;">
        <div class="FloatTitle">
            <span>导出页面</span>
            <a href="javascript:;" class="FloatClose">关闭</a>
        </div>
        <div class="FloatCnt">
            <div>
                <select id="SelExportPageParentId" disabled="disabled" name="ParentID"></select>
            </div>
            <div>
                请选择要导出的页面
            </div>
            <div id="DivExportPageSelList" class="FloatScrollCnt"></div>
            <div>
                勾选上方需要导出的页面，下方生成配置文本<br />
                <textarea id="TxtExportPagesCfgTxt" style="width:670px;height:100px;"></textarea><br />
                复制上方配置文本，导入页面时使用
            </div>
        </div>
    </div>
    
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="../js/Utility.js"></script>
    <script type="text/javascript">

        //#region 数据处理
        function GetPageData(lnk) {
            return GetPageDataByT($(lnk).parent().parent());
        }
        function GetPageDataByT(p) {
            return {
                PID: parseInt(p.attr("dataPID"))
                , PName: p.attr("dataPName")
                , PUrl: p.attr("dataPUrl")
                , Queue: p.attr("dataQueue")
                , DefShowChild: p.attr("dataDefShowChild")
                , ParentID: parseInt(p.attr("dataParentID"))
                , HideParentID: parseInt(p.attr("dataHideParentID"))
            };
        }

        function UpdateData(data, replaceOnePageJq) {
            if (data.PageHTML) {
                if (replaceOnePageJq) {
                    replaceOnePageJq.replaceWith(data.PageHTML);
                } else {
                    $("#DivAllPages").html(data.PageHTML);
                }
                UpdatePagesTree();
            }
            if (data.SelHTML) {
                $("#List_AllPages").replaceWith(data.SelHTML);
                UpdateAllPageDrList();
            }
            if (data.HidePageList) {
                $("#DivHidePages").html(data.HidePageList);
                UpdateHidePageList();
            }
            if (data.ChkNewPagesListHTML) {
                $("#DivCkNewPages").html(data.ChkNewPagesListHTML);
            }
            $("#SelAddHideReParentId").change();
        }

        //更新所有页面下拉菜单的关联
        function UpdatePagesTree() {
            $("#DivAllPageTreeP ul").each(function () {
                $(this).children("li").removeClass("PageItemLast").filter(":last").addClass("PageItemLast");
            });
            $("#DivAllPageTreeP .PageTitle").each(function () {
                var t = $(this);
                if (t.siblings("ul").children("li").length > 0) {
                    t.find(">ins")[0].className = "PageItemIconOpen";
                } else {
                    t.find(">ins")[0].className = "PageItemIcon";
                }
            });
        }
        //树形收缩和展开控制
        $("#DivAllPageTreeP").delegate("ins", "click", function () {
            var t = $(this);
            if (t.is(".PageItemIconOpen")) {
                t.parent().siblings("ul").hide();
                t[0].className = "PageItemIconClose";
            } else if (t.is(".PageItemIconClose")) {
                t.parent().siblings("ul").show();
                t[0].className = "PageItemIconOpen";
            }
        });

        //更新所有页面下拉菜单的关联
        function UpdateAllPageDrList() {
            var p, pv;

            p = FDEditPage.jq.find("[name='ParentID']");
            pv = p.val();
            p.children(":not(.fixed)").remove();
            p.append($("#List_AllPages").children().clone()).val(pv);

            FDAddChildPage.jq.find("[name='ParentID']")
                .add(FDAddHidePage.jq.find("[name='ParentID']"))
                .add(FDExportPageCfg.jq.find("[name='ParentID']"))
                .add(FDImportPageCfg.jq.find("[name='ParentID']"))
                .each(function () {
                p = $(this);
                pv = p.val();
                p.children(":not(.fixed)").remove();
                p.append($("#List_AllPages").children().clone()).val(pv).change();
            });
        }
        //更新所有隐藏页面的关联
        function UpdateHidePageList() {
            var tpl1 = $("#DivAddHideRePageTpl").html(), listHTML = "";
            $("#DivHidePages").children().each(function () {
                var d = GetPageDataByT($(this)), rowHTML = tpl1;
                for (var n in d) {
                    rowHTML = rowHTML.replace(new RegExp("\{" + n + "\}", "gi"), d[n]);
                }
                listHTML += rowHTML;
            });
            $("#DivAddHideRePageList").html(listHTML);
        }

        function formatJsStr(obj) {
            return obj.replace(/\\/ig, "\\\\").replace(/\r/ig, "\\r").replace(/\n/ig, "\\n").replace(/"/ig, "\\\"");
        }

        //#endregion

        //#region 编辑
        //显示编辑页面窗体
        function ShowEditPage(lnk) {
            var p = GetPageData(lnk)
            FDEditPage.fadeIn("fast");
            FDEditPage.jq.find("[name='PID']").val(p.PID);
            FDEditPage.jq.find("[name='PName']").val(p.PName);
            FDEditPage.jq.find("[name='PUrl']").val(p.PUrl);
            FDEditPage.jq.find("[name='Queue']").val(p.Queue); 
            FDEditPage.jq.find("[name='DefShowChild']").attr("checked", p.DefShowChild);
            if (p.ParentID == "-1") {
                FDEditPage.jq.find("[name='ParentID']").val(p.HideParentID).attr("disabled", true);
                FDEditPage.jq.find(".ShowHidePageInfo").show();
            } else {
                FDEditPage.jq.find("[name='ParentID']").val(p.ParentID).attr("disabled", false);
                FDEditPage.jq.find(".ShowHidePageInfo").hide();
            }
            FDEditPage.jq[0].pageTitle = $(lnk).parent().parent();
            return false;
        }
        //提交编辑页面字段-AJAX处理
        function EditPage() {
            $.ajax({
                data: FDEditPage.jq.find("form").serialize() + "&Action=EditPage"
                , success: function (data) {
                    if (data.ErrMsg) alert(data.ErrMsg);
                    //100成功,要刷单个
                    //200成功,要刷列表
                    //101失败,不操作
                    //202失败,刷页面
                    if (data.Code == 100) {
                        UpdateData(data, FDEditPage.jq[0].pageTitle);
                        FDEditPage.hide();
                    } else if (data.Code == 200) {
                        UpdateData(data);
                        FDEditPage.hide();
                    } else if (data.Code == 202) {
                        window.location.reload();
                    }
                }
            });
        }
        //#endregion

        //#region 删除
        //提交删除页面-AJAX处理
        function DeletePage(lnk) {
            var p = GetPageData(lnk), msgBefore = (p.PUrl ? ("[" + p.PUrl + "]<br />") : ""), msg = msgBefore + "删除后无法恢复，确认删除？";
            var procArr = [];
            procArr.push({
                title: "删除页面"
                , back: function () {
                    DeletePageDo(p.PID, p.HideParentID, 1);
                }
            });
            if (p.ParentID == "-1") {
                procArr.push({
                    title: "只删除引用"
                    , back: function () {
                        DeletePageDo(p.PID, p.HideParentID, 0);
                    }
                });
            }
            showContrim(msg, procArr);
            return false;
        }
        function DeletePageDo(pID, HideParentID, delPage) {
            $.ajax({
                data: { Action: "DeletePage", PID: pID, HideParentID: HideParentID, DelPage: delPage }
                , success: function (data) {
                    if (data.ErrMsg) alert(data.ErrMsg);
                    if (data.Code == 100) {
                        if (pID > 0) {
                            var jq = $("#DivAllPagePanel [dataPID='" + pID + "']" + (!delPage && HideParentID != "0" ? ("[dataHideParentID='" + HideParentID + "']") : ""));
                            if (jq.length > 0) jq.attr("dataPID", "").parent().fadeOut("fast", function () { $(this).remove() });
                        }
                    }
                    UpdateData(data);
                }
            });
            return false;
        }

        //#endregion

        //#region 添加页面

        //显示关联隐藏页面
        function ShowAddHidePage(lnk) {
            var d = GetPageData(lnk);
            FDAddHidePage.fadeIn("fast");
            FDAddHidePage.jq[0].d = d;
            FDAddHidePage.jq.find("[name='ParentID']").val(d.PID).change();
            return false;
        }

        //显示添加页面浮窗
        function ShowAddChildPage(lnk) {
            var d = GetPageData(lnk);
            FDAddChildPage.fadeIn("fast");
            FDAddChildPage.jq[0].d = d;
            FDAddChildPage.jq.find("[name='ParentID']").val(d.PID).change();
            ShowAddChildPageForm({ ParentID: d.PID });
            return false;
        }

        //折叠或展开检测新页面的内容
        function FoldCkNewPageInfoDiv(lnk, showLnkHTML, hideLnkHTML) {
            lnk = $(lnk), p = lnk.parent().parent(), d = p.find(".CkNewPageInfo"), h = p.find(".CkNewPageHideTxt");
            if (d.is(":hidden")) {
                d.show();
                h.hide();
                lnk.html(showLnkHTML);
            } else {
                d.hide();
                h.show();
                lnk.html(hideLnkHTML);
            }
            FDAddChildPage.resetPos();
            return false;
        }

        //检测新页面转到添加窗体
        function MoveCkNewPageToAdd(lnk) {
            var p = $(lnk).parent();

            ShowAddChildPageForm({
                PType: "ShowPage"
                , PName: p.find("[name='PName']").val()
                , PUrl: p.find("[name='PUrl']").val()
                , ParentId: FDAddChildPage.jq[0].d
            }, function () {
                p.remove();
            });
            return false;
        }

        //显示添加窗体,带字段默认值和完成回调
        function ShowAddChildPageForm(d, succeedBack) {
            var cnt = $("#LnkAddPageFrom").click()[0].cnt;
            cnt.find(":text").val("");
            for (var n in d) {
                cnt.find("[name='" + n + "']").val(d[n]);
            }
            allAddPageSelType.filter("[value='" + d.PType + "']").attr("checked", true).change();
            FDAddChildPage.fadeIn("fast");
            window.AddChildPageFormSucceedBack = succeedBack;
            return false;
        }

        //提交添加隐藏页面关联-AJAX处理
        function AddHidePageRe(pID, parentID) {
            $.ajax({
                data: { Action: "AddHidePageRe", PID: pID, parentID: parentID }
                , success: function (data) {
                    if (data.ErrMsg) alert(data.ErrMsg);
                    if (data.Code == 100) {
                        
                    }
                    UpdateData(data);
                    $("#SelAddHideReParentId").change();
                }
            });
            return false;
        }

        function AddChildPage() {
            $.ajax({
                data: $("#FormAddPage").serialize() + "&Action=AddChildPage"
                , success: function (data) {
                    if (data.ErrMsg) alert(data.ErrMsg);
                    //100成功,要刷单个
                    //200成功,要刷列表
                    //101失败,不操作
                    //202失败,刷页面
                    if (data.Code == 200) {
                        UpdateData(data);
                        FDAddChildPage.hide();
                    } else if (data.Code == 202) {
                        window.location.reload();
                    }
                }
            });
        }

        //刷新检测新页面列表
        function reflushChkNewPages() {
            $.ajax({
                data: "&Action=GetChkNewPages"
                , success: function (data) {
                    if (data.ErrMsg) alert(data.ErrMsg);
                    if (data.Code == 200) {
                        UpdateData(data);
                    }
                }
            });
            return false;
        }

        //#endregion
        
        //#region 导入导出页面

        function showImportPage(lnk) {
            var d = GetPageData(lnk);
            FDImportPageCfg.jq[0].d = d;
            FDImportPageCfg.jq.find("[name='ParentID']").val(d.PID).change();
            $("#TxtImportPageCfgTxt").val("");
            FDImportPageCfg.show("fast");
            return false;
        }
        function ImportPage() {
            var cfg = $("#TxtImportPageCfgTxt").val();
            var pID = FDImportPageCfg.jq.find("[name='ParentID']").val();
            $.ajax({
                data: { Action: "ImportPageCfg", PID: pID, CfgJSON: cfg }
                , success: function (data) {
                    if (data.ErrMsg) alert(data.ErrMsg);
                    if (data.Code == 200) {
                        FDImportPageCfg.hide();
                    }
                    UpdateData(data);
                }
            });
            return false;
        }

        //显示树形结构，带复选框
        function showPagesTree(isTop, typeName, toEl, pagesTreeItem) {
            var title = pagesTreeItem.children(".PageTitle");
            var checkbox = $('<input type="checkbox" name="' + typeName + '"/>');
            var data = checkbox[0].data = GetPageDataByT(title);
            var childs = pagesTreeItem.find(">.PageChilds>.PageItem");
            var childsToShow = $('<div class="Childs">');
            toEl
                .append(
                    $('<div class="Item' + (isTop ? ' ItemTop' : "") + '">')
                        .append(
                            $('<label class="Title">')
                                .append(
                                    checkbox
                                        .change(function () {
                                            var p = $(this);
                                            if (!p.parent().parent().is(".ItemTop")) {
                                                //非顶级，需要关联
                                                if (p.attr("checked")) {
                                                    //如果某一项选中了，那么它的所有父级都需要选中
                                                    while ((p = p.parent().parent().parent().parent(":not(.ItemTop)").find(">.Title>:checkbox[name='" + typeName + "']")).length > 0) {
                                                        p.attr("checked", true);
                                                    }
                                                } else {
                                                    //如果某一项取消选中了，那么它的所有子集都需要取消选中
                                                    p.parent().siblings(".Childs").find(":checkbox[name='" + typeName + "']").attr("checked", false);
                                                }
                                            }
                                        }).click(function () { $(this).change();})
                                )
                                .append(data.PName + (data.PUrl ? "[" + data.PUrl + "]" : ""))
                        )
                        .append(childsToShow)
                )
            ;
            childs.each(function () {
                showPagesTree(false, typeName, childsToShow, $(this));
            });
        }
        //对树中选中项的进行操作
        function procTreeChecked(treeChildsEl, procBefore, procAfter) {
            if (treeChildsEl.length <= 0) return;
            var list = treeChildsEl.find(">.Item>.Title>:checked");
            if (list.length <= 0) {
                if (treeChildsEl.find(">.ItemTop").length > 0) {
                    //如果当前是顶级，那么允许顶层不选，子集选
                    procTreeChecked(treeChildsEl.find(">.Item>.Childs"), procBefore, procAfter);
                }
                return;
            }
            var isFirst = true;
            list.each(function () {
                var d = this.data;
                procBefore(d, isFirst);
                procTreeChecked($(this).parent().parent().find(">.Childs"), procBefore, procAfter);
                procAfter(d);
                isFirst = false;
            });
        }


        function showExportPage(lnk) {
            var d = GetPageData(lnk);
            FDExportPageCfg.jq[0].d = d;
            FDExportPageCfg.jq.find("[name='ParentID']").val(d.PID).change();
            showPagesTree(true, "ExportPageItem", $("#DivExportPageSelList").empty(), $(lnk).parent().parent().parent());
            $("#DivExportPageSelList :checkbox").change(function () {
                var str = "[\r\n", hasChild = false;
                procTreeChecked($("#DivExportPageSelList")
                    , function (d, isFirst) {
                        if (!isFirst) str += ',';
                        str += '{\r\n';
                        str += 'PName:"' + formatJsStr(d.PName) + '",\r\n';
                        str += 'PUrl:"' + formatJsStr(d.PUrl) + '",\r\n';
                        str += 'Queue:' + formatJsStr(d.Queue) + ',\r\n';
                        str += 'DefShowChild:' + (d.DefShowChild ? "true" : "false") + ',\r\n';
                        str += 'HideParentID:' + d.HideParentID + ',\r\n';
                        str += 'Childs:[\r\n';
                    }, function (d) {
                        str += ']\r\n';
                        str += '}\r\n';
                    }
                );
                str += "]";
                $("#TxtExportPagesCfgTxt").val(str);
            }).change();
            FDExportPageCfg.show("fast");
            return false;
        }

        //#endregion

        //#region 界面/事件 初始化
        //弹窗初始化
        var FDEditPage = new FloatDiv("#DivEditPage", { closeSel: ".FloatClose", vPosIsParentIfrWin: true });
        var FDLoading = new FloatDiv("#DivLoading", { closeSel: ".FloatClose", vPosIsParentIfrWin: true });
        var FDAddChildPage = new FloatDiv("#DivAddChildPage", { closeSel: ".FloatClose", vPosIsParentIfrWin: true });
        var FDAddHidePage = new FloatDiv("#DivAddHidePage", { closeSel: ".FloatClose", vPosIsParentIfrWin: true });
        var FDConfirm = new FloatDiv("#DivFloatProc", { closeSel: ".FloatClose", vPosIsParentIfrWin: true });
        var FDExportPageCfg = new FloatDiv("#DivExportPageCfg", { closeSel: ".FloatClose", vPosIsParentIfrWin: true });
        var FDImportPageCfg = new FloatDiv("#DivImportPageCfg", { closeSel: ".FloatClose", vPosIsParentIfrWin: true });

        //显示确认操作框
        function showContrim(title, procArr) {
            FDConfirm.jq.find(".ProcTitleTxt").html(title);
            var cnt = FDConfirm.jq.find(".ProcCnt").empty();
            for (var i = 0, len = procArr.length; i < len; i++) {
                var lnk = $('<a href="javascript:;" class="Btn">').html(procArr[i].title).appendTo(cnt);
                lnk[0].proc = procArr[i];
                lnk.click(function () {
                    var t = this;
                    if (t.proc && t.proc.back) t.proc.back();
                    FDConfirm.hide();
                });
            }
            FDConfirm.show();
        }

        //页面列表鼠标经过效果
        var AllPagePanel = $("#DivAllPagePanel")
            .delegate(".PageItem", "mouseenter", function () { $(this).removeClass("PageItemHover").addClass("PageItemHover") })
            .delegate(".PageItem", "mouseleave", function () { $(this).removeClass("PageItemHover") })
            .delegate(".PageTitle", "mouseenter", function () { $(this).removeClass("PageTitleHover").addClass("PageTitleHover") })
            .delegate(".PageTitle", "mouseleave", function () { $(this).removeClass("PageTitleHover") })
            .delegate(".PageTitle", "click", function () { AllPagePanel.find(".PageTitleCurr").removeClass("PageTitleCurr"); $(this).addClass("PageTitleCurr"); });

        //AJAX全局参数初始化
        $.ajaxSetup({
            type: "POST"
            , dataType: "json"
            , url: location.href
            , beforeSend: function () { FDLoading.show(); }
            , complete: function () { FDLoading.hide(); }
            , error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("请求发生错误\n textStatus:" + textStatus + "\n errorThrown:" + errorThrown);
            }
        });

        //添加隐藏页关联,父级下拉菜单选择自动隐藏已添加的关联
        $("#SelAddHideReParentId").change(function () {
            var pid = $(this).val();
            $("#DivAddHideRePageList .AddHideRePageItem").removeClass("Disabled");
            $("[dataPID='" + pid + "']").siblings(".PageChilds:first").find(">.PageTypeHide>.PageTitle").each(function () {
                var d = GetPageDataByT($(this));
                $("#DivAddHideRePageList .AddHideRePageItem[dataPId='" + d.PID + "']").addClass("Disabled");
            });
        });

        //添加页面的选项卡切换初始化
        $("#UlAddCntTabs a").click(function () {
            var t = $(this), li = t.parent(), i = li.index(), cnt = $("#DivAddCnts>*").eq(i);
            li.addClass("Onthis").siblings().removeClass("Onthis");
            cnt.show().siblings().hide();
            this.cnt = cnt;
            FDAddChildPage.resetPos();
            return false;
        }).first().click();

        //滚动条滚动不会动到浏览器的滚动条
        $("#DivCkNewPages,#DivAddHideRePageList,.FloatScrollCnt").bind("wheel mousewheel DOMMouseScroll MozMousePixelScroll", function (e) {
            var v = getWheelV(e);
            if (v != 0) {
                this.scrollTop += v * 100;
            }
            return false;
        });

        //添加页面，选择页面类型
        var allAddPageSelType = $("#DivAddPageSelType :radio").bind("change click", function () {
            allAddPageSelType.not(this).each(function () {
                $($(this).attr("hideinp")).show();
            });
            $($(this).attr("hideinp")).hide();
        });
        allAddPageSelType.filter("[value='ShowPage']").attr("checked", true).change();

        UpdateHidePageList();
        UpdateAllPageDrList();
        UpdatePagesTree();

        //#endregion

    </script>
</body>
</html>
