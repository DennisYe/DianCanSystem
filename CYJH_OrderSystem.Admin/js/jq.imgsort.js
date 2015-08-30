// JavaScript Document
(function($){
	$.fn.extend({
		imgStat: function(fixto,config){	//参数 fixto  图片将填充的容器的ID   config 扩展配置
			var _config;
			var stat={};
			var keys=new Array();
			var totalStat=0;
			var maxStat=0;
			var otherTotal=0;
			if (config==null){
				_config={
					nameindex:0,	//取名称的列标识
					dataindex:2,	//取数值的列标识
					steps:2,		//一个像素的长度表示多少数值
					maxlen:600,		//最大允许的像素值
					minlen:1,		//最小的像素值[当数值为0时,DIV的长度]
					top:0,			//取前几项
					minstat:1, 		//小于这个数值，将被过滤
					asc:false,		//是否升序
					animate:true,	//动画效果
					hide:false,		//是否隐藏
					tableid:'__jqstatfix',	//生成的列表的表格ID
					showother:false,			//是否显示被过滤的列
					othertext:'被过滤的内容'	//被过滤的列的标题
					//otherfunction:a		//被过滤的列点击后的事件
				}	
			}else{
				_config=config;	
			}
			
			if($("#" + fixto).size()==0){
				$("body").append($('<div id="'+ fixto +'"></div>').hide()); //如果不存在	
			}
			
			function createImg(gameClassName,votenum,table,isotherrow){
				/*if(_config.steps==0){
					_config.steps = totalStat/_config.maxlen;
				}*/
				//debugger;
				var pers = parseFloat(votenum/totalStat);
				//var ps = votenum/_config.steps;
				ps= pers * _config.maxlen;
				
				ps= ps>_config.maxlen? _config.maxlen:ps;
				ps= ps<_config.minlen? _config.minlen:ps;
				var initWidth = ps;
				if(_config.animate){
					initWidth=_config.minlen;
				}
				var imgcell=$('<div class="imgrow" title="'+ votenum +'" style="width:'+ initWidth +'px"></div>');
				var textspan=$('<span style="cursor:pointer">' + votenum + '/' + totalStat + '<font color="red">('+ parseFloat(parseInt(votenum)*100/totalStat).toFixed(2) +'%)</font></span>')
				imgcell.data("name",gameClassName);
				imgcell.data("value",votenum);
				textspan.data("name",gameClassName);
				textspan.data("value",votenum);
				if(isotherrow){
					imgcell.click(function(){$("#othertr").toggle()})
					textspan.click(function(){$("#othertr").toggle()})
				}else if(_config.rowclick!=null && table.attr("id")==_config.tableid){
					imgcell.click(_config.rowclick);
					textspan.click(_config.rowclick);
				}
				else if(table.attr("id")==_config.tableid + "_other" && _config.otherfunction!=null){
					imgcell.click(_config.otherfunction);
					textspan.click(_config.otherfunction);
				}
				//imgcell.data("voteCount",votenum)
				var newrow='<tr><td width="80px" align="right">' + gameClassName + '</td><td></td></tr>';
				
				
				var jqnewrow =$(newrow);
				table.append(jqnewrow);
				jqnewrow.find("td").eq(1).append(imgcell);
				
				//显示数值
				if(_config.animate){
					imgcell.animate({
						width:ps				
					},ps,function(){
						$(this).after(textspan)
					});
				}else{
					imgcell.after(textspan);
				}
			}
			
			function sortCount(a,b){
				if(_config.asc){
					return stat[a]- stat[b];	
				}else{
					return stat[b]- stat[a];	
				}
			}
					
			return this.each(function(){	
				$('table[id^="'+ _config.tableid +'"]' ).remove();
				$(this).find("tr").each(function(i){ //取值
					if(i==0){
					}else{
						var zqm = $(this).find("td").eq(_config.nameindex).text();	// 专区名
						var djl = parseInt( $(this).find("td").eq(_config.dataindex).text());  //点击率
						if(stat[zqm]>=0){
							stat[zqm]=stat[zqm]+djl;
						}else{
							stat[zqm] = djl;
							keys.push(zqm);
						}
						totalStat += djl;
					}
					
				})
				keys.sort(sortCount)
				var table=$('<table width="100%" border="0" cellpadding="3" id="'+ _config.tableid +'" cellspacing="3"></table>');
				var othertable=$('<table width="100%" border="0" cellpadding="3" id="'+ _config.tableid +'_other" cellspacing="3"></table>');
				var index=0;
				var _top=0;
				var _minstat=0;
				if(_config.top>0){
					_top=_config.top
				}else{
					_top= keys.length;	
				}
				if(_config.minstat>0){
					_minstat=_config.minstat;
				}
				for (key in keys){
					var name=keys[key];
					var statnum = stat[name];
					if(index>=_top){
						if(_config.showother==true){
							otherTotal+=statnum;
							createImg(name,statnum,othertable,false);
						}else{
							break;	
						}
					}else if(!_config.asc && statnum<_minstat && _minstat>0){
						if(_config.showother==true){
							otherTotal+=statnum;
							createImg(name,statnum,othertable,false);
						}else{
							break;	
						}	
					}else if (_config.asc && statnum<_minstat && _minstat>0){
						if(_config.showother==true){
							otherTotal+=statnum;
							createImg(name,statnum,othertable,false);
						}
					}else{
						createImg(name,statnum,table);
						index++;
					}
				}
				if(_config.showother==true){
					createImg(_config.othertext,otherTotal,table,true);
					var otherrow=$('<tr id="othertr"><td></td><td align="left"></td></tr>').hide();
					table.append(otherrow);
			
					otherrow.find("td").eq(1).append(othertable);
					
				}
				if(_config.hide){
					$('#' + fixto).hide();
				}else{
					$('#' + fixto).show();
				}
				$('#' + fixto).append(table);
			})
		}
	})
})(jQuery);