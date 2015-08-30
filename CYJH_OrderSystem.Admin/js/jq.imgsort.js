// JavaScript Document
(function($){
	$.fn.extend({
		imgStat: function(fixto,config){	//���� fixto  ͼƬ������������ID   config ��չ����
			var _config;
			var stat={};
			var keys=new Array();
			var totalStat=0;
			var maxStat=0;
			var otherTotal=0;
			if (config==null){
				_config={
					nameindex:0,	//ȡ���Ƶ��б�ʶ
					dataindex:2,	//ȡ��ֵ���б�ʶ
					steps:2,		//һ�����صĳ��ȱ�ʾ������ֵ
					maxlen:600,		//������������ֵ
					minlen:1,		//��С������ֵ[����ֵΪ0ʱ,DIV�ĳ���]
					top:0,			//ȡǰ����
					minstat:1, 		//С�������ֵ����������
					asc:false,		//�Ƿ�����
					animate:true,	//����Ч��
					hide:false,		//�Ƿ�����
					tableid:'__jqstatfix',	//���ɵ��б�ı��ID
					showother:false,			//�Ƿ���ʾ�����˵���
					othertext:'�����˵�����'	//�����˵��еı���
					//otherfunction:a		//�����˵��е������¼�
				}	
			}else{
				_config=config;	
			}
			
			if($("#" + fixto).size()==0){
				$("body").append($('<div id="'+ fixto +'"></div>').hide()); //���������	
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
				
				//��ʾ��ֵ
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
				$(this).find("tr").each(function(i){ //ȡֵ
					if(i==0){
					}else{
						var zqm = $(this).find("td").eq(_config.nameindex).text();	// ר����
						var djl = parseInt( $(this).find("td").eq(_config.dataindex).text());  //�����
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