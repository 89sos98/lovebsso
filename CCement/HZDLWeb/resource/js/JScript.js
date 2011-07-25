//////后台相关脚本
function $(id) {
    return document.getElementById(id);
}
function onTBox(box) {
    box.style. borderColor = "#FF6600";
    box.style.backgroundColor = "#ffffcc";
}
function outTBox(box) {
    box.style.borderColor = "#cccccc";
    box.style.backgroundColor = "#ffffff";
}

////展开收起图片切换
function onChangeImg(obj) {
    var d = obj.src;
    if (d.indexOf("collapsed.gif") > -1) {
        d = d.replace("collapsed.gif", "expanded.gif");
        if (d.indexOf("?")) { obj.src = d; }
        else { obj.src = d + "?timestamp=" + Date(); }
        obj.title = "收起";
        obj.alt = "收起";
    }
    else {
        d = d.replace("expanded.gif", "collapsed.gif");
        if (d.indexOf("?")) { obj.src = d; }
        else { obj.src = d + "?timestamp=" + Date(); }
        obj.title = "展开";
        obj.alt = "收起";
    }
}
////展开收起栏目
function onShowChildren(cid) {
    var trs = document.getElementsByTagName("tr");
    for (var i = 0; i < trs.length; i++) {
        if (trs[i].className == cid) {
            if (trs[i].style.display == "none")
                trs[i].style.display = "block";
            else
                trs[i].style.display = "none";
        }
    }

////    if ($("." + cid).is(":visible"))
////        $("." + cid).hide();
////    else
////        $("." + cid).show();
}

////全选
function checkAll(all) {
    var a = document.getElementsByName("arts");
    for (var i = 0; i < a.length; i++) {
        if (!a[i].disabled)
            a[i].checked = all.checked;
    }
}

////检查是否有选中项,并提示删除
function checkCheck() {
    var a = document.getElementsByName("arts");
    var b = false;
    for (var i = 0; i < a.length; i++) {
        if (a[i].checked) { b = true; break; }
    }
    if (b) {
        if (confirm("删除数据不可恢复,您确定要删除选中项吗?"))
            return true;
        else
            return false;
    } else {
        alert("请选择项");
        return false;
    }
}

////检查是否有选中项
function onlyCheck() {
    var a = document.getElementsByName("arts");
    var b = false;
    for (var i = 0; i < a.length; i++) {
        if (a[i].checked) { b = true; break; }
    }
    if (b) {
        return true;
    } else {
        alert("请选择项");
        return false;
    }
}