using System;
using System.Web.UI.WebControls;
using System.Drawing;


public class CFileUpload
{
    private FileUpload _fileUpload;//控件实例
    private string _savePath;//保存目录(绝对位置)，不包含文件名
    private bool _AutoGenFileName = false;//是否自动生成文件扩展名

    private string PICTURE_FILE = "[.gif.png.jpeg.jpg.bmp]";//允许的图片扩展名
    private string ZIP_FILE = "[.zip.rar]";//允许的压缩包扩展名
    private string MUILT_MEDIA_FILE = "[.mpeg.mpg.fla.exe.wma]";//允许的视频扩展名

    private int IMG_MAX_WIDTH = 0;//未指定宽度
    private int IMG_MAX_HEIGHT = 0;//未指定高度

    private short Width;
    private short Height;


    private string _LastUploadedFile = string.Empty;
    /// <summary>
    /// 文件Src
    /// </summary>
    public string LastUploadedFile { get { return _LastUploadedFile; } }

    /// <summary>
    /// 构造器
    /// </summary>
    /// <param name="fileUpload">Asp.net FileUpload对象</param>
    /// <param name="savePath">保存目录，不包含文件名</param>
    /// <param name="autoGenFileName">自动生成文件名</param>
    public CFileUpload(FileUpload fileUpload, string savePath, bool autoGenFileName)
    {
        _savePath = savePath;
        _fileUpload = fileUpload;
        _AutoGenFileName = autoGenFileName;
    }

    /// <summary>
    /// 构造器
    /// </summary>
    /// <param name="fileUpload">Asp.net FileUpload对象</param>
    /// <param name="savePath">保存目录，不包含文件名</param>
    public CFileUpload(FileUpload fileUpload, string savePath)
    {
        _savePath = savePath;
        _fileUpload = fileUpload;
    }

    /// <summary>
    /// 上传RAR 文件
    /// </summary>
    public bool UploadRARFile()
    {
        return DoUpload(ZIP_FILE);
    }

    /// <summary>
    /// 上传视频文件
    /// </summary>
    public bool UploadVideo()
    {
        return DoUpload(MUILT_MEDIA_FILE);
    }

    /// <summary>
    /// 上传图片文件
    /// </summary>
    public bool UploadImage()
    {
        return DoUpload(PICTURE_FILE);
    }

    public bool UploadImage(int maxWidth, int maxHeight)
    {
        this.IMG_MAX_WIDTH = maxWidth;
        this.IMG_MAX_HEIGHT = maxHeight;
        return DoUpload(PICTURE_FILE);
    }

    /// <summary>
    /// 上传任何支持的文件
    /// </summary>
    public bool UploadAnySupported()
    {
        return DoUpload(PICTURE_FILE + ZIP_FILE + MUILT_MEDIA_FILE);
    }

    /// <summary>
    /// 生成新的文件名
    /// </summary>
    private string GetNewFileName(string folder, string fileName)
    {
        //_AutoGenFileName==true 或者文件名长度>50,自动生成32位GUID文件名
        //if (_AutoGenFileName || StrUtils.GetStringLength(fileName) >= 50)
        if (_AutoGenFileName || fileName.Length >= 50)
        {
            string ext = System.IO.Path.GetExtension(fileName);
            //string newfile = Guid.NewGuid().ToString().Replace("-", "") + ext;
            Random rd = new Random();
            string newfile = DateTime.Now.ToString("yyyyMMddHHmmss") + rd.Next(9) + rd.Next(9) + ext;
            _LastUploadedFile = newfile;
            return folder + newfile;
        }
        else
        {
            if (System.IO.File.Exists(folder + fileName))
            {
                string ext = System.IO.Path.GetExtension(fileName);
                string filebody = fileName.Replace(ext, "");

                int x = 1;
                while (true) //如果文件存在，生成尾部带(x)的文件
                {
                    string newfile = folder + filebody + "(" + x.ToString() + ")" + ext;
                    if (!System.IO.File.Exists(newfile))
                        return folder + filebody + "(" + x.ToString() + ")" + ext;
                    else
                        x++;
                }
            }
            else
                return folder + fileName;
        }
    }

    /// <summary>
    /// 最大支持小于2MB的文件。
    /// </summary>
    private bool AllowMaxSize(int fileLength)
    {
        int MAX_SIZE_UPLOAD = 2048;//最大支持上传小于2MB的文件。
        double kb = fileLength / 1024;
        return (int)kb < MAX_SIZE_UPLOAD;
    }

    private bool DoUpload(string allowedExtensions)
    {
        bool fileOK = false;

        if (!_fileUpload.HasFile) return false; //上传控件中如果不包含文件，退出

        // 得到文件的后缀
        string fileExtension = System.IO.Path.GetExtension(_fileUpload.FileName).ToLower();

        // 看包含的文件是否是被允许的文件后缀
        fileOK = allowedExtensions.IndexOf(fileExtension) > 0;

        //检查上传文件大小
        fileOK = fileOK & AllowMaxSize(_fileUpload.FileBytes.Length);

        if (!fileOK) return false; //如检查不通过，退出

        try
        {
            // 文件另存在服务器指定目录下
            string savefile = GetNewFileName(_savePath, _fileUpload.FileName);

            if (IsUploadImage(fileExtension))//保存图片
            {
                System.Drawing.Image output = CImageLibrary.FromBytes(_fileUpload.FileBytes);

                // 检查图片宽度/高度/大小
                if (this.IMG_MAX_WIDTH != 0 && output.Width > this.IMG_MAX_WIDTH)
                {
                    output = CImageLibrary.GetOutputSizeImage(output, this.IMG_MAX_WIDTH);
                }
                Bitmap bmp = new Bitmap(output);
                bmp.Save(savefile, output.RawFormat);

                Width = (short)output.Width;
                Height = (short)output.Height;
            }
            else//其它任何文件
            {
                _fileUpload.PostedFile.SaveAs(savefile);
            }
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool IsUploadImage(string fileExtension)
    {
        bool isImage = PICTURE_FILE.IndexOf(fileExtension) > 0;
        return isImage;
    }
}

