(function($) {

    $.fn.extend({
        dropDownList: function(config) {
            var _config = {
                inputclass: 'ELIAN_AUTOSELECT_DROPDOWNLIST_INPUT',
                itemsclass: 'ELIAN_AUTOSELECT_DROPDOWNLIST_ITEMS'
            }
            $.extend(_config, config)//取得最终的设置
            var tmp = this; //JQ对象
            var tmpdom = tmp.get(0); //DOM对象


            //创建一个下拉选项。 传入参数（显示文本，值，序号）
            var anoption = function(text, value, index) {
                this.text = text;
                this.value = value;
                this.index = index;
                this.jqElement = null;
                this.getjqElement = function() {
                    if (!this.jqElement) {
                        this.jqElement = $('<li>' + this.text + '</li>');
                    }
                    return this.jqElement;
                }
            }

            var source = []; //下拉菜单所有项目，在初始化的时候赋值
            var searchresult = []; //当前下拉菜单查找到的项目，保存index即可
            var result = []; //仅保存序号
            var currentIndex = -1; //当前选中的项目序号， indexOf searchresult
            var listlength = 0;
            var selectinput; //用于输入文本进行自动查找的文本框
            var itemslist; //用于显示匹配到的项目的文本框
            var tmptimeout; //计时器,N毫秒后开始自动搜索
            var hideoptiontimeout; //计时器，用于隐藏选项
            var isselecting; //当前选项的显示状态，true:显示 false:隐藏
            var firsttext;
            initOptions();
            createNewItem(); //创建控件
            initHanlder();
            selectinput.val(firsttext);
            doSearch();
            function initOptions() {
                tmp.find("option").each(function(i) {
                    var tmpoption = $(this);
                    source.push(new anoption(tmpoption.text(), tmpoption.val(), i));
                    if (tmpoption.get(0).selected) {
                        firsttext = tmpoption.text();
                    }
                })
                listlength = source.length;
            }

            function changeType() {
                var isold = false;
                if (tmpdom.style.display == "none")
                    isold = true;
                if (isold) {
                    selectinput.hide();
                    tmp.show();
                } else {
                    var dropDownListText = tmpdom.options[tmpdom.selectedIndex].innerHTML; //获取选择项的值

                    selectinput.val(dropDownListText);
                    selectinput.show();
                    tmp.hide();
                }
            }

            function createNewItem() {
                var tmpdomwidth = tmpdom.offsetWidth + "px";
                selectinput = $('<input class="' + _config.inputclass + '" type="text" />')
                selectinput.width(tmpdomwidth);
                selectinput.insertBefore(tmp);

                var changeTyped = $('<span class="ELIAN_AUTOSELECT_CHANGETYPEBTN">切换模式</span>');
                changeTyped.click(function() { changeType() });
                tmp.after(changeTyped);

                itemslist = $('<ul style="display:none;position:absolute;" class="' + _config.itemsclass + '"></ul>');
                itemslist.width(tmpdomwidth);
                itemslist.insertAfter(tmp);
                var sdomoffset = getOffset(selectinput.get(0));
                itemslist.css({ top: sdomoffset.top + sdomoffset.height, left: sdomoffset.left });
                tmp.hide();

            }

            function getOffset(dom) {
                var top = 0; var left = 0; var height = 0; var width = 0;
                height = dom.offsetHeight;
                width = dom.offsetWidth;
                var p = dom;
                while (p) {
                    top += p.offsetTop;
                    left += p.offsetLeft;
                    p = p.offsetParent;
                }
                return { top: top, left: left, height: height, width: width };
            }

            function initHanlder() {
                selectinput
                    .focus(function() {
                        beginSearch();
                    }).blur(function() {
                        beginHideOption();
                    });
                selectinput.keydown(function() { return fetchKeyCode(); });

                itemslist.hover(function() { window.clearTimeout(hideoptiontimeout); }, function() { });
            }

            function fetchKeyCode() {
                var keycode = window.event.keyCode;
                if (keycode == 13) {//回车，表示确定某个选项
                    inputEnter();
                    return false;
                } else if (keycode == 38) {//上移
                    moveCurrent(-1);
                    return false;
                } else if (keycode == 40) {//下移
                    moveCurrent(1);
                    return false;
                } else {//其他文字，开始搜索
                    tmptimeout = window.setTimeout(function() { beginSearch() }, 300);
                    return true;
                }
            }

            function setvalByText(text) {
                tmp.find("option").each(function(i) {
                    var tmpoption = $(this);
                    if (tmpoption.text() == text) {
                        this.selected = true;
                    }
                })
            }

            //设置值
            function setValue(setinput) {
                var thisitem = source[result[currentIndex]];
                if (!thisitem)
                    return;
                if (setinput)
                    selectinput.val(thisitem.text);

                thisitem.getjqElement().addClass('selected');
                tmp.val(source[result[currentIndex]].value);
                if (typeof tmpdom.onchange == "function") {
                    tmpdom.onchange();
                }
            }

            //按下回车键，如果当前没有选项，直接开始查找，否则，将选项设置到文本框中
            function inputEnter() {
                $("#Label1").val(isselecting ? "1" : "2");
                if (!isselecting) {
                    beginSearch();
                    return false;
                } else {
                    setValue(true);
                    itemslist.hide();
                    currentIndex = -1;
                    return false;
                }
            }

            function beginHideOption() {
                window.clearTimeout(hideoptiontimeout);
                hideoptiontimeout = window.setTimeout(function() { toggleOpeions(true) }, 200)
            }

            //开始查找
            function beginSearch() {
                window.clearTimeout(hideoptiontimeout);
                doSearch();
                toggleOpeions();
            }

            //仅控制显示或隐藏查找到的选项[只要当前有结果，就是显示]
            function toggleOpeions(hidden) {
                $("#Label1").val(result.length);
                hidden = hidden ? hidden : false; //是否总是隐藏
                if (hidden || result.length == 0) {//如果没有结果，其实不需要显示
                    itemslist.hide();
                    isselecting = false;
                    currentIndex = -1;
                } else {
                    itemslist.show();
                    isselecting = true;
                    if (currentIndex >= 0)
                        source[result[currentIndex]].getjqElement().addClass("selected");
                }
            }

            //移动到选择的项目  
            // num : 与op 组合
            // op : - 表示为上移  num 个单位 
            // op : + 表示下移 num 个单位 
            // op : = 表示移动到 num 位置
            function moveCurrent(num, op, setvalue) {
                op = op ? op : "+";
                var rescount = result.length;
                var nowIndex = 0;

                switch (op) {
                    case '+': nowIndex = currentIndex + num; break;
                    case '-': nowIndex = currentIndex - num; break;
                    case '=': nowIndex = num; break;
                }

                if (op != '=') {
                    if (nowIndex >= rescount)
                        nowIndex = 0;
                    if (nowIndex < 0)
                        nowIndex = rescount - 1;
                }

                if (isselecting == false) {//如果还没有显示下拉，则优先显示
                    currentIndex = nowIndex;
                    toggleOpeions();
                    return;
                } else { //如果已经显示了。那么设置高亮。同时设置下拉菜单的值

                    if (currentIndex >= 0) {
                        var thisitem = source[result[currentIndex]];
                        if (thisitem)
                            thisitem.getjqElement().removeClass('selected');
                    }
                    currentIndex = nowIndex;
                    setValue();
                }
            }


            function bindItemWhenShowResult(k, tmpresult) {
                var tmpindex = tmpresult[k];
                var element = source[tmpindex].getjqElement();
                element.removeClass('selected').appendTo(itemslist).unbind()
	                .click(function() { inputEnter() })
	                .hover(function() { moveCurrent(k, "=", false) }, function() { this.className = this.className.replace("selected", "") });
            }

            function doSearch() {//查找列表，如果能查找到内容，返回true
                var searchkey = $.trim(selectinput.val()).toUpperCase();
                //var thesameItemIndex = -1;
                if (!searchresult[searchkey]) {
                    searchresult[searchkey] = [];
                    for (var index = 0; index < listlength; index++) {
                        var ttext = source[index].text.toUpperCase()
                        if (ttext.indexOf(searchkey) >= 0) {
                            if (ttext == searchkey) setvalByText(searchkey);
                            //thesameItemIndex = index;

                            searchresult[searchkey].push(index);
                        }
                    }
                }
                currentIndex = -1;
                if (searchresult[searchkey].length > 0) {
                    var tmpresult = searchresult[searchkey];
                    itemslist.empty();
                    for (var k in tmpresult) {
                        bindItemWhenShowResult(k, tmpresult);
                    }
                    result = tmpresult;
                    //                    if (currentIndex > -1)
                    //                        setValue();
                    return true;
                } else {
                    return false;
                }
            }
        }
    })
})(jQuery)