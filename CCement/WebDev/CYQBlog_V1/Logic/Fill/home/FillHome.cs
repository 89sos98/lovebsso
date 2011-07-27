
using Web.Core;
using CYQ.Data;
using CYQ.Entity.MySpace;
using CYQ.Entity;
using CYQ.Data.Xml;
namespace Logic
{
    public class FillHome:CoreBase
    {
        public FillHome(ICore custom) : base(custom){ }
        public void FillAllUser()//填充主页所有用户
        {
            using (MAction action = new MAction(TableNames.Blog_User))
            {
                Document.Set(IDKey.labUserName, SetType.A, "{0}[{1}]",Config.HttpHost+"/{1}");
                Document.LoadData(action.Select());
                Document.SetForeach(IDKey.labAllUser, SetType.InnerXml, Users.NickName, Users.UserName);
            }
        }
    }
}
