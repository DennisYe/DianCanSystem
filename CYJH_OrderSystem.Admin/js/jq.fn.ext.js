(function($){
	$.fn.extend({
	//Begin Extend
	
	popup:function(config){	//²ÎÊý config ÅäÖÃÏî
			return this.each(function(){
				function hidePopup(Pop){
					topopup.hide();
					$(Pop).remove()
				}		  
				var topopup =$(this);
				var bodyWidth=$("body").width();
				var bodyHeight=$("body").height();
				var divWidth=$(this).width();
				var divHeight=$(this).height();
				
				var bHeight = document.body.scrollHeight;
				var dHeight = document.documentElement.scrollHeight;
				bodyHeight=Math.max(Math.max(bHeight,dHeight),Math.max(bodyHeight,divHeight+100))
				var _config;
				if(config==null){
					_config={
						paddingHeight:0,
						top:30,
						left:(bodyWidth-divWidth)/2,
						popupClass:"jq_popup"
					};	
				}else{
					_config=config;
				}
				
				if(_config.paddingHeight>0){
					bodyHeight += parseInt(_config.paddingHeight);
				}
				$("." + _config.popupClass).remove();
				var divLeft=((typeof(_config.left)=="undefined")?(bodyWidth-divWidth)/2:_config.left);
				var divtop =((typeof(_config.top)=="undefined")?30:_config.top);
				var fullScreenDiv=$('<div class="'+ _config.popupClass +'"></div>')
					.width(bodyWidth)
					.height(bodyHeight)
					.appendTo("body")
					.click(function(){hidePopup(this)});
				$(this).css({position:"absolute",top:divtop,left:divLeft,zIndex:201}).show();
			})
		}
	
	//End Extend
	});

})(jQuery)