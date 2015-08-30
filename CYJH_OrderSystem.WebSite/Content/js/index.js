$(function () {
    //var activeId = $("#navtab a").first().attr("id");
    //ShowActiveTbody(activeId);
    
    //MakeMenuActive();    

    //$("#navtab li").bind("click", function () {
    //    $("#navtab li").removeAttr("class");
    //    $(this).attr("class", "active");
    //    var thisId = $("#navtab li.active a").attr("id");
    //    ShowActiveTbody(thisId);
    //});

    //$("#menuNav li").bind("click", function () {
    //    //由于会刷新页面，所以在这里做active操作没有意义，因此放在载入页面时判断
    //    var activeMenuId = $(this).attr("id");
    //    window.location = "/Home/Index/" + activeMenuId;
    //})
});

function MakeMenuActive()
{
    $("#menuNav li").removeAttr("class");
    $("#"+menuId).attr("class", "active");
}

function ShowActiveTbody(element) {
    $("table tbody").hide();
    $("." + element).show();
}

var userNameFlag = "UserName";
function CreateOrder(menuId) {
    var createName = GetUserName();
    if (createName == null) {
        var createName = prompt("请输入您的名字：", "");
        if (createName != null && createName != "") {
            SetCookie(userNameFlag,createName);
        }
        else {
            return;
        }       
    }
    $.ajax({
        url: "/Home/CreateOrder",
        type: "post",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ 'menuId': menuId, 'createName': createName }),
        success: function (data) {
            window.location = "/Home/Order/" + data;
        },
        error: function (err) {
            alert(err);
        }

    })
}

function GetUserName() {
    return GetCookie(userNameFlag);
}

