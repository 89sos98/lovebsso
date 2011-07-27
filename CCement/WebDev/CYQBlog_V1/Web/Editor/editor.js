function DEditorClass()
{
    this.EditorDesignMode=EditorDesignMode;
    this.OverIco=OverIco;
    this.OutIco=OutIco;
    this.FontBold=FontBold;
    this.FontItalic=FontItalic;
    this.FontUnderLine=FontUnderLine;
    this.RemoveFormats=RemoveFormats;
    this.SetFontSize=SetFontSize;
    this.SetFontName=SetFontName;
    this.SetFontColor=SetFontColor;
    this.InsertLink=InsertLink;
    this.InsertLinkGo=InsertLinkGo;    
    this.InsertUnlink=InsertUnlink;
    this.InsertsImageGo=InsertsImageGo;
    this.InsertsFlashGo=InsertsFlashGo;
    this.InsertSmiley=InsertSmiley;
    this.SendUploadImg=SendUploadImg;
    this.SendUploadFlash=SendUploadFlash;
    this.InsertImage=InsertImage;
    this.InsertFlash=InsertFlash;
    this.InsertTypeDataGo=InsertTypeDataGo;
    this.InsertTypeData=InsertTypeData;
    this.IframeToContentText=IframeToContentText;
    this.ContentTextToIframe=ContentTextToIframe;
    this.SaveContentTextGo=SaveContentTextGo;
    this.LoadContentTextGo=LoadContentTextGo;  
    this.HideDiv=HideDiv;
    this.UpFile_OnSubmit=UpFile_OnSubmit;
    this.EditorSize=EditorSize;
    this.OpenWindow=OpenWindow;
    this.EditorDesign=EditorDesign;
    this.EditorPreview=EditorPreview;
    this.EditorHtmlCode=EditorHtmlCode;
    this.InsertsFlashGo=InsertsFlashGo;
    this.SendUploadFlashToPreview=SendUploadFlashToPreview;    

    //是否开启设计模式
    function EditorDesignMode(EditorID)
    {	
		var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
        deditor.document.designMode="on";
        //deditor.contentEditable="true";
        deditor.document.open();
		deditor.document.writeln("<html><head><title></title></head><body></body></html>");
        deditor.document.close();
    }

    //当鼠标经过工具条图标时
    function OverIco(object)
    {
	    object.className="toolbar_ico_onmouseover";
    }

    //当鼠标离开工具条图标时
    function OutIco(object, basePath)
    {
	    object.className="toolbar_ico_onmouseout";
	    object.style.backgroundimage=basePath+"/editor/images/Icobg.gif";
    }

    //字体加粗
    function FontBold(EditorID)
    {
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
	    // 聚焦编辑器
	    deditor.focus();
	    // 文本加粗处理
	    deditor.document.execCommand('bold',false,null);
	    // 聚焦编辑器
	    deditor.focus();
    }

    //字体斜体
    function FontItalic(EditorID)
    {
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
	    // 聚焦编辑器
	    deditor.focus();
	    // 文本斜体处理
	    deditor.document.execCommand('italic',false,null);
	    // 聚焦编辑器
	    deditor.focus();
    }

    //字体下划线
    function FontUnderLine(EditorID)
    {
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
	    // 聚焦编辑器
	    deditor.focus();
	    // 文本下划线处理
	    deditor.document.execCommand('underline',false,null);
	    // 聚焦编辑器
	    deditor.focus();
    }

    //清除格式
    function RemoveFormats(EditorID)
    {
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
	    // 聚焦编辑器
	    deditor.focus();
    	
        // 清除格式处理
        deditor.document.execCommand('RemoveFormat',false,null);
    	
	    // 聚焦编辑器
	    deditor.focus();
    }

    //设置字体大
    function SetFontSize(EditorID)
    {
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
        // 聚焦编辑器		
	    deditor.focus();
	    //取得下接框选中大小值
	    var _FontSize = document.getElementById('dropFontSize').value;
    	
	    // 设置字体大处理
	    deditor.document.execCommand('FontSize',false,_FontSize);
    	
	    // 聚焦编辑器
	    deditor.focus();
    }

    //设置字体
    function SetFontName(EditorID)
    {
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
	    // 聚焦编辑器
	    deditor.focus();
	    // 取得字体名称
	    var _FontName = document.getElementById("dropFontName").value;
	    // 设置字体处理
	    deditor.document.execCommand('FontName',false, _FontName);
	    // 聚焦编辑器
	    deditor.focus();
    }

    //设置字体颜色
    function SetFontColor(EditorID)
    {
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
	    // 聚焦编辑器
	    deditor.focus();
	    //取得颜色名称
	    var _FontColor = document.getElementById("dropFontColor").value;
    	
	    // 设置字体颜色
	    deditor.document.execCommand('ForeColor',false,_FontColor);
    	
	    // 聚焦编辑器
	    deditor.focus();
    }
    
    //插入链接至编辑器
    function InsertLinkGo(EditorID, strUrl)
    {
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
	    // 聚焦编辑器
	    deditor.focus();
    	
    	if (window.navigator.userAgent.indexOf("MSIE")>=1)
    	{//为IE时
    	    if (deditor.document.selection.createRange().htmlText.length>0)
    	    {//已选中文本
    	        // 插入链接
	            deditor.document.execCommand('CreateLink',false,strUrl);
    	    }else{//未选中文本
    	        deditor.document.selection.createRange().pasteHTML(strUrl);
    	    }
	    }
	    else
	    {//非IE时
    	    if (deditor.window.getSelection().toString().length>0)
    	    {//已选中文本
    	        // 建立链接
	            deditor.document.execCommand('CreateLink',false,strUrl);
    	    }else{//未选中文本
    	        //插入链接
    	        deditor.document.execCommand('insertHTML',false,"<a href='"+strUrl+"' target='_blank'>"+strUrl+"</a>");
    	    }
    	}
    	
	    // 聚焦编辑器
	    deditor.focus();
	    HideDiv("PopDiv");
    }
    
     //取消链接
    function InsertUnlink(EditorID)
    {
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
	    // 聚焦编辑器
	    deditor.focus();
    	
	    // 取消链接
	    deditor.document.execCommand('Unlink',false,null);
    	
	    // 聚焦编辑器
	    deditor.focus();
    }
      

    
    //插入图片至编辑器
    function InsertsImageGo(imageurl,EditorID)
	{
	    
		var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
		
	    // 聚焦编辑器
	    deditor.focus();
	    
	    //插入图片处理
		if(imageurl != "")
		{
		    if (window.navigator.userAgent.indexOf("MSIE")<1)
		    {
		        deditor.document.execCommand('InsertImage',false,imageurl);
		    }else{
	            var _image = document.createElement("img");
                _image.src=imageurl;
                _image.border="0";

	            if (deditor.document.selection.type.toLowerCase() != "none")
                {
                    deditor.document.selection.clear() ;
                }
                deditor.document.selection.createRange().pasteHTML(_image.outerHTML);
            }
		}
		
	    // 聚焦编辑器
	    deditor.focus();
	    HideDiv("PopDiv");
	}
	
	//插入Flash至编辑器
    function InsertsFlashGo(contentText,EditorID)
	{
	    var _deditor = document.getElementById(EditorID+"_iframe");
		var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
		
	    // 聚焦编辑器
	    deditor.focus();
	    
	    //插入Flash处理
		if(contentText != "")
		{           
            if (window.navigator.userAgent.indexOf("MSIE")>=1)
            {
                if (deditor.document.selection.type.toLowerCase() != "none")
                {
                    deditor.document.selection.clear() ;
                }
                deditor.document.selection.createRange().pasteHTML(contentText);
            }else{
                deditor.document.execCommand('insertHTML',false,contentText);
            }
		}
		
	    // 聚焦编辑器
	    deditor.focus();
	    HideDiv("PopDiv");
	}
    
    //插入表情页面代码
    function InsertSmiley(basePath,EditorID)
    {
        var arrSmiley = Array(
						    Array('1.gif'),
						    Array('2.gif'),
						    Array('3.gif'),
						    Array('4.gif'),
						    Array('5.gif'),
						    Array('6.gif'),
						    Array('7.gif'),
						    Array('8.gif'),
						    Array('9.gif'),
						    Array('10.gif'),
						    Array('11.gif'),
						    Array('12.gif'),
						    Array('13.gif'),
						    Array('14.gif'),
						    Array('15.gif'),
						    Array('16.gif'),
						    Array('17.gif'),
						    Array('18.gif'),
						    Array('19.gif'),
						    Array('20.gif'),
						    Array('21.gif'),
						    Array('22.gif')
				      );
		
		var strSmiley="";
        for (i = 0; i < arrSmiley.length; i++)
        {
            strSmiley+="<img src='"+basePath+"/editor/smiley/"+arrSmiley[i]+"' onmouseover=this.className='smiley_ico_onmouseover' onmouseout=this.className='smiley_ico_onmouseout' class='smiley_ico' onclick=deditorClass.InsertsImageGo(this.src,'"+EditorID+"')>";
        }
        return strSmiley;
    }
    
    //插入上传的图片至编辑器
    function SendUploadImg(fileUrl, EditorID)
    {
        InsertsImageGo(fileUrl,EditorID);
    }
    
    
    //插入上传的Flash至编辑器
    function SendUploadFlash(fileUrl, EditorID, width, height)
    {
        if (fileUrl=="" || fileUrl=="http://")
        {
            alert("请上传或填入Flash文件地址!");
            return false;
        }
        
        if (!width)
        {
            width=300;
        }
        if (!height)
        {
            height=200;
        }
        
        var strHtml="";
        strHtml+="<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http:\/\/download.macromedia.com\/pub\/shockwave\/cabs\/flash\/swflash.cab#version=7,0,19,0\" width=\""+width+"\" height=\""+height+"\">";
        strHtml+="  <param name=\"movie\" value=\""+fileUrl+"\" \/>";
        strHtml+="  <param name=\"quality\" value=\"high\" \/>";
        strHtml+="  <embed src=\""+fileUrl+"\" quality=\"high\" pluginspage=\"http:\/\/www.macromedia.com\/go\/getflashplayer\" type=\"application\/x-shockwave-flash\" width=\""+width+"\" height=\""+height+"\"><\/embed>";
        strHtml+="<\/object>";
        
        InsertsFlashGo(strHtml,EditorID);
    }
    
    //插入上传的Flash至预览层
    function SendUploadFlashToPreview(fileUrl, width, height)
    {
        if (fileUrl=="" || fileUrl=="http://")
        {
            return "";
        }
        
        if (width=="")
        {
            width=300;
        }
        if (height=="")
        {
            height=200;
        }
               
        var strHtml="";
        strHtml+="<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"http:\/\/download.macromedia.com\/pub\/shockwave\/cabs\/flash\/swflash.cab#version=7,0,19,0\" width=\""+width+"\" height=\""+height+"\">";
        strHtml+="  <param name=\"movie\" value=\""+fileUrl+"\" \/>";
        strHtml+="  <param name=\"quality\" value=\"high\" \/>";
        strHtml+="  <embed src=\""+fileUrl+"\" quality=\"high\" pluginspage=\"http:\/\/www.macromedia.com\/go\/getflashplayer\" type=\"application\/x-shockwave-flash\" width=\""+width+"\" height=\""+height+"\"><\/embed>";
        strHtml+="<\/object>";
        
        return strHtml;
    }
    
    
    //插入链接
    function InsertLink(basePath,EditorID)
    {
        var strHtml="";
        strHtml+="<div class=\"upload_window\">";
		strHtml+="	类型：<select name=\"LinkType\" id=\"LinkType\" onchange=\"document.getElementById(\'txtLinkUrl\').value=this.value\"><option>其它<\/option><option value=\"file:\">file:<\/option><option value=\"ftp:\">ftp:<\/option><option value=\"gopher:\">gopher:<\/option><option value=\"http:\" selected=\"selected\">http:<\/option><option value=\"https:\">https:<\/option><option value=\"mailto:\">mailto:<\/option><option value=\"news:\">news:<\/option><option value=\"telnet:\">telnet:<\/option><option value=\"wais:\">wais:<\/option><\/select>";
		strHtml+="<\/div>";
		strHtml+="<div class=\"upload_window\">";
		strHtml+="	地址：<input name=\"txtLinkUrl\" id=\"txtLinkUrl\" type=\"text\" value=\"http:\/\/\" size=\"32\">";
		strHtml+="<\/div>";
		strHtml+="<div class=\"upload_window\" style=\"text-align:center;\"><input type=\"button\" name=\"Submit\" value=\"确定插入链接\" onclick=\"deditorClass.InsertLinkGo('"+EditorID+"',document.getElementById('txtLinkUrl').value)\"><\/div>";
        	
        return strHtml;
    }
    
    //插入图片文件页面呈现代码
    function InsertImage(basePath,EditorID,upfilePath)
    {
        var strHtml="";
        strHtml+="<iframe id=\"upfile_iframe\" name=\"upfile_iframe\" width=\"0\" height=\"0\" frameborder=\"0\" scrolling=\"no\"><\/iframe>";
		strHtml+="<form method=\"post\" enctype=\"multipart\/form-data\" name=\"upfile_form\" id=\"upfile_form\" onSubmit=\"return deditorClass.UpFile_OnSubmit(\'"+basePath+"\',\'"+EditorID+"_iframe\',\'"+upfilePath+"\');\" target=\"upfile_iframe\">";
		strHtml+="  <div class=\"upload_window\">";
		strHtml+="  	本地上传：<input type=\"file\" name=\"UpFile\" id=\"UpFile\">&nbsp;<input type=\"submit\" name=\"btnUpFile\" value=\"上传\" id=\"btnUpFile\">";
		strHtml+="  <\/div>";
		strHtml+="<\/form>";
		strHtml+="<div class=\"upload_window\">";
		strHtml+="	远程地址：<input name=\"txtFileUrl\" id=\"txtFileUrl\" type=\"text\" value=\"http:\/\/\" size=\"38\">";
		strHtml+="<\/div>";
		strHtml+="<div id=\"fileUp_Load\" class=\"upload_window\" style=\"display:none;\"><\/div>";
		strHtml+="<div id=\"filePreview_div\" class=\"upload_window\" style=\"display:none;\">";
		strHtml+="  <div class=\"imgPreview\" id=\"filePreview\"><\/div>";
		strHtml+="<\/div>";
		strHtml+="<div class=\"upload_window\" style=\"text-align:center;\"><input type=\"button\" name=\"Submit\" value=\"确定插入图片\" onclick=\"deditorClass.SendUploadImg(document.getElementById('txtFileUrl').value, '"+EditorID+"')\"><\/div>";
        	
        return strHtml;
    }
    
    
    //插入Flash文件页面呈现代码
    function InsertFlash(basePath,EditorID,upfilePath)
    {
        var strHtml="";
		strHtml+="<form method=\"post\" enctype=\"multipart\/form-data\" name=\"upfile_form\" id=\"upfile_form\" onSubmit=\"return deditorClass.UpFile_OnSubmit(\'"+basePath+"\',\'"+EditorID+"_iframe\',\'"+upfilePath+"\');\" target=\"upfile_iframe\">";
		strHtml+="  <div class=\"upload_window\">";
		strHtml+="  	本地上传：<input type=\"file\" name=\"UpFile\" id=\"UpFile\">&nbsp;<input type=\"submit\" name=\"btnUpFile\" value=\"上传\" id=\"btnUpFile\">";
		strHtml+="  	<iframe id=\"upfile_iframe\" name=\"upfile_iframe\" width=\"0\" height=\"0\" frameborder=\"0\" scrolling=\"no\"><\/iframe>";
		strHtml+="  <\/div>";
		strHtml+="<\/form>";
		strHtml+="<div class=\"upload_window\">";
		strHtml+="	远程地址：<input name=\"txtFileUrl\" id=\"txtFileUrl\" type=\"text\" value=\"http:\/\/\" size=\"38\">";
		strHtml+="<\/div>";
		strHtml+="<div class=\"upload_window\">";
		strHtml+="	宽：<input name=\"txtWidth\" id=\"txtWidth\" type=\"text\" size=\"3\"> px&nbsp;&nbsp;&nbsp高：<input name=\"txtHeight\" id=\"txtHeight\" type=\"text\" size=\"3\"> px";
		strHtml+="<\/div>";
		strHtml+="<div id=\"fileUp_Load\" class=\"upload_window\" style=\"display:none;\"><\/div>";
		strHtml+="<div id=\"filePreview_div\" class=\"upload_window\" style=\"display:none;\">";
		strHtml+="  <div class=\"imgPreview\" id=\"filePreview\"><\/div>";
		strHtml+="<\/div>";
		strHtml+="<div class=\"upload_window\" style=\"text-align:center;\"><input type=\"button\" name=\"Submit\" value=\"确定插入Flash\" onclick=\"deditorClass.SendUploadFlash(document.getElementById('txtFileUrl').value, '"+EditorID+"', document.getElementById('txtWidth').value, document.getElementById('txtHeight').value)\"><\/div>";
        return strHtml;
    }

    
    //返回插入内容数据
    function InsertTypeDataGo(InsertType, basePath, EditorID,upfilePath)
    {
        switch (InsertType)
        {
            case "InsertLink":    //插入链接
                return InsertLink(basePath, EditorID);  //返回插入链接页面代码
                break;
            case "InsertSmiley":    //插入表情
                return InsertSmiley(basePath, EditorID);  //返回表情数据集合页面代码
                break;
            case "InsertImage": //插入图片文件
                return InsertImage(basePath,EditorID,upfilePath);  //返回文件上传窗口页面代码
                break;
            case "InsertFlash": //插入Flash
                return InsertFlash(basePath,EditorID,upfilePath);  //返回文件上传窗口页面代码
                break;
        }
    }
    
    //定义初始变量
    var divResult=null;
    var creatFlag=false;
    //弹出层，用与插入相关内容
    function InsertTypeData(object,InsertType,width,basePath,EditorID,upfilePath)
    {       
	    if(creatFlag == true){
	        creatFlag=false;
	        HideDiv("PopDiv");
		    return false;
	    }
	    else{	
		    var etop  = object.offsetTop;
		    var eleft = object.offsetLeft;
		    while (object = object.offsetParent){etop+=object.offsetTop; eleft+=object.offsetLeft;}

		    var divResult = document.createElement("div");
		    divResult.id = "PopDiv";
		    divResult.style.position = "absolute";
		    divResult.style.background = "#DBEFFD";
		    divResult.style.width = width+"px";
		    divResult.style.border = "1px solid #7DACE3";
		    divResult.style.padding = "3px";
		    divResult.style.top = etop+23;
		    divResult.style.left = eleft+1;

            divResult.innerHTML += "<div style='text-align:right;width:100%;'><img src='"+basePath+"/editor/images/div_close.gif' onClick=deditorClass.HideDiv('PopDiv') style='cursor:pointer;' alt='关闭' /></div>"
            divResult.innerHTML += "<div style='width:100%;'>"+InsertTypeDataGo(InsertType,basePath,EditorID,upfilePath)+"</div>";
            document.body.appendChild(divResult);
		    
            creatFlag=true;
	    }
    }
    

    //保存文本至TextBox
    function IframeToContentText(EditorID)
    {  
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
        var _txtContentText=document.getElementById(EditorID);
        
		_txtContentText.value=deditor.document.body.innerHTML;  //给隐藏域赋值
    }
    
    //保存文本至Iframe
    function ContentTextToIframe(EditorID)
    {  
        
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
        var _txtContentText=document.getElementById(EditorID);
        
		deditor.document.body.innerHTML=_txtContentText.value;  //给iframe赋值
    }
    
    
  	//载入初始值
    function LoadContentTextGo(EditorID, BodyText)
    {
        var deditor = document.getElementById(EditorID+"_iframe").contentWindow;
        var _txtContent=document.getElementById(EditorID);
		
        if(_txtContent!=null)
        {
            if (window.navigator.userAgent.indexOf("MSIE")>=1)
            {
                deditor.document.body.innerHTML=_txtContent.innerText;  //给iframe赋值
            }
            else
            {
                deditor.document.body.innerHTML=_txtContent.textContent;  //给iframe赋值
            }
        }
    }
    
    
	
	//定时为隐藏域赋值
    function SaveContentTextGo(EditorID)
    {
        
        var _deditor = document.getElementById(EditorID+"_iframe");
        var deditor = _deditor.contentWindow;
        var _txtContentText=document.getElementById(EditorID);
        
        if (window.navigator.userAgent.indexOf("MSIE")<1)
        {//只有当浏览器为非IE时才调用以下方法从iframe向隐藏域赋值
            if(_txtContentText!=null && deditor.document.body!=null && _deditor.style.display=="block" && _txtContentText.style.display=="none")
            {
                IframeToContentText(EditorID);
            }
            window.setTimeout("deditorClass.SaveContentTextGo('"+EditorID+"')",300);
        }
    }
    
    
       
    //隐藏层
    function HideDiv(objet)
    {
	    var divResults = document.getElementById(objet);
	    if (divResults) {
		    var oOpenForm = divResults.parentNode;
		    var oOpenContext = oOpenForm.parentNode;
		    oOpenForm.removeChild(divResults);
		    creatFlag=false;
	    }
    }
	
	//上传文件提交
	function UpFile_OnSubmit(basePath,EditorID,upfilePath)
	{
	    
		if (document.getElementById('UpFile').value.length == 0 )
		{
			alert( '请选择完成要上传传的文件!' ) ;
			return false ;
		}
   
		// 上传提示
		document.getElementById('fileUp_Load').style.display="block";
		document.getElementById('fileUp_Load').innerHTML = '文件上传中，请稍侯...' ;
		//document.getElementById('btnUpFile').disabled = true ;
		
		//执行Post提交
		document.upfile_form.action=basePath+"/Editor/Upload.ashx?upfilePath="+upfilePath+"&editid="+EditorID;


		return true ;
	}
	
	//增大缩小输入框
	function EditorSize(sizeNum, EditorID, Editor_Div)
	{
	    
	    var deditor = document.getElementById(EditorID+"_iframe");
	    var deditor_div = document.getElementById(Editor_Div);
	    var _txtContentText=document.getElementById(EditorID);
	       
	    var deditor_div_h=parseInt(deditor_div.style.height);
	    var deditor_h=parseInt(deditor.style.height);
	    var _txtContentText_h=parseInt(_txtContentText.style.height);
	    
	    if (deditor_div_h+sizeNum>0 && deditor_h+sizeNum>0)
	    {
	        deditor_div.style.height=deditor_div_h + sizeNum+"px";
	        deditor.style.height=deditor_h + sizeNum+"px";
	        _txtContentText.style.height=deditor_h + sizeNum+"px";
	    }
	}
	
	//弹出新窗口
	function OpenWindow(url,width,height,type)
	{ 
        var intWidth=width; //窗口宽度
        var intHeight=height;//窗口高度
        var intTop=(window.screen.height-intHeight)/2;
        var intLeft=(window.screen.width-intWidth)/2;
        
        if (type==1)
        {//showModalDialog方式
            window.showModalDialog(url,window,"dialogHeight: "+intHeight+"px; dialogWidth: "+intWidth+"px;dialogTop: "+intTop+"; dialogLeft: "+intLeft+"; resizable: no; status: no;scroll:no");
        }
        else
        {//open方式
            window.open(url,"Detail","Scrollbars=no,Toolbar=no,Location=no,Direction=no,Resizeable=no,Width="+intWidth+" ,Height="+intHeight+",top="+intTop+",left="+intLeft);
        }
    }
	
	//返回设计模式
	function EditorDesign(EditorID)
	{
	    
	    document.getElementById("toolbar").style.display="block";
	    document.getElementById("EditorDesign_div").className="toolbar_mode_b";
	    document.getElementById("EditorPreview_div").className="toolbar_mode_a";
	    document.getElementById("EditorHtmlCode_div").className="toolbar_mode_a";
	    
	    document.getElementById(EditorID).style.display="none";
	    document.getElementById(EditorID+"_iframe").style.display="block";
	}
	
	//返回预览模式
	function EditorPreview(EditorID,basePath,width, height)
	{
	    HideDiv("PopDiv");
	    //OpenWindow(basePath+"Preview.htm?EditorID="+EditorID,width,height,2);
	    OpenWindow(basePath+"/Editor/Preview.htm?EditorID="+EditorID,width,height,2);
	}
	
	//返回HTML源码格式
	function EditorHtmlCode(EditorID)
	{
	    HideDiv("PopDiv");
	    document.getElementById("toolbar").style.display="none";
	    document.getElementById("EditorDesign_div").className="toolbar_mode_a";
	    document.getElementById("EditorPreview_div").className="toolbar_mode_a";
	    document.getElementById("EditorHtmlCode_div").className="toolbar_mode_b";
	    
	    document.getElementById(EditorID+"_iframe").style.display="none";
	    document.getElementById(EditorID).style.display="block";
	}
}

var deditorClass = new DEditorClass();