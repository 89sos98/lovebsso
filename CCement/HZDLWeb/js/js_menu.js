window.onload=displayMenu;
 
 function displayMenu(){
 
 //��ȡ�˵�id
 
   var menu=getId("menu");
   
  //����2���˵���3��
  // for(var i=0;i<getTNs(menu,"ul").length;i++){
//     getTNs(menu,"ul")[i].style.display="none";
//   }
  
  //��ȡ�˵�����li
 var menu_li_1=getTNs(menu,"li");
 
  //������ʾ�˵�
 for(var i=0;i<menu_li_1.length;i++){
  if(menu_li_1[i].getElementsByTagName("ul").length>0){
    menu_li_1[i].onmouseover=displayChildMenu;
 menu_li_1[i].onmouseout=disappearChildMenu;
  }
 }
 }
 
 //��ȡid����
 function getId(elem){
   return document.getElementById(elem);
 }
 
 //��ȡtagname
 function getTNs(elem,name){
   return (elem || document).getElementsByTagName(name);
 }
 
 //��ʾ�˵�����
 function displayChildMenu(){
   getTNs(this,"ul")[0].style.display="block";
 } 
 
 //���ز˵�����
 function disappearChildMenu(){
   for(var i=0;i<getTNs(this,"ul").length;i++){
    getTNs(this,"ul")[i].style.display="none";
   }
 }