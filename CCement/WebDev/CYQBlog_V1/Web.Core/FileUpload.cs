using System;
using System.Web;
using System.IO;
using CYQ.Data;
using CYQ.Entity.MySpace;
using System.Drawing;
using System.Drawing.Imaging;

namespace Web.Core
{
    public enum UploadType
    {
        Editor,
        UserPhoto,
        UserHead,
    }
    public class FileUpload
    {
        private UploadType uploadType = UploadType.Editor;
        private HttpPostedFile _PostFile;   //HttpPostedFile模式文件传入
        public FileUpload(HttpPostedFile postFile)
        {
            _PostFile = postFile;
        }
        public FileUpload(HttpPostedFile postFile, UploadType type)
        {
            _PostFile = postFile;
            uploadType = type;
        }
        #region 文件上传后的属性

       
        /// <summary>
        /// 上传文件扩展名
        /// </summary>
        public string exName = string.Empty;
        /// <summary>
        /// 上传生成的文件名
        /// </summary>
        public string fileName = string.Empty;
        /// <summary>
        /// 上传后文件完整路径
        /// </summary>
        public string fileNameWithHttp = string.Empty;
        /// <summary>
        /// 上传后文件相对路径
        /// </summary>
        public string fileNameWithPath = string.Empty;
        /// <summary>
        /// 文件大小(单位KB)
        /// </summary>
        public int fileSize = 0;
        #endregion

        private bool CheckExName()
        {
            if (_PostFile == null || _PostFile.ContentLength < 1 || _PostFile.ContentLength > 1024 * 1024 * 4)
            {
                return false;
            }
            exName = Path.GetExtension(_PostFile.FileName).ToLower();
            if (Config.FileAllowExName.IndexOf(exName) == -1)
            {
                return false;
            }
            fileSize = _PostFile.ContentLength / 1024;
            return true;
        }
        private string CheckFolder()
        {
            string todayFoler = AppDomain.CurrentDomain.BaseDirectory + UploadPath+Today;
            if (!Directory.Exists(todayFoler))
            {
                Directory.CreateDirectory(todayFoler);
            }
            return todayFoler;
        }
        internal string Today
        {
            get
            {
                if (uploadType == UploadType.UserHead)
                { return string.Empty; }
                return DateTime.Today.ToString("yyyyMMdd/");
            }
        }
        internal string NowTime
        {
            get
            {
                return DateTime.Now.ToString("hhmmss");
            }
        }
        private bool InserInfo()
        {
            bool result = false;
            using (MAction action = new MAction(TableNames.Blog_File))
            {
                int userID = 0;
                UserLogin user = new UserLogin();
                if (user.IsOnline(false))
                {
                    userID = user.UserInfo.Get<int>(Users.ID);
                }
                action.Set(Files.UserID, userID);
                action.Set(Files.FilePath, fileNameWithPath);
                action.Set(Files.FileName, fileName);
                action.Set(Files.Size, fileSize);
                result = action.Insert();
            }
            return result;
        }


        private string UploadPath
        {
            get
            {
                switch (uploadType)
                {
                    case UploadType.UserHead:
                        return Config.UserHeadUploadPath;
                    case UploadType.UserPhoto:
                        return Config.UserPhotoUploadPath;
                    default:
                        return Config.EditorUploadPath;
                }
            }
        }

        #region 上传方法

        public bool Upload()
        {
           return Upload(true,0, 0,false);
        }
        public bool Upload(bool keepSource,int width,int height,bool toBlackWhite)
        {
            if (CheckExName())
            {
                string folderPath = CheckFolder();
                if (fileName == string.Empty)
                {
                    fileName = NowTime + exName;
                }
                if (keepSource)
                {
                    _PostFile.SaveAs(folderPath + fileName);
                }
                if (width>0 && height>0)
                {
                    string name = folderPath + (uploadType == UploadType.UserHead ? fileName : fileName.Replace(exName, "_m" + exName));
                    MakeSmallImg(_PostFile.InputStream, name, new Rectangle(0, 0, width, height), toBlackWhite);
                }
                fileNameWithPath = UploadPath + Today+ fileName;
                fileNameWithHttp = Config.HttpHost + fileNameWithPath;
                switch (uploadType)
                {
                    case UploadType.Editor:
                        return InserInfo();
                    default:
                        return true;

                }

            }
            return false;
        }
        public ImageFormat GetImageFormat()
        {
            if (uploadType == UploadType.UserHead)
            {
                return ImageFormat.Bmp;
            }
            switch(exName)
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                    
                case ".gif":
                    return ImageFormat.Gif;
                case ".png":
                    return ImageFormat.Png;
                case ".icon":
                    return ImageFormat.Icon;
                case ".tiff":
                    return ImageFormat.Tiff;
                default:
                    return ImageFormat.Bmp;
            }

        }
        /// <summary>
        /// 图片保存
        /// </summary>
        /// <param name="fileName">原始图片上传路径</param>
        /// <param name="saveImg">保存的图片路径</param>
        /// <param name="OutputArea">输入图片的大小[0,0,width,height]</param>
        /// <param name="toBlackWhite">是否转成黑白图片</param>
        private  void MakeSmallImg(Stream fileStream, string saveImg, Rectangle OutputArea, bool toBlackWhite)
        {
            System.Drawing.Image ImageDemo = System.Drawing.Image.FromStream(fileStream, true);

            System.Drawing.Bitmap OutputImage = new System.Drawing.Bitmap(OutputArea.Width, OutputArea.Height);

            System.Drawing.Graphics MapGraphy = System.Drawing.Graphics.FromImage(OutputImage);

            MapGraphy.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            MapGraphy.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            MapGraphy.Clear(System.Drawing.Color.White);
            if (toBlackWhite)
            {
                ColorMatrix _matrix = new ColorMatrix();
                _matrix[0, 0] = 1 / 3f;
                _matrix[0, 1] = 1 / 3f;
                _matrix[0, 2] = 1 / 3f;
                _matrix[1, 0] = 1 / 3f;
                _matrix[1, 1] = 1 / 3f;
                _matrix[1, 2] = 1 / 3f;
                _matrix[2, 0] = 1 / 3f;
                _matrix[2, 1] = 1 / 3f;
                _matrix[2, 2] = 1 / 3f;
                ImageAttributes _attributes = new ImageAttributes();
                _attributes.SetColorMatrix(_matrix);
                MapGraphy.DrawImage(ImageDemo, OutputArea, 0, 0, ImageDemo.Width, ImageDemo.Height, GraphicsUnit.Pixel, _attributes);
            }
            else
            {
                MapGraphy.DrawImage(ImageDemo, OutputArea);
            }

            OutputImage.Save(saveImg, GetImageFormat());

            MapGraphy.Dispose();
            OutputImage.Dispose();
            ImageDemo.Dispose();
        }
        #endregion
    }
}
