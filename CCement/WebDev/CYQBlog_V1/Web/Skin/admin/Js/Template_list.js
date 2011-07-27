window.onload=WindowOnLoad;
function WindowOnLoad()
{
	document.forms[0].onsubmit=function()
	{
	   document.getElementsByName('btnSelectTemplate')[0].style.display='none';
	   document.getElementById('template_msg').innerText=document.getElementById('template_msg').title;
	}
}
