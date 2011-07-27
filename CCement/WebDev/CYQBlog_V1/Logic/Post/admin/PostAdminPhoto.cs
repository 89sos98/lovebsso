using System;
using System.Collections.Generic;
using System.Text;
using Web.Core;
using CYQ.Data;
using CYQ.Entity.MySpace;
using CYQ.Entity;

namespace Logic
{
    public class PostAdminPhoto:CoreBase
    {
        public PostAdminPhoto(ICore custom) : base(custom,true) { }

        public bool PostPhoto()
        {
            bool result = false;
            FileUpload file=new FileUpload(Request.Files[0], UploadType.UserPhoto);
            if (!file.Upload(true,100,100,false))
            {
                return false;
            }
            string filePath = file.fileNameWithPath;
            //地址如： admin/article/post
            using (MAction action = new MAction(TableNames.Blog_Content))
            {
                action.Set(Content.Icon, filePath.Replace(file.exName, "_m" + file.exName));
                action.Set(Content.Body, filePath);
                action.Set(Content.TypeID, 1);
                action.Set(Content.UserID, LoginUserID);
                action.SetAutoPrefix("txt","hid");
                if (action.Insert(true))
                {
                    int classID=action.Get<int>(Content.ClassID);//分类文章统计
                    int count = action.GetCount(Content.ClassID + "=" + classID);
                    if (action.ResetTable(TableNames.Blog_Class))
                    {
                        action.Set(Class.Count, count);
                        result=action.Update(classID);
                    }
                }
            }
            return result;
        }
        public bool PostEditPhoto()
        {
            bool result = false;
            //地址如： admin/article/edit/{id}
            int id = GetParaInt(4);
            if (id>0)
            {
                using (MAction action = new MAction(TableNames.Blog_Content))
                {
                    string where = Content.UserID + "=" + LoginUserID + " and ID=" + id;
                    action.Set(Content.EditTime, DateTime.Now);
                    action.SetAutoPrefix("txt","hid");
                    result = action.Update(where, true);
                }
            }
            return result;
        }
        public bool PostClass()
        {
            string value = Get(IDKey.txtName);
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            bool result = false;
            //地址如： admin/article/class/post
            using (MAction action = new MAction(TableNames.Blog_Class))
            {
                action.Set(Class.TypeID, 1);
                action.Set(Class.UserID, LoginUserID);
                action.Set(Class.Name, value);
                result=action.Insert(true);
            }
            return result;
        }
        public bool PostEditClass()
        {
            string value = Get(IDKey.txtName);
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }
            bool result = false;
            //地址如： admin/article/class/edit/{id}
            int id = GetParaInt(5);
            if (id>0)
            {
                using (MAction action = new MAction(TableNames.Blog_Class))
                {
                    string where = Class.UserID + "=" + LoginUserID + " and ID=" + id;
                    action.Set(Class.Name, value);
                    result=action.Update(where, true);
                }
            }
            return result;
        }
    }
}
