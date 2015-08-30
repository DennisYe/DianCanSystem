/// <reference path="jquery-1.4.1-vsdoc.js" />

/* 填充表格的数据到集合中（非数组） */
function TableToScriptInfoList(tableId) {
    var arr = [];
    $("#" + tableId).find("tr").each(function(i, j) {
        if (i >= 1) {
            arr.push(new ScriptInfo(j));
        }
    })
    return arr;
}

/* 将表格的一行填充到对象中 */
function ScriptInfo(arowdom) {
    var tds = $(arowdom).find("td");
    this.scriptId = tds.eq(0).text();
    this.gameArea = tds.eq(1).text();
    this.scriptName = tds.eq(2).text();
    this.orderTimes = parseInt(tds.eq(3).text());
    this.orderFee = parseInt(tds.eq(4).text());
    return this;
}


/* 对象转换成数组，方便排序使用 */
function HtToArray(itemlist,removeItemFn) {
    var tmparr = new Array();
    if(typeof removeItemFn != "function"){
        removeItemFn = function (){return false;}
    }
    for (var key in itemlist) {
        if(!removeItemFn(itemlist[key])){
        tmparr.push(itemlist[key]);}
    }
    return tmparr;
}

/*创建数据条 返回HTML*/
function CreateDataDom(val, total, maxwidth) {
    maxwidth = maxwidth ? maxwidth : 600;
    var width = 0;
    if (maxwidth > 0 && total > 0)
        width = parseInt((val * maxwidth / total));
    width = width < 2 ? 2 : width;
    var result = '<label style=" cursor: pointer; height: 15px; background-color: #33ccff; display:inline-block ; width: ' + width + 'px"></label>' + val + "/" + total;
    return result;

}

/************* 以下为单个结果集的对比查询 ***************/

/* 根据游戏专区分组统计 - 返回一个对象  
{allTimes : 总次数,
 allFee : 总金额,
 detail : [
        {gameArea:专区名,totalTimes:专区总次数 ,totalFee:专区订购总额 ,scriptList:[脚本列表]}
    ]}
*/
function GroupScriptInfoList(scriptinfoList) {
    var result = { allTimes: 0, allFee: 0, detail: [] };
    $.each(scriptinfoList, function(i, scriptinfo) {
        if (!result.detail[scriptinfo.gameArea]) {
            result.detail[scriptinfo.gameArea] = {
                gameArea: scriptinfo.gameArea,
                totalTimes: 0,
                totalFee: 0,
                scriptList: []
            };
        }
        result.allFee += scriptinfo.orderFee;
        result.allTimes += scriptinfo.orderTimes;

        result.detail[scriptinfo.gameArea].totalTimes += scriptinfo.orderTimes;
        result.detail[scriptinfo.gameArea].totalFee += scriptinfo.orderFee;
        result.detail[scriptinfo.gameArea].scriptList.push(scriptinfo);
    })
    return result;
}



/* 创建某次查询结果的对比表单, 返回创建出的表格的JQUERY对象 */
/* showDataType   1 表示 消费统计， 2表示 订购统计  */
function CreateStatResultTable(groupResult , showDataType , orderGameAreaFunction, orderDetailFunction) {
    /*创建明细表，返回HTML*/
    function CreateDetailInnerHTML(detailList) {
        //先排序
        var detailListArr = HtToArray(detailList);
        if (typeof orderDetailFunction == "function") {
            detailListArr.sort(orderDetailFunction);
        }

        var tmptable = '<table width="95%" border="0" cellpadding="3" cellspacing="3" class="statResult">';

        tmptable += '<tr><th>脚本编号</th><th>脚本名称</th><th>订购次数</th><th>订购总额</th></tr>';

        for (var key in detailListArr) {
            var tmp = detailListArr[key];
            tmptable += '<tr><td>' + tmp.scriptId + '</td><td>' + tmp.scriptName + '</td><td>' + tmp.orderTimes + '</td><td>' + tmp.orderFee + '</td></tr>'
        }
        tmptable += '</table>';
        return tmptable;
    }
 
    /*创建列*/
    function CreateTr(gameInfo, tmptable, fn) {
        var normalBgColor = "#FFF";
        var hoverBgColor = "#FFF";
        var row = $('<tr style="cursor:pointer; background-color:' + normalBgColor + '"><td style="width:120px">' + gameInfo.gameArea + '</td><td>' + fn(gameInfo) + '</td></tr>');
        var detailrow = $('<tr style="display:none;"><td>&nbsp;</td><td>' + CreateDetailInnerHTML(gameInfo.scriptList) + '</td></tr>');
        row.click(function() { detailrow.toggle() })
        row.hover(function() { this.style.backgroundColor = hoverBgColor }, function() { this.style.backgroundColor = normalBgColor });
        tmptable.append(row);
        tmptable.append(detailrow);

    }

    /*主线程*/
    var newtable = $('<table width="97%" border="0" cellpadding="5px" cellspacing="0" class="statResult"></table>');
    if (showDataType == 1) {
        newtable.append('<tr align="left"><th style="width:120px" width="120px">专区名称</th><th>订购总额</th></tr>');
    } else if (showDataType == 2) {
        newtable.append('<tr align="left"><th style="width:120px" width="120px">专区名称</th><th>订购次数</th></tr>');
    }
    var showDataFn = function(ginfo){
    return CreateDataDom(ginfo.totalTimes, groupResult.allTimes);
    }
    if (showDataType == "1") {
        showDataFn = function(ginfo) {
        return CreateDataDom(ginfo.totalFee, groupResult.allFee);
        }
    }

    //debugger;
    //排序
    var gamedetail = HtToArray(groupResult.detail);
    if (typeof orderGameAreaFunction == "function") {
        gamedetail.sort( orderGameAreaFunction);
    }
    
    var gamedetaillen = gamedetail.length;
    for (var index = 0; index < gamedetaillen; index++) {
        CreateTr(gamedetail[index] , newtable, showDataFn);
    }

    //for (var key in gamedetail) {
    //      var tmp = gamedetail[key];
    //     CreateTr(tmp, newtable, showDataFn);
    //}
    return newtable;
}




/************* 以下为两个结果集的对比查询 ***************/


/*常用标识*/
var toSign = "→";
var upColor = "red";
var downColor = "green";
var upSign = '<font color="' + upColor + '">↑</font>';
var downSigh = '<font color="' + downColor + '">↓</font>';
var noSign = '';


/* 合并两个结果集，返回JoinModel对象集合（非数组） */
function JoinTwoResult(scriptListLastTime, scriptListThisTime) {
    var result = [];
    for (var key in scriptListLastTime) {
        var tmpitem = scriptListLastTime[key];
        var resultkey = tmpitem.scriptId;
        if (!result[resultkey]) {
            result[resultkey] = new JoinModel(tmpitem.scriptId, tmpitem.scriptName, tmpitem.gameArea);
        }
        result[resultkey].lastFee = tmpitem.orderFee;
        result[resultkey].lastTimes = tmpitem.orderTimes;
    }
    for (var key in scriptListThisTime) {
        var tmpitem = scriptListThisTime[key];
        var resultkey = tmpitem.scriptId;
        if (!result[resultkey]) {
            result[resultkey] = new JoinModel(tmpitem.scriptId, tmpitem.scriptName, tmpitem.gameArea);
        }
        result[resultkey].thisFee = tmpitem.orderFee;
        result[resultkey].thisTimes = tmpitem.orderTimes;
    }
    return result;


    /*实例一个新的对象*/
    function JoinModel(scriptId, scriptName, gameArea) {
        this.scriptId = scriptId;
        this.scriptName = scriptName;
        this.gameArea = gameArea;
        this.lastFee = 0;
        this.thisFee = 0;
        this.lastTimes = 0;
        this.thisTimes = 0;
        return this;
    }
}




/* 传入两个值，计算涨幅 */
function CreateUpDownNum(oldData, newData) {
    if (oldData < newData)
        return '<font color="' + upColor + '">' + upSign + (newData - oldData) + '</font>';
    if (newData < oldData)
        return '<font color="' + downColor + '">' + downSigh + (oldData - newData) + '</font>';
    return "0";
}

/* 传入两个数值，拼接成字串 */
function CreatToStr(oldData, newData) {
    if (oldData == 0)
        return upSign + '<font color="' + upColor + '">' + oldData + toSign + newData + '</font>';
    if (newData == 0)
        return downSigh + '<font color="' + downColor + '">' + oldData + toSign + newData + '</font>';
    if (oldData > newData)
        return downSigh + oldData + toSign + newData;
    if (newData > oldData)
        return upSign + oldData + toSign + newData;
    return oldData + toSign + newData;
}


/*合并两个结果集到一个对象*/
/* 返回值
    {
        lastAllFee : 上次总订购金额,
        lastAllTimes : 上次订购次数,
        thisAllFee : 本次订购总额,
        thisAllTimes : 本次订购次数,
        detail: [
            {gameArea:专区名,
            lastTotalTimes:上次专区总次数 ,
            lastTotalFee:上次专区订购总额 ,
            thisTotalTimes:本次专区总次数 ,
            thisTotalFee:本次专区订购总额 ,
            scriptList:[
                {scriptId:脚本ID,
                 scriptName: 脚本名字,
                 lastFee:上次订购金额,
                 thisFee:本次订购金额,
                 lastTimes:上次订购次数,
                 thisTimes:本次订购次数}
            ]}
        ]
    }
*/
function GroupTwoList(scriptListLastTime, scriptListThisTime, removeItemFn) {
    /*主线程*/
    var result = {
        lastAllFee: 0,
        lastAllTimes: 0,
        thisAllFee: 0,
        thisAllTimes: 0,
        detail: []
    };

    if (typeof removeItemFn != "function") {
        removeItemFn = function(item) { return false; } //返回true表示要移除
    }

    var groupResult = JoinTwoResult(scriptListLastTime, scriptListThisTime);

    for (var i in groupResult) {
        var scriptinfo = groupResult[i];

        if (!removeItemFn(scriptinfo)) {//如果不需要从结果集中移除该脚本信息则进行统计
            if (!result.detail[scriptinfo.gameArea]) {
                result.detail[scriptinfo.gameArea] = {
                    gameArea: scriptinfo.gameArea,
                    lastTotalTimes: 0,
                    lastTotalFee: 0,
                    thisTotalTimes: 0,
                    thisTotalFee: 0,
                    scriptList: []
                };
            }
            result.lastAllFee += scriptinfo.lastFee;
            result.lastAllTimes += scriptinfo.lastTimes;
            result.thisAllFee += scriptinfo.thisFee;
            result.thisAllTimes += scriptinfo.thisTimes;

            result.detail[scriptinfo.gameArea].lastTotalTimes += scriptinfo.lastTimes;
            result.detail[scriptinfo.gameArea].lastTotalFee += scriptinfo.lastFee;
            result.detail[scriptinfo.gameArea].thisTotalTimes += scriptinfo.thisTimes;
            result.detail[scriptinfo.gameArea].thisTotalFee += scriptinfo.thisFee;

            result.detail[scriptinfo.gameArea].scriptList.push(scriptinfo);
        }
    }
    return result;
}


/*创建明细表，返回HTML*/
/*传入一个带对比结果的数据集*/
function CreateDetailInnerHTML(detailList, orderDetailFunction , removeItemFn) {
    //先排序
    var detailListArr = HtToArray(detailList, removeItemFn);
    
    if (typeof orderDetailFunction == "function") {
        detailListArr.sort(orderDetailFunction);
    }

    var tmptable = '<table width="97%" border="0" class="statResult">';
    tmptable += '<tr align="center"><th align="left">脚本名称</th><th>订购次数<br/>（上次' + toSign + '本次）</th><th>订购总额<br/>（上次' + toSign + '本次）</th><th>涨幅<br/>（订购次数）</th></tr>';

    for (var key in detailListArr) {
        var tmp = detailListArr[key];
        tmptable += '<tr align="center"><td align="left">' + tmp.scriptName + '</td><td>' + CreatToStr(tmp.lastTimes, tmp.thisTimes) + '</td><td>' + CreatToStr(tmp.lastFee, tmp.thisFee) + '</td><td>' + CreateUpDownNum(tmp.lastTimes, tmp.thisTimes) + '</td></tr>'
    }
    tmptable += '</table>';
    return tmptable;
}

/*创建两次专区对比结果的表格 ， 结果集中已经过滤了不需要的项目了*/
function CreateCompareTable(joinResult, orderGameAreaFunction, orderDetailFunction, normalBgColor, hoverBgColor) {
   
    /*创建列*/
    function CreateTr(gameInfo, tmptable) {
          normalBgColor = normalBgColor ? normalBgColor : "#FFF";
          hoverBgColor = hoverBgColor ? hoverBgColor : normalBgColor;

        var row = $('<tr align="center" style="cursor:pointer; background-color:' + normalBgColor + '"><td  align="left" style="width:120px">' + gameInfo.gameArea + '</td><td>' + CreatToStr(gameInfo.lastTotalTimes, gameInfo.thisTotalTimes) + '</td><td>' + CreatToStr(gameInfo.lastTotalFee, gameInfo.lastTotalFee) + '</td><td>' + CreateUpDownNum(gameInfo.lastTotalTimes, gameInfo.thisTotalTimes) + '</td></tr>');

        var detailrow = $('<tr style="display:none;"><td>&nbsp;</td><td colSpan="3">' + CreateDetailInnerHTML(gameInfo.scriptList, orderDetailFunction) + '</td></tr>');
        row.click(function() { detailrow.toggle() })
        row.hover(function() { this.style.backgroundColor = hoverBgColor }, function() { this.style.backgroundColor = normalBgColor });
        tmptable.append(row);
        tmptable.append(detailrow);

    }

    /*主线程*/
    var newtable = $('<table width="97%" border="0" cellpadding="2px" cellspacing="2px" class="statResult"></table>');
    newtable.append('<tr align="center"><th align="left" style="width:120px" width="120px">专区名称</th><th>订购次数（上次' + toSign + '本次）</th><th>订购总额（上次' + toSign + '本次）</th><th>涨幅（订购次数）</th></tr>');

    //debugger;
    //排序
    var gamedetail = HtToArray(joinResult.detail);
    if (typeof orderGameAreaFunction == "function") {
        gamedetail.sort(orderGameAreaFunction);
    }

    for (var key in gamedetail) {
        var tmp = gamedetail[key];
        CreateTr(tmp, newtable);
    }
    return newtable;
}