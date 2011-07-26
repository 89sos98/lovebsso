/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function(config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.language = 'zh-cn'; // 配置语言

    config.uiColor = '#ffccff'; // 背景颜色

    config.width = '800px'; // 宽度

    config.height = '220px'; // 高度

    config.skin = 'v2'; // 界面v2,kama,office2003

    config.toolbar = 'Full'; // 工具栏风格Full,Basic

    // 添加字体

    config.font_names = '宋体/宋体;黑体/黑体;仿宋/仿宋_GB2312;楷体/楷体_GB2312;隶书/隶书;幼圆/幼圆;' + config.font_names;

    config.toolbarLocation = 'top'; // 可选：bottom

    config.resize_enabled = true;

    // 配置复制offic中的文件时是否清除原本样式 （配置后可进行选择）

    config.pasteFromWordPromptCleanup = true;

    // 配置ckfinder
    config.filebrowserBrowseUrl = 'ckeditor/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = 'ckeditor/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = 'ckeditor/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = 'ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = 'ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = 'ckeditor/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
config.filebrowserWindowWidth = '800';
config.filebrowserWindowHeight = '500';
    
};