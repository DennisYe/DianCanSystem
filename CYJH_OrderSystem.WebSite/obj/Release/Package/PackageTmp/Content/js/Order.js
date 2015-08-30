var userNameFlag = "UserName";

$(function () {
    if (!SetUserName()) {
        window.location="/Home/Index"
        return;
    };
    var activeId = $("#navtab a").first().attr("id");
    ShowActiveTbody(activeId);
    FoodSummary();
    $("#logUserName").text("切换用户:("+GetCookie(userNameFlag) + ")");

    $("#navtab li").bind("click", function () {
        $("#navtab li").removeAttr("class");
        $(this).attr("class", "active");
        var thisId = $("#navtab li.active a").attr("id");
        ShowActiveTbody(thisId);
    });

    $(".foodcomment").bind("click", function () {
        $(this).parent().append("<input type='text' class='commentText' />");
        $(this).remove();
    });

    setInterval("RefreshOrderItem()", 1000);
    //RefreshOrderItem();
});

function ShowActiveTbody(element) {
    $("#menudetail tbody").hide();
    $("." + element).show();
}

function AddOrderItem() {
    var itemList = [];


    $(":checked").each(function (i, val) {
        var item = {
            OrderId: 0,
            UserName: '',
            FoodId: '',
            Amount:0,
            comment: ''
        };
        item.OrderId = orderId;
        item.UserName = GetCookie(userNameFlag);
        item.FoodId = $(this).val();
        item.Amount = $(this).parents("tr").find(".amount").val();
        item.comment = $(this).parents("tr").find(".commentText").val();
        itemList.push(item);
    });

    $.ajax({
        url: "/Home/AddOrderItems",
        type: "post",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ 'itemList': itemList }),
        success: function () {
            window.location = window.location;
        },
        error: function (err) {
            window.location = window.location;
        }
    })
}

function FoodSummary() {
    var userName = [];
    var sumPrice=0, foodCount = 0;
    
    $(".userName").each(function () {
        var value = $(this).text();
        var isContain = false;
        for (var i = 0; i < userName.length; i++) {
            if ($.trim(userName[i]) == $.trim(value))
                isContain = true;
        }
        if (!isContain) {
            userName.push(value);
        }
    })

    $(".foodPrice").each(function () {
        var price = parseFloat($(this).text());
        var amount = parseInt($(this).next().text());
        sumPrice += price*amount;
        foodCount++;
    })
    var average = 0;
    if (userName.length > 0) {
        average = sumPrice / userName.length;
    }
    
    var belongCatArray = [];
    //统计饭，菜，汤的总数
    $(".belongCate").each(function () {
        var cat = $(this).text();
        var isContain = false;
        var amount = parseInt($(this).parent().find(".amount").text());
        var index = 0;
        for (var i = 0; i < belongCatArray.length; i++) {
            if ($.trim(cat) == $.trim(belongCatArray[i].BelongCat))
            {
                isContain = true;
                index = i;
            }
                
        }
        if (!isContain) {
            belongCatArray.push({ BelongCat: cat, Count: amount });
        } else {
            belongCatArray[index].Count+=amount
        }
    })
    
    var bolongCatString = "";
    for (var i = 0; i < belongCatArray.length; i++) {
        bolongCatString += belongCatArray[i].BelongCat + ":" + belongCatArray[i].Count + " ";
    }

    var participantPeople = "";
    for (var i = 0; i < userName.length; i++) {
        participantPeople += " "+userName[i] + " ,";
    }
    participantPeople = participantPeople.substring(0, participantPeople.length-1)

    $("#summary").text(userName.length + " 人, "  + bolongCatString + " 共计：" + sumPrice + "元（" + average.toFixed(2) + "/人）");
    $("#participants").text(participantPeople);
    OrderFoodGroup();
}

function OrderFoodGroup() {
    var foodGroup = [];
    $("#collapseThree .panel-body ul").remove();
    $(".foodName").each(function () {
        var name = $(this).text();
        var isContain = false;
        var amount = parseInt($(this).parent().find(".amount").text());
        var index = 0;
        for (var i = 0; i < foodGroup.length; i++) {
            if ($.trim(name) == $.trim(foodGroup[i].FoodName)) {
                isContain = true;
                index = i;
            }

        }
        if (!isContain) {
            foodGroup.push({ FoodName: name, Count: amount });
        } else {
            foodGroup[index].Count += amount
        }
    })

    var foodGroupList = "";
    foodGroup.sort();
    for (var i = 0; i < foodGroup.length; i++) {
        foodGroupList += "<li>" + foodGroup[i].FoodName + ":" + foodGroup[i].Count + "</li>";
    }
    if (foodGroupList != "") {
        $("#collapseThree .panel-body").append("<ul>" + foodGroupList + "</ul>");
    }
}




function SetUserName() {
    var createName = GetCookie(userNameFlag);
    if (createName == null) {
        return UserLogin();
    }
    return true;
}

function UserLogin() {
    var createName = prompt("请输入您的名字：", "");
    if (createName != null && createName != "") {
        SetCookie(userNameFlag, createName);
        $("#logUserName").text("切换用户:("+createName + ")");
        return true;
    }
    else {
        return false;
    }
}

function DeleteFood(orderItemId) {
    $.ajax({
        url: "/Home/DeleteFoodItem",
        type: "post",
        contentType: "application/json; charset=utf-8",
        dataType: "text",
        data: JSON.stringify({ 'orderItemId': orderItemId }),
        success: function () {
            $("#" + orderItemId).parent().remove();
            FoodSummary();
        },
        error: function (err) {
            alert(err.responseText)
        }
    })
}

function RefreshOrderItem() {
    $.ajax({
        url: "/Home/GetOrderItems",
        type: "post",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify({ 'orderId': orderId }),
        success: function (data) {
            $("#orderItemListTbl tbody tr").remove();
            for (var i = 0; i < data.length; i++) {
                //这里的拼接方式改为先写好string，然后用format带进参数会更好 。
                var tr="<tr>"+
                        "<td class='userName'>"+data[i].UserName+"</td>"+
                        "<td class='foodName'>"+data[i].FoodName+"</td>"+
                        "<td class='foodPrice'>" + data[i].FoodPrice + "</td>" +
                        "<td class='amount'>" + data[i].Amount + "</td>" +
                        "<td>" + data[i].Comment + "</td>" +                       
                        "<td>"+data[i].CreateTime+"</td>"+
                        "<td id="+data[i].Id+"><button type='button' class='close' aria-hidden='true' onclick='DeleteFood("+data[i].Id+");'>&times;</button></td>"+
                        "<td class='belongCate' style='display:none'>"+data[i].BelongCate+"</td>"+
                        "</tr>"
                $("#orderItemListTbl tbody").append(tr);
            }
            FoodSummary();
        }
    })
}