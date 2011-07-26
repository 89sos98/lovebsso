window.onload=displayMenu;
 
 function displayMenu(){
 
 //获取菜单id
 
   var menu=getId("menu");
   
  //隐藏2级菜单和3级
  // for(var i=0;i<getTNs(menu,"ul").length;i++){
//     getTNs(menu,"ul")[i].style.display="none";
//   }
  
  //获取菜单所有li
 var menu_li_1=getTNs(menu,"li");
 
  //隐藏显示菜单
 for(var i=0;i<menu_li_1.length;i++){
  if(menu_li_1[i].getElementsByTagName("ul").length>0){
    menu_li_1[i].onmouseover=displayChildMenu;
 menu_li_1[i].onmouseout=disappearChildMenu;
  }
 }
 }
 
 //获取id函数
 function getId(elem){
   return document.getElementById(elem);
 }
 
 //获取tagname
 function getTNs(elem,name){
   return (elem || document).getElementsByTagName(name);
 }
 
 //显示菜单函数
 function displayChildMenu(){
   getTNs(this,"ul")[0].style.display="block";
 } 
 
 //隐藏菜单函数
 function disappearChildMenu(){
   for(var i=0;i<getTNs(this,"ul").length;i++){
    getTNs(this,"ul")[i].style.display="none";
   }
 }