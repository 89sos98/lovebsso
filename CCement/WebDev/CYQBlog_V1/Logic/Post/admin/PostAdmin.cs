using System;
using CYQ.Data;
using Web.Core;
using CYQ.Entity.MySpace;
using CYQ.Entity;


namespace Logic
{
    public class PostAdmin : CoreBase
    {
        public PostAdmin(ICore custom) : base(custom,true) {  }
        public bool PostSetting()
        {
            bool result = false;
            string headUrl =string.Empty;
            System.Web.HttpPostedFile postedFile=Request.Files[0];
            if (postedFile != null)
            {

                FileUpload upload = new FileUpload(postedFile, UploadType.UserHead);
                upload.fileName = DomainID + ".bmp";
                if (upload.Upload(false, 50, 50, Get(IDKey.labToBlackWhile)=="on"))
                {
                    headUrl = upload.fileNameWithPath;
                }
            }
            using (MAction action = new MAction(TableNames.Blog_User))
            {
                string where = Users.ID + "=" + LoginUserID;
                if (headUrl != string.Empty)
                {
                    action.Set(Users.HeadUrl, headUrl);
                }
                action.SetAutoPrefix("txt");
                result = action.Update(where, true);
            }
            return result;
        }
        public bool PostLink()
        {
            bool result = false;
            using (MAction action = new MAction(TableNames.Blog_Link))
            {
                action.Set(Links.UserID, LoginUserID);
                action.SetAutoPrefix("txt");
                result = action.Insert(true);
            }
            return result;
        }
        public bool PostEditLink()
        {
            bool result = false;
            int id = GetParaInt(4);
            if (id>0)
            {
                using (MAction action = new MAction(TableNames.Blog_Link))
                {
                    string where = Links.UserID + "=" + LoginUserID + " and ID=" + id;
                    action.SetAutoPrefix("txt");
                    result = action.Update(where, true);
                }
            }
            return result;
        }
        public bool PostTemplate()
        {
            bool result = false;
            using (MAction action = new MAction(TableNames.Blog_User))
            {
                action.Set(Users.SkinID, Get(IDKey.labSetTemplate, "1"));
                result = action.Update(DomainID);
            }
            return result;
        }
        public bool PostEditPassword()
        {
            string oldPwd = Get(IDKey.txtPassword, "");
            string newPwd = Get(IDKey.txtNewPassword, "");
            string newPwdAgain = Get(IDKey.txtNewPasswordAgain, "");
            bool result = false;
            using (MAction action = new MAction(TableNames.Blog_User))
            {
                if (oldPwd != "" && Encode.Password(oldPwd,true) == DomainUser.Get<string>(Users.Password)
                    && newPwd.Length > 5 && newPwd == newPwdAgain)
                {
                    action.Set(Users.Password,Encode.Password(newPwd,true));
                    result = action.Update(DomainID);
                    if (result)
                    {
                        UserAction.SetCookie(Domain,newPwd,24*60);
                    }
                }
            }
            return result;
        }
    }
}
